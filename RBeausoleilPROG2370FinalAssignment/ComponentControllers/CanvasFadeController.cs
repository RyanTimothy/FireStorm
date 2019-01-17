/*  CanvasFadeController.cs
 *  Handles the Fade In/Out of the canvas renderer
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.ComponentControllers
{
    public class CanvasFadeController : ComponentController
    {
        private CanvasRenderer canvasRenderer;
        private bool hiding;
        private bool showing;
        private float autoHideDuration;

        public float HideDuration { get; set; } = 1f;
        public float ShowDuration { get; set; } = 1f;

        public bool IsVisible { get; private set; }

        public override void Start()
        {
            canvasRenderer = GameObject.GetComponent<CanvasRenderer>();

            Enabled = false;
        }

        /// <summary>
        /// Start the Fade Out sequence
        /// </summary>
        public void Hide()
        {
            hiding = true;
            showing = false;

            Enabled = true;
            IsVisible = false;
        }

        /// <summary>
        /// Start the Fade In sequence
        /// </summary>
        public void Show()
        {
            autoHideDuration = -1;

            if (!GameObject.ActiveInGame)
            {
                GameObject.SetActive(true);
            }

            showing = true;
            hiding = false;

            Enabled = true;
            IsVisible = true;
        }

        /// <summary>
        /// Start the Fade In sequence and setting an Auto Hide duration
        /// </summary>
        public void Show(float autoHideDuration)
        {
            Show();

            this.autoHideDuration = autoHideDuration;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (hiding || showing)
            {

                if (hiding)
                {
                    if (canvasRenderer.MasterAlpha - deltaTime / HideDuration <= 0)
                    {
                        hiding = false;
                        Enabled = false;
                    }

                    canvasRenderer.MasterAlpha -= deltaTime / HideDuration;
                }
                else
                {
                    if (canvasRenderer.MasterAlpha + deltaTime / ShowDuration >= 1)
                    {
                        showing = false;

                        if (autoHideDuration <= 0)
                        {
                            Enabled = false;
                        }
                    }

                    canvasRenderer.MasterAlpha += deltaTime / ShowDuration;
                }
            }
            else
            {
                autoHideDuration -= deltaTime;

                if (autoHideDuration <= 0)
                {
                    Hide();
                }
            }
        }
    }
}