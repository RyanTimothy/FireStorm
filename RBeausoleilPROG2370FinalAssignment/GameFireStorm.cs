/*  GameFireStorm.cs
 *  Handles the game's setup
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.10.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RBeausoleilPROG2370FinalAssignment.ComponentControllers;
using RBeausoleilPROG2370FinalAssignment.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RBeausoleilPROG2370FinalAssignment
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameFireStorm : GameController
    {
        private readonly Color CLEAR_COLOR = new Color(4, 110, 140);
        private KeyboardState prevKeyboardState;
        private GamePadState prevGamepadState;
        private TankController tankController;
        private readonly SamplerState samplerStates;

        public bool GamePaused { get; set; }
        public GameObject MenuUI { get; private set; }
        public GameObject HelpMenuUI { get; private set; }
        public GameObject CreditsMenuUI { get; private set; }
        public GameObject HealthBarUI { get; private set; }
        public GameObject GameMessageUI { get; private set; }
        public ParticleSystem ParticleSystemExplosion { get; private set; }
        public ParticleSystem ParticleSystemExplosionFlash { get; private set; }
        public ParticleSystem ParticleSystemDirtExplosion { get; private set; }
        public ParticleSystem ParticleSystemSmoke { get; private set; }

        public GameObjectPrefabPool GameObjectPrefabPool { get; private set; }
        public GameObject TargetBuildingsUI { get; internal set; }

        public GameFireStorm()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080,
                SynchronizeWithVerticalRetrace = false,
            };
            Graphics.ToggleFullScreen();

            Graphics.ApplyChanges();
            IsFixedTimeStep = false;

            Content.RootDirectory = "Content";

            GameObjectPrefabPool = new GameObjectPrefabPool(this);

            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 50.0f);

            samplerStates = new SamplerState
            {
                Filter = TextureFilter.MinLinearMagPointMipLinear,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MipMapLevelOfDetailBias = -16
            };
        }

        public override void ResetSamplerStates()
        {
            GraphicsDevice.SamplerStates[0] = samplerStates;
            GraphicsDevice.SamplerStates[1] = samplerStates;
            GraphicsDevice.SamplerStates[2] = samplerStates;
            GraphicsDevice.SamplerStates[3] = samplerStates;
            GraphicsDevice.SamplerStates[4] = samplerStates;
            GraphicsDevice.SamplerStates[5] = samplerStates;
            GraphicsDevice.SamplerStates[6] = samplerStates;
            GraphicsDevice.SamplerStates[7] = samplerStates;
            GraphicsDevice.SamplerStates[8] = samplerStates;
        }

        
        protected override void Initialize()
        {
            ResetSamplerStates();

            // Player collisions
            CollisionLayer.AddCollisions((int)LayerType.Player, (int)LayerType.EnemyBullet);
            CollisionLayer.AddCollisions((int)LayerType.Player, (int)LayerType.EnemyStructure);
            CollisionLayer.AddCollisions((int)LayerType.Player, (int)LayerType.FallingDestruction);
            CollisionLayer.AddCollisions((int)LayerType.Player, (int)LayerType.Tree);
            CollisionLayer.AddCollisions((int)LayerType.Player, (int)LayerType.Wall);

            // PlayerBullet collision layers
            CollisionLayer.AddCollisions((int)LayerType.PlayerBullet, (int)LayerType.EnemyStructure);
            CollisionLayer.AddCollisions((int)LayerType.PlayerBullet, (int)LayerType.Tree);
            CollisionLayer.AddCollisions((int)LayerType.PlayerBullet, (int)LayerType.Wall);

            // Enemy bullet
            CollisionLayer.AddCollisions((int)LayerType.EnemyBullet, (int)LayerType.Tree);
            CollisionLayer.AddCollisions((int)LayerType.EnemyBullet, (int)LayerType.EnemyStructure);

            // Group Particles
            ParticleSystemExplosion = GameObjectPrefabManager.InstantiateType(PrefabType.ExplosionFire, this).GetComponent<ParticleSystem>() as ParticleSystem;
            ParticleSystemExplosionFlash = GameObjectPrefabManager.InstantiateType(PrefabType.ExplosionFlash, this).GetComponent<ParticleSystem>() as ParticleSystem;
            ParticleSystemDirtExplosion = GameObjectPrefabManager.InstantiateType(PrefabType.ExplosionDirt, this).GetComponent<ParticleSystem>() as ParticleSystem;
            GameObject.DontDestroyOnLoad(ParticleSystemExplosion.GameObject);
            GameObject.DontDestroyOnLoad(ParticleSystemExplosionFlash.GameObject);
            GameObject.DontDestroyOnLoad(ParticleSystemDirtExplosion.GameObject);

            // Camera
            GameObject cameraController = GameObject.Instantiate(this, (int)GameObjectTag.CameraController);
            GameObject.DontDestroyOnLoad(cameraController);
            cameraController.Transform.RotationLocked = true;

            GameObject cameraGameObject = GameObject.Instantiate(this, (int)GameObjectTag.Camera);
            CameraViewController cameraViewController = cameraGameObject.AddComponent<CameraViewController>() as CameraViewController;
            cameraViewController.Enabled = false;
            cameraGameObject.Transform.Position = new Vector3(0.0f, -25.0f, 60.0f);
            cameraGameObject.Transform.Rotation = Quaternion.CreateFromAxisAngle(cameraGameObject.Transform.Right, MathHelper.ToRadians(75.0f));
            Camera = cameraGameObject.AddComponent<Camera>() as Camera;
            Camera.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(40.0f), GraphicsDevice.Viewport.AspectRatio, 1.0f, 150.0f);

            cameraGameObject.Transform.Parent = cameraController;


            // Menu UI ----------------------------------------------------------------------------------------------------------------------------
            MenuUI = GameObjectPrefabManager.InstantiateType(PrefabType.MenuUI, this);
            MenuUI.Transform.Position = Camera.Transform.Position + Camera.Transform.Forward * 25 + Camera.Transform.Up * 2;
            MenuUI.Transform.Rotation = Camera.Transform.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-15.0f));
            MenuUI.Transform.Parent = cameraGameObject;
            GameObject.DontDestroyOnLoad(MenuUI);
            //-------------------------------------------------------------------------------------------------------------------------------------
            // Help Menu UI -----------------------------------------------------------------------------------------------------------------------
            HelpMenuUI = GameObjectPrefabManager.InstantiateType(PrefabType.HelpMenuUI, this);
            HelpMenuUI.Transform.Position = Camera.Transform.Position + Camera.Transform.Forward * 22;
            HelpMenuUI.Transform.Rotation = Camera.Transform.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-5.0f));
            HelpMenuUI.Transform.Parent = cameraGameObject;
            GameObject.DontDestroyOnLoad(HelpMenuUI);
            //-------------------------------------------------------------------------------------------------------------------------------------
            // Credits Menu UI --------------------------------------------------------------------------------------------------------------------
            CreditsMenuUI = GameObjectPrefabManager.InstantiateType(PrefabType.CreditMenuUI, this);
            CreditsMenuUI.Transform.Position = Camera.Transform.Position + Camera.Transform.Forward * 22;
            CreditsMenuUI.Transform.Rotation = Camera.Transform.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-5.0f));
            CreditsMenuUI.Transform.Parent = cameraGameObject;
            GameObject.DontDestroyOnLoad(CreditsMenuUI);
            //-------------------------------------------------------------------------------------------------------------------------------------
            // Health Bar UI ----------------------------------------------------------------------------------------------------------------------
            HealthBarUI = GameObjectPrefabManager.InstantiateType(PrefabType.HealthBarUI, this);
            HealthBarUI.Transform.Position = Camera.Transform.Position + Camera.Transform.Forward * 4 + Camera.Transform.Left * 2f + Camera.Transform.Down * 1.15f;
            HealthBarUI.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(87), MathHelper.ToRadians(30), MathHelper.ToRadians(60));
            HealthBarUI.Transform.Parent = cameraGameObject;
            GameObject.DontDestroyOnLoad(HealthBarUI);
            //-------------------------------------------------------------------------------------------------------------------------------------
            // Game Message UI --------------------------------------------------------------------------------------------------------------------
            GameMessageUI = GameObjectPrefabManager.InstantiateType(PrefabType.GameMessageUI, this);
            GameMessageUI.Transform.Position = Camera.Transform.Position + Camera.Transform.Forward * 14;
            GameMessageUI.Transform.Rotation = Camera.Transform.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-15.0f));
            GameMessageUI.Transform.Parent = cameraGameObject;
            GameObject.DontDestroyOnLoad(GameMessageUI);
            //-------------------------------------------------------------------------------------------------------------------------------------
            // Game Message UI --------------------------------------------------------------------------------------------------------------------
            TargetBuildingsUI = GameObjectPrefabManager.InstantiateType(PrefabType.TargetBuildingsUI, this);
            TargetBuildingsUI.Transform.Position = Camera.Transform.Position + Camera.Transform.Forward * 22 + Camera.Transform.Up * 7.5f;
            TargetBuildingsUI.Transform.Rotation = Camera.Transform.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-15.0f));
            TargetBuildingsUI.Transform.Parent = cameraGameObject;
            GameObject.DontDestroyOnLoad(GameMessageUI);
            //-------------------------------------------------------------------------------------------------------------------------------------

            GameObject tank = GameObjectPrefabManager.InstantiateType(PrefabType.Tank, this);
            tankController = tank.GetComponent<TankController>();
            GameObject.DontDestroyOnLoad(tank);
            tank.SetActive(false);

            SetScene<LoadingScene>();

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            GameObjectPrefabManager.LoadContent(this);

            base.LoadContent();
        }

        
        protected override void UnloadContent()
        {
            Content.Unload();
        }


        protected override void OnSceneChange()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            if ((gamepadState.Buttons.Back == ButtonState.Pressed && prevGamepadState.Buttons.Back == ButtonState.Released)
                || (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape)))
            {
                if (!MenuUI.ActiveInGame && !HelpMenuUI.ActiveInGame && !CreditsMenuUI.ActiveInGame)
                {
                    HealthBarUI.SetActive(false);
                    GameMessageUI.SetActive(false);

                    MenuUI.SetActive(true);
                    MenuUI.Transform.Position = Camera.Transform.Position + Camera.Transform.Forward * 25 + Camera.Transform.Up * 2;
                    MenuUI.Transform.Rotation = Camera.Transform.Rotation * Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.ToRadians(-15.0f));

                    (MenuUI.GetComponent<CanvasRenderer>() as CanvasRenderer).RenderMode = RenderMode.ScreenSpaceOverlay;
                }
                else
                {
                    if (MenuUI.ActiveInGame && ActivevSceneType != typeof(LoadingScene) && !tankController.GameOver)
                    {
                        MenuUI.SetActive(false);
                        HealthBarUI.SetActive(true);

                        if (tankController.IsDead)
                        {
                            GameMessageUI.SetActive(true);
                        }
                    }
                    else if (HelpMenuUI.ActiveInGame)
                    {
                        HelpMenuUI.SetActive(false);
                        MenuUI.SetActive(true);
                    }
                    else if (CreditsMenuUI.ActiveInGame)
                    {
                        CreditsMenuUI.SetActive(false);
                        MenuUI.SetActive(true);
                    }
                }
            }
            else if (keyboardState.IsKeyDown(Keys.F11) && prevKeyboardState.IsKeyDown(Keys.F11))
            {
                Graphics.ToggleFullScreen();
            }

            if (!GamePaused)
            {
                base.Update(gameTime);
            }
            else
            {
                CanvasEventSystem[] canvasEventSystems = FindObjectsByType<CanvasEventSystem>();

                for (int i = 0; i < canvasEventSystems.Length; i++)
                {
                    canvasEventSystems[i].Update(gameTime);
                }

                CanvasRenderer[] canvases = FindObjectsByType<CanvasRenderer>();

                for (int i = 0; i < canvases.Length; i++)
                {
                    canvases[i].DrawRenderTarget2D();
                }
            }

            prevKeyboardState = keyboardState;
            prevGamepadState = gamepadState;
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(CLEAR_COLOR);

            base.Draw(gameTime);
        }
    }
}