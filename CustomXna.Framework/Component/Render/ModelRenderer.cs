/*  ModelRenderer.cs
 *  Renders Models and their meshes
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.9.24: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public class ModelRenderer : Renderer
    {
        private Vector3 _ambientLightColor = new Vector3(0.6f, 0.6f, 0.6f);
        private Vector3 _diffuseColor = new Vector3(0.85f, 0.75f, 0.75f);
        private Vector3 _specularColor = new Vector3(0.25f, 0.2f, 0.3f);
        private Model _model;

        public DepthStencilState DepthStencilState { get; set; } = DepthStencilState.Default;
        public BlendState BlendState { get; set; } = BlendState.Opaque;


        public Model Model
        {
            get => _model;
            set
            {
                _model = value;

                GetBoundingSphereLocal();
            }
        }

        public BoundingSphere BoundingSphereLocal { get; private set; }
        public BoundingSphere BoundingSphereWorld { get; private set; }
        public bool TextureEnabled { get; set; } = true;

        public float SpecularPower { get; set; } = 100f;
        // TODO: Make these Properties for Directional lights 1, 2, & 3 to keep it modular
        public Vector3 AmbientLightColor
        {
            get => _ambientLightColor;
            set
            {
                _ambientLightColor = value;
            }
        }
        public Vector3 DiffuseColor
        {
            get => _diffuseColor;
            set
            {
                _diffuseColor = value;
            }
        }
        public Vector3 SpecularColor
        {
            get => _specularColor;
            set
            {
                _specularColor = value;
            }
        }

        public ModelRenderer() : base()
        {
            
        }

        public override void Start()
        {
            Transform.TransformChanged += Transform_TransformChanged;

            GetBoundingSphereLocal();
        }

        private void Transform_TransformChanged(object sender, EventArgs e)
        {
            if (_model != null)
            {
                // change the BoundingSphereWorld of the Transform's Scale, Rotation & Position from the BoundingSphereLocal
                BoundingSphereWorld = BoundingSphereLocal.Transform(Matrix.CreateScale(Transform.Scale) * Matrix.CreateFromQuaternion(Transform.Rotation) * Matrix.CreateTranslation(Transform.Position));
            }
        }

        /// <summary>
        /// Gets the overall bounding sphere for the model and all its meshes
        /// </summary>
        private void GetBoundingSphereLocal()
        {
            if (_model != null)
            {
                BoundingSphere? boundingSphere = null;
                Matrix[] modelTransforms = new Matrix[Model.Bones.Count];
                Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

                for (int i = 0; i < _model.Meshes.Count; i++)
                {
                    if (i == 0)
                    {
                        // assign the first sphere with its AbsoluteBoneTransform position
                        boundingSphere = _model.Meshes[i].BoundingSphere.Transform(modelTransforms[_model.Meshes[i].ParentBone.Index]);
                    }
                    else
                    {
                        // merge sphere with its AbsoluteBoneTransform position to the previous boundingSphere
                        BoundingSphere transformed = _model.Meshes[i].BoundingSphere.Transform(modelTransforms[_model.Meshes[i].ParentBone.Index]);
                        boundingSphere = BoundingSphere.CreateMerged((BoundingSphere)boundingSphere, transformed);
                    }
                }

                // assign the BoundingSphereLocal of this newly created BoundingSphere
                BoundingSphereLocal = (BoundingSphere)boundingSphere;

                // change the BoundingSphereWorld of the Transform's Scale and Position from the BoundingSphereLocal (rotation not needed for sphere)
                BoundingSphereWorld = BoundingSphereLocal.Transform(Matrix.CreateScale(Transform.Scale) * Matrix.CreateTranslation(Transform.Position));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw if the Model isn't null *and* if the Model's BoundingSphere is within the Camera's view Frustrum
            if (Model != null && Game.Camera.BoundingFrustum?.Contains(BoundingSphereWorld) != ContainmentType.Disjoint)
            {
                Game.GraphicsDevice.DepthStencilState = DepthStencilState;
                Game.GraphicsDevice.BlendState = BlendState;
                    
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.FogEnabled = Game.Camera.FogEnabled;

                        if (Game.Camera.FogEnabled)
                        {
                            effect.FogColor = Game.Camera.FogColor;
                            effect.FogStart = Game.Camera.FogStart;
                            effect.FogEnd = Game.Camera.FogEnd;
                        }
                        
                        effect.TextureEnabled = TextureEnabled;

                        effect.LightingEnabled = true;

                        effect.AmbientLightColor = AmbientLightColor;

                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight1.Enabled = true;

                        //todo: add const for lighting
                        effect.DirectionalLight0.DiffuseColor = DiffuseColor;
                        effect.DirectionalLight0.Direction = Vector3.Forward + Vector3.Up * 0.2f + Vector3.Left * 0.05f;

                        effect.DirectionalLight1.SpecularColor = SpecularColor;
                        effect.DirectionalLight1.Direction = Vector3.Forward * 1f + Vector3.Down * 0.01f;

                        effect.SpecularPower = SpecularPower;

                        effect.World = Transform.GetWorldMatrix();

                        effect.View = Game.Camera?.View ?? Matrix.Identity;
                        effect.Projection = Game.Camera?.Projection ?? Matrix.Identity;
                    }

                    mesh.Draw();
                }

                Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                Game.GraphicsDevice.BlendState = BlendState.Opaque;
            }
        }
    }
}