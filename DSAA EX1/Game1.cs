using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DSAA_Ex1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public enum gameState { PLAYERMOVE, CPUMOVE, WIN, LOSE, GAMEOVER }

        public gameState currentState;

        // Set up random generator.
        Random randomGenerator = new Random();

        // ENUM version
        // enum Color {BLUE, GREEN, RED}

        // Have an array for the colours that the player can choose. 
        Texture2D blankBox;

        // Colour Management.
        string[] colourNames = { "Blue", "Red", "Green", "Purple", "Yellow" };
        Color[] colours = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Yellow };

        int positionX = 0;
        int positionY = 0;


        Colour[] colourObjects;

        Colour selectedColour;

        Colour selectedColourCPU;

        // Week 2: Experimentation with positioning. 
        Rectangle startPosition;

        // Declare font for game information.
        SpriteFont gameFont;

        // Set up timer.
        int counter = 4;
        int limit = 0;

        // Set a countDuration value to 1 for every second.
        float countDuration = 1f; 
        float currentTime = 0f;

        // Testing scores.
        int scorePlayer = 0;
        int scoreCPU = 0;

        // Week 2: Set up arrow image.
        Texture2D arrowImage;
        Arrow arrowPointer;

        // Week 2 - Set up counter to keep track of which colour is highlighted.
        int selectCounter = 0;

        #region Sound Effects
        private SoundEffect CPUWin1;
        private SoundEffect CPUWin2;
        private SoundEffect CPUWin3;
        private SoundEffect CPUWin4;

        SoundEffect[] winSounds;

        private SoundEffect CPULose1;
        private SoundEffect CPULose2;
        private SoundEffect CPULose3;

        SoundEffect[] loseSounds;

        private SoundEffect CPUMove1;
        private SoundEffect CPUMove2;

        SoundEffect[] moveSounds;
        #endregion

        // Music?
        Song backingTrack;

        // Declare bool to keep track of whether or not the game is over.
        bool gameOverOccurred;

        // Declare minimum and maximum value for a colour object's ID Value.
        const int MINIMUM_COLOUR_VALUE = 10;
        const int MAXIMUM_COLOUR_VALUE = 99;

        // Set a winning score for the game. The first player to reach this score will win the game.
        int winningScore = 500;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here   
            Viewport viewport = GraphicsDevice.Viewport;

            // Set up size of array, this line will not have to be altered if more colours are added.
            colourObjects = new Colour[colours.Length];

            // Week 2: Set co-ordinates.
            positionX = viewport.Width / 2 - 400;
            positionY = viewport.Height / 2;

            // Set start position for the first box.
            startPosition = new Rectangle(positionX, positionY, 100, 100);

            // Week 2: Set up Arrow.
            arrowImage = Content.Load<Texture2D>("Exercise 1 Assets/Arrow");
            arrowPointer = new Arrow(this, arrowImage, new Rectangle(startPosition.X, startPosition.Y - 60, startPosition.Width, startPosition.Height));

            // Set up Input Manager class.
            new InputManager(this);

            // Make mouse visible in-game.
            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load in game font.
            gameFont = Content.Load<SpriteFont>("gameInfo");

            // Set initial gameplay state.
            currentState = gameState.PLAYERMOVE;

            // Load texture for the boxes.
            blankBox = Content.Load<Texture2D>("Exercise 1 Assets/Blank Box");

            // Create colour objects.          
            for (int i = 0; i < colours.Length; i++)
            {
                // Create colour box.
                colourObjects[i] = new Colour(this, blankBox, colours[i], false, startPosition, colourNames[i], randomGenerator.Next(MINIMUM_COLOUR_VALUE, MAXIMUM_COLOUR_VALUE), gameFont);

                // Ensure the next box will be to the right of the previous box.
                positionX += 200;

                // Set the position for the next box to be made.
                startPosition = new Rectangle(positionX, positionY, 100, 100);
            }

            #region Load sound effects.
            // I own none of these sound effects.
            // Sound effects are owned by Atlus Co.
            CPUWin1 = Content.Load<SoundEffect>("Exercise 1 Sound Effects/CPUWin1");
            CPUWin2 = Content.Load<SoundEffect>("Exercise 1 Sound Effects/CPUWin2");
            CPUWin3 = Content.Load<SoundEffect>("Exercise 1 Sound Effects/CPUWin3");
            CPUWin4 = Content.Load<SoundEffect>("Exercise 1 Sound Effects/CPUWin4");

            winSounds = new SoundEffect[] { CPUWin1, CPUWin2, CPUWin4 };

            CPULose1 = Content.Load<SoundEffect>("Exercise 1 Sound Effects/CPULose");
            CPULose2 = Content.Load<SoundEffect>("Exercise 1 Sound Effects/CPULose2");
            CPULose3 = Content.Load<SoundEffect>("Exercise 1 Sound Effects/CPULose3");

            loseSounds = new SoundEffect[] { CPULose1, CPULose2 };

            CPUMove1 = Content.Load<SoundEffect>("Exercise 1 Sound Effects/CPUMove1");
            CPUMove2 = Content.Load<SoundEffect>("Exercise 1 Sound Effects/CPUMove2");

            moveSounds = new SoundEffect[] { CPUMove1, CPUMove2 };

            #endregion
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currentState)
            {
                #region What occurs when it is the player's turn.
                case gameState.PLAYERMOVE:
                    // Update colour objects.

                    // Reset what the computer selected as well as the time counter.
                    selectedColourCPU = null;
                    counter = 4;

                    // Update the colour objects.
                    for (int i = 0; i < colourObjects.Length; i++)
                    {
                        colourObjects[i].Update(gameTime);
                    }

                    #region Week 2 - Arrow Selection                   
                    if (InputManager.IsKeyPressed(Keys.Right))
                    {
                        MoveRight();
                    }

                    else if (InputManager.IsKeyPressed(Keys.Left))
                    {
                        MoveLeft();
                    }
                    #endregion

                    #region Check if any colour has been chosen.
                    for (int i = 0; i < colourObjects.Length; i++)
                    {
                        if (colourObjects[i].Clicked == true)
                        {
                            #region Highlight the selected colour
                            // Change arrow position to that of the clicked colour.
                            arrowPointer.BoundingRectangle = new Rectangle(colourObjects[i].BoundingRectangle.X, colourObjects[i].BoundingRectangle.Y - 60,
                                    colourObjects[i].BoundingRectangle.Width, colourObjects[i].BoundingRectangle.Width);

                            // Set the select counter to that of the clicked colour's index.
                            selectCounter = i;
                            #endregion

                            // Set the clicked colour as "selected".
                            selectedColour = colourObjects[i];

                            // Reset the selected colour's "Clicked" status.
                            colourObjects[i].Clicked = false;

                            // Change the game's state over to the CPU's Turn.
                            currentState = gameState.CPUMOVE;
                        }
                    }
                    #endregion

                    // When the Enter Key is typed...
                    if (InputManager.IsKeyPressed(Keys.Enter))
                    {
                        // ... Check which colour has been selected.
                        if (colourObjects[selectCounter].BoundingRectangle.Intersects(arrowPointer.BoundingRectangle))
                        {
                            selectedColour = colourObjects[selectCounter];
                            currentState = gameState.CPUMOVE;
                        }
                    }
                    break;
                #endregion

                #region What occurs when it is the CPU's turn.
                case gameState.CPUMOVE:
                    // Experimenting with countdown timer.
                    currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

                    #region Handle countdown and sound effects.
                    if (currentTime >= countDuration)
                    {
                        counter--;

                        // "use up" the time
                        currentTime -= countDuration; 
                                                      

                        // Play sound effect when the CPU's move is revealed.               
                        if (counter == 2)
                        {
                            moveSounds[randomGenerator.Next(0, 2)].Play();
                        }

                        if (selectedColourCPU == null)
                        {
                            selectedColourCPU = colourObjects[randomGenerator.Next(0, colourObjects.Length)];
                        }
                    }
                    #endregion

                    // Determine who won the round.
                    if (counter <= limit)
                    {
                        #region What happens when the CPU wins.
                        if (selectedColourCPU == selectedColour)
                        {
                            // Add onto the CPU's score.
                            // To make the game fairer on the CPU, the CPU will gain all of the player's points as well as the value of their chosen box if they win the round.                                  
                            scoreCPU += scorePlayer + selectedColourCPU.ID; 


                            // Only play the win sound if the winning score hasn't been reached yet. This will prevent sound effects overlapping.
                            if (scoreCPU < winningScore)
                            {
                                winSounds[randomGenerator.Next(0, winSounds.Length)].Play();
                            }

                            // Change game state.
                            currentState = gameState.LOSE;
                        }
                        #endregion

                        #region What happens when the CPU loses.
                        else
                        {                 
                            // Add onto the player's score.          
                            scorePlayer+= selectedColour.ID;

                            // Only play the lose sound if the winning score hasn't been reached yet. This will prevent sound effects overlapping.
                            if (scorePlayer < 500)
                            {
                                loseSounds[randomGenerator.Next(0, loseSounds.Length)].Play();
                            }

                            // Change game state.
                            currentState = gameState.WIN;
                        }
                        #endregion
                    }                                    
                    break;
                #endregion

                #region What occurs when a round has ended.
                default:

                    if (scorePlayer >= winningScore || scoreCPU >= winningScore)
                    {
                        currentState = gameState.GAMEOVER;
                    }

                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Y))
                        {
                            currentState = gameState.PLAYERMOVE;
                        }

                        else if (Keyboard.GetState().IsKeyDown(Keys.N))
                        {
                            currentState = gameState.GAMEOVER;
                        }
                    }
                    break;
                #endregion

                #region What will occur when the game has ended.
                case gameState.GAMEOVER:
                    // What happens when the CPU wins.
                    if (scorePlayer < scoreCPU && gameOverOccurred == false)
                    {
                        CPUWin3.Play();
                        gameOverOccurred = true;
                    }

                    // What happens when the player wins.
                    else if (scorePlayer > scoreCPU && gameOverOccurred == false)
                    {
                        CPULose3.Play();
                        gameOverOccurred = true;
                    }

                    // What happens when the game ends in a tie.
                    else if (scorePlayer == scoreCPU && gameOverOccurred == false)
                    {
                        CPULose3.Play();
                        gameOverOccurred = true;
                    }

                    break;
                    #endregion
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            // Draw the boxes.
            for (int i = 0; i < colours.Length; i++)
            {
                colourObjects[i].Draw(spriteBatch);
            }

            // Draw the arrow.
            arrowPointer.Draw(spriteBatch);

            spriteBatch.Begin();

            // Display the player's score.
            spriteBatch.DrawString(gameFont, "Player: " + scorePlayer, new Vector2(400, 600), Color.White);           

            // Display different messages depending on the game's current state.
            switch (currentState)
            {
                #region What will display during the player's turn.
                case gameState.PLAYERMOVE:
                    // Prompt the player to select a colour.
                    spriteBatch.DrawString(gameFont, "Choose a Colour.", new Vector2(510, 10), Color.White);

                    // Display the score value that will win the game. 
                    spriteBatch.DrawString(gameFont, "First to " + winningScore + " wins!", new Vector2(510, 210), Color.White);

                    // Display the controls to the player.
                    spriteBatch.DrawString(gameFont, "Use Enter key to select.", new Vector2(510, 70), Color.White);
                    spriteBatch.DrawString(gameFont, "Use left and right arrow keys or mouse.", new Vector2(235, 480), Color.White);                   
                    break;
                #endregion

                #region What will display during the CPU's turn.
                case gameState.CPUMOVE:

                    // Tell the player which colour they picked.
                    spriteBatch.DrawString(gameFont, "You Chose " + selectedColour.ColourName + ".", new Vector2(510, 10), selectedColour.BoxColour);

                    // Draw the CPU's message.
                    spriteBatch.DrawString(gameFont, "My Move.", new Vector2(510, 70), Color.White);
                                     
                    if (counter <= 2)
                    {
                        // Display what the CPU chose.
                        spriteBatch.DrawString(gameFont, "I Choose " + selectedColourCPU.ColourName + ".", new Vector2(510, 130), selectedColourCPU.BoxColour);
                    }
                    break;
                #endregion

                #region What will display when a round has ended.
                case gameState.LOSE:
                    spriteBatch.DrawString(gameFont, "I thought you would be a challenge.", new Vector2(380, 40), Color.White);

                    spriteBatch.DrawString(gameFont, "Try again? (Y/N)", new Vector2(380, 100), Color.White);

                    break;

                case gameState.WIN:
                    spriteBatch.DrawString(gameFont, "You win.", new Vector2(510, 40), Color.White);

                    spriteBatch.DrawString(gameFont, "Try again? (Y/N)", new Vector2(380, 100), Color.White);
                    break;
                #endregion

                #region What will display when the game has ended.
                case gameState.GAMEOVER:
                    spriteBatch.DrawString(gameFont, "Game Over", new Vector2(510, 40), Color.White);

                    if (scoreCPU > scorePlayer)
                    {
                        #region Display Lose Message.
                        // If the player quits early.
                        if (scoreCPU < winningScore)
                        {
                            spriteBatch.DrawString(gameFont, "Giving up so soon?", new Vector2(510, 100), Color.White);
                        }

                        // Otherwise...
                        else
                        {
                            spriteBatch.DrawString(gameFont, "Better luck next time.", new Vector2(510, 100), Color.White);
                        }
                        #endregion
                    }

                    else if (scoreCPU < scorePlayer)
                    {
                        #region Display Win message
                        // If the player quits early.
                        if (scorePlayer < winningScore)
                        {
                            spriteBatch.DrawString(gameFont, "Leaving before I can catch up?", new Vector2(510, 100), Color.White);
                        }

                        // Otherwise...
                        else
                        {
                            spriteBatch.DrawString(gameFont, "Well Done.", new Vector2(510, 100), Color.White);
                        }
                        #endregion
                    }

                    else if (scoreCPU == scorePlayer)
                    {
                        #region Display Draw message.
                        // If the player quits early.
                        if (scorePlayer < winningScore && scoreCPU < winningScore)
                        {
                            spriteBatch.DrawString(gameFont, "...and it was just getting interesting.", new Vector2(440, 100), Color.White);
                        }

                        // Otherwise.
                        else
                        {
                            spriteBatch.DrawString(gameFont, "It seems we are quite evenly matched.", new Vector2(310, 100), Color.White);
                        }
                        #endregion
                    }

                    break;
                    #endregion
            }

            // Display the CPU's score.
            spriteBatch.DrawString(gameFont, "CPU: " + scoreCPU, new Vector2(800, 600), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void MoveRight()
        {
            // Increase the selection counter.
            selectCounter++;

            #region Ensure the arrow will cycle from the last box back to the first box.
            if (selectCounter == colourObjects.Length)
            {
                // Reset the counter to 0.
                selectCounter = 0;
            }
            #endregion

            // Change the arrow's position to that of the highlighted box.
            arrowPointer.BoundingRectangle = new Rectangle(colourObjects[selectCounter].BoundingRectangle.X, colourObjects[selectCounter].BoundingRectangle.Y - 60,
               colourObjects[selectCounter].BoundingRectangle.Width, colourObjects[selectCounter].BoundingRectangle.Width);
        }

        public void MoveLeft()
        {
            // Decrease the selection counter.
            selectCounter--;

            #region Ensure the arrow will cycle from the first box back to the last box.
            if (selectCounter == -1)
            {
                // Reset the counter to the highest box index.
                selectCounter = colourObjects.Length - 1;
            }
            #endregion

            // Change the arrow's position to that of the highlighted box.    
            arrowPointer.BoundingRectangle = new Rectangle(colourObjects[selectCounter].BoundingRectangle.X, colourObjects[selectCounter].BoundingRectangle.Y - 60,
               colourObjects[selectCounter].BoundingRectangle.Width, colourObjects[selectCounter].BoundingRectangle.Width);
        }
    }
}
