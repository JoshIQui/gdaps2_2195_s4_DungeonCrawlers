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
    // Enum for Enemy's Current State
    enum EnemyState
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
    class Enemy : GameObject, IHaveAI
    {
        private int speed;
        private int health;
        private int width;
        private int height;
        private EnemyState enemyState;
        private TileManager manager;
        private bool isJumping = false;
        private int jumpHeight = 300;
        private bool canJump = false;
        private double jumpVelocity = 12;
        private double fallVelocity = 1;
        const double gravity = 0.5;
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
        const int PlayerRectWidth = 88;           // The width of a single frame
        const int Displacement = 37;              // How many pixels the sprite needs to move left to allign with its box

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Enemy(Texture2D asset, Texture2D uIAsset, Hitbox position, int screenWidth, int screenHeight, int health = 100)
            : base(asset, position)
        {
            this.health = health;
            this.width = screenWidth;
            this.height = screenHeight;
            enemyState = EnemyState.FacingRight;
            manager = TileManager.Instance;
            this.uIAsset = uIAsset;

            // Initialize
            fps = 9.0;                     // Will cycle through 5 frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
        }
        public override void Update(GameTime gametime)
        {
            List<Hitbox> hitboxes = manager.HitBoxes;
            position.WorldPositionY += 2;
            UpdateAnimation(gametime);
            CheckCollision(hitboxes);
        }
        // Drawing enemy
        public override void Draw(SpriteBatch sb)
        {
            switch (enemyState)
            {
                case EnemyState.FacingRight:
                    DrawStanding(SpriteEffects.None, sb);
                    break;
                case EnemyState.FacingLeft:
                    DrawStanding(SpriteEffects.FlipHorizontally, sb);
                    break;
                case EnemyState.WalkingRight:
                    DrawWalking(SpriteEffects.None, sb);
                    break;
                case EnemyState.WalkingLeft:
                    DrawWalking(SpriteEffects.FlipHorizontally, sb);
                    break;
                case EnemyState.AttackingRight:
                    DrawAttacking(SpriteEffects.None, sb);
                    break;
                case EnemyState.AttackingLeft:
                    DrawAttacking(SpriteEffects.FlipHorizontally, sb);
                    break;
                case EnemyState.JumpingRight:
                    DrawJumping(SpriteEffects.None, sb);
                    break;
                case EnemyState.JumpingLeft:
                    DrawJumping(SpriteEffects.FlipHorizontally, sb);
                    break;
            }

            //Draws the health bar above the enemy's head
            sb.Draw(uIAsset, new Vector2(position.ScreenPositionX + 50, position.Box.Y - 5), new Rectangle(0, 760, 25, 30), Color.Gray);
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
        // Method for Drawing the Enemy Idle Animation
        private void DrawStanding(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            if (frame > ShortFrameCount)
            {
                frame = 0;
            }
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX - Displacement, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * PlayerRectWidth,     //   - This rectangle specifies
                    PlayerSpriteSheetHeight,           //	   where "inside" the texture
                    PlayerRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.Yellow,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Drawing the Enemy Walk Animation
        private void DrawWalking(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX - Displacement, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * PlayerRectWidth,     //   - This rectangle specifies
                    PlayerRectHeight + PlayerSpriteSheetHeight,           //	   where "inside" the texture
                    PlayerRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.Yellow,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Drawing the Enemy Attack Animation
        private void DrawAttacking(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX - Displacement, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * PlayerRectWidth,     //   - This rectangle specifies
                    PlayerRectHeight * 2 + PlayerSpriteSheetHeight,           //	   where "inside" the texture
                    PlayerRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.Yellow,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Drawing the Enemy Jump Animation
        private void DrawJumping(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX - Displacement, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    3 * PlayerRectWidth,     //   - This rectangle specifies
                    PlayerRectHeight + PlayerSpriteSheetHeight,           //	   where "inside" the texture
                    PlayerRectWidth,             //     to get pixels (We don't want to
                    PlayerRectHeight),           //     draw the whole thing)
                Color.Yellow,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Logic for Enemy pathfinding
        public void Logic(Hero target)
        {
            // If the enemy isn't jumping make the enemy fall
            if(!isJumping)
            {
                fallVelocity += gravity;
                position.WorldPositionY += (int)fallVelocity;
            }

            // If enemy is jumping make enemy go up
            if(isJumping)
            {
                canJump = false;
                jumpVelocity -= gravity;
                position.WorldPositionY -= (int)jumpVelocity;
            }

            // Resets enemy jump speed at peak of jump and makes enemy begin to fall
            if(jumpVelocity <= 0)
            {
                isJumping = false;
                jumpVelocity = 12;
            }

            // If the enemy collides with the hero then make it attack the hero
            if(target.Position.Box.Intersects(position.Box) && health > 0)
            {
                enemyState = EnemyState.AttackingLeft;
                
                target.Health--;
                
                health -= 2;
                target.Position.WorldPositionX = position.WorldPositionX; // Makes hero stop to fight the enemy
            }

            if(target.Position.WorldPositionX < position.WorldPositionX) // Hero is to the left of the enemy
            {
                position.WorldPositionX -= 4;
                if (canJump) // If on the ground move left
                {
                    enemyState = EnemyState.WalkingLeft;
                }
                if (target.Position.WorldPositionY < position.WorldPositionY && canJump) // If hero is above the enemy and is able to jump then jump
                {
                    isJumping = true;
                    enemyState = EnemyState.JumpingLeft;
                }
            }
            if(target.Position.WorldPositionX > position.WorldPositionX) // Hero is to the left of the enemy
            {
                if(canJump) // If on te ground move right
                {
                    enemyState = EnemyState.WalkingRight;
                }
                position.WorldPositionX += 4;
                if (target.Position.WorldPositionY < position.WorldPositionY) // If hero is above the enemy and is able to jump then jump
                {
                    isJumping = true;
                    enemyState = EnemyState.JumpingRight;
                }
            }
            
        }

        // Logic for Enemy Collisions
        public override void CheckCollision(List<Hitbox> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    if (objects[i].BoxType == BoxType.Collision && position.Box.Intersects(objects[i].Box)) // Immobile Tiles
                    {
                        if (position.WorldPositionY * 2 + position.Box.Height < objects[i].WorldPositionY * 2 + objects[i].Box.Height
                            && position.WorldPositionX > objects[i].WorldPositionX - position.Box.Width + 10 && position.WorldPositionX + position.Box.Width < objects[i].WorldPositionX + objects[i].Box.Width + position.Box.Width - 10) // Top of Tile
                        {
                            position.WorldPositionY = objects[i].WorldPositionY - position.Box.Height;
                            canJump = true;
                        }
                        if (position.WorldPositionY * 2 + position.Box.Height > objects[i].WorldPositionY * 2 + objects[i].Box.Height
                            && position.WorldPositionX > objects[i].WorldPositionX - position.Box.Width + 10 && position.WorldPositionX + position.Box.Width < objects[i].WorldPositionX + objects[i].Box.Width + position.Box.Width - 10)// Bottom of Tile
                        {
                            position.WorldPositionY = objects[i].WorldPositionY + objects[i].Box.Height;
                        }
                        if (position.WorldPositionX * 2 + position.Box.Width < objects[i].WorldPositionX * 2 + objects[i].Box.Width
                            && position.WorldPositionY > objects[i].WorldPositionY - position.Box.Height + 10 && position.WorldPositionY + position.Box.Height < objects[i].WorldPositionY + objects[i].Box.Height + position.Box.Height - 10) // Left of Tile
                        {
                            position.WorldPositionX = objects[i].WorldPositionX - position.Box.Width;
                        }
                        if (position.WorldPositionX * 2 + position.Box.Width > objects[i].WorldPositionX * 2 + objects[i].Box.Width
                            && position.WorldPositionY > objects[i].WorldPositionY - position.Box.Height + 10 && position.WorldPositionY + position.Box.Height < objects[i].WorldPositionY + objects[i].Box.Height + position.Box.Height - 10) // Right of Tile
                        {
                            position.WorldPositionX = objects[i].WorldPositionX + objects[i].Box.Width;
                        }
                    }
                }

                if (objects[i] != null)
                {
                    if (objects[i].BoxType == BoxType.Hurtbox) // Anything that could damage the enemy
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
}
