/*  ParticleSystem.cs
 *  System to handle all particle effects in game
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.11.01: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public class ParticleSystem : Renderer, IUpdateable, IComparable<ParticleSystem>
    {
        // play and loop
        private bool _isPlaying;
        private bool _playOnAwake;
        private bool _looping;

        // time and duration
        private float _duration = 5;
        private float _time;
        private float _startDelay;

        //animation
        private int _animateTilesX = 1;
        private int _animateTilesY = 1;
        private float _animateTilesDivisionX = 1;
        private float _animateTilesDivisionY = 1;
        private float _animateTilesPerSecond = 1;
        private bool _animateParticles;

        // particles
        private float _masterAlpha = 1;
        private int _maxParticles = 10;
        private int _particleCount;
        private float _startSize = 1.0f;
        private float _startVelocity = 0;
        private bool _startRandomRotation;
        private float _angularVelocity;

        private float _startLifetime = 5;
        private float _emissionRate = 5;
        private float _emissionTime;

        private float[] _alphaOverLifeTime;

        // spawn area
        private float _spawnAreaRadius;
        private float _spawnAngle;


        private Particle[] _particles;
        private ParticleSystemSimulationSpace _simulationSpace;
        private BillboardConstraint _billboardConstraint;

        private BasicEffect _basicEffect;

        private Texture2D _texture;

        private Random random = new Random();

        private struct Particle
        {
            public VertexPositionTexture[] verts;
            public Vector3 position;
            public Vector3 velocity;
            public float angularVelocity;
            public float size;
            public float angle;
            public float life;
            public float tileFrameTime;
            public int tileFrame;
        }

        /// <summary>
        /// The BlendState of the particle system - default Blendstate.AlphaBlend
        /// </summary>
        public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

        /// <summary>
        /// Is the Particle System playing
        /// </summary>
        public bool IsPlaying { get => _isPlaying; }

        /// <summary>
        /// If set to true, the particle system will automatically start playing on startup.
        /// </summary>
        public bool PlayOnAwake { get => _playOnAwake; set => _playOnAwake = value; }

        /// <summary>
        /// If true, the emission cycle will repeat after duration.
        /// </summary>
        public bool Looping { get => _looping; set => _looping = value; }

        /// <summary>
        /// The length of time in seconds the particle system is emitting particles. It stops emitting when Time reaches zero if Looping isn't true.
        /// </summary>
        public float Duration
        {
            get => _duration;
            set => _duration = Math.Max(0, value); // make sure it's set at zero or higher
        }

        /// <summary>
        /// Playback position in seconds
        /// </summary>
        public float Time
        {
            get => _time;
            set => _time = Math.Max(0, value); // make sure it's set at zero or higher
        }

        /// <summary>
        /// Start delay in seconds.
        /// </summary>
        public float StartDelay { get => _startDelay; set => _startDelay = value; }

        /// <summary>
        /// The texture tile X division amount for animation.
        /// </summary>
        public int AnimateTilesX
        {
            get => _animateTilesX;
            set
            {
                _animateTilesX = Math.Max(1, value); // set value to 1 or higher
                _animateTilesDivisionX = 1.0f / _animateTilesX;
            }
        }

        /// <summary>
        /// The texture tile Y division amount for animation.
        /// </summary>
        public int AnimateTilesY
        {
            get => _animateTilesY;
            set
            {
                _animateTilesY = Math.Max(1, value); // set value to 1 or higher
                _animateTilesDivisionY = 1.0f / _animateTilesY;
            }
        }

        /// <summary>
        /// Set the amount of frames per second. If set to zero, animate will be set to false.
        /// </summary>
        public float AnimateTilesPerSecond
        {
            get => _animateTilesPerSecond;
            set
            {
                if (value < float.Epsilon)
                {
                    _animateParticles = false;
                }

                _animateTilesPerSecond = Math.Max(0, value);
            }
        }

        /// <summary>
        /// If the particles will animate or not
        /// </summary>
        public bool Animate { get => _animateParticles; set => _animateParticles = value; }

        /// <summary>
        /// The maximum amount of particles the system will emit. Must be minimum of 1 particle size.
        /// </summary>
        public int MaxParticles
        {
            get => _maxParticles;
            set
            {
                int newSize = Math.Max(1, value); // array must be 1 or higher

                // if this is null, the ComponentController hasn't initiated Start yet
                if (_particles != null)
                {
                    // copy array into new array of new _maxParticles size
                    Array.Copy(_particles, _particles = new Particle[newSize], _maxParticles);
                }

                _maxParticles = newSize;
            }
        }

        /// <summary>
        /// Sets the space in which to simulate the particles. World or Local space.
        /// </summary>
        public ParticleSystemSimulationSpace SimulationSpace { get => _simulationSpace; set => _simulationSpace = value; }

        /// <summary>
        /// Set the axis the billboard is constrained to
        /// </summary>
        public BillboardConstraint BillboardConstraint { get => _billboardConstraint; set => _billboardConstraint = value; }

        /// <summary>
        /// The initial size of the particles when emitted.
        /// </summary>
        public float StartSize
        {
            get => _startSize;
            set => _startSize = Math.Max(0, value); // make sure the start size is set zero or higher
        }

        /// <summary>
        /// The factor the particle's size will increase every second
        /// </summary>
        public float SizeFactorModifier { get; set; }

        /// <summary>
        /// The start speed of particles applied in the starting direction.
        /// </summary>
        public float StartVelocity { get => _startVelocity; set => _startVelocity = value; }

        /// <summary>
        /// The particle will start with a random rotation if not constrained to billboard
        /// </summary>
        public bool StartRandomRotation { get => _startRandomRotation; set => _startRandomRotation = value; }

        /// <summary>
        /// Start lifetime in seconds, the particle will destroy when it reaches zero.
        /// </summary>
        public float StartLifetime
        {
            get => _startLifetime;
            set => _startLifetime = Math.Max(0, value); // make sure the start lifetime is zero or higher
        }

        /// <summary>
        /// The rate of gravity external force (note: 9.8 meters per second is "realistic").
        /// </summary>
        public Vector3 GravityModifier { get; set; }

        /// <summary>
        /// The numbers of particles emitted per second.
        /// </summary>
        public float EmissionRate
        {
            get => _emissionRate;
            set => _emissionRate = Math.Max(0, value); // make sure the emission rate is 0 or higher
        }

        /// <summary>
        /// The master alpha of the particles between 0 and 1
        /// </summary>
        public float MasterAlpha
        {
            get => _masterAlpha;
            set => _masterAlpha = Math.Max(0, Math.Min(1, value));
        }

        /// <summary>
        /// The radius of the spawn emission area
        /// </summary>
        public float SpawnAreaRadius
        {
            get => _spawnAreaRadius;
            set => _spawnAreaRadius = Math.Max(0, value); // make sure the radius is 0 or higher
        }

        /// <summary>
        /// The angle of the spawn's outer shape - the spawn's initial direction of travel will depend on this. Min 0; Max 90
        /// </summary>
        public float SpawnAngle
        {
            get => _spawnAngle;
            set => _spawnAngle = Math.Max(0, Math.Min(90f, value));
        }

        /// <summary>
        /// The rotational velocity of each particle during its lifetime
        /// </summary>
        public float AngularVelocity
        {
            get => MathHelper.ToDegrees(_angularVelocity);
            set => _angularVelocity = MathHelper.ToRadians(value);
        }

        /// <summary>
        /// Whether the AngularVelocity direction will be randomized on particle spawn
        /// </summary>
        public bool SpawnRandomAngularVelocityDirection { get; set; }

        /// <summary>
        /// The level of Alpha over the lifetime of the particle
        /// </summary>
        public float[] AlphaOverLifetime
        {
            get => _alphaOverLifeTime;
            set => _alphaOverLifeTime = value;
        }


        private static readonly Vector3[] vertexPositions =
        {
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0)
        };


        private int _updateOrder;

        private event EventHandler<EventArgs> _updateOrderChanged;

        public Texture2D Texture
        {
            get => _texture;
            set
            {
                _texture = value;
            }
        }

        public int UpdateOrder
        {
            get
            {
                return _updateOrder;
            }
            set
            {
                _updateOrder = value;
                _updateOrderChanged?.Invoke(this, null);

                OnUpdateOrderChanged(this, null);
            }
        }

        event EventHandler<EventArgs> IUpdateable.EnabledChanged
        {
            add
            {
                _enabledChanged += value;
            }
            remove
            {
                _enabledChanged -= value;
            }
        }

        event EventHandler<EventArgs> IUpdateable.UpdateOrderChanged
        {
            add
            {
                _updateOrderChanged += value;
            }
            remove
            {
                _updateOrderChanged -= value;
            }
        }

        public ParticleSystem() : base()
        {
            
        }

        public void Play()
        {
            // start Update and Draw
            Enabled = true;
            Visible = true;

            // restart timer regardless if it was already playing
            _emissionTime = _startDelay;
            _time = _duration + _startDelay;

            // do particle emission
            DoParticleEmission();

            _isPlaying = true;
        }

        public override void Start()
        {
            if (GameObject.Game != null)
            {
                _basicEffect = new BasicEffect(GameObject.Game.GraphicsDevice)
                {
                    TextureEnabled = true,
                    Texture = _texture
                };
            }

            // set the emission time
            _emissionTime = _startDelay;

            // set the time to match the duration
            _time = _duration + _startDelay;

            if (!_playOnAwake)
            {
                // stop Update and Draw from being called
                Enabled = false;
                Visible = false;
            }
            else
            {
                _isPlaying = true;
            }
        }

        private void DoParticleEmission()
        {
            if (_emissionTime <= float.Epsilon)
            {
                // only emit if particleCount is less than maxParticles and emissionRate is not zero
                if (_particleCount < _maxParticles && _emissionRate > float.Epsilon)
                {
                    _emissionTime = 1.0f / _emissionRate;


                    // TODO: Put vertexTileRatio into a method and only call it when _animateTilesX or _animateTilesY changes (or _texture)
                    Vector3 vertexTileRatio = Vector3.One;

                    if (_texture != null)
                    {
                        // tile width/height
                        float tilesWidth = _texture.Width / _animateTilesX;
                        float tilesHeight = _texture.Height / _animateTilesY;

                        // get the ratio for the tile set width/height
                        if (tilesWidth < tilesHeight)
                        {
                            vertexTileRatio = new Vector3(tilesWidth / tilesHeight, 1, 0);
                        }
                        else if (tilesWidth > tilesHeight)
                        {
                            vertexTileRatio = new Vector3(1, tilesHeight / tilesWidth, 0);
                        }
                    }

                    // check if array has been set, or if it's not the right size for whatever reason
                    if (_particles == null || _particles.Length != _maxParticles)
                    {
                        _particles = new Particle[_maxParticles];
                    }

                    // random offset on X & Y. Between -0.5 and 0.5
                    Vector3 randomSpawnOffset = new Vector3(
                        (float)random.NextDouble() - 0.5f,
                        (float)random.NextDouble() - 0.5f,
                        0);

                    Vector3 velocity = Vector3.Zero;

                    // assign start velocity direction if _startVelocity is not zero
                    if (Math.Abs(_startVelocity) > float.Epsilon)
                    {
                        Quaternion rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(-randomSpawnOffset.X * _spawnAngle), MathHelper.ToRadians(randomSpawnOffset.Y * _spawnAngle), 0);
                        velocity = Vector3.Normalize(Vector3.Transform(Vector3.Forward * _startVelocity, Quaternion.Concatenate(rotation, Transform.Rotation))) * _startVelocity;
                    }

                    // create the Particle
                    _particles[_particleCount] = new Particle()
                    {
                        size = _startSize,
                        position = Vector3.Transform(randomSpawnOffset * _spawnAreaRadius, Transform.Rotation) + (_simulationSpace == ParticleSystemSimulationSpace.World ? Transform.Position : Vector3.Zero),
                        velocity = velocity,
                        angularVelocity = (SpawnRandomAngularVelocityDirection ? (random.Next(0, 1) == 1 ? _angularVelocity : -_angularVelocity) : _angularVelocity),
                        life = StartLifetime,
                        tileFrameTime = 1.0f / _animateTilesPerSecond,
                        tileFrame = 0,
                        angle = _startRandomRotation ? MathHelper.ToRadians(random.Next(0, 359)) : 0,

                        // 6 verts for each "square" particle quad
                        verts = new VertexPositionTexture[6]
                        {
                            new VertexPositionTexture { Position = vertexPositions[0] * vertexTileRatio * _startSize, TextureCoordinate = new Vector2(0, 0) },
                            new VertexPositionTexture { Position = vertexPositions[1] * vertexTileRatio * _startSize, TextureCoordinate = new Vector2(_animateTilesDivisionX, 0) },
                            new VertexPositionTexture { Position = vertexPositions[2] * vertexTileRatio * _startSize, TextureCoordinate = new Vector2(0, _animateTilesDivisionY) },
                            new VertexPositionTexture { Position = vertexPositions[3] * vertexTileRatio * _startSize, TextureCoordinate = new Vector2(_animateTilesDivisionX, _animateTilesDivisionY) },
                            new VertexPositionTexture { Position = vertexPositions[4] * vertexTileRatio * _startSize, TextureCoordinate = new Vector2(0, _animateTilesDivisionY) },
                            new VertexPositionTexture { Position = vertexPositions[5] * vertexTileRatio * _startSize, TextureCoordinate = new Vector2(_animateTilesDivisionX, 0) }
                        }
                    };

                    // add to Particle Count
                    _particleCount++;
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Texture.Name == "muzzlefire")
            {

            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_time > float.Epsilon)
            {
                // keep counting down
                _emissionTime -= deltaTime;
                _time -= deltaTime;

                DoParticleEmission();
            }
            else if (_looping)
            {
                // repeat the time if looping is true
                _time = _duration;
            }
            else if (_particleCount == 0)
            {
                // stop Update and Draw from being called
                Enabled = false; 
                Visible = false;

                _isPlaying = false;
            }

            // iterate through all particles, apply movement, gravity, and change animation frames
            for (int i = 0; i < _particleCount; i++)
            {
                // kill off particle if Life is lower than zero and replace this array index with last array index 
                if (_particles[i].life <= 0)
                {
                    // only swap array index if this isn't the last element
                    if (_particleCount - 1 != i)
                    {
                        _particles[i] = _particles[_particleCount - 1];

                        i--;
                    }

                    // subtract increment value to not skip this index
                    _particleCount--;

                    // move onto next particle
                    continue;
                }

                // keep counting down
                _particles[i].life -= deltaTime;

                // only increase size if the Factor "zero"
                if (Math.Abs(SizeFactorModifier) > float.Epsilon)
                {
                    // change the particle size - Absolute value so that it doesn't flip the billboard upside down
                    _particles[i].size += Math.Abs(_particles[i].size * SizeFactorModifier * deltaTime);

                    for (int j = 0; j < _particles[i].verts.Length; j++)
                    {
                        _particles[i].verts[j].Position = vertexPositions[j] * _particles[i].size;
                    }
                }

                // apply velocity to position
                _particles[i].position += _particles[i].velocity * deltaTime;
                // apply gravity to velocity
                _particles[i].velocity += GravityModifier * deltaTime;

                // apply the rotational velocity
                _particles[i].angle += _particles[i].angularVelocity * deltaTime;

                // TODO: When _animateParticles is false this doesn't seem to display a texture
                // If Animate is true, animate the Tile's texture coordinates
                if (_animateParticles)
                {
                    // only change the TileFrame if the Time is below zero
                    if (_particles[i].tileFrameTime < 0)
                    {
                        _particles[i].tileFrame = (_particles[i].tileFrame < _animateTilesX * _animateTilesY ? _particles[i].tileFrame + 1 : 0);

                        // assign the Texture Coordinates for UV positions
                        float xLeft = (_particles[i].tileFrame % _animateTilesX) * _animateTilesDivisionX;
                        float xRight = xLeft + _animateTilesDivisionX;
                        float yTop = (_particles[i].tileFrame / _animateTilesY) * _animateTilesDivisionY;
                        float yBottom = yTop + _animateTilesDivisionY;

                        // set Texture Coordinates for animation
                        _particles[i].verts[0].TextureCoordinate = new Vector2(xLeft, yTop);
                        _particles[i].verts[1].TextureCoordinate = new Vector2(xRight, yTop);
                        _particles[i].verts[2].TextureCoordinate = new Vector2(xLeft, yBottom);

                        _particles[i].verts[3].TextureCoordinate = new Vector2(xRight, yBottom);
                        _particles[i].verts[4].TextureCoordinate = new Vector2(xLeft, yBottom);
                        _particles[i].verts[5].TextureCoordinate = new Vector2(xRight, yTop);

                        // reset tileFrameTime
                        _particles[i].tileFrameTime = 1.0f / _animateTilesPerSecond;
                    }

                    // keep counting down
                    _particles[i].tileFrameTime -= deltaTime;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (_particleCount > 0)
            {
                // To draw Alpha channel sprites Turn on DepthStencilState.DepthRead & BlendState.AlphaBlend
                GameObject.Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
                GameObject.Game.GraphicsDevice.BlendState = BlendState;

                _basicEffect.FogEnabled = Game.Camera.FogEnabled;
                _basicEffect.FogColor = Game.Camera.FogColor;
                _basicEffect.FogStart = Game.Camera.FogStart;
                _basicEffect.FogEnd = Game.Camera.FogEnd;

                // set the BasicEffect's View & Projection from the Game Camera
                _basicEffect.View = GameObject.Game.Camera.View;
                _basicEffect.Projection = GameObject.Game.Camera.Projection;

                Vector3 cameraPosition = GameObject.Game.Camera.Transform.Position;
                Vector3 cameraForwardVector = GameObject.Game.Camera.Transform.Forward;

                for (int i = _particleCount - 1; i >= 0; i--)
                {
                    // only apply AlphaOverLifeTime if the array isn't null or empty
                    if (_alphaOverLifeTime != null && _alphaOverLifeTime.Length > 0)
                    {
                        // get the index of the Alpha array based on the particle's life
                        int ai = Math.Max(0, Math.Min(_alphaOverLifeTime.Length - 1, (int)((_alphaOverLifeTime.Length - 1) * (1f - (_particles[i].life / _startLifetime)))));

                        _basicEffect.Alpha = _masterAlpha * _alphaOverLifeTime[ai];
                    }
                    else
                    {
                        _basicEffect.Alpha = _masterAlpha;
                    }

                    // set the BasicEffect world - do the constrained axis billboards first
                    if (_billboardConstraint == BillboardConstraint.Horizontal)
                    {
                        _basicEffect.World = Matrix.CreateFromAxisAngle(Vector3.UnitZ, _particles[i].angle) * Matrix.CreateFromQuaternion(Transform.Rotation) * Matrix.CreateTranslation(_simulationSpace == ParticleSystemSimulationSpace.World ? _particles[i].position : Transform.Position + _particles[i].position); 
                    }
                    else if (_billboardConstraint == BillboardConstraint.Vertical)
                    {
                        _basicEffect.World = //Matrix.CreateFromAxisAngle(Vector3.UnitY, _particles[i].angle) *
                            Matrix.CreateConstrainedBillboard(_simulationSpace == ParticleSystemSimulationSpace.World ? _particles[i].position : Transform.Position + _particles[i].position, cameraPosition, Transform.Forward, cameraForwardVector, Transform.Forward);
                    }
                    else // do the non constrained billboards with rotation
                    {
                        Matrix billboard = Matrix.CreateBillboard(_simulationSpace == ParticleSystemSimulationSpace.World ? _particles[i].position : Transform.Position + _particles[i].position, cameraPosition, GameObject.Game.Camera.Transform.Up, cameraForwardVector);

                        // apply sprite rotation to the billboard
                        _basicEffect.World = Matrix.CreateFromAxisAngle(Vector3.Backward, _particles[i].angle) * billboard;
                    }

                    // iterate through the passes that may be required to draw the primitive
                    foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
                    {
                        // apply the effectpass for the current technique - must be done before assigning World position to the BasicEffect
                        pass.Apply();

                        // draw the billboard particle
                        GameObject.Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _particles[i].verts, 0, 2);
                    }
                }
                
                // Switch back to regular drawing - DepthStencilState.Default & BlendState.Opaque
                GameObject.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GameObject.Game.GraphicsDevice.BlendState = BlendState.Opaque;
            }
        }

        protected virtual void OnUpdateOrderChanged(object sender, EventArgs args)
        {

        }

        #region IComparable<ComponentController> Members

        public int CompareTo(ParticleSystem other)
        {
            return other.UpdateOrder - this.UpdateOrder;
        }

        #endregion
    }
}
