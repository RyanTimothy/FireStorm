/*  HealthBarController.cs
 *  Handles the update of the healthbar's visuals
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
    public class HealthBarController : ComponentController
    {
        private float healthSpriteWidth;
        private bool flashing;
        private float flashInterval;
        private Color color;

        private CanvasFadeController healthBarFadeController;

        public CanvasSprite HealthSprite { get; set; }

        public override void Start()
        {
            healthSpriteWidth = HealthSprite.Width;

            healthBarFadeController = GameObject.GetComponent<CanvasFadeController>() as CanvasFadeController;
        }

        /// <summary>
        /// Adjust the health bar meter - health and max health
        /// </summary>
        public void AdjustHealthBar(int health, int healthMax)
        {
            if (health > 0)
            {
                if (!healthBarFadeController.IsVisible)
                {
                    healthBarFadeController.Show();
                }
            }
            else if (healthBarFadeController.IsVisible)
            {
                healthBarFadeController.Hide();
            }

            HealthSprite.Width = healthSpriteWidth * ((float)health / (float)healthMax);

            color = Color.Lerp(Color.DarkRed, Color.White, (float)health / ((float)healthMax * 0.7f));
            HealthSprite.Color = color;

            if (!flashing && health < 8)
            {
                flashing = true;
                flashInterval = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (flashing)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                flashInterval += deltaTime * 3;

                HealthSprite.Color = Color.Lerp(Color.White, color, (flashInterval % 1) * 1.5f);
            }
        }
    }
}