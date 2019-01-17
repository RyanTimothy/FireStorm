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
    public class Level02 : Scene
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
            playerBase.Transform.Position = new Vector3(91, -113, 0);
            (playerBase.GetComponent<BaseController>() as BaseController).SpawnRotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), MathHelper.ToRadians(90.0f), MathHelper.ToRadians(90.0f));


            GameObject terrain = GameObject.Instantiate(Game);
            TerrainTextureGrid2D terrainComponent = terrain.AddComponent<TerrainTextureGrid2D>() as TerrainTextureGrid2D;
            FindObjectByType<TankController>().TerrainTextureGrid2D = terrainComponent;
            terrainComponent.Texture = Game.Content.Load<Texture2D>("tiles");
            terrain.Transform.Scale = 2;
            terrain.Transform.Position = new Vector3(0, 0, 0);
            terrainComponent.TextureSetX = 8;
            terrainComponent.TextureSetY = 8;
            terrainComponent.SetGrid( new ushort?[,]
            {
                { 44,45,44,44,36,37,44,36,45,44,37,45,45,45,45,45,45,44,45,36,44,37,36,45,36,37,36,45,37,36,44,45,44,44,37,44,45,45,45,45,44,37,37,37,45,44,44,45,37,44,44,37,45,45,37,45,36,44,44,45,36,37,36,45 },
                { 36,44,36,45,44,36,36,36,44,36,44,44,44,36,36,45,36,44,44,44,36,45,36,36,44,37,36,36,36,45,36,44,36,37,37,37,37,37,45,44,45,37,37,37,44,37,37,44,44,37,37,37,37,37,45,44,44,44,45,36,44,45,45,37 },
                { 45,36,45,36,45,37,37,37,37,37,44,37,44,45,45,44,44,44,44,37,44,44,37,37,44,45,36,45,45,37,36,37,45,37,44,44,36,44,44,36,45,44,37,45,45,44,37,36,45,36,44,36,37,44,36,44,36,36,36,45,44,44,45,36 },
                { 44,36,44,44,36,37,36,37,44,36,45,36,44,45,37,37,45,44,44,45,44,45,44,37,44,36,36,37,36,44,45,37,36,44,36,45,36,37,45,45,36,36,44,44,36,36,37,37,37,37,45,37,36,45,36,44,44,44,45,44,37,37,36,37 },
                { 44,44,37,37,44,44,36,44,45,45,36,36,36,36,44,45,44,45,45,36,44,44,36,37,37,45,36,44,45,37,36,44,37,44,36,36,36,45,45,37,44,37,36,45,37,45,45,37,45,45,36,37,37,37,36,37,37,36,44,37,44,36,44,37 },
                { 36,44,45,37,44,37,36,37,45,45,44,37,36,36,45,37,36,36,44,44,44,44,36,45,36,37,45,36,45,44,44,36,54,60,61,61,61,60,61,60,61,61,60,55,44,37,45,36,44,44,45,37,36,44,44,36,36,45,45,45,45,44,45,36 },
                { 45,44,45,45,44,36,44,37,45,45,44,44,37,45,37,45,45,36,44,37,37,45,37,37,44,36,54,61,61,60,60,60,57,7,4,7,4,7,6,6,7,4,5,56,60,61,61,61,61,61,55,45,37,45,44,45,36,44,44,45,44,45,44,36 },
                { 45,44,37,37,37,44,37,36,36,44,36,36,45,37,45,45,36,36,44,37,36,37,54,60,60,61,57,6,5,6,5,23,9,8,17,7,23,9,9,8,8,9,16,6,4,5,5,7,6,7,56,60,55,45,44,36,45,45,37,36,44,45,37,36 },
                { 37,36,44,37,36,37,37,36,45,37,36,44,36,36,45,45,36,37,37,37,54,60,57,7,6,4,22,9,9,9,8,33,3,3,42,9,25,3,3,0,2,3,42,9,9,8,8,8,8,8,17,6,56,60,55,44,37,37,45,45,37,45,45,36 },
                { 36,45,44,44,45,45,37,36,45,44,37,44,36,37,45,36,44,37,54,61,57,6,7,6,23,9,41,1,3,3,3,2,1,0,3,2,0,0,2,1,2,3,3,1,1,1,3,2,2,2,34,8,17,6,56,61,55,37,44,37,37,36,45,37 },
                { 44,37,45,45,44,36,37,36,36,45,37,45,45,37,37,45,54,61,57,5,23,9,8,8,25,3,0,3,3,1,3,0,2,1,2,2,2,3,0,1,3,0,0,3,1,2,2,1,3,0,1,0,42,8,17,4,56,60,55,37,44,45,45,44 },
                { 37,44,36,36,37,37,45,45,37,37,45,36,37,36,36,54,57,6,4,22,25,0,2,2,2,1,0,1,1,1,1,3,1,1,3,2,2,3,1,2,1,2,3,1,0,0,1,0,2,0,2,2,3,0,11,7,6,4,56,55,44,36,45,45 },
                { 36,45,45,36,45,37,36,36,45,36,37,45,44,44,54,57,7,4,22,41,0,2,2,2,1,0,3,3,2,0,1,0,1,0,2,0,2,3,2,0,2,1,3,3,2,1,1,3,1,3,3,0,2,2,34,16,6,6,4,56,55,44,45,37 },
                { 36,45,37,37,37,36,37,45,45,45,44,45,37,44,51,7,7,23,25,0,0,2,1,1,3,3,1,0,0,1,2,0,3,1,3,0,2,1,3,3,1,0,1,0,0,1,1,2,2,2,1,2,2,1,1,26,16,7,6,6,50,36,45,45 },
                { 36,37,44,36,44,37,45,37,44,37,44,45,44,45,51,6,5,14,0,0,0,2,0,0,3,2,0,1,3,1,2,3,3,0,2,3,3,2,0,1,0,2,2,1,1,0,2,2,0,0,0,0,1,1,1,2,11,6,6,4,58,37,37,36 },
                { 45,37,44,37,45,37,44,37,45,37,44,44,37,44,59,7,6,14,1,0,1,3,3,1,3,3,3,1,0,2,3,0,1,2,3,3,0,0,1,2,0,2,2,0,2,3,3,0,3,1,0,2,1,3,3,3,26,16,6,5,50,36,45,44 },
                { 37,36,37,45,44,44,36,44,36,45,45,36,44,37,62,49,5,14,3,1,0,0,2,2,2,3,1,0,0,2,3,1,0,3,3,0,3,3,0,0,2,3,1,0,2,0,0,0,0,3,0,0,0,1,0,3,3,10,6,6,50,45,36,36 },
                { 36,37,37,36,37,36,37,44,45,44,44,44,44,37,45,59,4,15,3,3,0,1,0,1,3,43,12,13,12,12,40,3,2,0,1,0,1,1,3,2,0,0,2,1,2,1,1,2,3,1,3,1,3,0,2,3,0,10,7,7,58,44,44,45 },
                { 36,45,44,44,45,44,44,37,37,45,44,36,36,36,45,51,5,20,32,0,2,3,3,35,13,19,4,4,7,5,21,13,40,0,3,2,3,3,0,1,1,1,3,2,2,2,0,2,0,3,2,0,3,1,1,2,1,10,7,5,50,44,36,45 },
                { 36,45,44,36,37,36,44,37,45,36,44,44,36,36,44,51,6,6,21,12,13,12,13,18,5,6,48,52,53,49,5,4,21,40,2,2,1,2,1,3,0,0,1,2,1,0,0,2,3,2,3,3,2,2,3,3,1,11,5,7,50,37,37,45 },
                { 44,37,36,45,45,36,45,37,45,36,36,36,44,36,45,51,6,4,4,6,6,5,7,6,5,48,63,45,44,62,53,49,4,20,32,0,2,1,0,0,0,0,1,2,0,3,3,1,2,0,3,1,0,3,0,1,1,10,7,48,63,45,45,44 },
                { 44,36,44,44,37,36,37,45,36,36,44,36,44,36,54,57,4,6,7,7,7,4,6,4,4,50,45,36,37,45,37,62,49,5,20,40,3,0,0,3,0,2,1,2,0,0,1,0,2,1,3,1,0,3,0,3,3,10,5,50,37,44,37,44 },
                { 37,37,37,36,45,37,37,36,36,44,44,37,36,45,51,4,4,4,7,7,7,5,4,4,48,63,44,36,45,37,44,37,62,49,4,21,24,1,1,0,1,1,0,3,3,2,1,1,0,2,0,2,3,0,2,2,0,11,5,58,45,44,37,44 },
                { 45,36,44,37,44,44,36,37,36,45,44,45,44,37,51,5,4,5,5,6,7,6,7,4,58,44,44,44,45,44,37,36,36,62,49,7,21,32,2,3,2,0,3,0,0,1,3,1,1,3,1,2,1,1,2,0,35,19,6,58,45,36,37,45 },
                { 45,37,36,44,44,44,44,37,37,45,45,45,44,44,59,7,5,4,4,6,7,6,5,6,50,45,37,36,45,36,44,37,36,37,62,49,7,20,32,1,2,3,0,0,0,3,0,3,2,3,0,1,3,3,2,1,11,4,48,63,36,45,45,44 },
                { 37,37,37,45,44,36,44,37,45,37,44,37,45,54,57,5,23,9,9,8,8,17,6,6,50,37,44,36,44,44,44,44,44,45,44,62,49,4,20,12,40,1,0,1,3,0,1,0,0,1,2,2,1,0,27,12,19,48,63,44,45,37,36,44 },
                { 45,44,45,36,45,36,37,45,45,36,36,45,45,51,4,23,33,3,2,2,0,42,16,4,56,55,44,44,45,44,45,36,45,36,37,36,62,49,5,5,21,12,32,3,0,2,1,0,2,0,0,1,2,27,18,7,48,63,44,45,45,37,44,45 },
                { 44,37,45,44,44,44,44,36,36,37,45,36,37,51,6,14,3,2,2,1,3,3,42,16,6,58,44,36,44,44,45,44,37,37,37,45,45,51,4,7,7,7,20,12,32,1,2,1,1,0,3,1,27,19,4,48,63,37,36,36,45,44,45,44 },
                { 36,45,36,45,37,36,44,45,44,45,37,37,37,59,6,14,0,1,3,1,2,2,0,11,7,58,44,36,37,36,36,45,44,36,44,44,36,62,49,5,4,7,4,5,14,0,0,3,2,1,2,27,19,6,48,63,45,36,44,36,45,37,36,37 },
                { 36,37,37,44,37,45,36,37,44,36,45,44,44,51,6,15,3,1,0,2,2,3,1,11,4,56,55,36,45,45,44,45,37,45,44,45,36,44,62,49,7,7,7,23,41,1,0,3,0,2,43,19,4,48,63,37,37,45,37,36,36,45,44,36 },
                { 36,36,44,45,37,37,36,37,45,45,37,36,45,51,4,21,24,3,0,0,1,2,35,19,7,6,58,37,44,36,37,45,36,45,45,44,36,45,36,59,4,7,7,15,0,2,1,2,1,43,18,6,48,63,44,45,37,36,36,44,45,45,37,36 },
                { 44,37,37,37,45,45,37,45,37,37,37,37,36,51,6,7,20,40,2,3,1,43,18,4,7,7,58,36,44,44,45,37,37,37,44,36,44,44,54,57,7,5,23,41,3,3,2,3,3,10,5,48,63,45,37,37,37,36,37,36,45,45,44,36 },
                { 36,36,36,45,37,44,44,45,37,36,37,37,37,62,49,6,4,20,12,12,13,18,7,4,4,7,56,61,55,45,45,44,44,45,37,44,44,45,51,7,7,22,33,2,3,0,3,0,27,18,5,50,36,45,37,45,37,36,37,44,45,45,45,37 },
                { 44,36,36,44,44,44,44,37,44,45,45,37,44,36,62,49,7,5,6,6,4,7,4,23,9,17,4,6,56,55,44,44,44,44,44,37,36,36,59,5,22,41,2,0,1,1,1,2,11,7,7,50,45,37,44,36,45,36,45,36,37,36,37,37 },
                { 36,37,45,44,45,44,37,44,44,37,45,44,45,37,45,62,53,53,49,7,4,6,5,15,2,11,7,4,7,58,37,36,37,37,44,36,44,45,51,4,14,0,1,2,3,2,1,2,42,17,7,58,44,45,37,44,37,36,44,37,37,45,36,37 },
                { 45,37,45,36,45,44,45,36,45,37,44,44,44,36,44,44,45,45,62,49,7,5,4,21,12,19,4,6,4,56,61,55,45,44,44,44,36,36,51,5,14,2,3,1,2,1,1,1,1,10,5,58,36,37,44,44,44,37,44,37,44,36,36,45 },
                { 44,37,45,45,36,37,45,37,37,36,36,45,37,44,45,45,44,37,37,62,49,5,6,5,5,4,4,6,7,6,4,56,61,55,37,36,44,37,51,5,15,2,2,2,0,0,2,0,0,10,7,58,36,37,45,45,44,45,37,37,45,44,36,36 },
                { 37,36,45,36,45,45,36,45,44,45,36,45,44,37,45,44,45,37,36,37,62,49,7,4,5,7,23,9,8,8,8,17,7,50,36,36,44,45,62,49,14,0,0,2,3,3,2,2,3,10,5,50,44,44,37,44,45,37,36,37,37,44,44,44 },
                { 45,45,37,44,36,44,37,44,37,36,36,44,37,37,36,45,36,45,37,44,36,59,5,6,23,8,25,3,1,0,3,42,17,56,55,37,44,45,36,51,14,0,3,3,1,3,3,1,0,10,5,50,44,36,37,45,45,37,37,37,45,36,37,36 },
                { 45,36,36,44,44,36,44,36,36,44,36,45,36,36,37,44,37,44,36,44,36,59,4,6,14,1,2,0,3,0,1,2,26,17,58,45,44,44,37,51,21,40,2,1,1,1,1,3,27,18,7,50,36,44,44,37,37,37,45,44,37,45,36,44 },
                { 37,37,45,37,45,36,45,45,36,37,45,44,37,44,36,36,37,36,37,37,44,62,49,4,14,2,3,3,2,2,2,1,2,10,58,36,45,37,45,59,4,21,12,24,2,3,43,13,18,6,6,50,36,36,45,37,37,36,45,45,44,45,37,44 },
                { 44,44,36,36,44,44,36,36,37,44,36,44,44,45,44,45,44,36,37,37,44,37,51,4,14,0,3,1,2,1,0,1,0,10,58,37,36,45,36,51,7,5,6,21,13,13,19,6,6,4,48,63,36,45,37,37,37,44,37,36,37,36,45,36 },
                { 45,36,36,44,45,37,36,45,44,45,45,45,44,45,36,44,37,44,45,36,37,37,59,4,14,1,1,1,3,0,0,3,2,11,58,36,37,37,44,62,49,5,6,5,5,6,6,5,4,7,58,44,37,37,44,37,45,37,36,45,44,37,37,44 },
                { 45,45,37,36,37,37,36,44,44,36,36,45,36,37,45,36,45,37,37,44,37,45,59,4,20,40,0,0,2,1,1,2,43,18,50,37,44,45,37,36,51,6,22,9,9,17,7,7,6,48,63,37,45,44,37,44,44,44,44,44,36,36,37,45 },
                { 44,36,37,44,44,45,37,44,44,37,44,44,44,36,36,44,44,44,44,37,44,44,59,5,7,21,13,32,0,2,35,12,19,5,58,37,45,45,37,45,51,23,41,2,3,26,17,7,4,50,36,36,36,45,37,45,45,37,44,45,44,44,44,37 },
                { 36,44,36,36,45,37,37,37,37,36,45,44,45,44,36,37,45,44,36,44,37,44,62,49,6,6,6,20,12,13,19,7,6,48,63,37,36,37,44,45,51,14,1,1,1,3,11,4,7,50,36,44,44,45,45,44,45,37,45,45,44,37,37,44 },
                { 36,37,45,37,44,37,37,37,45,36,37,36,44,45,36,45,44,36,36,37,44,44,44,51,5,7,4,6,5,4,4,48,52,63,36,36,45,44,37,54,57,15,1,3,0,3,10,7,48,63,45,37,44,45,36,37,44,37,45,45,45,45,45,45 },
                { 45,36,45,37,45,44,44,44,37,44,36,45,45,36,36,44,37,36,44,36,37,45,45,62,52,52,53,53,53,52,52,63,36,45,45,44,45,36,44,51,6,21,12,40,0,1,11,4,50,44,36,44,37,44,45,44,37,37,45,45,37,37,36,45 },
                { 36,37,37,37,44,44,36,37,44,44,44,36,45,44,37,36,45,44,37,45,37,44,36,44,36,37,36,37,36,37,36,44,45,37,37,36,45,37,44,51,7,5,6,20,12,13,19,5,56,55,44,45,44,36,45,44,45,36,36,36,36,44,44,44 },
                { 45,37,45,45,44,45,37,45,36,37,45,36,44,36,36,36,45,37,36,36,36,37,45,45,37,36,45,36,44,36,44,45,36,36,45,45,45,36,36,51,7,5,6,5,6,4,4,6,4,58,45,45,36,44,45,37,45,45,37,44,37,44,36,36 },
                { 36,44,37,45,37,45,45,37,45,36,37,44,44,44,36,36,37,37,44,44,36,36,44,36,45,36,44,37,45,44,36,36,37,37,44,36,45,36,54,57,5,22,8,9,8,8,8,16,6,56,55,36,45,44,44,44,45,44,36,36,37,36,36,37 },
                { 37,36,37,45,44,37,37,36,36,44,37,45,36,44,36,44,45,36,44,36,36,36,44,37,45,37,36,44,37,36,36,44,45,36,44,36,36,37,59,5,22,41,2,2,1,0,2,42,17,4,56,55,37,45,36,36,36,44,36,45,45,36,36,36 },
                { 44,37,44,36,44,37,45,44,36,44,36,36,36,45,44,36,44,44,44,45,44,37,36,36,36,44,37,36,45,45,45,37,37,44,37,36,44,36,51,22,25,3,3,3,3,2,3,1,26,16,7,56,55,45,37,45,36,36,44,37,37,45,37,37 },
                { 45,37,45,45,37,45,45,45,37,45,36,45,44,37,44,45,37,36,45,36,44,45,44,37,44,45,44,45,44,37,44,36,36,45,36,45,37,54,57,15,0,2,0,0,3,3,2,3,1,42,16,5,58,36,36,44,37,45,36,37,44,37,37,37 },
                { 44,44,45,44,44,45,44,45,44,37,36,37,37,44,45,36,45,36,37,45,44,45,45,44,37,37,37,37,45,36,36,36,37,36,44,37,45,51,6,14,3,3,1,1,3,1,2,1,2,0,11,6,58,44,36,44,44,44,44,44,36,37,37,36 },
                { 45,44,36,45,45,37,37,37,44,44,45,37,45,45,37,37,36,37,36,36,44,37,37,44,37,44,36,37,37,36,44,37,37,36,45,36,45,51,7,15,0,1,2,3,null,null,null,2,0,3,10,7,58,44,37,44,36,44,36,45,44,45,45,37 },
                { 45,37,45,45,44,44,44,45,37,44,45,44,45,45,44,37,44,45,44,37,44,36,36,45,44,45,44,45,45,45,37,37,44,44,44,45,45,51,5,15,2,2,1,2,null,null,null,2,0,0,10,6,58,44,45,44,37,45,37,45,45,37,44,44 },
                { 45,45,37,44,37,44,36,45,37,37,44,36,44,36,37,36,36,45,36,36,36,44,36,44,45,37,45,36,44,36,44,44,36,37,45,44,45,62,49,20,40,1,1,0,null,null,null,2,3,2,11,5,58,37,44,44,36,44,44,44,45,37,37,37 },
                { 44,44,37,36,37,45,44,36,45,44,37,37,45,45,45,36,37,45,36,45,44,37,36,37,36,45,36,37,45,45,45,45,37,37,44,37,36,44,59,5,21,40,2,2,3,2,1,0,2,43,19,48,63,36,37,37,45,37,37,45,36,37,36,45 },
                { 37,37,45,44,36,37,37,37,44,45,36,37,36,45,44,36,36,44,44,45,45,36,36,36,37,44,44,37,37,45,37,44,37,45,44,37,37,37,62,49,6,21,12,24,2,2,2,43,13,19,6,58,37,45,44,44,44,45,45,45,45,45,44,45 },
                { 37,36,36,37,44,36,45,44,45,37,44,44,44,45,36,45,45,45,36,36,44,44,44,45,37,36,37,45,37,44,37,36,36,45,37,36,45,44,37,62,49,6,7,20,12,13,12,19,4,5,48,63,44,45,37,37,45,44,37,36,45,37,36,37 },
                { 37,36,36,37,36,37,44,44,45,37,44,44,37,37,44,36,37,45,36,45,36,45,44,36,37,37,45,45,44,36,45,37,45,45,44,36,37,37,44,45,62,53,52,52,52,53,52,53,53,52,63,44,37,37,44,45,44,37,37,44,45,37,44,37 },
                { 45,44,36,37,45,37,44,44,37,45,44,37,37,37,45,37,36,45,44,44,44,37,36,36,36,36,37,45,37,36,44,44,36,45,37,45,44,37,37,37,37,44,37,37,45,37,36,44,37,45,44,44,44,37,37,37,36,44,44,45,45,37,36,45 },
                { 37,45,36,45,44,37,37,45,45,44,36,44,37,44,36,37,45,37,45,45,36,37,45,45,36,36,45,37,44,45,36,37,44,45,37,45,36,45,45,37,45,44,44,45,45,37,45,45,45,45,44,37,36,44,44,36,36,44,37,45,37,36,36,45 }
            });

            // Tower ----------------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(87, -72, 0));

            // FORTRESS 1
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(109, -31, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(109, -35, 0)); // VERTICAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(109, -39, 0)); // VERTICAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(109, -43, 0)); // VERTICAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(109, -47, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(105, -47, 0)); // HORIZONTAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(101, -47, 0)); // HORIZONTAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(97, -47, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(93, -47, 0)); // HORIZONTAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(89, -47, 0)); // HORIZONTAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(85, -47, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(85, -43, 0)); // VERTICAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(85, -39, 0)); // VERTICAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.WallVertical, Game, new Vector3(85, -35, 0)); // VERTICAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(85, -31, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(105, -31, 0)); // HORIZONTAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.WallHorizontal, Game, new Vector3(101, -31, 0)); // HORIZONTAL WALL
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(97, -31, 0));

            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(39, -31, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(39, -59, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TurretTower, Game, new Vector3(63, -83, 0));

            // Building A ----------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.BuildingA, Game, new Vector3(47, -25, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.BuildingA, Game, new Vector3(52, -25, 0));

            // Hanger --------------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.Hanger, Game, new Vector3(105, -39, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.Hanger, Game, new Vector3(99, -39, 0));

            GameObjectPrefabManager.InstantiateType(PrefabType.Hanger, Game, new Vector3(55, -81, 0), Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), 0, MathHelper.ToRadians(90.0f)));
            GameObjectPrefabManager.InstantiateType(PrefabType.Hanger, Game, new Vector3(55, -86, 0), Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), 0, MathHelper.ToRadians(90.0f)));

            // Hospital ------------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.Hospital, Game, new Vector3(90, -39, 0));

            // Lookout Tower --------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.LookoutTower, Game, new Vector3(90, -90, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.LookoutTower, Game, new Vector3(93, -39, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.LookoutTower, Game, new Vector3(33, -59, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.LookoutTower, Game, new Vector3(61, -21, 0));

            // Single Tree ----------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(81, -107, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(99, -115, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(85, -117, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(91, -95, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(87, -89, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(95, -79, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(83, -75, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(95, -67, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(87, -53, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(103, -55, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(95, -39, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(113, -41, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(109, -27, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(97, -19, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(85, -21, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(67, -17, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(55, -21, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(43, -23, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(39, -37, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(51, -33, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(67, -37, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(81, -49, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(35, -55, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(49, -69, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(41, -63, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(51, -79, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeSingle, Game, new Vector3(59, -89, 0));

            // Tree Double ----------------------------------------------------------------------------------
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(83, -105, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(89, -119, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(97, -107, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(91, -103, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(85, -93, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(83, -77, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(87, -65, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(101, -57, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(79, -49, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(111, -39, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(107, -23, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(81, -17, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(57, -19, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(39, -27, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(47, -35, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(63, -35, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(41, -53, 0));
            GameObjectPrefabManager.InstantiateType(PrefabType.TreeDouble, Game, new Vector3(61, -77, 0));
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
