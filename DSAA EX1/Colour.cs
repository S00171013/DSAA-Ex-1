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

        //Texture2D tx;
        //Color c;
        //bool clicked = false;
        //Rectangle boundingRect;

        public string colourName;

        public Texture2D Texture { get; set; }
        public Color BoxColour { get; set; }
        public bool Clicked { get; set; }
        public Rectangle BoundingRectangle { get; set; }

        public string ColourName { get; set; }
        

        public Colour(Game gameIn, Texture2D textureIn, Color colourIn, bool clickedStatusIn, Rectangle boundingRectangleIn)
        {
            myGame = gameIn;
            Texture = textureIn;
            BoxColour = colourIn;
            Clicked = clickedStatusIn;
            BoundingRectangle = boundingRectangleIn;      
            
            if(BoxColour == Color.Blue)
            {
                ColourName = "Blue";
            }

            else if (BoxColour == Color.Red)
            {
                ColourName = "Red";
            }

            if (BoxColour == Color.Green)
            {
                ColourName = "Green";
            }

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

            spriteBatch.End();
        }
    }
}

