﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Dungeon_Crawlers
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont titleFont;
        Vector2 titlePosition;
        int screenWidth;
        int screenHeight;
        Player player;                  // The player object for the main character
        Hero hero;                      // The hero object for the enemy of the game
        Texture2D charTextures;         // The textures for the hero
        Texture2D squareObject;         // use only for debug
        Enemy enemy;                    // The ally object for the other monsters who help you
        Texture2D goblinTextures;       // The textures for the goblin
        Texture2D slimeTextures;        // The textures for the slime
        Texture2D wizardTextures;       // The textures for the wizard
        Texture2D tileTextures;         // The textures for the level tiles
        Texture2D uI;                   // The textures for the game's UI
        KeyboardState kbState;          // Tracks the current state of the keyboard
        KeyboardState prevKbState;      // Tracks the state of the keyboard from the last frame
        MouseState mState;              
        MouseState prevmsState;         
        List<Hitbox> hitBoxes;
        List<EnemyPickUp> pickups;
        List<Enemy> enemies = new List<Enemy>();
        TileManager manager;
        Camera camera;
        Texture2D indoorBackground1;
        Texture2D indoorBackground2;
        Texture2D indoorBackground3;
        Texture2D indoorBackground4;
        Texture2D outsideBackground;
        List<Hitbox> scrollingBackgrounds;
        Texture2D titleScreen;
        Texture2D instructionScreen;
        Texture2D pauseScreen;
        Texture2D helpScreen;
        Texture2D loseScreen;
        Texture2D winScreen;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            manager = TileManager.Instance;

            //Sets up the positions for text
            titlePosition = new Vector2((screenWidth / 2), (screenHeight / 2));
            IsMouseVisible = true;

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

            //Loads all the game state screens
            titleScreen = Content.Load<Texture2D>("TitleScreen");
            instructionScreen = Content.Load<Texture2D>("Instructions");
            pauseScreen = Content.Load<Texture2D>("Pause");
            helpScreen = Content.Load<Texture2D>("Help");
            loseScreen = Content.Load<Texture2D>("Lose");
            winScreen = Content.Load<Texture2D>("Win");

            //Loads the UI elements into the game
            uI = Content.Load<Texture2D>("UI-Spritesheet");

            //Loads the hero and his textures
            charTextures = Content.Load<Texture2D>("Character-Spritesheet");
            Hitbox heroBox = new Hitbox(new Rectangle(300,700,62,96),BoxType.Hurtbox); //96x96 size because 2x scaleing, will change to 1 time (48x48) after debug
            hero = new Hero(charTextures, uI, heroBox, screenWidth, screenHeight);

            Hitbox playerBox = new Hitbox(new Rectangle(800, 700, 68, 92), BoxType.Hitbox);
            player = new Player(charTextures, uI, playerBox, screenWidth, screenHeight);
            camera = new Camera(player.Position, screenWidth, screenHeight);
            // Loads in the backgrounds
            outsideBackground = Content.Load<Texture2D>("Outside Background");
            indoorBackground1 = Content.Load<Texture2D>("Indoor Background 1");
            indoorBackground2 = Content.Load<Texture2D>("Indoor Background 2");
            indoorBackground3 = Content.Load<Texture2D>("Indoor Background 3");
            indoorBackground4 = Content.Load<Texture2D>("Indoor Background 4");
            scrollingBackgrounds = new List<Hitbox>();
            for (int i = 0; i < 4; i++)
            {
                scrollingBackgrounds.Add(new Hitbox(new Rectangle(i * 3648, 0, 3648, 900), BoxType.Hitbox));
            }

            // Loads the level
            tileTextures = Content.Load<Texture2D>("Tile_Spritesheet");
            manager.LoadLevel(tileTextures, charTextures, "..//..//..//level01.txt", 0);
            manager.LoadLevel(tileTextures, charTextures, "..//..//..//level02.txt", 1);
            manager.LoadLevel(tileTextures, charTextures, "..//..//..//level03.txt", 2);
            manager.LoadLevel(tileTextures, charTextures, "..//..//..//level04.txt", 3);
            // Initializes collection of hitboxes
            hitBoxes = manager.TileHitBoxes;
            pickups = manager.EnemyPickUps;
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

            // TODO: Add your update logic here'
            //Gets the current keyboard state
            kbState = Keyboard.GetState();

            //Checks the state and updates accordingly
            switch (StateManager.Instance.CurrentState)
            {
                //Title updates
                case GameState.Title:
                    //If enter is pressed
                    if (kbState.IsKeyDown(Keys.Enter) && kbState != prevKbState)
                    {
                        //Start the game
                        StateManager.Instance.ChangeState(GameState.Instructions);
                    }
                    
                    break;

                //Instructions updates
                case GameState.Instructions:
                    //If enter is pressed
                    if (kbState.IsKeyDown(Keys.Enter) && kbState != prevKbState)
                    {
                        //Start the game
                        ResetGame(player, hero, pickups);
                        StateManager.Instance.ChangeState(GameState.Game);
                    }
                    //If M is pressed
                    if (kbState.IsKeyDown(Keys.M) && kbState != prevKbState)
                    {
                        //Go back to the main menu
                        StateManager.Instance.ChangeState(GameState.Title);
                    }
                    break;

                //Game updates
                case GameState.Game:
                    //If P is pressed
                    if (kbState.IsKeyDown(Keys.P) && kbState != prevKbState)
                    {
                        //Pause the game
                        StateManager.Instance.ChangeState(GameState.Pause);
                    }
                    hero.UpdateAnimation(gameTime);
                    player.CheckCollision(hitBoxes);                    
                    hero.logic(player, hitBoxes);
                    player.Update(gameTime);
                    camera.Update();

                        foreach (EnemyPickUp p in pickups)
                        {
                            p.Logic(player);
                        }

                    // If E is pressed then if possible spawn an Enemy
                    if(kbState.IsKeyUp(Keys.E) && prevKbState.IsKeyDown(Keys.E))
                    {
                        if(player.NumEnemies > 0)
                        {
                            Hitbox enemyBox = new Hitbox(new Rectangle(player.Position.WorldPositionX, player.Position.WorldPositionY, 36 * 2, 45 * 2), BoxType.Hitbox);
                            enemy = new Enemy(charTextures, uI, enemyBox, screenWidth, screenHeight);
                            enemies.Add(enemy);
                            player.NumEnemies--;
                        }
                    }

                    // Update Logic for each Enemy
                    foreach(Enemy enemy in enemies)
                    {
                        enemy.Update(gameTime);
                        enemy.Logic(hero);
                    }
                    break;

                //Pause updates
                case GameState.Pause:
                    //If R is pressed
                    if (kbState.IsKeyDown(Keys.R) && kbState != prevKbState)
                    {
                        //Unpause the game
                        StateManager.Instance.ChangeState(GameState.Game);
                    }
                    //If H is pressed
                    if (kbState.IsKeyDown(Keys.H) && kbState != prevKbState)
                    {
                        //Display the help screen
                        StateManager.Instance.ChangeState(GameState.Help);
                    }
                    //If M is pressed
                    if (kbState.IsKeyDown(Keys.M) && kbState != prevKbState)
                    {
                        //Display the help screen
                        StateManager.Instance.ChangeState(GameState.Title);
                    }
                    break;

                //Help updates
                case GameState.Help:
                    //If P is pressed
                    if (kbState.IsKeyDown(Keys.P) && kbState != prevKbState)
                    {
                        //Go back to the pause menu
                        StateManager.Instance.ChangeState(GameState.Pause);
                    }
                    //If P is pressed
                    if (kbState.IsKeyDown(Keys.R) && kbState != prevKbState)
                    {
                        //Go back to the pause menu
                        StateManager.Instance.ChangeState(GameState.Game);
                    }
                    break;

                //Game over updates
                case GameState.GameOver:
                    //If enter is pressed
                    if (kbState.IsKeyDown(Keys.Enter) && kbState != prevKbState)
                    {
                        //Return to the title screen
                        StateManager.Instance.ChangeState(GameState.Title);
                    }
                    break;

                //Win updates
                case GameState.Win:
                    //If enter is pressed
                    if (kbState.IsKeyDown(Keys.Enter) && kbState != prevKbState)
                    {
                        //Return to the title screen
                        StateManager.Instance.ChangeState(GameState.Title);
                    }
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


            switch (StateManager.Instance.CurrentState)
            {
                case GameState.Title:
                    //Draws the title screen
                    spriteBatch.Draw(titleScreen, new Vector2(0, 0), Color.White);
                    break;


                case GameState.Instructions:
                    //Draws the instruction screen
                    spriteBatch.Draw(instructionScreen, new Vector2(0, 0), Color.White);
                    break;

                case GameState.Game:
                    spriteBatch.Draw(outsideBackground, new Rectangle(0, -200, 1600, 1443), Color.White);
                    spriteBatch.Draw(indoorBackground1, new Vector2(scrollingBackgrounds[0].ScreenPositionX, scrollingBackgrounds[0].WorldPositionY), Color.LightGray);
                    spriteBatch.Draw(indoorBackground2, new Vector2(scrollingBackgrounds[1].ScreenPositionX, scrollingBackgrounds[1].WorldPositionY), Color.LightGray);
                    spriteBatch.Draw(indoorBackground3, new Vector2(scrollingBackgrounds[2].ScreenPositionX, scrollingBackgrounds[2].WorldPositionY), Color.LightGray);
                    spriteBatch.Draw(indoorBackground4, new Vector2(scrollingBackgrounds[3].ScreenPositionX, scrollingBackgrounds[3].WorldPositionY), Color.LightGray);
                    manager.DrawLevel(spriteBatch);hero.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    foreach(Enemy enemy in enemies)
                    {
                        if (enemy.Health > 0)
                        {
                            enemy.Draw(spriteBatch);
                        }
                    }
                    break;

                case GameState.Pause:
                    //Draws the pause screen
                    spriteBatch.Draw(pauseScreen, new Vector2(0, 0), Color.White);
                    break;

                case GameState.Help:
                    //Draws the help screen
                    spriteBatch.Draw(helpScreen, new Vector2(0, 0), Color.White);
                    break;

                case GameState.GameOver:
                    //Draws the game over screen
                    spriteBatch.Draw(loseScreen, new Vector2(0, 0), Color.White);
                    break;

                case GameState.Win:
                    //Draws the win screen
                    spriteBatch.Draw(winScreen, new Vector2(0, 0), Color.White);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Helper method that resets the game
        private void ResetGame(Player player, Hero hero, List<EnemyPickUp> enemyPickUps)
        {
            // Reset Positions and health
            player.Position.WorldPositionX = 800;
            player.Position.WorldPositionY = 700;
            player.Health = 100;
            player.NumEnemies = 0;
            player.PlayerState = PlayerState.FacingRight;

            hero.Position.WorldPositionX = 300;
            hero.Position.WorldPositionY = 700;
            hero.Health = 100;


            foreach(EnemyPickUp p in enemyPickUps)
            {
                p.PickedUp = false;
            }

            Camera.WorldPositionX = 0;
        }
    }
}
