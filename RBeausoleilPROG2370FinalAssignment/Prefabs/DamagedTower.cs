using CustomXna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RBeausoleilPROG2370FinalAssignment.ComponentControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.Prefabs
{
    public class DamagedTower : GameObject
    {
        public override void OnCreated()
        {
            Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
            ModelRenderer modelRenderer = AddComponent<ModelRenderer>() as ModelRenderer;
            modelRenderer.Model = Game.Content.Load<Model>("towerdamaged");
            modelRenderer.SpecularColor = new Vector3(0.01f, 0.01f, 0.01f);
            modelRenderer.SpecularPower = 800;
            Layer = (int)LayerType.EnemyStructure;
            StructureDamageController towerBaseStructure = AddComponent<StructureDamageController>() as StructureDamageController;
            towerBaseStructure.HitHealth = 5;
            //towerBaseStructure.PrefabName = "TowerRubble";

            //towerBaseStructure.DestructedPrefab

            //**************************************
            GameObject towerBaseCollider = Instantiate(Game);
            (towerBaseCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(5.0f, 5.0f);
            towerBaseCollider.Transform.Parent = this;
            towerBaseCollider.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-90.0f));
        }
    }
}