/*  GameObjectPrefabManager.cs
 *  Handles the mass construction of Prefab types
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.11.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RBeausoleilPROG2370FinalAssignment.ComponentControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment
{
    public static class GameObjectPrefabManager
    {
        private static Random random = new Random();

        public static void LoadContent(GameController game)
        {
            if (game != null)
            {
                game.Content.Load<Model>("tankammo");
                game.Content.Load<Model>("baseouterwalls");
                game.Content.Load<Model>("lookouttower");
                game.Content.Load<Model>("hanger");
                game.Content.Load<Model>("hospital");
                game.Content.Load<Model>("buildingA");
                game.Content.Load<Model>("palmtreedebris01");
                game.Content.Load<Model>("palmtreedebris02");
                game.Content.Load<Model>("palmtree01");
                game.Content.Load<Model>("palmtree02");
                game.Content.Load<Model>("wall");
                game.Content.Load<Model>("walldamaged");
                game.Content.Load<Model>("walldamagedfull");
                game.Content.Load<Model>("tower");
                game.Content.Load<Model>("turret");
                game.Content.Load<Model>("towerdamaged");
                game.Content.Load<Model>("towerrubble");

                game.Content.Load<Texture2D>("muzzlefire");
                game.Content.Load<Texture2D>("bulletdebristexture");
                game.Content.Load<Texture2D>("smoke");
                game.Content.Load<Texture2D>("explosiondebristexture");
                game.Content.Load<Texture2D>("explosion");
                game.Content.Load<Texture2D>("explosionflash");
                game.Content.Load<Texture2D>("dirtexplosion");

                game.Content.Load<SoundEffect>("sounds/concretedebris");
                game.Content.Load<SoundEffect>("sounds/explosionecho");
                game.Content.Load<SoundEffect>("sounds/explosion");
                game.Content.Load<SoundEffect>("sounds/fallingtree");
                game.Content.Load<SoundEffect>("sounds/tankshooting");
            }
        }

        public static GameObject InstantiateType(PrefabType type, GameController game, Vector3 position, Quaternion localRotation)
        {
            GameObject gameObject = InstantiateType(type, game);

            if (gameObject != null)
            {
                gameObject.Transform.Position = position;
                gameObject.Transform.LocalRotation = localRotation;
            }

            return gameObject;
        }

        public static GameObject InstantiateType(PrefabType type, GameController game, Vector3 position)
        {
            GameObject gameObject = InstantiateType(type, game);

            if (gameObject != null)
            {
                gameObject.Transform.Position = position;
            }

            return gameObject;
        }

        public static GameObject InstantiateType(PrefabType type, GameController game)
        {
            switch (type)
            {
                case PrefabType.MenuUI:
                    {
                        GameObject menu = GameObject.Instantiate(game);
                        menu.Transform.Position = new Vector3(0, 12, 0.9f);
                        menu.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(60.0f));

                        menu.AddComponent<CanvasEventSystem>();

                        MenuEventHandler menuEventHandler = menu.AddComponent<MenuEventHandler>() as MenuEventHandler;

                        CanvasRenderer canvas = menu.AddComponent<CanvasRenderer>() as CanvasRenderer;
                        canvas.BufferWidth = 584;
                        canvas.BufferHeight = 492;
                        canvas.Width = 20;
                        canvas.Height = 16.8f;
                        canvas.RenderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.DrawOrder = 1000;
                        canvas.VisibleChanged += menuEventHandler.Menu_VisibleChanged;

                        CanvasSprite fireStorm = menu.AddComponent<CanvasSprite>() as CanvasSprite;
                        fireStorm.Texture = game.Content.Load<Texture2D>("firestormlogo");
                        fireStorm.Width = 497;
                        fireStorm.Height = 111;
                        fireStorm.Position = new Vector2(44, 100);

                        CanvasButton newGame = menu.AddComponent<CanvasButton>() as CanvasButton;
                        newGame.NormalTexture = game.Content.Load<Texture2D>("menunewgame");
                        newGame.HoverTexture = game.Content.Load<Texture2D>("menunewgameselected");
                        newGame.Width = 134;
                        newGame.Height = 26;
                        newGame.Position = new Vector2(225, 243);
                        newGame.Clicked += menuEventHandler.NewGame_Clicked;

                        CanvasButton help = menu.AddComponent<CanvasButton>() as CanvasButton;
                        help.NormalTexture = game.Content.Load<Texture2D>("menuhelp");
                        help.HoverTexture = game.Content.Load<Texture2D>("menuhelpselected");
                        help.Width = 69;
                        help.Height = 24;
                        help.Position = new Vector2(258, 285);
                        help.Clicked += menuEventHandler.Help_Clicked;

                        CanvasButton credits = menu.AddComponent<CanvasButton>() as CanvasButton;
                        credits.NormalTexture = game.Content.Load<Texture2D>("menucredits");
                        credits.HoverTexture = game.Content.Load<Texture2D>("menucreditsselected");
                        credits.Width = 108;
                        credits.Height = 26;
                        credits.Position = new Vector2(238, 325);
                        credits.Clicked += menuEventHandler.Credits_Clicked;

                        CanvasButton exit = menu.AddComponent<CanvasButton>() as CanvasButton;
                        exit.NormalTexture = game.Content.Load<Texture2D>("menuexit");
                        exit.HoverTexture = game.Content.Load<Texture2D>("menuexitselected");
                        exit.Width = 61;
                        exit.Height = 24;
                        exit.Position = new Vector2(262, 367);
                        exit.Clicked += menuEventHandler.Exit_Clicked;

                        CanvasText text1 = menu.AddComponent<CanvasText>() as CanvasText;
                        text1.SpriteFont = game.Content.Load<SpriteFont>("stencil");
                        text1.Text = "Ryan Beausoleil";
                        text1.Width = 256;
                        text1.Color = Color.Black;
                        text1.Position = new Vector2(401, 456);

                        CanvasText text2 = menu.AddComponent<CanvasText>() as CanvasText;
                        text2.SpriteFont = game.Content.Load<SpriteFont>("stencil");
                        text2.Text = "Ryan Beausoleil";
                        text2.Width = 256;
                        text2.Position = new Vector2(400, 455);

                        menu.SetActive(false);

                        return menu;
                    }

                case PrefabType.GameMessageUI:
                    {
                        GameObject gameMessage = GameObject.Instantiate(game);

                        CanvasRenderer canvas = gameMessage.AddComponent<CanvasRenderer>() as CanvasRenderer;
                        canvas.BufferWidth = 256;
                        canvas.BufferHeight = 40;
                        canvas.Width = 5;
                        canvas.Height = 0.78f;
                        canvas.RenderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.DrawOrder = 1000;

                        CanvasText text1 = gameMessage.AddComponent<CanvasText>() as CanvasText;
                        text1.SpriteFont = game.Content.Load<SpriteFont>("stencil");
                        text1.Text = "The Game Message";
                        text1.Width = 256;
                        text1.Color = Color.Black;
                        text1.Position = new Vector2(1, 1);

                        CanvasText text2 = gameMessage.AddComponent<CanvasText>() as CanvasText;
                        text2.SpriteFont = game.Content.Load<SpriteFont>("stencil");
                        text2.Text = "The Game Message";
                        text2.Width = 256;
                        text2.Position = new Vector2(0, 0);

                        CanvasFadeController canvasFadeController = gameMessage.AddComponent<CanvasFadeController>() as CanvasFadeController;
                        canvasFadeController.ShowDuration = 0.2f;
                        canvasFadeController.HideDuration = 0.2f;

                        gameMessage.SetActive(false);

                        return gameMessage;
                    }

                case PrefabType.TargetBuildingsUI:
                    {
                        GameObject targetBuildings = GameObject.Instantiate(game);

                        CanvasRenderer canvas = targetBuildings.AddComponent<CanvasRenderer>() as CanvasRenderer;
                        canvas.BufferWidth = 256;
                        canvas.BufferHeight = 40;
                        canvas.Width = 8;
                        canvas.Height = 1.25f;
                        canvas.RenderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.DrawOrder = 1000;

                        CanvasText text1 = targetBuildings.AddComponent<CanvasText>() as CanvasText;
                        text1.SpriteFont = game.Content.Load<SpriteFont>("stencil");
                        text1.Text = "XXX Targets Remaining";
                        text1.Width = 256;
                        text1.Color = Color.Black;
                        text1.Position = new Vector2(1, 1);

                        CanvasText text2 = targetBuildings.AddComponent<CanvasText>() as CanvasText;
                        text2.SpriteFont = game.Content.Load<SpriteFont>("stencil");
                        text2.Text = "XXX Targets Remaining";
                        text2.Width = 256;
                        text2.Position = new Vector2(0, 0);

                        CanvasFadeController canvasFadeController = targetBuildings.AddComponent<CanvasFadeController>() as CanvasFadeController;
                        canvasFadeController.ShowDuration = 0.3f;
                        canvasFadeController.HideDuration = 0.2f;

                        targetBuildings.SetActive(false);

                        return targetBuildings;
                    }

                case PrefabType.HelpMenuUI:
                    {
                        GameObject menu = GameObject.Instantiate(game);

                        menu.AddComponent<CanvasEventSystem>();

                        MenuEventHandler menuEventHandler = menu.AddComponent<MenuEventHandler>() as MenuEventHandler;

                        CanvasRenderer canvas = menu.AddComponent<CanvasRenderer>() as CanvasRenderer;
                        canvas.BufferWidth = 584;
                        canvas.BufferHeight = 492;
                        canvas.Width = 15;
                        canvas.Height = 12.64f;
                        canvas.RenderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.DrawOrder = 1000;
                        canvas.VisibleChanged += menuEventHandler.Menu_VisibleChanged;

                        CanvasButton helpMenu = menu.AddComponent<CanvasButton>() as CanvasButton;
                        helpMenu.NormalTexture = game.Content.Load<Texture2D>("helpmenu");
                        helpMenu.HoverTexture = helpMenu.NormalTexture;
                        helpMenu.Width = 584;
                        helpMenu.Height = 493;
                        helpMenu.Position = new Vector2(0, 0);
                        helpMenu.Clicked += menuEventHandler.HelpMenu_Clicked;

                        menu.SetActive(false);

                        return menu;
                    }

                case PrefabType.CreditMenuUI:
                    {
                        GameObject menu = GameObject.Instantiate(game);

                        menu.AddComponent<CanvasEventSystem>();

                        MenuEventHandler menuEventHandler = menu.AddComponent<MenuEventHandler>() as MenuEventHandler;

                        CanvasRenderer canvas = menu.AddComponent<CanvasRenderer>() as CanvasRenderer;
                        canvas.BufferWidth = 584;
                        canvas.BufferHeight = 494;
                        canvas.Width = 15;
                        canvas.Height = 12.69f;
                        canvas.RenderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.DrawOrder = 1000;
                        canvas.VisibleChanged += menuEventHandler.Menu_VisibleChanged;

                        CanvasButton creditsMenu = menu.AddComponent<CanvasButton>() as CanvasButton;
                        creditsMenu.NormalTexture = game.Content.Load<Texture2D>("creditsmenu");
                        creditsMenu.HoverTexture = creditsMenu.NormalTexture;
                        creditsMenu.Width = 584;
                        creditsMenu.Height = 493;
                        creditsMenu.Position = new Vector2(0, 0);
                        creditsMenu.Clicked += menuEventHandler.CreditsMenu_Clicked;

                        menu.SetActive(false);

                        return menu;
                    }

                case PrefabType.HealthBarUI:
                    {
                        GameObject healthBarUI = GameObject.Instantiate(game, (int)GameObjectTag.UIHealthBar);

                        CanvasRenderer canvas = healthBarUI.AddComponent<CanvasRenderer>() as CanvasRenderer;
                        canvas.BufferWidth = 128;
                        canvas.BufferHeight = 32;
                        canvas.Width = 1;
                        canvas.Height = 0.25f;
                        canvas.DrawOrder = 1000;
                        canvas.RenderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.MasterAlpha = 0;

                        CanvasSprite canvasHealthMeterSprite = healthBarUI.AddComponent<CanvasSprite>() as CanvasSprite;
                        canvasHealthMeterSprite.Texture = game.Content.Load<Texture2D>("healthbartexture");
                        canvasHealthMeterSprite.Width = 128;
                        canvasHealthMeterSprite.Height = 32;

                        CanvasSprite canvasHealthBarSprite = healthBarUI.AddComponent<CanvasSprite>() as CanvasSprite;
                        canvasHealthBarSprite.Texture = game.Content.Load<Texture2D>("healthlifetexture");
                        canvasHealthBarSprite.Width = 91;
                        canvasHealthBarSprite.Height = 20;
                        canvasHealthBarSprite.Position = new Vector2(33, 6);

                        CanvasSprite canvasHealthBarDividerSprite = healthBarUI.AddComponent<CanvasSprite>() as CanvasSprite;
                        canvasHealthBarDividerSprite.Texture = game.Content.Load<Texture2D>("healthlifedividertexture");
                        canvasHealthBarDividerSprite.Width = 91;
                        canvasHealthBarDividerSprite.Height = 20;
                        canvasHealthBarDividerSprite.Position = new Vector2(33, 6);

                        HealthBarController healthBarController = healthBarUI.AddComponent<HealthBarController>() as HealthBarController;
                        healthBarController.HealthSprite = canvasHealthBarSprite;

                        CanvasFadeController canvasFadeController = healthBarUI.AddComponent<CanvasFadeController>() as CanvasFadeController;
                        canvasFadeController.ShowDuration = 0.3f;
                        canvasFadeController.HideDuration = 0.2f;

                        CanvasText text1 = healthBarUI.AddComponent<CanvasText>() as CanvasText;
                        text1.SpriteFont = game.Content.Load<SpriteFont>("stencil");
                        text1.Text = "3";
                        text1.Width = 256;
                        text1.Color = Color.Black;
                        text1.Position = new Vector2(8, 1);

                        CanvasText text2 = healthBarUI.AddComponent<CanvasText>() as CanvasText;
                        text2.SpriteFont = game.Content.Load<SpriteFont>("stencil");
                        text2.Text = "3";
                        text2.Width = 256;
                        text2.Color = Color.DarkGoldenrod;
                        text2.Position = new Vector2(7, 0);

                        healthBarUI.SetActive(false);

                        return healthBarUI;
                    }

                case PrefabType.Tank:
                    {
                        // tank
                        GameObject tankBottom = GameObject.Instantiate(game, (int)GameObjectTag.Player);
                        tankBottom.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        (tankBottom.AddComponent<ModelRenderer>() as ModelRenderer).Model = game.Content.Load<Model>("tankbottom");
                        TankController tankController = tankBottom.AddComponent<TankController>() as TankController;
                        tankController.DrivingSoundEffect = game.Content.Load<SoundEffect>("sounds/tankdriving").CreateInstance();
                        tankController.TankCollisionSoundEffect = game.Content.Load<SoundEffect>("sounds/tankcollision").CreateInstance();
                        tankController.TankDrivingOverConcreteSoundEffect = game.Content.Load<SoundEffect>("sounds/tankdrivingconcrete").CreateInstance();
                        tankBottom.AddComponent<RigidBody2D>();
                        tankBottom.Layer = (int)LayerType.Player;

                        GameObject tankBottomCollider = GameObject.Instantiate(game, (int)GameObjectTag.Player);
                        (tankBottomCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(2.2f, 4.0f);
                        tankBottomCollider.Transform.Parent = tankBottom;
                        tankBottomCollider.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-90.0f));

                        GameObject tankTurret = GameObject.Instantiate(game, (int)GameObjectTag.TankTurret);
                        tankTurret.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        tankTurret.Transform.Parent = tankBottom;
                        (tankTurret.AddComponent<ModelRenderer>() as ModelRenderer).Model = game.Content.Load<Model>("tanktop");
                        TankTurretController tankTurretController = tankTurret.AddComponent<TankTurretController>() as TankTurretController;
                        tankTurretController.TurretRotateSoundEffect = game.Content.Load<SoundEffect>("sounds/turretrotate").CreateInstance();
                        tankTurretController.ShootingSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/tankshooting"));

                        return tankBottom;
                    }

                case PrefabType.TankModelOnly:
                    {
                        // tank
                        GameObject tankBottom = GameObject.Instantiate(game);
                        tankBottom.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        (tankBottom.AddComponent<ModelRenderer>() as ModelRenderer).Model = game.Content.Load<Model>("tankbottom");

                        GameObject tankTurret = GameObject.Instantiate(game, (int)GameObjectTag.TankTurret);
                        tankTurret.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        tankTurret.Transform.Parent = tankBottom;
                        tankTurret.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(40.0f));
                        (tankTurret.AddComponent<ModelRenderer>() as ModelRenderer).Model = game.Content.Load<Model>("tanktop");

                        return tankBottom;
                    }

                case PrefabType.Bullet:
                    {
                        GameObject bullet = GameObject.Instantiate(game, (int)GameObjectTag.Bullet);
                        (bullet.AddComponent<ModelRenderer>() as ModelRenderer).Model = game.Content.Load<Model>("tankammo");
                        (bullet.AddComponent<CircleCollider2D>() as CircleCollider2D).Radius = 0.8f;
                        BulletController bulletController = bullet.AddComponent<BulletController>() as BulletController;
                        bulletController.ExplosionSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/explosion"));
                        return bullet;
                    }

                case PrefabType.Base:
                    {
                        GameObject baseWalls = GameObject.Instantiate(game, (int)GameObjectTag.Base);
                        baseWalls.AddComponent<BaseController>();
                        (baseWalls.AddComponent<ModelRenderer>() as ModelRenderer).Model = game.Content.Load<Model>("baseouterwalls");
                        baseWalls.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));

                        return baseWalls;
                    }

                case PrefabType.LookoutTower:
                    {
                        GameObject lookoutTower = GameObject.Instantiate(game, (int)GameObjectTag.TargetBuilding);
                        ModelRenderer lookoutTowerRenderer = lookoutTower.AddComponent<ModelRenderer>() as ModelRenderer;
                        lookoutTowerRenderer.Model = game.Content.Load<Model>("lookouttower");
                        lookoutTowerRenderer.SpecularPower = 600;
                        lookoutTower.Layer = (int)LayerType.EnemyStructure;
                        lookoutTower.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));

                        StructureDestructionController lookoutTowerStructure = lookoutTower.AddComponent<StructureDestructionController>() as StructureDestructionController;
                        lookoutTowerStructure.HitHealth = 1; 
                        lookoutTowerStructure.DestroyFallHeight = -5f;
                        lookoutTowerStructure.FallingSpeed = 40f;
                        lookoutTowerStructure.Enabled = false;

                        GameObject lookoutTowerCollider = GameObject.Instantiate(game);
                        (lookoutTowerCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(2f, 2f);
                        lookoutTowerCollider.Transform.Parent = lookoutTower;
                        lookoutTowerCollider.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-90.0f));

                        lookoutTowerCollider.Static = true;

                        return lookoutTower;
                    }

                case PrefabType.Hanger:
                    {
                        GameObject hanger = GameObject.Instantiate(game, (int)GameObjectTag.TargetBuilding);
                        ModelRenderer hangerRenderer = hanger.AddComponent<ModelRenderer>() as ModelRenderer;
                        hangerRenderer.Model = game.Content.Load<Model>("hanger");
                        hangerRenderer.SpecularPower = 600;
                        hanger.Layer = (int)LayerType.EnemyStructure;
                        hanger.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));

                        StructureDestructionController hangerStructure = hanger.AddComponent<StructureDestructionController>() as StructureDestructionController;
                        hangerStructure.HitHealth = 3;
                        hangerStructure.DestroyFallHeight = -2.1f;
                        hangerStructure.FallingSpeed = 40f;
                        hangerStructure.Enabled = false;

                        GameObject hangerCollider = GameObject.Instantiate(game);
                        (hangerCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(4f, 6f);
                        hangerCollider.Transform.Parent = hanger;
                        hangerCollider.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-90.0f));

                        hangerCollider.Static = true;

                        return hanger;
                    }

                case PrefabType.Hospital:
                    {
                        GameObject hospital = GameObject.Instantiate(game, (int)GameObjectTag.TargetBuilding);
                        ModelRenderer hospitalRenderer = hospital.AddComponent<ModelRenderer>() as ModelRenderer;
                        hospitalRenderer.Model = game.Content.Load<Model>("hospital");
                        hospitalRenderer.SpecularPower = 600;
                        hospital.Layer = (int)LayerType.EnemyStructure;
                        hospital.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));

                        StructureDestructionController hospitalStructure = hospital.AddComponent<StructureDestructionController>() as StructureDestructionController;
                        hospitalStructure.HitHealth = 3;
                        hospitalStructure.DestroyFallHeight = -1.8f;
                        hospitalStructure.FallingSpeed = 40f;
                        hospitalStructure.Enabled = false;

                        GameObject hospitalCollider = GameObject.Instantiate(game);
                        (hospitalCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(4f, 4f);
                        hospitalCollider.Transform.Parent = hospital;
                        hospitalCollider.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-90.0f));

                        hospitalCollider.Static = true;

                        return hospital;
                    }

                case PrefabType.BuildingA:
                    {
                        GameObject buildingA = GameObject.Instantiate(game, (int)GameObjectTag.TargetBuilding);
                        ModelRenderer hospitalRenderer = buildingA.AddComponent<ModelRenderer>() as ModelRenderer;
                        hospitalRenderer.Model = game.Content.Load<Model>("buildingA");
                        hospitalRenderer.SpecularPower = 600;
                        buildingA.Layer = (int)LayerType.EnemyStructure;
                        buildingA.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));

                        StructureDestructionController buildingAStructure = buildingA.AddComponent<StructureDestructionController>() as StructureDestructionController;
                        buildingAStructure.HitHealth = 3;
                        buildingAStructure.DestroyFallHeight = -2.75f;
                        buildingAStructure.FallingSpeed = 40f;
                        buildingAStructure.Enabled = false;

                        GameObject buildingACollider = GameObject.Instantiate(game);
                        (buildingACollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(4f, 4f);
                        buildingACollider.Transform.Parent = buildingA;
                        buildingACollider.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-90.0f));

                        buildingACollider.Static = true;

                        return buildingA;
                    }

                case PrefabType.TreeDebrisSingle:
                    {
                        GameObject tree = GameObject.Instantiate(game);
                        tree.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f)) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(random.Next(-40, 40)));
                        ModelRenderer treeModelRenderer = tree.AddComponent<ModelRenderer>() as ModelRenderer;
                        treeModelRenderer.Model = game.Content.Load<Model>("palmtreedebris01");
                        treeModelRenderer.AmbientLightColor = new Vector3(0.8f, 0.6f, 0.1f);
                        treeModelRenderer.SpecularPower = 400;

                        return tree;
                    }

                case PrefabType.TreeDebrisDouble:
                    {
                        GameObject tree = GameObject.Instantiate(game);
                        tree.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f)) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(random.Next(-40, 40)));
                        ModelRenderer treeModelRenderer = tree.AddComponent<ModelRenderer>() as ModelRenderer;
                        treeModelRenderer.Model = game.Content.Load<Model>("palmtreedebris02");
                        treeModelRenderer.AmbientLightColor = new Vector3(0.8f, 0.6f, 0.1f);
                        treeModelRenderer.SpecularPower = 400;

                        return tree;
                    }

                case PrefabType.TreeSingle:
                    {
                        GameObject tree = GameObject.Instantiate(game, (int)GameObjectTag.Tree);
                        tree.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        tree.Layer = (int)LayerType.Tree;
                        tree.Static = true;

                        ModelRenderer treeModelRenderer = tree.AddComponent<ModelRenderer>() as ModelRenderer;
                        treeModelRenderer.Model = game.Content.Load<Model>("palmtree01");
                        treeModelRenderer.AmbientLightColor = new Vector3(1f, 0.8f, 0.8f);

                        (tree.AddComponent<CircleCollider2D>() as CircleCollider2D).Radius = 0.675f;
                        TreeDestructionController structure = tree.AddComponent<TreeDestructionController>() as TreeDestructionController;
                        structure.TreeDestructionSound = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/fallingtree"));
                        structure.FallingSpeed = 35f;
                        structure.DestroyFallHeight = -4.8f;
                        structure.PrefabReplacement = PrefabType.TreeDebrisSingle;
                        structure.PrefabReplacementDelay = 0.4f;
                        structure.Enabled = false;

                        return tree;
                    }

                case PrefabType.TreeDouble:
                    {
                        GameObject tree = GameObject.Instantiate(game, (int)GameObjectTag.Tree);
                        tree.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        tree.Layer = (int)LayerType.Tree;
                        tree.Static = true;

                        ModelRenderer treeModelRenderer = tree.AddComponent<ModelRenderer>() as ModelRenderer;
                        treeModelRenderer.Model = game.Content.Load<Model>("palmtree02");
                        treeModelRenderer.AmbientLightColor = new Vector3(0.9f, 1f, 0.7f);

                        (tree.AddComponent<CircleCollider2D>() as CircleCollider2D).Radius = 0.7f;
                        TreeDestructionController structure = tree.AddComponent<TreeDestructionController>() as TreeDestructionController;
                        structure.TreeDestructionSound = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/fallingtree"));
                        structure.FallingSpeed = 35f;
                        structure.DestroyFallHeight = -4.8f;
                        structure.PrefabReplacement = PrefabType.TreeDebrisDouble;
                        structure.PrefabReplacementDelay = 0.35f;
                        structure.Enabled = false;

                        return tree;
                    }

                case PrefabType.WallHorizontal:
                    {
                        GameObject wall = GameObject.Instantiate(game, (int)GameObjectTag.WallHorizontal);
                        wall.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        ModelRenderer wallRenderer = wall.AddComponent<ModelRenderer>() as ModelRenderer;
                        wallRenderer.Model = game.Content.Load<Model>("wall");
                        wallRenderer.SpecularPower = 600;
                        wall.Layer = (int)LayerType.Wall;

                        ConcreteDestructionController wallBaseStructure = wall.AddComponent<ConcreteDestructionController>() as ConcreteDestructionController;
                        wallBaseStructure.ConcreteDebrisSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/concretedebris"));
                        wallBaseStructure.ExplosionEchoSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/explosionecho"));
                        wallBaseStructure.HitHealth = 4;
                        wallBaseStructure.DustCloudHeight = 3f;
                        wallBaseStructure.FallingSpeed = 150f;
                        //wallBaseStructure.PrefabReplacement = PrefabType.TurretTowerDamaged;
                        wallBaseStructure.DestroyFallHeight = -3.5f;
                        wallBaseStructure.Enabled = false;

                        //**************************************
                        GameObject wallCollider = GameObject.Instantiate(game);
                        (wallCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(4.0f, 2.9f);
                        wallCollider.Transform.Position = wall.Transform.Position;
                        wallCollider.Transform.Parent = wall;
                        wallCollider.Layer = (int)LayerType.Wall;
                        //**************************************

                        wall.Static = true;

                        return wall;
                    }

                case PrefabType.WallVertical:
                    {
                        GameObject wall = InstantiateType(PrefabType.WallHorizontal, game);
                        wall.Tag = (int)GameObjectTag.WallVertical;
                        wall.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), MathHelper.ToRadians(180.0f), MathHelper.ToRadians(90.0f));

                        return wall;
                    }

                case PrefabType.WallDamagedEast:
                    {
                        GameObject wall = GameObject.Instantiate(game, (int)GameObjectTag.WallDamagedEast);
                        wall.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        ModelRenderer wallRenderer = wall.AddComponent<ModelRenderer>() as ModelRenderer;
                        wallRenderer.Model = game.Content.Load<Model>("walldamaged");
                        wallRenderer.SpecularPower = 600;
                        wall.Layer = (int)LayerType.Wall;

                        ConcreteDestructionController wallBaseStructure = wall.AddComponent<ConcreteDestructionController>() as ConcreteDestructionController;
                        wallBaseStructure.ConcreteDebrisSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/concretedebris"));
                        wallBaseStructure.ExplosionEchoSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/explosionecho"));
                        wallBaseStructure.HitHealth = 4;
                        wallBaseStructure.DustCloudHeight = 3f;
                        wallBaseStructure.FallingSpeed = 150f;
                        //wallBaseStructure.PrefabReplacement = PrefabType.TurretTowerDamaged;
                        wallBaseStructure.DestroyFallHeight = -3.5f;
                        wallBaseStructure.Enabled = false;

                        //**************************************
                        GameObject wallCollider = GameObject.Instantiate(game);
                        (wallCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(4.0f, 2.9f);
                        wallCollider.Transform.Position = wall.Transform.Position;
                        wallCollider.Transform.Parent = wall;
                        wallCollider.Layer = (int)LayerType.Wall;
                        //**************************************

                        wall.Static = true;

                        return wall;
                    }

                case PrefabType.WallDamagedWest:
                    {
                        GameObject wall = InstantiateType(PrefabType.WallDamagedEast, game);
                        wall.Tag = (int)GameObjectTag.WallDamagedWest;
                        wall.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), MathHelper.ToRadians(-90.0f), MathHelper.ToRadians(90.0f));

                        return wall;
                    }

                case PrefabType.WallDamagedNorth:
                    {
                        GameObject wall = InstantiateType(PrefabType.WallDamagedEast, game);
                        wall.Tag = (int)GameObjectTag.WallDamagedNorth;
                        wall.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), MathHelper.ToRadians(0f), MathHelper.ToRadians(90.0f));

                        return wall;
                    }

                case PrefabType.WallDamagedSouth:
                    {
                        GameObject wall = InstantiateType(PrefabType.WallDamagedEast, game);
                        wall.Tag = (int)GameObjectTag.WallDamagedSouth;
                        wall.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), MathHelper.ToRadians(180.0f), MathHelper.ToRadians(90.0f));

                        return wall;
                    }

                case PrefabType.WallDamagedFullHorizontal:
                    {
                        GameObject wall = GameObject.Instantiate(game, (int)GameObjectTag.WallDamagedFullHorizontal);
                        wall.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        ModelRenderer wallRenderer = wall.AddComponent<ModelRenderer>() as ModelRenderer;
                        wallRenderer.Model = game.Content.Load<Model>("walldamagedfull");
                        wallRenderer.SpecularPower = 600;
                        wall.Layer = (int)LayerType.Wall;

                        ConcreteDestructionController wallBaseStructure = wall.AddComponent<ConcreteDestructionController>() as ConcreteDestructionController;
                        wallBaseStructure.ConcreteDebrisSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/concretedebris"));
                        wallBaseStructure.ExplosionEchoSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/explosionecho"));
                        wallBaseStructure.HitHealth = 4;
                        wallBaseStructure.DustCloudHeight = 3f;
                        wallBaseStructure.FallingSpeed = 150f;
                        //wallBaseStructure.PrefabReplacement = PrefabType.TurretTowerDamaged;
                        wallBaseStructure.DestroyFallHeight = -3.5f;
                        wallBaseStructure.Enabled = false;

                        //**************************************
                        GameObject wallCollider = GameObject.Instantiate(game);
                        (wallCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(4.0f, 2.9f);
                        wallCollider.Transform.Position = wall.Transform.Position;
                        wallCollider.Transform.Parent = wall;
                        wallCollider.Layer = (int)LayerType.Wall;
                        //**************************************

                        wall.Static = true;

                        return wall;
                    }

                case PrefabType.WallDamagedFullVertical:
                    {
                        GameObject wall = InstantiateType(PrefabType.WallDamagedFullHorizontal, game);
                        wall.Tag = (int)GameObjectTag.WallDamagedFullVertical;
                        wall.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(90.0f), MathHelper.ToRadians(180.0f), MathHelper.ToRadians(90.0f));

                        return wall;
                    }

                case PrefabType.TurretTowerModelOnly:
                    {
                        GameObject tower = GameObject.Instantiate(game, (int)GameObjectTag.TurretTower);
                        tower.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        ModelRenderer towerRenderer = tower.AddComponent<ModelRenderer>() as ModelRenderer;
                        towerRenderer.Model = game.Content.Load<Model>("tower");
                        towerRenderer.SpecularPower = 600;

                        GameObject towerTurret = GameObject.Instantiate(game);
                        towerTurret.Transform.Position = tower.Transform.Position + tower.Transform.Up * 5f;
                        towerTurret.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        ModelRenderer modelRenderer = towerTurret.AddComponent<ModelRenderer>() as ModelRenderer;
                        modelRenderer.Model = game.Content.Load<Model>("turret");
                        modelRenderer.SpecularPower = 900;
                        towerTurret.Transform.Parent = tower;

                        return tower;
                    }

                case PrefabType.TurretTower:
                    {
                        GameObject tower = GameObject.Instantiate(game, (int)GameObjectTag.TurretTower);
                        tower.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        ModelRenderer towerRenderer = tower.AddComponent<ModelRenderer>() as ModelRenderer;
                        towerRenderer.Model = game.Content.Load<Model>("tower");
                        towerRenderer.SpecularPower = 600;

                        tower.Layer = (int)LayerType.EnemyStructure;
                        ConcreteDestructionController towerBaseStructure = tower.AddComponent<ConcreteDestructionController>() as ConcreteDestructionController;
                        towerBaseStructure.ConcreteDebrisSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/concretedebris"));
                        towerBaseStructure.ExplosionEchoSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/explosionecho"));
                        towerBaseStructure.HitHealth = 6;
                        towerBaseStructure.DustCloudHeight = 5f;
                        towerBaseStructure.PrefabReplacement = PrefabType.TurretTowerDamaged;
                        towerBaseStructure.DestroyFallHeight = -5.5f;
                        towerBaseStructure.Enabled = false;

                        //**************************************
                        GameObject towerCollider = GameObject.Instantiate(game);
                        (towerCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(5.0f, 5.0f);
                        towerCollider.Transform.Position = tower.Transform.Position;
                        towerCollider.Transform.Parent = tower;
                        towerCollider.Tag = (int)GameObjectTag.TurretTower;
                        //**************************************

                        tower.Static = true;

                        GameObject towerTurret = GameObject.Instantiate(game);
                        towerTurret.Transform.Position = tower.Transform.Position + tower.Transform.Up * 5f;
                        towerTurret.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        ModelRenderer modelRenderer = towerTurret.AddComponent<ModelRenderer>() as ModelRenderer;
                        modelRenderer.Model = game.Content.Load<Model>("turret");
                        modelRenderer.SpecularPower = 900;
                        towerTurret.Transform.Parent = tower;

                        TowerTurretController towerTurretController = towerTurret.AddComponent<TowerTurretController>() as TowerTurretController;
                        towerTurretController.ShootingSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/tankshooting"));

                        return tower;
                    }

                case PrefabType.TurretTowerDamaged:
                    {
                        GameObject towerRubble = GameObject.Instantiate(game, (int)GameObjectTag.ConcreteRubble);
                        towerRubble.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));

                        ModelRenderer towerRubbleModelRenderer = (towerRubble.AddComponent<ModelRenderer>() as ModelRenderer);
                        towerRubbleModelRenderer.Model = game.Content.Load<Model>("towerrubble");
                        towerRubbleModelRenderer.SpecularPower = 1000;

                        CircleCollider2D circleCollider2D = towerRubble.AddComponent<CircleCollider2D>() as CircleCollider2D;
                        circleCollider2D.Radius = 2.7f;
                        circleCollider2D.IsTrigger = true;

                        GameObject damagedTower = GameObject.Instantiate(game, (int)GameObjectTag.TurretTowerDamaged);

                        damagedTower.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));
                        damagedTower.Layer = (int)LayerType.EnemyStructure;

                        ModelRenderer modelRenderer = damagedTower.AddComponent<ModelRenderer>() as ModelRenderer;
                        modelRenderer.Model = game.Content.Load<Model>("towerdamaged");
                        modelRenderer.SpecularColor = new Vector3(0.01f, 0.01f, 0.01f);
                        modelRenderer.SpecularPower = 800;

                        ConcreteDestructionController towerBaseStructure = damagedTower.AddComponent<ConcreteDestructionController>() as ConcreteDestructionController;
                        towerBaseStructure.ConcreteDebrisSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/concretedebris"));
                        towerBaseStructure.ExplosionEchoSoundEffect = SoundEffectInstancePool.Assign(game.Content.Load<SoundEffect>("sounds/explosionecho"));
                        towerBaseStructure.HitHealth = 5;
                        towerBaseStructure.DustCloudHeight = 3f;
                        towerBaseStructure.DestroyFallHeight = -3f;
                        towerBaseStructure.FallingSpeed = 50f;
                        towerBaseStructure.Enabled = false;

                        GameObject towerBaseCollider = GameObject.Instantiate(game);
                        (towerBaseCollider.AddComponent<BoxCollider2D>() as BoxCollider2D).Size = new Vector2(5.0f, 5.0f);
                        towerBaseCollider.Transform.Parent = damagedTower;
                        towerBaseCollider.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-90.0f));
                        towerBaseCollider.Static = true;
                        towerBaseCollider.Tag = (int)GameObjectTag.TurretTowerDamaged;

                        damagedTower.Transform.Parent = towerRubble;

                        return towerRubble;
                    }

                case PrefabType.TowerMuzzleFlash:
                    {
                        GameObject muzzleFlash = GameObject.Instantiate(game, (int)GameObjectTag.TowerMuzzleFlash);

                        ParticleSystem particleSystemTowerMuzzleFire = muzzleFlash.AddComponent<ParticleSystem>() as ParticleSystem;
                        particleSystemTowerMuzzleFire.SimulationSpace = ParticleSystemSimulationSpace.Local;
                        particleSystemTowerMuzzleFire.BillboardConstraint = BillboardConstraint.Vertical;
                        particleSystemTowerMuzzleFire.PlayOnAwake = true;
                        particleSystemTowerMuzzleFire.AnimateTilesX = 8;
                        particleSystemTowerMuzzleFire.AnimateTilesY = 1;
                        particleSystemTowerMuzzleFire.Texture = game.Content.Load<Texture2D>("muzzlefire");
                        particleSystemTowerMuzzleFire.DrawOrder = 102;
                        particleSystemTowerMuzzleFire.StartSize = 3.5f;
                        particleSystemTowerMuzzleFire.EmissionRate = 10f;
                        particleSystemTowerMuzzleFire.StartLifetime = 0.32f;
                        particleSystemTowerMuzzleFire.Duration = 0.1f;
                        particleSystemTowerMuzzleFire.MaxParticles = 1;
                        particleSystemTowerMuzzleFire.Animate = true;
                        particleSystemTowerMuzzleFire.AnimateTilesPerSecond = 26;

                        particleSystemTowerMuzzleFire.AlphaOverLifetime = new float[]
                        {
                            1f,
                            1f,
                            1f,
                            1f,
                            1f,
                            1f,
                            0.6f,
                            0.3f,
                        };

                        (muzzleFlash.AddComponent<ParticleSystemPoolController>() as ParticleSystemPoolController).PrefabType = type;

                        return muzzleFlash;
                    }

                case PrefabType.BulletDebrisParticles:
                    {
                        GameObject debrisExplosion = GameObject.Instantiate(game);
                        debrisExplosion.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(180.0f));

                        ParticleSystem debris = debrisExplosion.AddComponent<ParticleSystem>() as ParticleSystem;
                        debris.SimulationSpace = ParticleSystemSimulationSpace.Local;
                        debris.BillboardConstraint = BillboardConstraint.Billboard;
                        debris.PlayOnAwake = true;
                        debris.SpawnAreaRadius = 2.2f;
                        debris.SpawnAngle = 20;
                        debris.AnimateTilesX = 8;
                        debris.AnimateTilesY = 8;
                        debris.Texture = game.Content.Load<Texture2D>("bulletdebristexture");
                        debris.DrawOrder = 110;
                        debris.StartSize = 0.35f;
                        debris.StartDelay = 0.1f;
                        debris.EmissionRate = 400f;
                        debris.StartLifetime = 0.7f;
                        debris.Duration = 0.4f;
                        debris.MaxParticles = 6;
                        debris.Animate = true;
                        debris.AnimateTilesPerSecond = 60;
                        debris.StartVelocity = 15f;
                        debris.GravityModifier = Vector3.Forward * 70f;

                        (debrisExplosion.AddComponent<ParticleSystemPoolController>() as ParticleSystemPoolController).PrefabType = type;

                        return debrisExplosion;
                    }

                case PrefabType.ExplodingConcreteDustCloudParticles:
                    {
                        GameObject dustExplosion = GameObject.Instantiate(game);
                        dustExplosion.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(180.0f));

                        ParticleSystem dust = dustExplosion.AddComponent<ParticleSystem>() as ParticleSystem;
                        dust.SimulationSpace = ParticleSystemSimulationSpace.Local;
                        dust.BillboardConstraint = BillboardConstraint.Billboard;
                        dust.PlayOnAwake = true;
                        dust.SpawnAreaRadius = 2.2f;
                        dust.SpawnAngle = 90f;
                        dust.AnimateTilesX = 6;
                        dust.AnimateTilesY = 5;
                        dust.Texture = game.Content.Load<Texture2D>("smoke");
                        dust.DrawOrder = 102;
                        dust.StartSize = 8f;
                        dust.EmissionRate = 100f;
                        dust.StartLifetime = 0.45f;
                        dust.StartDelay = 0f;
                        dust.Duration = 0.2f;
                        dust.MaxParticles = 7;
                        dust.Animate = true;
                        dust.AnimateTilesPerSecond = 10;
                        dust.StartRandomRotation = true;
                        dust.StartVelocity = 3f;
                        dust.GravityModifier = Vector3.Forward * 25f;

                        dust.AlphaOverLifetime = new float[]
                        {
                            0.3f,
                            0.6f,
                            0.6f,
                            0.6f,
                            0.6f,
                            0.6f,
                            0.6f,
                            0.6f,
                            0.6f,
                            0.5f,
                            0.4f,
                            0.3f,
                            0.2f,
                            0.1f,
                        };

                        (dustExplosion.AddComponent<ParticleSystemPoolController>() as ParticleSystemPoolController).PrefabType = type;

                        return dustExplosion;
                    }

                case PrefabType.ConcreteExplodeDebrisParticles:
                    {
                        GameObject debrisExplosion = GameObject.Instantiate(game);
                        debrisExplosion.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(180.0f));

                        ParticleSystem debris = debrisExplosion.AddComponent<ParticleSystem>() as ParticleSystem;
                        debris.SimulationSpace = ParticleSystemSimulationSpace.Local;
                        debris.BillboardConstraint = BillboardConstraint.Billboard;
                        debris.PlayOnAwake = true;
                        debris.SpawnAreaRadius = 3.5f;
                        debris.SpawnAngle = 35;
                        debris.AnimateTilesX = 8;
                        debris.AnimateTilesY = 8;
                        debris.Texture = game.Content.Load<Texture2D>("explosiondebristexture");
                        debris.DrawOrder = 102;
                        debris.StartSize = 0.9f;
                        debris.EmissionRate = 600f;
                        debris.StartLifetime = 0.6f;
                        debris.StartDelay = 0f;
                        debris.Duration = 0.4f;
                        debris.MaxParticles = 40;
                        debris.Animate = true;
                        debris.AnimateTilesPerSecond = 80;
                        debris.StartVelocity = 20f;
                        debris.AngularVelocity = 90f;
                        debris.SpawnRandomAngularVelocityDirection = true;
                        debris.GravityModifier = Vector3.Forward * 98f;

                        (debrisExplosion.AddComponent<ParticleSystemPoolController>() as ParticleSystemPoolController).PrefabType = type;

                        return debrisExplosion;
                    }

                case PrefabType.BulletConcreteDustParticles:
                    {
                        GameObject bulletDustExplosion = GameObject.Instantiate(game);
                        bulletDustExplosion.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(180.0f));

                        ParticleSystem dust = bulletDustExplosion.AddComponent<ParticleSystem>() as ParticleSystem;
                        dust.SimulationSpace = ParticleSystemSimulationSpace.Local;
                        dust.BillboardConstraint = BillboardConstraint.Billboard;
                        dust.PlayOnAwake = true;
                        dust.SpawnAreaRadius = 0.4f;
                        dust.SpawnAngle = 80;
                        dust.AnimateTilesX = 6;
                        dust.AnimateTilesY = 5;
                        dust.Texture = game.Content.Load<Texture2D>("smoke");
                        dust.DrawOrder = 101;
                        dust.StartSize = 4f;
                        dust.EmissionRate = 30f; //15f;
                        dust.StartLifetime = 0.5f;
                        //dust.StartDelay = 0.2f;
                        dust.Duration = 0.4f;
                        dust.MaxParticles = 6;
                        dust.Animate = true;
                        dust.StartRandomRotation = true;
                        dust.StartVelocity = 3.5f;
                        dust.AnimateTilesPerSecond = 42f;

                        dust.AlphaOverLifetime = new float[]
                        {
                            0.5f,
                            1f,
                            1f,
                            0.9f,
                            0.8f,
                            0.7f,
                            0.6f,
                            0.5f,
                            0.4f,
                            0.3f,
                            0.2f,
                            0.1f,
                        };

                        (bulletDustExplosion.AddComponent<ParticleSystemPoolController>() as ParticleSystemPoolController).PrefabType = type;

                        return bulletDustExplosion;
                    }

                case PrefabType.ExplosionFire:
                    {
                        GameObject explosionGameObject = GameObject.Instantiate(game);
                        explosionGameObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));

                        ParticleSystem ParticleSystemExplosion = explosionGameObject.AddComponent<ParticleSystem>() as ParticleSystem;
                        ParticleSystemExplosion.SimulationSpace = ParticleSystemSimulationSpace.World;
                        ParticleSystemExplosion.PlayOnAwake = false;
                        ParticleSystemExplosion.AnimateTilesX = 4;
                        ParticleSystemExplosion.AnimateTilesY = 4;
                        ParticleSystemExplosion.Texture = game.Content.Load<Texture2D>("explosion");
                        ParticleSystemExplosion.DrawOrder = 102;
                        ParticleSystemExplosion.StartSize = 4;
                        ParticleSystemExplosion.EmissionRate = 10f;
                        ParticleSystemExplosion.StartLifetime = 0.6f;
                        ParticleSystemExplosion.StartRandomRotation = true;
                        ParticleSystemExplosion.MaxParticles = 60;
                        ParticleSystemExplosion.Duration = 0.05f;
                        ParticleSystemExplosion.Animate = true;
                        ParticleSystemExplosion.AnimateTilesPerSecond = 24;

                        return explosionGameObject;
                    }

                case PrefabType.ExplosionFlash:
                    {
                        GameObject explosionFlashGameObject = GameObject.Instantiate(game);
                        explosionFlashGameObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(180.0f));

                        ParticleSystem ParticleSystemExplosionFlash = explosionFlashGameObject.AddComponent<ParticleSystem>() as ParticleSystem;
                        ParticleSystemExplosionFlash.SimulationSpace = ParticleSystemSimulationSpace.World;
                        ParticleSystemExplosionFlash.BillboardConstraint = BillboardConstraint.Horizontal;
                        ParticleSystemExplosionFlash.PlayOnAwake = false;
                        ParticleSystemExplosionFlash.Texture = game.Content.Load<Texture2D>("explosionflash");
                        ParticleSystemExplosionFlash.DrawOrder = 103;
                        ParticleSystemExplosionFlash.StartSize = 6;
                        ParticleSystemExplosionFlash.SizeFactorModifier = 2;
                        ParticleSystemExplosionFlash.EmissionRate = 1;
                        ParticleSystemExplosionFlash.StartLifetime = 0.2f;
                        ParticleSystemExplosionFlash.StartRandomRotation = true;
                        ParticleSystemExplosionFlash.MaxParticles = 60;
                        ParticleSystemExplosionFlash.Duration = 0.001f;
                        ParticleSystemExplosionFlash.AnimateTilesPerSecond = 24;
                        ParticleSystemExplosionFlash.Animate = true;
                        ParticleSystemExplosionFlash.BlendState = BlendState.Additive;
                        ParticleSystemExplosionFlash.AlphaOverLifetime = new float[]
                            {
                                0.4f,
                                0.6f,
                                0.8f,
                                0.9f,
                                1f,
                                1f,
                                1f,
                                0.9f,
                                0.9f,
                                0.8f,
                                0.8f,
                                0.7f,
                                0.6f,
                                0.5f,
                                0.3f,
                                0.1f
                            };

                        return explosionFlashGameObject;
                    }

                case PrefabType.ExplosionDirt:
                    {
                        GameObject dirtExplosionGameObject = GameObject.Instantiate(game);
                        dirtExplosionGameObject.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(90.0f));

                        ParticleSystem ParticleSystemDirtExplosion = dirtExplosionGameObject.AddComponent<ParticleSystem>() as ParticleSystem;
                        ParticleSystemDirtExplosion.SimulationSpace = ParticleSystemSimulationSpace.World;
                        ParticleSystemDirtExplosion.PlayOnAwake = false;
                        ParticleSystemDirtExplosion.AnimateTilesX = 4;
                        ParticleSystemDirtExplosion.AnimateTilesY = 2;
                        ParticleSystemDirtExplosion.Texture = game.Content.Load<Texture2D>("dirtexplosion");
                        ParticleSystemDirtExplosion.DrawOrder = 100;
                        ParticleSystemDirtExplosion.StartSize = 4;
                        ParticleSystemDirtExplosion.EmissionRate = 10f;
                        ParticleSystemDirtExplosion.StartLifetime = 0.46f;
                        ParticleSystemDirtExplosion.MaxParticles = 20;
                        ParticleSystemDirtExplosion.Duration = 0.05f;
                        ParticleSystemDirtExplosion.Animate = true;
                        ParticleSystemDirtExplosion.AnimateTilesPerSecond = 9;

                        return dirtExplosionGameObject;
                    }
            }

            return null;
        }
    }
}