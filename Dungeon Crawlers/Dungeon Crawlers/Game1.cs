using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawlers
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StateManager stateManager;      // The object that manages all states of the game
        SpriteFont titleFont;
        Vector2 titlePosition;
        int screenWidth;
        int screenHeight;
        Player player;                  // The player object for the main character
        Hero hero;                      // The hero object for the enemy of the game
        Texture2D heroTextures;         // The textures for the hero
        Enemy enemy;                    // The ally object for the other monsters who help you
        Texture2D goblinTextures;       // The textures for the goblin
        Texture2D slimeTextures;        // The textures for the slime
        Texture2D wizardTextures;       // The textures for the wizard
        KeyboardState kbState;          // Tracks the current state of the keyboard
        KeyboardState prevKbState;      // Tracks the state of the keyboard from the last frame
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 1000;
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
            stateManager = new StateManager();
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            //Sets up the positions for text
            titlePosition = new Vector2((screenWidth / 2), (screenHeight / 2));

            //Sets up the state manager
            stateManager = new StateManager();

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

            // TODO: use this.Content to load your game content here
            //Loads the title text
            titleFont = Content.Load<SpriteFont>("fonts/titleFont");

            //Loads the hero and his textures
            heroTextures = Content.Load<Texture2D>("Hero-Spritesheet");
            Hitbox heroBox = new Hitbox(new Rectangle(0,0,heroTextures.Width,heroTextures.Height),BoxType.Hitbox);
            hero = new Hero(heroTextures, heroBox, screenWidth, screenHeight);
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

            // TODO: Add your update logic here
            //Gets the current keyboard state
            kbState = Keyboard.GetState();

            //Checks the state and updates accordingly
            switch (stateManager.CurrentState)
            {
                //Title updates
                case GameState.Title:
                    //If enter is pressed
                    if (kbState.IsKeyDown(Keys.Enter) && kbState != prevKbState)
                    {
                        //Start the game
                        stateManager.ChangeState(GameState.Game);
                    }
                    //If I is pressed
                    if (kbState.IsKeyDown(Keys.I) && kbState != prevKbState)
                    {
                        //Show the instructions
                        stateManager.ChangeState(GameState.Instructions);
                    }
                    break;

                //Instructions updates
                case GameState.Instructions:
                    //If enter is pressed
                    if (kbState.IsKeyDown(Keys.Enter) && kbState != prevKbState)
                    {
                        //Start the game
                        stateManager.ChangeState(GameState.Game);
                    }
                    //If M is pressed
                    if (kbState.IsKeyDown(Keys.M) && kbState != prevKbState)
                    {
                        //Go back to the main menu
                        stateManager.ChangeState(GameState.Title);
                    }
                    break;

                //Game updates
                case GameState.Game:
                    //If esc is pressed
                    if (kbState.IsKeyDown(Keys.Escape) && kbState != prevKbState)
                    {
                        //Pause the game
                        stateManager.ChangeState(GameState.Pause);
                    }
                    break;

                //Pause updates
                case GameState.Pause:
                    //If esc is pressed
                    if (kbState.IsKeyDown(Keys.Escape) && kbState != prevKbState)
                    {
                        //Unpause the game
                        stateManager.ChangeState(GameState.Game);
                    }
                    //If H is pressed
                    if (kbState.IsKeyDown(Keys.H) && kbState != prevKbState)
                    {
                        //Display the help screen
                        stateManager.ChangeState(GameState.Help);
                    }
                    break;

                //Help updates
                case GameState.Help:
                    //If esc is pressed
                    if (kbState.IsKeyDown(Keys.Escape) && kbState != prevKbState)
                    {
                        //Go back to the pause menu
                        stateManager.ChangeState(GameState.Pause);
                    }
                    break;

                //Game over updates
                case GameState.GameOver:
                    //If enter is pressed
                    if (kbState.IsKeyDown(Keys.Enter) && kbState != prevKbState)
                    {
                        //Return to the title screen
                        stateManager.ChangeState(GameState.Title);
                    }
                    break;

                //Win updates
                case GameState.Win:
                    break;
            }

            //Sets the previous keyboard state to the current keyboard state now that the frame is done
            prevKbState = kbState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //Checks the state and draws accordingly
            switch (stateManager.CurrentState)
            {
                case GameState.Title:
                    //Draws the title text
                    spriteBatch.DrawString(titleFont, "Dungeon Crawlers", titlePosition, Color.OrangeRed);

                    //Draws the instructions for starting the game
                    spriteBatch.DrawString(titleFont, "Press ENTER to Start", new Vector2(500, 700), Color.OrangeRed);
                    break;

                case GameState.Instructions:
                    break;

                case GameState.Game:
                    //Draws the player
                    spriteBatch.Draw(player.Asset, player.Position.Box, Color.White);

                    //Draws the hero
                    spriteBatch.Draw(hero.Asset, hero.Position.Box, Color.White);
                    break;

                case GameState.Pause:
                    break;

                case GameState.Help:
                    break;

                case GameState.GameOver:
                    break;

                case GameState.Win:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
