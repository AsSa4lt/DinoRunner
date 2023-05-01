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
}

