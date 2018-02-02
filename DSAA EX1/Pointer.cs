using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DSAA_Ex1
{
    class Arrow
    {
        protected Game myGame;

        public Texture2D Texture { get; set; }
        public Rectangle BoundingRectangle { get; set; }

        // Constructor
        public Arrow(Game gameIn, Texture2D textureIn, Rectangle boundingRectangleIn)
        {
            myGame = gameIn;
            Texture = textureIn;
            BoundingRectangle = boundingRectangleIn;
        }

        // Draw method.
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Texture, BoundingRectangle, Color.White);

            spriteBatch.End();
        }


    }
}
