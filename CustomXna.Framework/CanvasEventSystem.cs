/*  CanvasEventSystem.cs
 *  The Event System for the canvas - handles mouse input and button hover
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CustomXna.Framework
{
    public class CanvasEventSystem : ComponentController
    {
        private bool _isMouseVisible = true;
        private CanvasButton _hoveredButton;
        private MouseState prevMouseState;

        private CanvasButton HoveredButton
        {
            get => _hoveredButton;
            set
            {
                if (_hoveredButton != value && _hoveredButton != null)
                {
                    _hoveredButton.IsHovered = false;
                }

                if (_hoveredButton != value)
                {
                    _hoveredButton = value;

                    if (_hoveredButton != null)
                    {
                        _hoveredButton.IsHovered = true;
                    }
                }
            }
        }

        public bool IsMouseVisible
        {
            get => _isMouseVisible;
            set
            {
                _isMouseVisible = value;

                if (ActiveAndEnabled)
                {
                    Game.IsMouseVisible = _isMouseVisible;
                }
            }
        }

        public override void Start()
        {
            if (ActiveAndEnabled)
            {
                Game.IsMouseVisible = _isMouseVisible;
            }
        }

        protected override void OnEnable()
        {
            Game.IsMouseVisible = _isMouseVisible;

            prevMouseState = Mouse.GetState();
        }

        protected override void OnDisable()
        {
            Game.IsMouseVisible = false;
        }

        public override void Update(GameTime gameTime)
        {
            CanvasRenderer canvas = GameObject.GetComponent<CanvasRenderer>();

            if (canvas != null)
            {
                MouseState mouseState = Mouse.GetState();

                // get actual mouse position based on display width/height vs preferred back buffer
                float viewPortPositionX = (int)(((float)mouseState.X / (float)Game.Graphics.GraphicsDevice.DisplayMode.Width) * (float)Game.Graphics.PreferredBackBufferWidth);
                float viewPortPositionY = (int)(((float)mouseState.Y / (float)Game.Graphics.GraphicsDevice.DisplayMode.Height) * (float)Game.Graphics.PreferredBackBufferHeight);

                //unproject the cursor positions
                Vector3 cursorPos0 = Game.GraphicsDevice.Viewport.Unproject(new Vector3(viewPortPositionX, viewPortPositionY, 0), Game.Camera.Projection, Game.Camera.View, Matrix.Identity);
                Vector3 cursorPos1 = Game.GraphicsDevice.Viewport.Unproject(new Vector3(viewPortPositionX, viewPortPositionY, 1), Game.Camera.Projection, Game.Camera.View, Matrix.Identity);

                //set the ray of the cursor near position and its direction
                Ray ray = new Ray(cursorPos0, cursorPos1 - cursorPos0);

                //set the plane with the normal and distance to 0,0,0
                Plane plane = new Plane(Transform.Backward, -Vector3.Dot(Transform.Backward, Transform.Position));

                float? depth = ray.Intersects(plane);

                //if intersection with ray and plane has occurred
                if (depth != null)
                {
                    //transform the plane & ray hitpoint with the inverse of the Transform's rotation for axis aligned checking
                    Vector3 local = Vector3.Transform((cursorPos0 + (cursorPos1 - cursorPos0) * depth.Value) - Transform.Position, Quaternion.Inverse(Transform.Rotation));

                    //if hitpoint is within the canvas's world dimensions
                    if (local.X > -canvas.HalfExtents.X && local.X < canvas.HalfExtents.X && local.Y > -canvas.HalfExtents.Y && local.Y < canvas.HalfExtents.Y)
                    {
                        //get the pixel position of the canvas (0,0 at top left, 1,1 at bottom right)
                        Vector2 pixel = new Vector2(((local.X + canvas.HalfExtents.X) / canvas.Width) * canvas.BufferWidth, (1f - ((local.Y + canvas.HalfExtents.Y) / canvas.Height)) * canvas.BufferHeight);

                        //get the buttons within the gameobject
                        CanvasButton[] buttons = GameObject.GetComponents<CanvasButton>();

                        CanvasButton hoveredButton = null;

                        for (int i = buttons.Length - 1; i >= 0; i--)
                        {
                            //is pixel position within button's dimensions
                            if (buttons[i].Enabled && buttons[i].Visible
                                && pixel.X >= buttons[i].Position.X && pixel.X <= buttons[i].Position.X + buttons[i].Width 
                                && pixel.Y >= buttons[i].Position.Y && pixel.Y <= buttons[i].Position.Y + buttons[i].Height)
                            {
                                hoveredButton = buttons[i];

                                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != ButtonState.Pressed)
                                {
                                    buttons[i].Click();
                                }

                                break;
                            }
                        }

                        //assign the hovered button
                        HoveredButton = hoveredButton;
                    }
                }

                prevMouseState = mouseState;
            }
        }
    }
}