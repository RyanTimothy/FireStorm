/*  MenuEventHandler.cs
 *  Handles the on click and visible events of the Menu UI
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using RBeausoleilPROG2370FinalAssignment.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.ComponentControllers
{
    public class MenuEventHandler : ComponentController
    {
        public void NewGame_Clicked(object sender, EventArgs e)
        {
            Game.SetScene<Level01>();

            GameObject.SetActive(false);

            (Game as GameFireStorm).HealthBarUI.SetActive(true);
            FindObjectByType<TankController>().PlayerHealth = TankController.START_HEALTH;
        }


        public void Help_Clicked(object sender, EventArgs e)
        {
            if (sender is CanvasButton button)
            {
                (Game as GameFireStorm).MenuUI.SetActive(false);
                (Game as GameFireStorm).HelpMenuUI.SetActive(true);
            }
        }


        public void Credits_Clicked(object sender, EventArgs e)
        {
            if (sender is CanvasButton button)
            {
                (Game as GameFireStorm).MenuUI.SetActive(false);
                (Game as GameFireStorm).CreditsMenuUI.SetActive(true);
            }
        }


        public void Exit_Clicked(object sender, EventArgs e)
        {
            Game.Exit();
        }
        

        public void HelpMenu_Clicked(object sender, EventArgs e)
        {
            if (sender is CanvasButton button)
            {
                (Game as GameFireStorm).HelpMenuUI.SetActive(false);
                (Game as GameFireStorm).MenuUI.SetActive(true);
            }
        }


        public void Menu_VisibleChanged(object sender, EventArgs e)
        {
            if (sender is CanvasRenderer canvas)
            {
                (Game as GameFireStorm).GamePaused = canvas.Visible;
            }
        }


        internal void CreditsMenu_Clicked(object sender, EventArgs e)
        {
            if (sender is CanvasButton button)
            {
                (Game as GameFireStorm).CreditsMenuUI.SetActive(false);
                (Game as GameFireStorm).MenuUI.SetActive(true);
            }
        }
    }
}