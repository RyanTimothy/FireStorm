/*  TankController.cs
 *  Handles the player's input and game features
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using CustomXna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using RBeausoleilPROG2370FinalAssignment.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment.ComponentControllers
{
    public class TankController : ComponentController
    {
        private KeyboardState newKeyState;
        private int _playerHealth = START_HEALTH;

        private HealthBarController healthBarController;

        private Vector3 tankAmbientLightColor;
        private Vector3 turretAmbientLightColor;

        private Vector3 flashDamageColor = new Vector3(4.4f, 1.8f, 1.6f);
        public ModelRenderer TankModelRenderer { get; private set; }
        public ModelRenderer TurretModelRenderer { get; private set; }

        private float hitEffectTimer = -1;

        private ParticleSystem particleSystemExplosion;
        private ParticleSystem particleSystemSmoke;

        private RigidBody2D rigidBody2D;
        private CameraViewController cameraViewController;

        public TerrainTextureGrid2D TerrainTextureGrid2D { get; set; }

        public SoundEffectInstance DrivingSoundEffect { get; set; }
        public SoundEffectInstance TankCollisionSoundEffect { get; set; }
        public SoundEffectInstance TankDrivingOverConcreteSoundEffect { get; set; }

        private bool spawning = true;
        private float sinkingHeight;
        private bool isSinking;
        public bool IsDead { get; private set; }
        private float respawnTimer;

        public bool HandleUserInput { get; set; } = false;
        public bool GameOver { get; private set; }
        public bool LevelComplete { get; private set; }

        public int PlayerHealth
        {
            get => _playerHealth;
            set
            {
                _playerHealth = value;

                healthBarController.AdjustHealthBar(PlayerHealth, START_HEALTH);
            }
        }

        private int _playerLives = START_LIVES;
        public int PlayerLives
        {
            get => _playerLives;
            private set
            {
                _playerLives = value;

                GameObject healthBarUI = (Game as GameFireStorm).HealthBarUI;
                CanvasText[] text = healthBarUI.GetComponents<CanvasText>();

                for (int i = 0; i < text.Length; i++)
                {
                    text[i].Text = _playerLives.ToString();
                }
            }
        }

        private Transform camera;
        private Transform cameraController;
        private Transform tankTurret;

        private Vector2 previousPosition;
        private Vector2 velocityOffset;
        private float velocityOffsetLength;

        private const float MAX_CAMERA_IDLE_POSITION = 6f;
        private const float SECONDS_UNTIL_IDLE_CAMERA = 2.5f;
        private const float CAMERA_FOLLOW_HEIGHT = 50f;
        private const float CAMERA_IDLE_LESS_HEIGHT = 35f;
        private const float CAMERA_OFFSET_MAX_LENGTH_Z = 15f;

        private const float TANK_FORWARD_SPEED = 10f;
        private const float TANK_REVERSE_SPEED = 7f;
        private const float TANK_ROTATE_TORQUE = 25f;

        public const int START_HEALTH = 23;
        public const int START_LIVES = 3;

        private const float RESPAWN_TIME = 3f;

        private bool cameraIsIdle = true;
        private float idleTimer = SECONDS_UNTIL_IDLE_CAMERA;
        private float idleCameraPosition = MAX_CAMERA_IDLE_POSITION;
        private Vector2 grid2DPosition;
        private float SINKING_UNIT_PERSECOND = 0.2f;

        public override void Start()
        {
            rigidBody2D = GameObject.GetComponent<RigidBody2D>();
            rigidBody2D.LinearDrag = 8;
            rigidBody2D.AngularDrag = 12;

            healthBarController = FindObjectByType<HealthBarController>();

            TankModelRenderer = GameObject.GetComponent<ModelRenderer>();
            TurretModelRenderer = GameObject.GetComponentInChildren<ModelRenderer>();

            tankAmbientLightColor = TankModelRenderer?.AmbientLightColor ?? Vector3.Zero;
            turretAmbientLightColor = TurretModelRenderer?.AmbientLightColor ?? Vector3.Zero;

            particleSystemExplosion = (GameObject.Game as GameFireStorm).ParticleSystemExplosion;
            particleSystemSmoke = (GameObject.Game as GameFireStorm).ParticleSystemSmoke;

            TankDrivingOverConcreteSoundEffect.IsLooped = true;

            DrivingSoundEffect.IsLooped = true;

            TankCollisionSoundEffect.Volume = 0.2f;

            camera = FindGameObjectByTag((int)GameObjectTag.Camera)?.Transform;

            tankTurret = FindGameObjectByTag((int)GameObjectTag.TankTurret)?.Transform;
            cameraController = FindGameObjectByTag((int)GameObjectTag.CameraController)?.Transform;

            cameraViewController = FindObjectByType<CameraViewController>();

            DrivingSoundEffect.Volume = 0.0f;
            DrivingSoundEffect.Play();
        }

        public void Restart()
        {
            previousPosition = new Vector2(Transform.Position.X, Transform.Position.Y);
            cameraViewController.HeightOffset = idleCameraPosition * 0.5f;

            PlayerLives = START_LIVES;

            camera.LocalPosition = new Vector3(0.0f, -25.0f, 60.0f);

            cameraIsIdle = true;
            idleTimer = SECONDS_UNTIL_IDLE_CAMERA;
            idleCameraPosition = MAX_CAMERA_IDLE_POSITION;

            rigidBody2D.Velocity = Vector2.Zero;
            rigidBody2D.AngularVelocity = 0;

            velocityOffsetLength = 0;

            GameOver = false;
            LevelComplete = false;

            IsDead = false;
            isSinking = false;
            ResetTankHitColor();

            UpdateTargetBuildingCount();
        }

        public void UpdateTargetBuildingCount()
        {
            GameObject[] targetBuildings = FindGameObjectsByTag((int)GameObjectTag.TargetBuilding);

            GameObject targetBuildingsUI = (Game as GameFireStorm).TargetBuildingsUI;

            if (targetBuildings.Length != 0)
            {
                CanvasRenderer canvasRenderer = targetBuildingsUI.GetComponent<CanvasRenderer>();
                CanvasText[] text = targetBuildingsUI.GetComponents<CanvasText>();

                string message;

                if (targetBuildings.Length != 1)
                {
                    message = $"{targetBuildings.Length} Buildings Remaining";
                }
                else
                {
                    message = $"{targetBuildings.Length} Building Remaining";
                }

                for (int i = 0; i < text.Length; i++)
                {
                    float fontWidth = text[i].SpriteFont.MeasureString(message).X;

                    text[i].Position = new Vector2((float)Math.Floor((canvasRenderer.BufferWidth - fontWidth) * 0.5f), text[i].Position.Y);
                    text[i].Text = message;
                }

                targetBuildingsUI.GetComponent<CanvasFadeController>().Show(3);
            }
            else
            {
                targetBuildingsUI.SetActive(false);
            }

            if (targetBuildings.Length == 0)
            {
                LevelComplete = true;
                DisplayGameMessage("Level Completed!");
                respawnTimer = 4;
                HandleUserInput = false;
                cameraIsIdle = true;
                IsDead = true;
            }
        }

        protected override void OnEnable()
        {
            grid2DPosition = Vector2.Zero;
            
            previousPosition = new Vector2(Transform.Position.X, Transform.Position.Y);

            cameraIsIdle = true;
            idleTimer = SECONDS_UNTIL_IDLE_CAMERA;
            idleCameraPosition = MAX_CAMERA_IDLE_POSITION;

            rigidBody2D.Velocity = Vector2.Zero;
            rigidBody2D.AngularVelocity = 0;

            velocityOffsetLength = 0;

            IsDead = false;
            isSinking = false;
            ResetTankHitColor();
        }

        public void SpawnComplete()
        {
            PlayerHealth = START_HEALTH;
            previousPosition = new Vector2(Transform.Position.X, Transform.Position.Y);

            spawning = false;
            HandleUserInput = true;
            IsDead = false;

            (Game as GameFireStorm).HealthBarUI.SetActive(true);
        }

        private void DisplayGameMessage(string message)
        {
            GameObject gameMessageUI = (Game as GameFireStorm).GameMessageUI;
            CanvasRenderer canvasRenderer = gameMessageUI.GetComponent<CanvasRenderer>();
            CanvasText[] text = gameMessageUI.GetComponents<CanvasText>();

            for (int i = 0; i < text.Length; i++)
            {
                float fontWidth = text[i].SpriteFont.MeasureString(message).X;

                text[i].Position = new Vector2((float)Math.Floor((canvasRenderer.BufferWidth - fontWidth) * 0.5f), text[i].Position.Y);
                text[i].Text = message;
            }

            gameMessageUI.SetActive(true);

            gameMessageUI.GetComponent<CanvasFadeController>().Show();
        }

        private void KillPlayer()
        {
            if (PlayerLives <= 0)
            {
                GameOver = true;
                DisplayGameMessage("Game Over!");
            }
            else
            {
                DisplayGameMessage("You died!");
            }

            PlayerHealth = 0;

            HandleUserInput = false;
            cameraIsIdle = true;
            rigidBody2D.Velocity = Vector2.Zero;
            rigidBody2D.AngularVelocity = 0;
            ResetTankHitColor();

            TankModelRenderer.Visible = false;
            TurretModelRenderer.Visible = false;

            IsDead = true;
            isSinking = false;
            respawnTimer = RESPAWN_TIME;
        }

        public override void OnCollisionEnter2D(Collider2D sender)
        {
            if (!IsDead && sender is Collider2D c)
            {
                if (c.GameObject.Static)
                {
                    TankCollisionSoundEffect.Play();
                }
                else if (sender.GameObject.Layer == (int)LayerType.EnemyBullet && PlayerHealth > 0)
                {
                    PlayerHealth--;

                    if (PlayerHealth <= 0)
                    {
                        KillPlayer();
                    }
                    else if (TankModelRenderer != null && TurretModelRenderer != null)
                    {
                        hitEffectTimer = 0.06f;

                        TankModelRenderer.AmbientLightColor = flashDamageColor;
                        TurretModelRenderer.AmbientLightColor = flashDamageColor;
                    }
                }
            }
        }

        public override void OnTriggerEnter2D(Collider2D sender)
        {
            if (sender.GameObject.Tag == (int)GameObjectTag.ConcreteRubble)
            {
                TankDrivingOverConcreteSoundEffect.Play();
            }
        }

        public override void OnTriggerStay2D(Collider2D sender)
        {
            // just in case there's an overlapping trigger that caused the SoundEffect to stop while still over another - check if it's not playing
            if (sender.GameObject.Tag == (int)GameObjectTag.ConcreteRubble && TankDrivingOverConcreteSoundEffect.State != SoundState.Playing)
            {
                TankDrivingOverConcreteSoundEffect.Play();
            }
        }

        public override void OnTriggerExit2D(Collider2D sender)
        {
            if (sender.GameObject.Tag == (int)GameObjectTag.ConcreteRubble)
            {
                TankDrivingOverConcreteSoundEffect.Pause();
            }
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            DoDrivingSoundEffects();

            if (IsDead)
            {
                respawnTimer -= deltaTime;

                if (respawnTimer <= 0)
                {
                    if (GameOver || LevelComplete)
                    {
                        (Game as GameFireStorm).TargetBuildingsUI.SetActive(false);
                        (Game as GameFireStorm).GameMessageUI.SetActive(false);
                        (Game as GameFireStorm).HealthBarUI.SetActive(false);

                        if (GameOver || (Game as GameFireStorm).ActivevSceneType == typeof(Level02))
                        {
                            (Game as GameFireStorm).MenuUI.SetActive(true);
                        }
                        else if((Game as GameFireStorm).ActivevSceneType == typeof(Level01))
                        {
                            Game.SetScene<Level02>();
                        }
                    }
                    else
                    {
                        IsDead = false;
                        PlayerLives--;
                        FindObjectByType<BaseController>().StartSpawn();
                        (Game as GameFireStorm).GameMessageUI.GetComponent<CanvasFadeController>().Hide();
                    }
                }
            }
            else
            {
                if (isSinking)
                {
                    Vector3 sinkingPosition = new Vector3(Transform.Position.X, Transform.Position.Y, sinkingHeight);

                    // get the duration rate of the sinking speed
                    float duration = Math.Abs(sinkingHeight - Transform.Position.Z) / SINKING_UNIT_PERSECOND * deltaTime;

                    Transform.Position = Vector3.Lerp(Transform.Position, sinkingPosition, duration);

                    if (Transform.Position.Z >= 0)
                    {
                        Transform.Position = new Vector3(Transform.Position.X, Transform.Position.Y, 0);
                        isSinking = false;
                    }

                    if (Transform.Position.Z <= -2.6)
                    {
                        isSinking = false;
                        KillPlayer();
                    }
                }

                DoGrid2DPositionAndTileTypeCheck();

                // Handle user input - false if in cutscene or menu is open
                if (HandleUserInput)
                {
                    DoInputControls(deltaTime);
                }
            }

            DoCameraIdleOrFollow(deltaTime); 
        }


        private void DoGrid2DPositionAndTileTypeCheck()
        {
            if (TerrainTextureGrid2D != null)
            {
                Vector3 positionDifference = TerrainTextureGrid2D.Transform.Position - Transform.Position;
                Vector2 newGridPosition = new Vector2((float)Math.Floor(-positionDifference.X) / TerrainTextureGrid2D.Transform.Scale, (float)Math.Floor(positionDifference.Y) / TerrainTextureGrid2D.Transform.Scale);

                if (newGridPosition != grid2DPosition)
                {
                    int? tileType = TerrainTextureGrid2D.GetTileType((int)newGridPosition.X, (int)newGridPosition.Y);

                    if (tileType != null && Enum.IsDefined(typeof(TileType), tileType + 1))
                    {
                        if (TileTypeBaseDictionary.GetBaseTileType((TileType)(tileType + 1)) == BaseTileType.DeepWater)
                        {
                            SetSinkingDepth(-3.0f);
                        }
                        else if (TileTypeBaseDictionary.GetBaseTileType((TileType)(tileType + 1)) == BaseTileType.Water)
                        {
                            SetSinkingDepth(-0.85f);
                        }
                        else if (TileTypeBaseDictionary.GetBaseTileType((TileType)(tileType + 1)) == BaseTileType.WaterEdge)
                        {
                            SetSinkingDepth(-0.4f);
                        }
                        else if (TileTypeBaseDictionary.GetBaseTileType((TileType)(tileType + 1)) == BaseTileType.Land && isSinking)
                        {
                            SetSinkingDepth(0.01f);
                        }
                    }
                }
            }
        }


        private void SetSinkingDepth(float sinkingHeight)
        {
            if (!IsDead)
            {
                this.sinkingHeight = sinkingHeight;

                isSinking = true;
            }
        }


        private void DoInputControls(float deltaTime)
        {
            newKeyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            // Move tank - Forward & Backward
            if (newKeyState.IsKeyDown(Keys.W) || (gamePadState.IsConnected && gamePadState.ThumbSticks.Left.Y > 0.1f))
            {
                if (newKeyState.IsKeyDown(Keys.W))
                {
                    rigidBody2D.AddVelocity(Vector2.UnitY * TANK_FORWARD_SPEED * deltaTime);
                }
                else
                {
                    rigidBody2D.AddVelocity(Vector2.UnitY * TANK_FORWARD_SPEED * gamePadState.ThumbSticks.Left.Y * deltaTime);
                }
            }
            if (newKeyState.IsKeyDown(Keys.S) || (gamePadState.IsConnected && gamePadState.ThumbSticks.Left.Y < -0.1f))
            {
                if (newKeyState.IsKeyDown(Keys.S))
                {
                    rigidBody2D.AddVelocity(-Vector2.UnitY * TANK_REVERSE_SPEED * deltaTime);
                }
                else
                {
                    rigidBody2D.AddVelocity(Vector2.UnitY * TANK_REVERSE_SPEED * gamePadState.ThumbSticks.Left.Y * deltaTime);
                }
            }

            // Rotate tank - Left & Right
            if (newKeyState.IsKeyDown(Keys.A) || (gamePadState.IsConnected && gamePadState.ThumbSticks.Left.X < -0.1f))
            {
                if (newKeyState.IsKeyDown(Keys.A))
                {
                    rigidBody2D.AddTorque(TANK_ROTATE_TORQUE * deltaTime);
                }
                else
                {
                    rigidBody2D.AddTorque(TANK_ROTATE_TORQUE * -gamePadState.ThumbSticks.Left.X * deltaTime);
                }
            }
            if (newKeyState.IsKeyDown(Keys.D) || (gamePadState.IsConnected && gamePadState.ThumbSticks.Left.X > 0.1f))
            {
                if (newKeyState.IsKeyDown(Keys.D))
                {
                    rigidBody2D.AddTorque(-TANK_ROTATE_TORQUE * deltaTime);
                }
                else
                {
                    rigidBody2D.AddTorque(TANK_ROTATE_TORQUE * -gamePadState.ThumbSticks.Left.X * deltaTime);
                }
            }

            if (hitEffectTimer > 0)
            {
                hitEffectTimer -= deltaTime;

                if (hitEffectTimer <= 0)
                {
                    ResetTankHitColor();
                }
            }
        }


        private void ResetTankHitColor()
        {
            hitEffectTimer = -1;

            if (TankModelRenderer != null && TurretModelRenderer != null)
            {
                TankModelRenderer.AmbientLightColor = tankAmbientLightColor;
                TurretModelRenderer.AmbientLightColor = turretAmbientLightColor;
            }
        }


        private void DoCameraIdleOrFollow(float deltaTime)
        {
            if (!IsDead)
            {
                // move the camera to follow based on speed and direction of travel - or transition into idle camera
                Vector2 newPosition = new Vector2(Transform.Position.X, Transform.Position.Y);
                Vector2 positionDifference = newPosition - previousPosition;
                previousPosition = newPosition;

                float positionDifferenceLength = positionDifference.Length();

                velocityOffset = Vector2.Lerp(velocityOffset + positionDifference, Vector2.Zero, deltaTime);
                velocityOffsetLength = velocityOffset.Length();

                if (!spawning && (positionDifferenceLength >= 0.001f || Math.Abs(rigidBody2D.AngularVelocity) > 0.001f))
                {
                    StopCameraIdle();
                    idleTimer = 0;
                }
                else if (!cameraIsIdle || idleCameraPosition <= float.Epsilon)
                {
                    idleTimer += deltaTime;

                    if (idleTimer >= SECONDS_UNTIL_IDLE_CAMERA)
                    {
                        cameraIsIdle = true;
                    }
                }
            }

            if (cameraIsIdle)
            {
                idleCameraPosition = Math.Min(MAX_CAMERA_IDLE_POSITION, idleCameraPosition + deltaTime * 1.7f);
            }
            else
            {
                idleCameraPosition = idleCameraPosition - deltaTime * 3.5f;

                if (idleCameraPosition <= 0)
                {
                    cameraIsIdle = false;
                    idleCameraPosition = 0;
                }
            }

            cameraViewController.HeightOffset = idleCameraPosition * 0.5f;

            if (cameraIsIdle || idleCameraPosition >= 0.01f)
            {
                float amount = idleCameraPosition / MAX_CAMERA_IDLE_POSITION;

                camera.Position = new Vector3(camera.Position.X, camera.Position.Y, Math.Min(70, CAMERA_FOLLOW_HEIGHT - (CAMERA_IDLE_LESS_HEIGHT * amount) + velocityOffsetLength));

                cameraController.Position = Transform.Position + (Vector3.Down * 2.5f * amount) + ((tankTurret.Forward * 1.2f + Transform.Forward * 1.6f) * (1 - amount));
            }
            else
            {
                camera.Position = new Vector3(camera.Position.X, camera.Position.Y, Math.Min(70, CAMERA_FOLLOW_HEIGHT + velocityOffsetLength));

                cameraController.Position = Transform.Position + tankTurret.Forward * 1.2f + Transform.Forward * 1.6f;
            }
        }

        public void StopCameraIdle()
        {
            cameraIsIdle = false;
            idleTimer = 0;
        }

        private void DoDrivingSoundEffects()
        {
            // get the Velocity/Angular percentages for the Tank idle/accel volume
            float velocityVolume = 0.1f * (rigidBody2D.Velocity.Length() / 0.1f);
            float angularVelocityVolume = 0.25f * (Math.Abs(rigidBody2D.AngularVelocity) / 1.3f);
            float velocityVolumePercentage = Math.Max(0, Math.Min(1, Math.Max(velocityVolume, angularVelocityVolume)));

            DrivingSoundEffect.Volume = Math.Max(0.01f, velocityVolumePercentage);

            if (TankDrivingOverConcreteSoundEffect.State == SoundState.Playing)
            {
                TankDrivingOverConcreteSoundEffect.Volume = velocityVolumePercentage;
            }
        }
    }
}
