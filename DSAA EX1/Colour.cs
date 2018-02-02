using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace DSAA_Ex1
{   
    class Colour
    {
        
        protected Game myGame;

        
        // Set up private field's for the colour's center and where the ID will be displayed.
        private Vector2 textPosition;
        private Vector2 MiddlePoint;

        public Texture2D Texture { get; set; }
        public Color BoxColour { get; set; }
        public bool Clicked { get; set; }
        public Rectangle BoundingRectangle { get; set; }
        public int ID { get; set; }

        public string ColourName { get; set; }
        public SpriteFont ValueFont { get; set; }

        

        public Colour(Game gameIn, Texture2D textureIn, Color colourIn, bool clickedStatusIn, Rectangle boundingRectangleIn, string colourNameIn, int IDIn, SpriteFont fontIn)
        {
            myGame = gameIn;
            Texture = textureIn;
            BoxColour = colourIn;
            Clicked = clickedStatusIn;
            BoundingRectangle = boundingRectangleIn;
            ColourName = colourNameIn;
            ID = IDIn;
            ValueFont = fontIn;

            // Attempting to get the middle point of a colour
            MiddlePoint = new Vector2(BoundingRectangle.X + BoundingRectangle.Width / 4, BoundingRectangle.Y + BoundingRectangle.Height / 4);

            // Get the size of the text.
            Vector2 textSize = ValueFont.MeasureString(ID.ToString());
            
            // Getting the text position based on the center point of the colour.
            textPosition = new Vector2((int)(MiddlePoint.X - textSize.X), (int)(MiddlePoint.Y - textSize.Y));
            

        }

        // Have an update and a draw method! Handle logic in the game 1!

        public void Update(GameTime gameTime)
        {
            // Declare variable to keep track of mouse state.
            var mouseState = Mouse.GetState();

            var mousePosition = new Point(mouseState.X, mouseState.Y);

            // If the user clicks on the colour...
            if (mouseState.LeftButton == ButtonState.Pressed && BoundingRectangle.Contains(mousePosition))
            {
                // ...This colour has been selected by the player.
                Clicked = true;          
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Texture, BoundingRectangle, BoxColour);

            // Draw the colour's value on top of the colour.
            spriteBatch.DrawString(ValueFont, Convert.ToString(ID) , MiddlePoint, Color.Black);

            spriteBatch.End();
        }
    }
}

