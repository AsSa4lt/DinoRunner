using System.Collections.Generic;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DinoRunner
{
    public class Player
    {
        public enum State
        {
            WAITING,
            RUNNING,
            JUMPING,
            DEAD
        }

        public State _playerState;
        private Texture2D _waitingTexture;
        private Texture2D _runningTexture1;
        private Texture2D _runningTexture2;
        private Texture2D _jumpingTexture;
        private Texture2D _deadTexture;
        private Texture2D _currentTexture;
        private Vector2 _position;
        private bool indexImage = false;
        private Vector2 _velocity;
        private const float Gravity = 400f;
        private Texture2D _rocketTexture;
        public List<Rocket> Rockets { get; set; }
        private double _lastShotTime = -1;
        private const double ShotCooldown = 1000; // 1000 milliseconds or 1 second


        private double _animationTimer;
        private double _animationInterval = 1000.0 / 30.0; // Update 5 times per second

        public Player(ContentManager content)
        {
            _playerState = State.WAITING;

            _waitingTexture = content.Load<Texture2D>("DINO_IDLE");
            _runningTexture1 = content.Load<Texture2D>("DINO_RUN1");
            _runningTexture2 = content.Load<Texture2D>("DINO_RUN2");
            _jumpingTexture = content.Load<Texture2D>("DINO_IDLE");
            _deadTexture = content.Load<Texture2D>("DINO_DEAD");

            _currentTexture = _waitingTexture;
            _position = new Vector2(100, 348);

            _rocketTexture = content.Load<Texture2D>("ROCKET");
            Rockets = new List<Rocket>();
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, _currentTexture.Width, _currentTexture.Height); }
        }

        public void Collide()
        {
            _playerState = State.DEAD;
            _currentTexture = _deadTexture;
        }



        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (_playerState == State.WAITING && keyboardState.IsKeyDown(Keys.Space))
            {
                _playerState = State.RUNNING;
            }

            if (_playerState == State.RUNNING && keyboardState.IsKeyDown(Keys.Space))
            {
                _playerState = State.JUMPING;
                _velocity.Y = -250f; // Jump strength, adjust as needed
            }

            if (_playerState == State.RUNNING)
            {
                _animationTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_animationTimer >= _animationInterval)
                {
                    _animationTimer = 0;
                    indexImage = !indexImage;
                    _currentTexture = indexImage ? _runningTexture1 : _runningTexture2;
                }
            }

            if (_playerState == State.JUMPING)
            {
                _position.Y += _velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _velocity.Y += Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Check if the player has landed
                if (_position.Y >= 348) // Adjust based on the ground level
                {
                    _position.Y = 348;
                    _playerState = State.RUNNING;
                    _velocity.Y = 0;
                }
            }


            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                Shoot(gameTime);
            }

            for (int i = Rockets.Count - 1; i >= 0; i--)
            {
                Rockets[i].Update(gameTime);

                if (Rockets[i].Position.X > 700)
                {
                    Rockets.RemoveAt(i);
                }
            }
            // TODO: Add update logic for other states (running, jumping, dead)
        }

        public void Shoot(GameTime gameTime)
        {
            if (_playerState != State.DEAD && (_lastShotTime == -1 || gameTime.TotalGameTime.TotalMilliseconds - _lastShotTime >= ShotCooldown))
            {
                Rockets.Add(new Rocket(_rocketTexture, new Vector2(_position.X + _currentTexture.Width, _position.Y + _currentTexture.Height / 2)));
                _lastShotTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2((float)1.5, (float)1.5);
            spriteBatch.Draw(_currentTexture, _position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            foreach (var rocket in Rockets)
            {
                rocket.Draw(spriteBatch);
            }
        }
    }

    
}
