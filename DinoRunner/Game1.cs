using System;
using System.Collections.Generic;
using DinoRunner;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DinoRunner
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int _gameSpeed;
        private int _gameScore;
        private Player _player;
        private Ground _ground;
        private List<Obstacle> _obstacles;
        private double _obstacleSpawnTimer;
        private double _obstacleSpawnInterval = 1000;
        private SpriteFont _scoreFont;
        private const int MinObstacleSpawnInterval = 1200;
        private const int MaxObstacleSpawnInterval = 2000;
        private Texture2D _backgroundTexture;
        private List<Bird> _birds;
        private double _birdSpawnTimer;
        private double _birdSpawnInterval = 3000;
        private const int MinBirdSpawnInterval = 1000;
        private const int MaxBirdSpawnInterval = 2500;


        private enum GameState
        {
            WAITING,
            PLAYTING,
            RESTARTING
        }

        GameState _gameState = GameState.WAITING;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 700; // Set desired width
            _graphics.PreferredBackBufferHeight = 480;

            // Set target elapsed time to 60 updates per second (1 second / 60 = 16.66667 ms)
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0 / 60.0);
        }


        protected override void Initialize()
        {
            _gameScore = 0;
            _obstacles = new List<Obstacle>();
            _birds = new List<Bird>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player = new Player(Content);
            _ground = new Ground(Content);


            _obstacles = new List<Obstacle>();
            _obstacleSpawnTimer = 0;

            _birds = new List<Bird>();
            _birdSpawnTimer = 0;

            _scoreFont = Content.Load<SpriteFont>("Font");
            _backgroundTexture = Content.Load<Texture2D>("BACKGROUND");

        }

        private void UpdateObstacles(GameTime gameTime) //Update Obstacles
        {
            _obstacleSpawnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_obstacleSpawnTimer > _obstacleSpawnInterval)
            {
                _obstacleSpawnTimer = 0;

                // Generate a random interval between the minimum and maximum spawn interval constants
                Random random = new Random();
                _obstacleSpawnInterval = random.Next(MinObstacleSpawnInterval, MaxObstacleSpawnInterval);

                // Generate a random obstacle type (1 or 2)
                int obstacleType = random.Next(1, 3);

                _obstacles.Add(new Obstacle(Content, new Vector2(GraphicsDevice.Viewport.Width, 364), obstacleType));
            }

            for (int i = _obstacles.Count - 1; i >= 0; i--)
            {
                _obstacles[i].Update(_gameSpeed);

                if (_obstacles[i].Position.X < -_obstacles[i].Width)
                {
                    _obstacles.RemoveAt(i);
                }
            }

            // Check for collisions
            foreach (var obstacle in _obstacles)
            {
                if (_player.Bounds.Intersects(obstacle.Bounds))
                {
                    // Collision detected, call the Collide method
                    _player.Collide();

                    if (_player.Health <= 0)
                    {
                        _gameState = GameState.RESTARTING;
                        break;
                    }
                }
            }
        }

        private void UpdateBirds(GameTime gameTime)
        {
            _birdSpawnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_birdSpawnTimer > _birdSpawnInterval)
            {
                _birdSpawnTimer = 0;

                Random random = new Random();
                _birdSpawnInterval = random.Next(MinBirdSpawnInterval, MaxBirdSpawnInterval);

                _birds.Add(new Bird(Content, new Vector2(GraphicsDevice.Viewport.Width, 280)));
            }

            for (int i = _birds.Count - 1; i >= 0; i--)
            {
                _birds[i].Update(gameTime, _gameSpeed);

                if (_birds[i].Position.X < -_birds[i].Width)
                {
                    _birds.RemoveAt(i);
                }
            }

            // Check for collisions with birds
            foreach (var bird in _birds)
            {
                if (_player.Bounds.Intersects(bird.Bounds))
                {
                    // Collision detected, call the Collide method
                    _player.Collide();

                    if (_player.Health <= 0)
                    {
                        _gameState = GameState.RESTARTING;
                        break;
                    }
                }
            }
        }

        private void CheckRocketBirdCollisions()
        {
            for (int i = _player.Rockets.Count - 1; i >= 0; i--)
            {
                var rocket = _player.Rockets[i];

                for (int j = _birds.Count - 1; j >= 0; j--)
                {
                    var bird = _birds[j];

                    if (rocket.Bounds.Intersects(bird.Bounds))
                    {
                        // Collision detected, remove bird and rocket
                        _birds.RemoveAt(j);
                        _player.Rockets.RemoveAt(i);
                        break;
                    }
                }
            }
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_gameState == GameState.PLAYTING)
            {
                _player.Update(gameTime);


                UpdateObstacles(gameTime);

                UpdateBirds(gameTime);

                CheckRocketBirdCollisions();


                _gameSpeed = 5 + _gameScore / 100000;
                _gameScore += 1;
            }
            else if (_gameState == GameState.RESTARTING)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    Initialize();
                    LoadContent();
                    _gameState = GameState.PLAYTING;
                    _player._playerState = Player.State.RUNNING;
                }


            }
            else if (_gameState == GameState.WAITING)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Initialize();
                    LoadContent();
                    _gameState = GameState.PLAYTING;
                    _player._playerState = Player.State.RUNNING;
                }
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);


            _spriteBatch.Begin();
            _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);


            _ground.Draw(_spriteBatch); // Move this line here
            _player.Draw(_spriteBatch);

            if (_gameState == GameState.PLAYTING)
            {
                foreach (var obstacle in _obstacles)
                {
                    obstacle.Draw(_spriteBatch);
                }
            }

            foreach (var bird in _birds)
            {
                bird.Draw(_spriteBatch);
            }

            if (_gameState == GameState.PLAYTING || _gameState == GameState.RESTARTING)
                _spriteBatch.DrawString(_scoreFont, $"Score: {_gameScore}", new Vector2(10, 10), Color.Black);

            if (_gameState == GameState.WAITING)
                _spriteBatch.DrawString(_scoreFont, "PRESS ENTER TO START", new Vector2(100, 320), Color.Black);
            else if (_gameState == GameState.RESTARTING)
                _spriteBatch.DrawString(_scoreFont, "PRESS R TO RESTART", new Vector2(100, 320), Color.Black);


            for (int i = 0; i < _player.Health; i++)
            {
                _spriteBatch.Draw(_player._heartTexture, new Vector2(10 + i * 60, _graphics.PreferredBackBufferHeight - 60), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}