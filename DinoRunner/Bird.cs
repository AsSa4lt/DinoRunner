using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DinoRunner
{
    public class Bird
    {
        private Texture2D _birdTexture1;
        private Texture2D _birdTexture2;
        private Vector2 _position;
        private float _animationTimer;
        private float _animationInterval = 100;
        private bool _isFirstTexture;

        public Bird(ContentManager content, Vector2 position)
        {
            _birdTexture1 = content.Load<Texture2D>("BIRD1");
            _birdTexture2 = content.Load<Texture2D>("BIRD2");
            _position = position;
            _animationTimer = 0;
            _isFirstTexture = true;
        }

        public void Update(GameTime gameTime, float speed)
        {
            _position.X -= 2*speed;
            UpdateAnimation(gameTime);
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, _birdTexture1.Width, _birdTexture1.Height); }
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            _animationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_animationTimer > _animationInterval)
            {
                _animationTimer = 0;
                _isFirstTexture = !_isFirstTexture;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float scaleFactor = 1.5f;
            Rectangle destinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, (int)(_birdTexture1.Width * scaleFactor), (int)(_birdTexture1.Height * scaleFactor));

            if (_isFirstTexture)
            {
                spriteBatch.Draw(_birdTexture1, destinationRectangle, Color.White);
            }
            else
            {
                spriteBatch.Draw(_birdTexture2, destinationRectangle, Color.White);
            }
        }


        public Vector2 Position
        {
            get { return _position; }
        }

        public float Width
        {
            get { return _birdTexture1.Width; }
        }
    }
}

