/*  Level01.cs
 *  The level01 blue prints
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.07: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RBeausoleilPROG2370FinalAssignment.ComponentControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.Scenes
{
    public class Level01 : Scene
    {
        public override void LoadContent()
        {
            GameObject camera = FindGameObjectByTag((int)GameObjectTag.Camera);
            camera.Transform.Position = new Vector3(0.0f, -25.0f, 60.0f);

            CameraViewController cameraViewController = FindObjectByType<CameraViewController>();
            if (cameraViewController != null)
            {
                cameraViewController.Enabled = true;
            }

            GameObject playerBase = GameObjectPrefabManager.InstantiateType(PrefabType.Base, Game);
            playerBase.Transform.Position = new Vector3(67, -107, 0);
            (playerBase.GetComponent<BaseController>() as BaseController).SpawnRotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), MathHelper.ToRadians(90.0f), MathHelper.ToRadians(90.0f));

            GameObject terrain = GameObject.Instantiate(Game);
            TerrainTextureGrid2D terrainComponent = terrain.AddComponent<TerrainTextureGrid2D>() as TerrainTextureGrid2D;
            FindObjectByType<TankController>().TerrainTextureGrid2D = terrainComponent;
            terrainComponent.Texture = Game.Content.Load<Texture2D>("tiles");
            terrain.Transform.Scale = 2;
            terrain.Transform.Position = new Vector3(0, 0, 0);
            terrainComponent.TextureSetX = 8;
            terrainComponent.TextureSetY = 8;
            terrainComponent.SetGrid(new ushort?[,]
            {
                { 45,44,37,44,37,37,37,36,44,44,36,36,36,45,37,45,45,36,36,44,44,45,44,37,36,37,36,45,36,36,44,37,45,36,36,36,44,44,44,44,37,44,36,36,45,44,44,36,45,44,45,36,44,36,44,36,45,44,44,36,44,37,37,44 },
                { 45,37,45,44,37,44,36,36,36,36,36,45,36,36,44,37,37,45,37,37,45,37,37,37,36,45,37,44,44,37,37,37,36,44,45,37,36,37,45,37,37,44,44,44,37,36,36,45,44,37,45,36,45,44,37,44,45,45,37,37,44,37,36,36 },
                { 45,44,36,45,44,36,36,45,37,45,44,37,45,45,37,37,45,37,45,44,37,36,45,37,44,37,44,36,45,37,45,37,45,45,44,36,44,45,37,37,37,44,44,37,37,45,36,44,36,44,44,45,36,44,37,36,36,37,44,36,45,36,44,45 },
                { 37,44,45,45,45,44,37,44,37,45,44,45,44,36,36,45,36,45,54,60,61,60,60,61,61,61,60,55,44,45,37,36,37,36,44,45,44,44,44,45,45,45,36,37,36,45,37,37,37,44,36,37,36,45,37,44,44,45,44,45,44,44,36,37 },
                { 36,36,45,36,44,45,36,36,36,45,37,45,37,54,60,60,60,60,57,5,4,4,4,6,4,7,4,56,60,55,37,37,45,37,44,37,37,44,37,44,44,36,44,37,44,44,44,36,37,37,36,45,44,44,45,37,44,37,44,37,37,36,36,37 },
                { 45,37,37,45,36,37,45,37,37,37,44,54,60,57,6,5,7,7,4,5,22,9,8,9,16,4,5,7,6,56,60,55,36,45,44,44,36,44,37,44,45,44,45,36,37,37,37,45,36,44,36,36,37,36,44,44,45,45,37,45,36,37,37,44 },
                { 36,45,44,45,37,37,45,45,37,45,54,57,7,4,4,6,22,8,8,9,25,3,2,1,34,8,17,6,4,6,4,56,61,55,36,44,36,36,45,36,45,36,44,45,37,44,36,44,36,36,44,45,36,37,37,37,36,36,44,37,36,37,37,36 },
                { 36,36,44,45,37,37,44,36,54,61,57,5,5,6,23,9,25,1,2,0,0,2,0,2,2,1,42,9,16,6,5,7,6,56,61,55,45,37,37,37,36,54,61,60,60,61,61,55,36,37,36,36,45,37,37,36,44,44,37,37,44,37,45,37 },
                { 36,37,45,36,37,36,44,54,57,5,6,6,7,23,33,3,3,2,3,2,3,1,0,3,2,0,3,3,34,9,17,5,6,6,6,56,55,45,37,44,37,59,6,5,6,4,7,58,37,54,60,60,61,55,36,45,37,44,36,44,36,37,36,45 },
                { 37,36,37,37,36,45,54,57,5,6,5,7,23,33,0,2,1,0,2,3,1,1,0,3,2,1,3,0,3,0,42,9,16,5,7,6,56,60,55,37,37,62,53,49,7,7,48,63,54,57,5,7,5,56,55,36,44,37,36,45,36,36,44,36 },
                { 37,36,36,44,36,36,51,7,6,6,4,22,41,0,0,1,0,0,0,3,2,2,2,3,0,3,3,1,3,1,1,1,34,8,17,7,6,5,56,61,55,45,44,62,49,48,63,54,57,6,5,4,5,5,56,55,36,45,37,44,37,36,36,45 },
                { 36,37,45,37,44,45,59,5,5,4,22,41,1,1,1,0,0,0,2,2,3,1,1,2,3,2,3,3,2,0,2,3,1,1,42,16,6,7,7,6,56,60,60,55,62,63,54,57,4,5,4,4,7,5,6,56,55,45,45,36,36,37,45,44 },
                { 45,37,37,37,44,36,51,7,5,6,15,1,2,0,2,3,3,0,3,0,3,3,0,0,0,2,3,1,1,0,3,3,3,1,0,42,8,16,7,6,6,5,7,56,61,60,57,6,4,6,23,9,9,17,6,5,56,55,44,45,37,37,37,37 },
                { 44,45,36,44,44,45,51,6,6,6,14,0,2,2,0,0,0,1,3,1,2,0,0,2,2,2,3,3,2,3,1,1,0,1,3,0,2,26,16,6,4,5,6,5,6,6,4,6,6,22,41,1,1,42,17,7,4,58,36,36,45,36,45,36 },
                { 37,36,36,44,36,44,59,7,5,22,41,0,2,0,3,3,3,0,3,1,1,1,1,0,1,0,2,2,3,3,1,2,0,2,1,2,0,1,11,6,4,5,5,5,6,4,5,7,6,15,1,3,2,1,10,7,5,58,36,37,45,44,36,44 },
                { 36,45,45,37,45,44,59,5,7,15,1,2,3,0,3,3,1,2,2,0,3,2,2,3,0,1,3,2,3,2,0,1,1,3,2,3,1,1,26,17,6,6,7,6,4,6,4,7,23,25,1,1,1,0,11,4,4,56,55,37,45,37,36,45 },
                { 45,44,37,36,45,44,62,49,4,14,0,1,3,1,2,0,2,3,0,3,2,3,2,2,3,2,3,1,1,1,3,3,2,1,0,2,3,1,2,34,17,7,7,7,7,7,4,4,14,2,0,0,3,3,42,16,5,6,50,44,44,37,45,45 },
                { 45,44,44,45,37,45,36,51,6,14,3,3,2,2,0,3,0,1,3,1,2,1,2,1,0,0,3,3,0,1,2,3,0,1,1,1,2,3,0,0,42,17,7,4,6,7,6,22,41,3,1,2,3,0,0,10,6,5,56,55,36,37,45,37 },
                { 37,36,36,44,44,44,37,59,6,20,24,2,1,3,3,3,0,2,2,1,3,2,0,1,0,1,3,1,2,1,1,0,1,0,0,0,1,2,1,0,2,26,8,9,9,16,4,14,3,2,3,2,3,1,3,10,4,6,5,58,36,37,45,44 },
                { 37,44,37,36,45,45,45,51,5,4,15,0,1,3,2,2,1,0,2,3,0,3,0,0,3,0,0,3,1,2,3,3,0,2,0,1,2,3,0,1,0,3,2,0,2,34,9,33,2,2,0,3,2,3,0,26,16,4,6,58,45,45,36,36 },
                { 44,44,45,36,37,37,54,57,6,7,14,3,2,1,0,3,3,2,3,1,1,1,3,0,2,0,0,0,0,0,0,1,2,1,0,3,3,1,2,3,2,1,0,1,2,1,3,2,0,1,0,0,1,3,2,0,10,5,7,58,37,36,36,37 },
                { 36,37,44,44,37,44,51,5,4,6,15,0,1,0,3,0,1,1,0,2,1,1,0,1,0,2,1,2,1,3,1,0,1,1,3,1,1,1,3,2,0,2,0,2,3,1,2,3,3,2,2,0,2,3,0,1,10,6,5,50,44,37,45,37 },
                { 44,37,36,45,36,44,59,7,6,5,21,24,1,2,2,3,3,1,3,0,0,2,2,1,2,0,2,0,2,1,2,0,2,0,0,2,0,3,0,2,2,3,0,2,2,1,3,1,2,0,0,1,1,3,2,0,11,6,4,50,44,45,45,36 },
                { 45,36,45,36,44,44,59,6,7,6,22,41,3,3,3,1,2,0,3,1,3,3,2,2,1,0,3,2,3,3,0,1,1,1,2,2,3,3,3,3,2,1,1,1,0,2,3,3,3,2,1,0,2,2,2,0,11,6,6,58,37,45,36,44 },
                { 45,45,37,45,36,54,57,7,6,22,25,2,2,1,1,0,2,1,2,1,3,3,1,0,3,3,0,3,2,1,3,1,2,2,0,3,3,0,3,0,2,2,3,0,2,1,0,2,0,2,1,3,0,0,3,1,11,6,6,50,37,45,36,44 },
                { 37,44,45,36,45,51,4,5,22,33,1,2,3,2,1,2,2,2,3,0,0,2,2,1,3,1,0,1,1,0,1,3,3,3,1,2,1,1,2,2,0,3,2,2,0,0,0,1,2,1,3,2,2,2,2,2,10,5,4,58,37,44,36,36 },
                { 37,44,45,37,45,59,5,4,15,0,1,3,2,2,0,1,2,3,2,2,2,1,3,3,0,2,1,3,1,2,3,0,3,1,2,2,2,3,0,3,2,2,3,1,3,2,3,1,0,3,2,0,0,2,2,43,19,7,4,50,36,44,45,36 },
                { 36,36,45,37,37,59,5,7,14,0,0,1,3,0,0,0,3,2,1,1,3,0,0,3,2,0,1,0,0,3,0,2,3,3,2,0,2,3,0,2,3,1,1,0,0,0,1,1,3,0,0,0,1,2,1,11,6,7,6,50,37,45,44,37 },
                { 36,36,36,36,45,59,4,7,14,1,2,3,0,0,1,1,3,2,2,0,1,2,1,0,1,3,3,1,2,1,3,1,1,3,0,2,2,0,1,1,1,3,1,1,2,0,0,1,2,0,0,2,1,1,0,10,7,5,48,63,36,45,45,37 },
                { 44,45,37,36,44,51,5,6,15,1,3,3,0,0,2,2,2,1,3,3,1,1,3,0,3,3,1,1,3,3,0,3,1,0,3,0,3,1,1,2,1,3,2,2,1,3,0,2,3,1,3,3,1,1,27,18,7,6,50,36,45,45,45,36 },
                { 44,44,45,36,45,62,49,5,21,40,2,0,3,0,1,1,0,0,3,0,0,3,3,1,1,2,3,1,1,2,1,0,2,2,3,2,2,3,0,3,1,2,0,0,1,2,2,1,3,3,0,0,3,3,11,5,5,5,58,36,45,44,36,37 },
                { 36,36,44,36,44,45,59,7,4,20,12,32,0,2,2,2,3,0,3,1,2,3,3,0,3,3,2,0,3,1,0,2,1,2,0,1,0,2,1,1,0,1,3,1,0,2,3,3,0,3,1,1,27,13,19,5,4,48,63,36,36,36,36,44 },
                { 37,36,45,45,44,45,62,49,7,4,5,21,24,1,1,1,2,1,3,3,0,3,0,3,3,0,3,3,3,0,3,1,1,0,3,1,1,3,3,1,1,0,0,2,0,0,3,3,1,1,35,12,19,7,4,5,48,63,45,37,45,44,45,37 },
                { 36,37,44,44,45,44,45,59,7,5,7,6,21,32,3,3,0,0,2,3,3,2,0,3,0,2,2,2,3,0,2,0,2,0,3,0,3,1,0,2,2,3,0,0,0,0,2,1,1,27,18,7,6,7,4,48,63,45,44,37,45,36,36,44 },
                { 37,36,45,45,45,45,44,62,49,5,6,5,7,20,24,2,3,0,2,1,0,2,3,3,0,2,3,0,2,0,3,1,3,0,0,0,2,0,2,1,2,0,2,1,2,2,1,1,27,18,6,5,5,48,53,63,37,36,44,45,44,36,44,36 },
                { 45,44,36,44,45,44,44,44,62,53,49,7,5,5,20,24,3,2,3,3,2,3,3,0,2,3,0,3,0,0,3,3,1,2,2,0,1,0,2,1,2,0,1,0,2,1,2,43,19,5,4,5,48,63,45,36,37,36,37,37,44,37,44,36 },
                { 36,37,45,36,44,44,44,36,44,44,62,53,49,7,4,20,40,2,0,1,2,3,2,3,0,3,2,0,3,1,1,1,0,2,1,0,1,3,1,1,1,3,1,2,0,2,27,18,4,5,4,48,63,44,36,37,36,45,45,37,37,45,45,37 },
                { 44,36,37,44,44,45,44,37,44,44,36,37,62,49,7,5,21,40,2,3,2,2,0,0,1,2,3,2,1,2,1,2,1,2,3,0,0,1,3,2,1,2,1,2,0,2,10,7,7,5,6,50,45,36,54,60,55,45,37,45,36,37,37,36 },
                { 44,44,37,44,37,37,36,36,44,36,45,45,36,59,4,4,6,21,24,2,2,0,1,0,3,1,2,1,3,3,1,1,3,1,3,0,3,3,2,3,2,1,1,3,2,35,18,4,4,7,48,63,44,54,57,4,56,55,37,36,44,37,45,36 },
                { 36,44,44,37,45,36,37,36,45,36,37,36,37,62,49,6,5,4,14,0,1,1,1,3,1,1,3,0,2,2,0,0,1,1,0,0,3,1,0,2,1,3,2,1,2,11,5,4,7,4,50,36,36,59,5,6,6,58,45,37,45,44,44,44 },
                { 36,37,45,36,45,44,36,44,36,44,36,44,45,36,62,49,4,4,14,1,2,3,0,0,0,2,3,3,0,0,2,2,2,3,3,1,0,1,3,0,0,0,2,2,2,11,5,4,6,48,63,36,44,59,5,4,6,50,36,37,44,44,37,44 },
                { 37,45,37,37,37,45,37,36,37,45,37,45,44,45,45,59,5,5,20,32,0,1,0,1,0,0,0,3,0,2,2,35,12,13,12,40,0,3,2,2,1,3,0,3,3,10,4,7,7,58,37,45,37,62,49,5,48,63,36,45,36,37,44,36 },
                { 45,45,36,36,37,36,44,44,37,37,45,54,61,55,44,59,5,7,5,15,3,0,3,2,3,2,2,1,1,43,13,19,4,7,5,20,24,2,3,3,2,1,2,3,1,11,6,4,7,50,37,44,45,37,62,52,63,37,37,37,37,45,44,45 },
                { 44,37,44,37,36,45,45,37,37,45,44,51,6,50,44,59,4,5,4,14,2,3,3,0,0,1,0,27,12,18,7,4,5,4,4,5,20,32,0,1,2,2,0,3,0,11,5,5,48,63,37,45,44,36,36,37,44,37,36,36,37,45,44,44 },
                { 45,36,36,36,36,45,45,45,45,45,45,59,4,56,55,62,49,4,5,15,3,2,0,0,2,3,0,10,6,7,7,7,4,5,7,4,23,25,2,1,1,2,1,2,1,10,4,7,58,44,45,44,37,37,44,37,36,45,44,36,44,45,37,37 },
                { 45,45,36,36,44,45,45,36,44,37,37,62,49,5,58,37,59,5,6,14,1,2,2,2,0,0,0,34,16,6,6,5,7,7,7,23,33,1,1,3,3,1,0,2,27,18,7,4,50,36,37,36,37,37,37,37,37,44,37,37,45,44,36,45 },
                { 45,37,44,37,37,36,36,36,44,44,36,45,62,53,63,44,59,7,4,21,40,2,0,2,3,3,2,2,42,9,9,8,9,8,9,41,2,2,2,2,3,2,1,1,11,6,4,7,50,45,44,36,45,37,37,37,37,36,45,44,36,44,37,36 },
                { 45,36,45,36,36,37,36,45,36,36,44,44,37,37,44,36,62,49,6,5,15,1,1,0,3,3,1,2,2,1,1,1,0,1,1,2,1,2,1,3,2,3,1,2,10,5,6,4,50,36,36,37,36,44,36,37,45,45,37,36,44,45,37,37 },
                { 36,36,37,45,44,37,37,37,44,45,37,45,37,36,36,44,44,59,7,5,20,32,2,3,0,1,1,3,1,2,1,0,0,2,2,0,2,1,0,2,0,1,1,2,10,6,6,48,63,45,36,44,36,37,37,45,45,36,36,36,45,45,36,45 },
                { 37,44,36,36,36,36,44,37,37,36,36,37,36,45,44,36,37,62,49,6,5,21,40,3,2,1,2,1,0,2,2,3,2,2,0,0,0,3,3,2,2,3,1,2,10,6,5,58,44,37,36,44,36,44,44,45,45,44,45,44,36,45,44,36 },
                { 36,36,36,45,36,36,37,45,44,37,37,36,37,44,36,37,45,45,62,49,6,7,20,24,3,2,2,1,2,1,0,0,3,2,1,1,2,1,3,2,0,0,2,27,19,4,5,58,36,44,45,36,45,37,37,44,45,37,37,44,44,44,44,45 },
                { 36,45,37,44,36,37,44,44,36,45,45,45,37,36,36,37,37,44,45,62,49,4,4,15,0,1,1,3,2,2,2,1,3,2,2,3,2,3,1,1,3,1,35,19,6,7,48,63,45,44,44,37,36,36,37,44,36,44,36,37,37,44,45,45 },
                { 44,45,44,36,45,37,37,45,45,37,44,36,44,36,37,44,45,36,44,36,51,5,4,21,32,0,1,1,0,0,1,2,null,null,null,0,3,3,0,3,3,3,10,6,5,7,58,37,44,45,44,37,36,37,37,37,44,36,36,44,36,36,45,36 },
                { 37,37,44,37,45,45,44,36,36,36,37,36,45,44,36,45,44,36,37,45,62,49,6,7,21,24,2,2,0,0,3,3,null,null,null,0,1,1,3,2,2,27,19,7,4,48,63,36,44,44,45,45,44,44,44,36,45,45,45,36,36,44,36,36 },
                { 37,45,44,37,44,37,36,37,37,36,37,45,37,37,37,37,37,44,45,36,36,62,49,7,6,21,12,40,2,0,2,0,null,null,null,3,0,3,1,0,27,19,7,7,5,58,37,44,37,45,36,37,44,44,36,44,37,36,44,36,36,36,37,45 },
                { 44,36,45,45,36,44,45,37,36,44,36,44,44,45,44,36,45,45,36,37,36,45,62,49,6,5,5,20,12,40,3,1,3,2,2,1,2,0,2,43,18,5,5,7,48,63,37,36,45,45,37,36,45,44,44,44,37,44,44,37,45,45,37,36 },
                { 36,45,44,44,37,45,44,37,37,36,44,45,36,36,36,44,36,37,44,37,37,45,45,62,49,7,4,4,6,21,12,13,13,12,13,12,13,13,13,19,6,6,7,48,63,45,44,44,44,45,37,36,45,45,45,37,44,44,36,37,36,37,37,36 },
                { 36,37,37,45,45,37,37,44,36,36,37,44,45,37,37,44,44,44,44,44,45,45,36,37,62,53,52,49,5,5,7,5,7,4,7,4,5,6,6,5,7,48,53,63,36,44,37,45,36,36,44,36,36,45,45,37,44,37,44,45,44,36,44,36 },
                { 44,45,37,45,44,36,45,45,37,37,44,44,44,44,44,37,37,45,44,44,45,36,45,44,44,37,37,62,52,53,53,52,53,52,53,52,53,52,53,52,52,63,44,36,45,36,45,45,36,37,36,44,45,36,37,36,37,36,45,45,36,44,36,44 },
                { 44,37,36,36,45,45,45,36,45,37,36,36,44,45,37,37,44,36,45,45,44,45,45,44,36,45,44,45,37,36,45,45,44,44,36,45,44,44,36,44,37,44,36,45,36,36,45,36,44,44,37,37,44,37,36,44,36,37,36,36,45,44,45,37 },
                { 37,45,44,44,36,45,36,37,36,45,45,45,44,45,45,36,37,36,37,45,37,36,45,37,45,44,36,37,45,37,44,36,44,36,37,44,36,45,45,36,44,45,44,44,44,45,36,45,37,44,44,37,45,45,44,44,37,36,44,44,36,44,37,37 },
                { 45,45,44,44,45,44,44,44,36,37,37,36,36,36,44,36,36,44,45,37,36,36,45,44,45,37,45,37,44,44,44,37,36,44,36,36,44,45,44,45,36,37,37,44,37,45,44,36,37,45,37,36,44,36,44,36,37,44,36,37,44,44,44,45 },
                { 36,36,37,45,44,37,36,37,36,44,37,36,36,45,45,45,36,36,44,44,44,44,45,44,44,44,36,36,36,36,44,45,36,44,45,44,45,36,36,45,37,37,37,36,44,44,36,37,36,45,44,37,36,36,37,37,45,37,45,45,37,37,36,36 },
                { 37,37,44,36,44,37,37,44,37,45,36,44,44,36,37,37,36,36,45,37,36,44,45,44,45,37,45,37,44,37,37,45,36,37,37,45,44,37,44,37,37,37,44,44,37,44,45,44,36,37,37,36,44,45,36,37,37,45,45,44,36,44,36,45 }
            });

            // Tower ----------------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(21, -57, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(37, -57, 0));
            // Door here
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(47, -57, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(63, -57, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(63, -41, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(63, -25, 0));

            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(105, -31, 0));

            // Wall Horizontal ------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(25, -57, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(29, -57, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(33, -57, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(51, -57, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(55, -57, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(59, -57, 0)); 

            // Wall Vertical --------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(63, -53, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(63, -49, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(63, -45, 0));

            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(63, -37, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(63, -33, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(63, -29, 0));

            // Lookout Tower --------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.LookoutTower, Game, new Vector3(23, -49, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.LookoutTower, Game, new Vector3(23, -25, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.LookoutTower, Game, new Vector3(47, -13, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.LookoutTower, Game, new Vector3(89, -39, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.LookoutTower, Game, new Vector3(61, -83, 0));

            // Tree Single ----------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(55, -93, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(83, -105, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(77, -89, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(41, -91, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(57, -109, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(91, -75, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(31, -69, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(41, -77, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(73, -71, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(55, -67, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(39, -81, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(53, -89, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(73, -83, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(81, -45, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(95, -41, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(101, -61, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(111, -43, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(85, -51, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(21, -35, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(55, -17, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(73, -27, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(109, -35, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(53, -71, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(85, -39, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(31, -39, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(57, -31, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(57, -22.5f, 0));

            // Tree Double ----------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(69, -43, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(77, -111, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(55, -107, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(55, -85, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(37, -75, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(87, -49, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(79, -35, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(71, -65, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(47, -99, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(43, -91, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(85, -101, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(75, -91, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(89, -73, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(107, -53, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(87, -83, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(97, -67, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(25, -63, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(63, -95, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(57, -69, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(25, -23, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(97, -37, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(63, -81, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(103, -63, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(20, -51, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(63, -21, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(101, -27, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(89, -89, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(35, -15, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(53, -73, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(33, -41, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(34, -26, 0));

            // Building A ----------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.BuildingA, Game, new Vector3(27, -40, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.BuildingA, Game, new Vector3(27, -45, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.BuildingA, Game, new Vector3(32, -45, 0));

            // Hospital ------------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.Hospital, Game, new Vector3(54, -30, 0));

            // Hanger --------------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.Hanger, Game, new Vector3(31, -24, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.Hanger, Game, new Vector3(55, -20, 0), Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), 0, MathHelper.ToRadians(90.0f)));
            GameObjectPrefabManager.InstantiateType(PrefabType.Hanger, Game, new Vector3(55, -25, 0), Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), 0, MathHelper.ToRadians(90.0f)));
        }

        public override void Start()
        {
            
        }

        public override void UnloadContent()
        {
            FindGameObjectByTag((int)GameObjectTag.Player).SetActive(false);

            (Game as GameFireStorm).GameMessageUI.SetActive(false);
        }
    }
}
