using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawlers
{
    // Enum for Player's Current State
    enum PlayerState
    {
        WalkingRight,
        WalkingLeft,
        FacingRight,
        FacingLeft,
        JumpingRight,
        JumpingLeft,
        AttackingRight,
        AttackingLeft
    }

    class Player : GameObject
    {
        // Fields
        private double health;
        private int numEnemies;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private PlayerState playerState;
        private bool canJump;
        private bool hitCeiling;
        private bool jumping;
        private bool falling;
        private double timer = 0;
        private double velocity = -900;
        private double acceleration = 1200;
        private int height;
        private double JumpDelay = 0.0333334;
        private TileManager manager;
        List<Hitbox> hitboxes;
        Texture2D uIAsset;


        // Animation Variables
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame

        // Constants for rectangle in the spritesheet
        const int LongFrameCount = 7;             // The number of frames in the longer animations
        const int ShortFrameCount = 3;            // The number of frames in the shorter animations
        const int PlayerSpriteSheetHeight = 241;  // How far down the first animation for the golbin is (IDLE)
        const int PlayerRectHeight = 46;          // The height of a single frame
        const int PlayerNormRectWidth = 40;       // The width of a single sprite when idle, walking, or jumping
        const int PlayerAttackRectWidth = 88;     // The actual width of a single frame which is the same as the width of the sprite when attacking
        const int Displacement = 15;              // How many pixels the actual sprite is away from the left edge of the frame

        // Properties
        public double Health
        {
            get { return health; }
            set { health = value; }
        }

        public int NumEnemies
        {
            get { return numEnemies; }
            set 
            { 
                if(numEnemies != 5) // Sets maximum amount of enemies a player can use at once
                {
                    numEnemies = value;
                }
            }
        }

        public PlayerState PlayerState
        {
            get { return playerState; }
        }

        // Constructor
        public Player(Texture2D asset, Texture2D uIAsset, Hitbox position, int screenWidth, int screenHeight, double health = 100.0, int numEnemies = 0)
            : base(asset, position)
        {
            this.asset = asset;
            this.uIAsset = uIAsset;
            this.position = position;
            this.health = health;
            this.numEnemies = numEnemies;
            playerState = PlayerState.FacingRight;
            canJump = false;
            jumping = false;
            falling = false;
            // Initialize
            fps = 9.0;                     // Will cycle through 5 frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
            manager = TileManager.Instance;
            hitboxes = manager.TileHitBoxes;
        }

        // Methods

        // Method for Player Updates in game
        public override void Update(GameTime gametime)
        {
            // Get Keyboard state for user input
            KeyboardState kbState = Keyboard.GetState();
            // Condition for when the Player falls through a gap
            if(position.WorldPositionY > 900)
            {
                StateManager.Instance.ChangeState(GameState.GameOver);
            }

            position.WorldPositionY += 2;
            if (!falling && !jumping)
            {
                height = position.WorldPositionY;
                timer = 0;
            }
            if (!jumping && !canJump)
            {
                falling = true;
                position.WorldPositionY = (int)(acceleration * Math.Pow(timer, 2) + height);
                timer += gametime.ElapsedGameTime.TotalSeconds;
            }
            // Logic for switching player states and player movement
            switch (playerState)
            {
                case PlayerState.FacingRight:
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerState.WalkingRight;
                    }
                    if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D))
                    {
                        playerState = PlayerState.FacingLeft;
                    }
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        if (canJump) // Jumps if on the ground
                        {
                            playerState = PlayerState.JumpingRight;
                            canJump = false;
                            velocity = -900;
                            jumping = true;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        playerState = PlayerState.AttackingRight;
                    }
                    break;

                case PlayerState.FacingLeft:
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerState.WalkingLeft;
                    }
                    if (kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A))
                    {
                        playerState = PlayerState.FacingRight;
                    }
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        if (canJump) // Jumps if on the ground
                        {
                            playerState = PlayerState.JumpingLeft;
                            canJump = false;
                            velocity = -900;
                            jumping = true;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        playerState = PlayerState.AttackingLeft;
                    }
                    break;

                case PlayerState.WalkingRight:
                    position.WorldPositionX += 5;
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        if (canJump) // Jumps if on the ground
                        {
                            playerState = PlayerState.JumpingRight;
                            canJump = false;
                            velocity = -900;
                            jumping = true;
                        }
                    }
                    if (kbState.IsKeyUp(Keys.D) && playerState == PlayerState.WalkingRight)
                    {
                        playerState = PlayerState.FacingRight;
                    }
                    break;

                case PlayerState.WalkingLeft:
                    position.WorldPositionX -= 5;
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        if (canJump) // Jumps if on the ground
                        {
                            playerState = PlayerState.JumpingLeft;
                            canJump = false;
                            velocity = -900;
                            jumping = true;
                        }
                    }
                    if (kbState.IsKeyUp(Keys.A) && playerState == PlayerState.WalkingLeft)
                    {
                        playerState = PlayerState.FacingLeft;
                    }
                    break;

                case PlayerState.AttackingRight:
                    if (kbState.IsKeyUp(Keys.Space) && playerState == PlayerState.AttackingRight)
                    {
                        playerState = PlayerState.FacingRight;
                    }
                    break;

                case PlayerState.AttackingLeft:
                    if (kbState.IsKeyUp(Keys.Space) && playerState == PlayerState.AttackingLeft)
                    {
                        playerState = PlayerState.FacingLeft;
                    }
                    break;
                case PlayerState.JumpingRight:
                        position.WorldPositionY = (int)(velocity * timer + acceleration * Math.Pow(timer, 2) + height);
                        timer += gametime.ElapsedGameTime.TotalSeconds;

                    if (kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A))
                    {
                        position.WorldPositionX += 5;
                    }
                    if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D))
                    {
                        position.WorldPositionX -= 5;
                        playerState = PlayerState.JumpingLeft;
                    }
                    break;

                case PlayerState.JumpingLeft:
                    position.WorldPositionY = (int)(velocity * timer + acceleration * Math.Pow(timer, 2) + height);
                    timer += gametime.ElapsedGameTime.TotalSeconds;

                    if (kbState.IsKeyDown(Keys.D) && kbState.IsKeyUp(Keys.A))
                    {
                        position.WorldPositionX += 5;
                        playerState = PlayerState.JumpingRight;
                    }
                    if (kbState.IsKeyDown(Keys.A) && kbState.IsKeyUp(Keys.D))
                    {
                        position.WorldPositionX -= 5;
                    }
                    break;
            }

            UpdateAnimation(gametime);
            CheckCollision(hitboxes);
            // If player dies then go to GameOver Screen
            if (health <= 0)
            {
                StateManager.Instance.ChangeState(GameState.GameOver);
            }

            // Update previous Keyboard State
            prevKbState = kbState;
        }

        // Method for Drawing the Player
        public override void Draw(SpriteBatch sb)
        {
            // Logic for changing what to draw on screen based on player's current state
            switch (playerState)
            {
                case PlayerState.FacingRight:
                    DrawStanding(SpriteEffects.None, sb);
                    break;
                case PlayerState.FacingLeft:
                    DrawStanding(SpriteEffects.FlipHorizontally, sb);
                    break;
                case PlayerState.WalkingRight:
                    DrawWalking(SpriteEffects.None, sb);
                    break;
                case PlayerState.WalkingLeft:
                    DrawWalking(SpriteEffects.FlipHorizontally, sb);
                    break;
                case PlayerState.AttackingRight:
                    DrawAttacking(SpriteEffects.None, 35, sb);
                    break;
                case PlayerState.AttackingLeft:
                    DrawAttacking(SpriteEffects.FlipHorizontally, 60, sb);
                    break;
                case PlayerState.JumpingRight:
                    DrawJumping(SpriteEffects.None, sb);
                    break;
                case PlayerState.JumpingLeft:
                    DrawJumping(SpriteEffects.FlipHorizontally, sb);
                    break;
            }

            //Draws the health bar above the player's head
            sb.Draw(uIAsset, new Vector2(position.ScreenPositionX + 11, position.Box.Y), new Rectangle(0, 760, ((int)health / 2), 30), Color.White);

            //Draws the enemy count UI element
            switch (numEnemies)
            {
                case 0:
                    sb.Draw(uIAsset, new Vector2(5, 40), new Rectangle(0, 720, 200, 40), Color.White);
                    break;
                case 1:
                    sb.Draw(uIAsset, new Vector2(5, 40), new Rectangle(0, 675, 200, 40), Color.White);
                    break;
                case 2:
                    sb.Draw(uIAsset, new Vector2(5, 40), new Rectangle(0, 630, 200, 40), Color.White);
                    break;
                case 3:
                    sb.Draw(uIAsset, new Vector2(5, 40), new Rectangle(0, 585, 200, 40), Color.White);
                    break;
                case 4:
                    sb.Draw(uIAsset, new Vector2(5, 40), new Rectangle(0, 540, 200, 40), Color.White);
                    break;
                case 5:
                    sb.Draw(uIAsset, new Vector2(5, 40), new Rectangle(0, 495, 200, 40), Color.White);
                    break;
            }
        }

        // Method for updating Player animations
        public void UpdateAnimation(GameTime gameTime)
        {
            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame

            // How much time has passed?  
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // If enough time has passed:
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                     // Adjust the frame to the next image

                if (frame > LongFrameCount)     // Check the bounds - have we reached the end of walk cycle?
                    frame = 0;

                timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
                                                // This keeps the time passed 
            }
        }
        // Method for Drawing the Player Idle Animation
        private void DrawStanding(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            if (frame > ShortFrameCount)
            {
                frame = 0;
            }
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    Displacement + (frame * PlayerAttackRectWidth),     //   - This rectangle specifies
                    PlayerSpriteSheetHeight,           //	   where "inside" the texture
                    PlayerNormRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.Yellow,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Drawing the Player Walk Animation
        private void DrawWalking(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    Displacement + (frame * PlayerAttackRectWidth),     //   - This rectangle specifies
                    PlayerRectHeight + PlayerSpriteSheetHeight,           //	   where "inside" the texture
                    PlayerNormRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.Yellow,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Drawing the Player Attack Animation
        private void DrawAttacking(SpriteEffects flipSprite, int positionCorrection, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX - positionCorrection, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    (frame * PlayerAttackRectWidth),     //   - This rectangle specifies
                    PlayerRectHeight * 2 + PlayerSpriteSheetHeight,           //	   where "inside" the texture
                    PlayerAttackRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.Yellow,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Drawing the Player Jump Animation
        private void DrawJumping(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    15+(3 * 88),     //   - This rectangle specifies
                    PlayerRectHeight + PlayerSpriteSheetHeight,           //	   where "inside" the texture
                    PlayerNormRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.Yellow,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Collision Checks for player
        public override void CheckCollision(List<Hitbox> objects)
        {
            int floorCollide = 0;
            int ceilingCollide = 0;
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    if (objects[i].BoxType == BoxType.Collision && position.Box.Intersects(objects[i].Box)) // Immobile Tiles
                    {
                        //if (position.WorldPositionY * 2 + position.Box.Height < objects[i].WorldPositionY * 2 + objects[i].Box.Height
                           // && position.WorldPositionX > objects[i].WorldPositionX - position.Box.Width + 10 && position.WorldPositionX + position.Box.Width < objects[i].WorldPositionX + objects[i].Box.Width + position.Box.Width - 10) // Top of Tile
                        if (position.WorldPositionX + position.Box.Width > objects[i].WorldPositionX + 5
                            && position.WorldPositionX < objects[i].WorldPositionX + objects[i].Box.Width - 5
                            && position.WorldPositionY + position.Box.Height > objects[i].WorldPositionY
                            && position.WorldPositionY < objects[i].WorldPositionY)
                        {
                            position.WorldPositionY = objects[i].WorldPositionY - position.Box.Height;
                            if (playerState == PlayerState.JumpingRight && timer >= JumpDelay)
                            {
                                playerState = PlayerState.FacingRight;
                                jumping = false;
                            }
                            else if (playerState == PlayerState.JumpingLeft && timer >= JumpDelay)
                            {
                                playerState = PlayerState.FacingLeft;
                                jumping = false;
                            }
                            canJump = true; // If player is on top of a block let them be able to jump
                            falling = false;
                            floorCollide += 1;
                        }
                        else //if not colliding with ALL block
                        {
                            if (floorCollide == 0)
                            {
                                canJump = false;
                            }
                        }
                        //if (position.WorldPositionY * 2 + position.Box.Height > objects[i].WorldPositionY * 2 + objects[i].Box.Height
                           // && position.WorldPositionX > objects[i].WorldPositionX - position.Box.Width + 10 && position.WorldPositionX + position.Box.Width < objects[i].WorldPositionX + objects[i].Box.Width + position.Box.Width - 10)// Bottom of Tile
                        if (position.WorldPositionX + position.Box.Width > objects[i].WorldPositionX + 5
                            && position.WorldPositionX < objects[i].WorldPositionX + objects[i].Box.Width - 5
                            && position.WorldPositionY < objects[i].WorldPositionY + objects[i].Box.Height
                            && position.WorldPositionY + position.Box.Height > objects[i].WorldPositionY + objects[i].Box.Height)
                        {
                            position.WorldPositionY = objects[i].WorldPositionY + objects[i].Box.Height;
                            // Only changes values the first time the player collides with a ceiling, within one interaction
                            // Makes the player reach its max height at the ceiling's height and starts its fall
                            if (!hitCeiling)
                            {
                                timer = 0;
                                velocity = 0;
                                height = position.WorldPositionY;
                            }
                            hitCeiling = true;
                        }
                        else //if not colliding with ALL block
                        {
                            if (ceilingCollide == 0)
                            {
                                hitCeiling = false;
                            }
                        }
                        //if (position.WorldPositionX * 2 + position.Box.Width < objects[i].WorldPositionX * 2 + objects[i].Box.Width
                           // && position.WorldPositionY > objects[i].WorldPositionY - position.Box.Height + 10 && position.WorldPositionY + position.Box.Height < objects[i].WorldPositionY + objects[i].Box.Height + position.Box.Height - 10) // Left of Tile
                        if (position.WorldPositionX + position.Box.Width > objects[i].WorldPositionX
                            && position.WorldPositionX < objects[i].WorldPositionX
                            && position.WorldPositionY + position.Box.Height > objects[i].WorldPositionY
                            && position.WorldPositionY < objects[i].WorldPositionY + objects[i].Box.Height)
                        {
                            position.WorldPositionX = objects[i].WorldPositionX - position.Box.Width;
                        }
                        //if (position.WorldPositionX * 2 + position.Box.Width > objects[i].WorldPositionX * 2 + objects[i].Box.Width
                            //&& position.WorldPositionY > objects[i].WorldPositionY - position.Box.Height + 10 && position.WorldPositionY + position.Box.Height < objects[i].WorldPositionY + objects[i].Box.Height + position.Box.Height - 10) // Right of Tile
                        if (position.WorldPositionX < objects[i].WorldPositionX + objects[i].Box.Width
                            && position.WorldPositionX + position.Box.Width > objects[i].WorldPositionX + objects[i].Box.Width
                            && position.WorldPositionY + position.Box.Height > objects[i].WorldPositionY + 5
                            && position.WorldPositionY < objects[i].WorldPositionY + objects[i].Box.Height)
                        {
                            position.WorldPositionX = objects[i].WorldPositionX + objects[i].Box.Width;
                        }
                    }
                    if (objects[i].BoxType == BoxType.Hurtbox) // Anything that could damage the player
                    {
                        if (position.Box.Intersects(objects[i].Box))
                        {
                            health-= 0.1;
                        }
                    }
                }
            }
        }
    }
}
