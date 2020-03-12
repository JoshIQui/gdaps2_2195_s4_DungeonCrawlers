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
        private int health;
        private int numEnemies;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private PlayerState playerState;
        private bool canJump;
        private bool jumping = false;
        private int jumpHeight = 0;

        // Animation Variables
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame

        // Constants for rectangle in the spritesheet
        const int WalkFrameCount = 3;       // The number of frames in the animation
        const int PlayerRectOffsetWalk = 48;   // How far down in the image are the frames? FOR THE RUN
        const int PlayerRectHeight = 45;     // The height of a single frame
        const int PlayerRectWidth = 88;     // The width of a single frame
        const int OffsetX = 50;

        // Properties
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int NumEnemies
        {
            get { return numEnemies; }
            set { numEnemies = value; }
        }

        // Constructor
        public Player(Texture2D asset, Hitbox position, int screenWidth, int screenHeight, int health = 100, int numEnemies = 0)
            : base(asset, position)
        {
            this.asset = asset;
            this.position = position;
            this.health = health;
            this.numEnemies = numEnemies;
            playerState = PlayerState.FacingRight;
            canJump = true;
            // Initialize
            fps = 5.0;                     // Will cycle through 5 frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
        }

        // Methods

        // Method for Player Updates in game
        public override void Update(GameTime gametime)
        {
            // Get Keyboard state for user input
            KeyboardState kbState = Keyboard.GetState();
            position.BoxY += 2;
            if(jumping && jumpHeight >0)
            {
                position.BoxY -= 10;
                jumpHeight -= 10;
            }
            else
            {
                jumping = false;
            }
            // Logic for switching player states and player movement
            switch(playerState)
            {
                case PlayerState.FacingRight:
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerState.WalkingRight;
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        playerState = PlayerState.FacingLeft;
                    }
                    if (kbState.IsKeyDown(Keys.W)) 
                    {
                        if(canJump) // Jumps if on the ground
                        {
                            playerState = PlayerState.JumpingRight;
                            jumpHeight = 100;
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
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        playerState = PlayerState.FacingRight;
                    }
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        if(canJump) // Jumps if on the ground
                        {
                            playerState = PlayerState.JumpingLeft;
                            jumpHeight = 100;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.Space))
                    {
                        playerState = PlayerState.AttackingLeft;
                    }
                    break;

                case PlayerState.WalkingRight:
                    position.BoxX += 5;
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        if(canJump) // Jumps if on the ground
                        {
                            playerState = PlayerState.JumpingRight;
                            jumpHeight = 100;
                        }
                    }
                    if (kbState.IsKeyUp(Keys.D) && playerState == PlayerState.WalkingRight)
                    {
                        playerState = PlayerState.FacingRight;
                    }
                    break;

                case PlayerState.WalkingLeft:
                    position.BoxX -= 5;
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        if(canJump) // Jumps if on the ground
                        {
                            playerState = PlayerState.JumpingLeft;
                            jumpHeight = 100;
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
                    canJump = false;
                    position.BoxY -= 10;
                    jumpHeight -= 10;
                    jumping = true;
                    if (kbState.IsKeyDown(Keys.W)) // Puts player out of jump state to prevent double jumping
                    {
                        playerState = PlayerState.FacingRight;
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        for (int i = 0; i < 2; i++) // Gradual movement (Same as above)
                        {
                            position.BoxX += 5;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        position.BoxX -= 5;
                        playerState = PlayerState.JumpingLeft;
                    }
                    break;

                case PlayerState.JumpingLeft:
                    canJump = false;
                    position.BoxY -= 10;
                    jumpHeight -= 10;
                    jumping = true;
                    if (kbState.IsKeyDown(Keys.W)) // Puts player out of jump state to prevent double jumping
                    {
                        playerState = PlayerState.FacingLeft;
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        for (int i = 0; i < 2; i++) // Gradual movement (Same as above)
                        {
                            position.BoxX -= 5;
                        }
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        position.BoxX += 5;
                        playerState = PlayerState.JumpingRight;
                    }
                    if (kbState.IsKeyUp(Keys.W) && playerState == PlayerState.JumpingLeft)
                    {
                        playerState = PlayerState.FacingLeft;
                    }
                    break;
            }

            // If player dies then go to GameOver Screen
            if(health <= 0)
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
                    DrawAttacking(SpriteEffects.None, sb);
                    break;
                case PlayerState.AttackingLeft:
                    DrawAttacking(SpriteEffects.FlipHorizontally, sb);
                    break;
                case PlayerState.JumpingRight:
                    DrawJumping(SpriteEffects.None, sb);
                    break;
                case PlayerState.JumpingLeft:
                    DrawJumping(SpriteEffects.FlipHorizontally, sb);
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

                if (frame > WalkFrameCount)     // Check the bounds - have we reached the end of walk cycle?
                    frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
                                                // This keeps the time passed 
            }
        }
        // Method for Drawing the Player Idle Animation
        private void DrawStanding(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX - OffsetX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * PlayerRectWidth,     //   - This rectangle specifies
                    PlayerRectOffsetWalk * 5,           //	   where "inside" the texture
                    PlayerRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
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
                new Vector2(position.BoxX - OffsetX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * PlayerRectWidth,     //   - This rectangle specifies
                    PlayerRectOffsetWalk * 6,           //	   where "inside" the texture
                    PlayerRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Drawing the Player Attack Animation
        private void DrawAttacking(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX - OffsetX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * PlayerRectWidth,     //   - This rectangle specifies
                    PlayerRectOffsetWalk * 7,           //	   where "inside" the texture
                    PlayerRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
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
                new Vector2(position.BoxX - OffsetX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    3 * PlayerRectWidth,     //   - This rectangle specifies
                    PlayerRectOffsetWalk * 6,           //	   where "inside" the texture
                    PlayerRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Collision Checks for player
        public override void CheckCollision(List<Hitbox> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].BoxType == BoxType.Collision) // Immobile Tiles
                {
                    if (position.Box.Intersects(objects[i].Box) && position.BoxY * 2 + position.Box.Height < objects[i].BoxY * 2 + objects[i].Box.Height
                        && position.BoxX > objects[i].BoxX - position.Box.Width + 10 && position.BoxX + position.Box.Width < objects[i].BoxX + objects[i].Box.Width + position.Box.Width -10) // Top of Tile
                    {
                        position.BoxY = objects[i].BoxY - position.Box.Height;
                        canJump = true; // If player is on top of a block let them be able to jump
                    }
                    if (position.Box.Intersects(objects[i].Box) && position.BoxY * 2 + position.Box.Height > objects[i].BoxY * 2 + objects[i].Box.Height
                        && position.BoxX > objects[i].BoxX - position.Box.Width + 10 && position.BoxX + position.Box.Width < objects[i].BoxX + objects[i].Box.Width + position.Box.Width -10)// Bottom of Tile
                    {
                        position.BoxY = objects[i].BoxY+ objects[i].Box.Height;
                    }
                    if(position.Box.Intersects(objects[i].Box) && position.BoxX  * 2 + position.Box.Width < objects[i].BoxX * 2 + objects[i].Box.Width
                        && position.BoxY > objects[i].BoxY - position.Box.Height +10 && position.BoxY + position.Box.Height < objects[i].BoxY + objects[i].Box.Height + position.Box.Height -10) // Left of Tile
                    {
                        position.BoxX = objects[i].BoxX - position.Box.Width;
                    }
                    if (position.Box.Intersects(objects[i].Box) && position.BoxX * 2 + position.Box.Width > objects[i].BoxX * 2 + objects[i].Box.Width
                        && position.BoxY > objects[i].BoxY - position.Box.Height + 10 && position.BoxY + position.Box.Height < objects[i].BoxY + objects[i].Box.Height + position.Box.Height - 10) // Right of Tile
                    {
                        position.BoxX = objects[i].BoxX+ objects[i].Box.Width;
                    }
                }
                if (objects[i].BoxType == BoxType.Hurtbox) // Anything that could damage the player
                {
                    if (position.Box.Intersects(objects[i].Box))
                    {
                        health--;
                    }
                } 
            }
        }
    }
}
