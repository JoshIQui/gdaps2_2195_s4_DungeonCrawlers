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

        // Animation Variables
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame

        // Constants for rectangle in the spritesheet
        const int WalkFrameCount = 3;       // The number of frames in the animation
        const int EnemyRectOffsetWalk = 48;   // How far down in the image are the frames? FOR THE RUN
        const int EnemyRectHeight = 45;     // The height of a single frame
        const int EnemyRectWidth = 88;     // The width of a single frame
        const int OffsetX = 50;

        public int Health
        {
            get { return health; }
        }

        public Enemy(Texture2D asset, Hitbox position, int screenWidth, int screenHeight, int health = 100)
            : base(asset, position)
        {
            this.health = health;
            this.width = screenWidth;
            this.height = screenHeight;
            enemyState = EnemyState.FacingRight;
            manager = TileManager.Instance;

            // Initialize
            fps = 5.0;                     // Will cycle through 5 frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
        }
        public override void Update(GameTime gametime)
        {
            List<Hitbox> hitboxes = manager.HitBoxes;
            position.BoxY += 2;
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
                    frame * EnemyRectWidth,     //   - This rectangle specifies
                    EnemyRectOffsetWalk * 5,           //	   where "inside" the texture
                    EnemyRectWidth,             //     to get pixels (We don't want to
                    EnemyRectHeight),           //     draw the whole thing)
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
                    frame * EnemyRectWidth,     //   - This rectangle specifies
                    EnemyRectOffsetWalk * 6,           //	   where "inside" the texture
                    EnemyRectWidth,             //     to get pixels (We don't want to
                    EnemyRectHeight),           //     draw the whole thing)
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
                    frame * EnemyRectWidth,     //   - This rectangle specifies
                    EnemyRectOffsetWalk * 7,           //	   where "inside" the texture
                    EnemyRectWidth,             //     to get pixels (We don't want to
                    EnemyRectHeight),           //     draw the whole thing)
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
                    3 * EnemyRectWidth,     //   - This rectangle specifies
                    EnemyRectOffsetWalk * 6,           //	   where "inside" the texture
                    EnemyRectWidth,             //     to get pixels (We don't want to
                    EnemyRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Logic for Enemy pathfinding
        public void Logic(Hero target)
        {
            if(target.Position.Box.Intersects(position.Box))
            {
                enemyState = EnemyState.AttackingLeft;
                /*
                target.Health--;
                health -= 2;
                */
            }

            if(target.Position.BoxX < position.BoxX)
            {
                position.BoxX -= 4;
                enemyState = EnemyState.WalkingLeft;
                if (target.Position.BoxY < position.BoxY)
                {
                    position.BoxY -= 4;
                }
                if (target.Position.BoxY > position.BoxY)
                {
                    position.BoxX += 4;
                }
            }
            if(target.Position.BoxX > position.BoxX)
            {
                position.BoxX += 4;
                enemyState = EnemyState.WalkingRight;
                if (target.Position.BoxY < position.BoxY)
                {
                    position.BoxY -= 4;
                }
                if (target.Position.BoxY > position.BoxY)
                {
                    position.BoxX += 4;
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
                        if (position.BoxY * 2 + position.Box.Height < objects[i].BoxY * 2 + objects[i].Box.Height
                            && position.BoxX > objects[i].BoxX - position.Box.Width + 10 && position.BoxX + position.Box.Width < objects[i].BoxX + objects[i].Box.Width + position.Box.Width - 10) // Top of Tile
                        {
                            position.BoxY = objects[i].BoxY - position.Box.Height;
                        }
                        if (position.BoxY * 2 + position.Box.Height > objects[i].BoxY * 2 + objects[i].Box.Height
                            && position.BoxX > objects[i].BoxX - position.Box.Width + 10 && position.BoxX + position.Box.Width < objects[i].BoxX + objects[i].Box.Width + position.Box.Width - 10)// Bottom of Tile
                        {
                            position.BoxY = objects[i].BoxY + objects[i].Box.Height;
                        }
                        if (position.BoxX * 2 + position.Box.Width < objects[i].BoxX * 2 + objects[i].Box.Width
                            && position.BoxY > objects[i].BoxY - position.Box.Height + 10 && position.BoxY + position.Box.Height < objects[i].BoxY + objects[i].Box.Height + position.Box.Height - 10) // Left of Tile
                        {
                            position.BoxX = objects[i].BoxX - position.Box.Width;
                        }
                        if (position.BoxX * 2 + position.Box.Width > objects[i].BoxX * 2 + objects[i].Box.Width
                            && position.BoxY > objects[i].BoxY - position.Box.Height + 10 && position.BoxY + position.Box.Height < objects[i].BoxY + objects[i].Box.Height + position.Box.Height - 10) // Right of Tile
                        {
                            position.BoxX = objects[i].BoxX + objects[i].Box.Width;
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
