using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DinoRunner
{
    public class Obstacle
    {
        private Texture2D _texture;
        public Vector2 _position;

        public Obstacle(ContentManager content, Vector2 position, int obstacleType)
        {
            if (obstacleType == 1)
            {
                _texture = content.Load<Texture2D>("OBSTACLE1");
            }
            else if (obstacleType == 2)
            {
                _texture = content.Load<Texture2D>("OBSTACLE2");
            }
            _position = position;
        }

        public void Update(float speed)
        {
            _position.X -= speed;
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height); }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public Vector2 Position
        {
            get { return _position; }
        }

        public float Width
        {
            get { return _texture.Width; }
        }

    }
}

