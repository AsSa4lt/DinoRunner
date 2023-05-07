using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DinoRunner
{
    public class Rocket
    {
        public Vector2 Position;

        private Texture2D _texture;
        private float _speed;

        public Rocket(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
            _speed = 400f;
        }

        public void Update(GameTime gameTime)
        {
            Position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height); }
        }
    }
}

