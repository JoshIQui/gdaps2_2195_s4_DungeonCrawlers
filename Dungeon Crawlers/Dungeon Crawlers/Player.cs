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
        CrouchingRight,
        CrouchingLeft,
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

        // Animation Variables
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame

        // Constants for rectangle in the spritesheet
        const int WalkFrameCount = 7;       // The number of frames in the animation
        const int PlayerRectOffsetWalk = 49;   // How far down in the image are the frames? FOR THE RUN
        const int PlayerRectHeight = 44;     // The height of a single frame
        const int PlayerRectWidth = 48;     // The width of a single frame

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

            // Initialize
            fps = 10.0;                     // Will cycle through 10 walk frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
        }

        // Methods

        // Method for Player Updates in game
        public override void Update(GameTime gametime)
        {
            // Logic for Player Movement
            if(kbState.IsKeyDown(Keys.D)) // Right
            {
                position.BoxX += 2;
            }
            if(kbState.IsKeyDown(Keys.A)) // Left
            {
                position.BoxX -= 2;
            }
            if (kbState.IsKeyDown(Keys.W)) // Up
            {
                position.BoxY -= 2;
            }
            if (kbState.IsKeyDown(Keys.S)) // Crouch
            {
                
            }

            // Update previous Keyboard State
            prevKbState = kbState;
        }

        // Method for Drawing the Player
        public override void Draw(SpriteBatch sb)
        {
            DrawWalking(SpriteEffects.None, sb);
        }

        // Method for Collision Checks for player
        protected override bool CheckCollision(List<Hitbox> objects)
        {
            return false;
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

        private void DrawWalking(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(100, 100),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * PlayerRectWidth,     //   - This rectangle specifies
                    PlayerRectOffsetWalk,           //	   where "inside" the texture
                    PlayerRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        
    }
}
