/*  Transform.cs
 *  Every GameObject has a Transform and every component references that Transforms
 *  Used to store and manipulate position, rotation & scale of the object and components.
 *  Has a parent and children, which allows for hierarchical manipulation of position, rotation & scale.
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.9.24: Created
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public class Transform
    {
        public GameObject GameObject { get; internal set; }

        private GameObject _root;
        private GameObject _parent;
        private List<GameObject> _children = new List<GameObject>();

        private Quaternion _localRotation = Quaternion.Identity;
        private Quaternion _rotation = Quaternion.Identity;

        private Vector3 _position = Vector3.Zero;
        private Vector3 _localPosition = Vector3.Zero;

        private float _scale = 1f;

        private event EventHandler<EventArgs> _transformChanged;

        private Vector3? _right = null;
        private Vector3? _left = null;
        private Vector3? _up = null;
        private Vector3? _down = null;
        private Vector3? _forward = null;
        private Vector3? _backward = null;

        /// <summary>
        /// The positive red axis of the transform in world space
        /// </summary>
        public Vector3 Right
        {
            get
            {
                if (_right == null)
                {
                    _right = Vector3.Transform(Vector3.Right, Rotation);
                }

                return (Vector3)_right;
            }
        }

        /// <summary>
        /// The negative red axis of the transform in world space
        /// </summary>
        public Vector3 Left
        {
            get
            {
                if (_left == null)
                {
                    _left = Vector3.Transform(Vector3.Left, Rotation);
                }

                return (Vector3)_left;
            }
        }

        /// <summary>
        /// The positive green axis of the transform in world space
        /// </summary>
        public Vector3 Up
        {
            get
            {
                if (_up == null)
                {
                    _up = Vector3.Transform(Vector3.Up, Rotation);
                }

                return (Vector3)_up;
            }
        }

        /// <summary>
        /// The negative green axis of the transform in world space
        /// </summary>
        public Vector3 Down
        {
            get
            {
                if (_down == null)
                {
                    _down = Vector3.Transform(Vector3.Down, Rotation);
                }

                return (Vector3)_down;
            }
        }

        /// <summary>
        /// The negative blue axis of the transform in world space
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                if (_forward == null)
                {
                    _forward = Vector3.Transform(Vector3.Forward, Rotation);
                }

                return (Vector3)_forward;
            }
        }

        /// <summary>
        /// The positive blue axis of the transform in world space
        /// </summary>
        public Vector3 Backward
        {
            get
            {
                if (_backward == null)
                {
                    _backward = Vector3.Transform(Vector3.Backward, Rotation);
                }

                return (Vector3)_backward;
            }
        }

        public event EventHandler<EventArgs> TransformChanged
        {
            add
            {
                _transformChanged += value;
            }
            remove
            {
                _transformChanged -= value;
            }
        }

        //todo: Remove this
        public bool RotationLocked { get; set; }

        //todo: turn scale into a Vector3 and have it adjust LocalPosition and such based off of scale
        public float Scale
        {
            get => _scale;
            set
            {
                _scale = value;

                int childrenCount = _children.Count;
                for (int i = 0; i < childrenCount; i++)
                {
                    _children[i].Transform.Scale *= _scale;
                }
            }
        }

        /// <summary>
        /// Returns the root GameObject - returns itself if transform has no GameObject parent
        /// </summary>
        public GameObject Root
        {
            get => _root;
            internal set
            {
                if (_root != value)
                {
                    _root = value;

                    int childrenCount = _children.Count;

                    for (int i = 0; i < childrenCount; i++)
                    {
                        _children[i].Transform.Root = value;
                    }
                }
            }
        }

        /// <summary>
        /// The parent GameObject - returns null if transform has no GameObject parent
        /// </summary>
        public GameObject Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (value != GameObject)
                {
                    // only if parent is being change to a different value, adjust the parent/root & children
                    if (Parent != value)
                    {
                        // if current parent is not null (and value isn't)
                        if (Parent != null)
                        {
                            // remove this child GameObject from current parent
                            Parent.Transform.Children.Remove(GameObject);
                        }

                        _parent = value;

                        // setting Parent to null, make Root itself
                        if (value == null)
                        {
                            Root = GameObject;
                        }
                        else
                        {
                            // keep Parent's layer value
                            GameObject.Layer = value.Layer;

                            // keep the world Position and Rotation of the GameObject and assign it as local position and rotation
                            _localPosition = Vector3.Transform(_position, Matrix.Invert(_parent.Transform.GetWorldMatrix()));
                            
                            if (!RotationLocked)
                            {
                                _localRotation = Quaternion.Inverse(_parent.Transform._rotation) * _rotation;
                            }

                            // if value isn't null add this GameObject as child to new Parent
                            value.Transform.Children.Add(GameObject);

                            // change Transform rotation/position for all children and components hierarchy
                            value.Transform.ChangeTransformInChildren();

                            // the current transform's GameObject is the root, make itself its own root
                            if (value.Transform.Root == GameObject)
                            {
                                value.Transform.Root = value;
                            }

                            // set root of this transform GameObject to have the root of its new parent's root
                            Root = value.Transform.Root;
                        }

                        GameObject.Game = Root.Game;
                    }
                }
                else
                {
                    _parent = null;
                }
            }
        }

        /// <summary>
        /// The list of the GameObject's children
        /// </summary>
        public List<GameObject> Children { get => _children; }

        /// <summary>
        /// The position of the GameObject in the world space
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;

                if (_parent == null)
                {
                    _localPosition = value;
                }
                else
                {
                    _localPosition = Vector3.Transform(_position, Matrix.Invert(_parent.Transform.GetWorldMatrix()));
                }

                ChangeTransformInChildren();
            }
        }

        /// <summary>
        /// The position of the GameObject in local space
        /// </summary>
        public Vector3 LocalPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;

                if (Parent == null)
                {
                    _position = value;
                }
                else
                {
                    _position = (GetLocalMatrix() * Parent.Transform.GetWorldMatrix()).Translation;
                }

                ChangeTransformInChildren();
            }
        }

        /// <summary>
        /// The rotation of the GameObject in world space stored as quaternion
        /// </summary>
        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;

                if (_parent == null)
                {
                    _localRotation = value;
                }
                else
                {
                    _localRotation = Quaternion.Inverse(_parent.Transform._rotation) * _rotation;
                }

                ChangeTransformInChildren();
            }
        }

        /// <summary>
        /// The rotation of the GameObject in local space stored as quaternion
        /// </summary>
        public Quaternion LocalRotation
        {
            get => _localRotation;
            set
            {
                _localRotation = value;

                if (Parent == null)
                {
                    //ORIG
                    _rotation = _localRotation;

                }
                else
                {
                    _rotation = Parent.Transform.Rotation * _localRotation;
                }

                ChangeTransformInChildren();
            }
        }

        /// <summary>
        /// Change the position/rotation of children
        /// </summary>
        protected void ChangeTransformInChildren()
        {
            _right = null;
            _left = null;
            _up = null;
            _down = null;
            _forward = null;
            _backward = null;

            int childrenCount = _children.Count;

            for (int i = 0; i < childrenCount; i++)
            {
                _children[i].Transform._position = (_children[i].Transform.GetLocalMatrix() * GetWorldMatrix()).Translation;

                if (!_children[i].Transform.RotationLocked)
                {
                    _children[i].Transform._rotation = _rotation * _children[i].Transform.LocalRotation;
                }

                _children[i].Transform.ChangeTransformInChildren();
            }

            _transformChanged?.Invoke(this, null);
        }

        /// <summary>
        /// The position and rotation of the GameObject in local space stored as Matrix
        /// </summary>
        public Matrix GetLocalMatrix()
        {
            return Matrix.CreateScale(_scale) * Matrix.CreateFromQuaternion(_localRotation) * Matrix.CreateTranslation(_localPosition);
        }

        /// <summary>
        /// The position and rotation of the GameObject in world space stored as Matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(_scale)  * Matrix.CreateFromQuaternion(_rotation) * Matrix.CreateTranslation(_position);
        }

        // Default Constructor - set as internal
        internal Transform()
        {
            
        }
    }
}
