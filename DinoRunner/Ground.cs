using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace DinoRunner
{
    public class Ground
    {
        private Texture2D _texture;
        private List<Vector2> _positions;

        public Ground(ContentManager content)
        {
            _texture = content.Load<Texture2D>("GROUND");
            _positions = new List<Vector2>();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < 25; i++)
            {
                Vector2 vector1 = new Vector2(i * 32, 396);
                Vector2 vector2 = new Vector2(i * 32, 424);
                Vector2 vector3 = new Vector2(i * 32, 452);
                spriteBatch.Draw(_texture, vector3, Color.White);
                spriteBatch.Draw(_texture, vector2, Color.White);
                spriteBatch.Draw(_texture, vector1, Color.White);
            }
        }
    }



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

