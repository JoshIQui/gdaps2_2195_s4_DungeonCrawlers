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
    enum HeroState
    {
        FaceLeft,
        WalkLeft,     
        FaceRight,
        WalkRight,
        Attack,
        Jumping
        // Add state(s) to support crouching
    }
    class Hero : GameObject, IHaveAI
    {
        // Fields     
        private int speed;
        private int health;
        private int width;
        private int height;
       
        // Animation
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame

        // Constants for "source" rectangle(inside the image)
        const int WalkFrameCount = 7;       // The number of frames in the animation
        const int HeroRectOffset = 48;   // How far down in the image are the frames?
        const int HeroRectHeight = 48;     // The height of a single frame
        const int HeroRectWidth = 48;
        Rectangle debug;

        HeroState currentState = HeroState.WalkRight;
        int moveSpd = 5;
        int gravity = 9;
        bool[,] obstacle;

        public int Health
        {
            get { return health; }
        }
       
        public Hero(Texture2D asset, Hitbox position, int screenWidth, int screenHeight, int health = 100)
            :base (asset,position)
        {
            this.health = health;
            this.width = screenWidth;
            this.height = screenHeight;

            obstacle = new bool[height+1, width+1];
            // Initialize
            fps = 10.0;                     // Will cycle through 10 walk frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
        }
        public void logic(MouseState mouse,List<Item> square)
        {
            debug.X = mouse.X;
            debug.Y = mouse.Y;
            debug.Width = 1;
            debug.Height = 1;


            while (speed > 0)
            {
                if (position.Box.Intersects(debug))
                {
                    currentState = HeroState.Attack;
                    break;
                }
                if (position.BoxY < mouse.Y) //going down
                {
                    position.BoxY += 1;
                    speed -= 1;
                    for (int a = 0; a < square.Count; a++)
                    {
                        if (square[a].Intersect(position.Box))
                        {
                            position.BoxY = square[a].Position.BoxY - HeroRectHeight * 2;// *2 because i use 200% scaling
                        }
                    }
                }
                if (position.BoxY > mouse.Y) //going up
                {
                    position.BoxY -= 1;
                    speed -= 1;
                    currentState = HeroState.Jumping;
                    for (int a = 0; a < square.Count; a++)
                    {
                        if (square[a].Intersect(position.Box))
                        {
                            position.BoxY = square[a].Position.BoxY + square[a].Position.Box.Height;// *2 because i use 200% scaling
                        }
                    }
                }
                if (position.BoxX < mouse.X) //GO RIGHT 
                {
                    position.BoxX += 1;
                    speed -= 1;
                    currentState = HeroState.WalkRight;                   
                    for (int a = 0; a < square.Count; a++)
                    {

                        if (square[a].Intersect(position.Box))
                        {
                            if ((position.BoxY + HeroRectHeight * 2)- square[a].Position.BoxY < square[a].Position.Box.Height)
                            {
                                position.BoxY = square[a].Position.BoxY - HeroRectHeight*2;
                            }
                            else
                            {
                                position.BoxX = square[a].Position.BoxX - HeroRectWidth * 2;// *2 because i use 200% scaling
                            }
                        }
                    }
                }
                if (position.BoxX > mouse.X) //GO LEFT
                {
                    position.BoxX -= 1;
                    speed -= 1;
                    currentState = HeroState.WalkLeft;
                    for (int a = 0; a < square.Count; a++)
                    {
                        if (square[a].Intersect(position.Box))
                        {
                            if ((position.BoxY + HeroRectHeight * 2) - square[a].Position.BoxY < square[a].Position.Box.Height)
                            {
                                position.BoxY = square[a].Position.BoxY - HeroRectHeight * 2;
                            }
                            else
                            {
                                position.BoxX = square[a].Position.BoxX + square[a].Position.Box.Width;// *2 because i use 200% scaling
                            }
                        }
                    }
                }
                
            }
            speed = 5;

        }
        public override void Update(GameTime gametime)
        {

        }
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
        private void DrawIdle(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * HeroRectWidth,     //   - This rectangle specifies
                    HeroRectOffset * 0,           //	   where "inside" the texture
                    HeroRectWidth,             //     to get pixels (We don't want to
                    HeroRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawWalking(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * HeroRectWidth,     //   - This rectangle specifies
                    HeroRectOffset*1,           //	   where "inside" the texture
                    HeroRectWidth,             //     to get pixels (We don't want to
                    HeroRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawAttack(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * HeroRectWidth,     //   - This rectangle specifies
                    HeroRectOffset*2,           //	   where "inside" the texture
                    HeroRectWidth,             //     to get pixels (We don't want to
                    HeroRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawGetUp(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * HeroRectWidth,     //   - This rectangle specifies
                    HeroRectOffset * 3,           //	   where "inside" the texture
                    HeroRectWidth,             //     to get pixels (We don't want to
                    HeroRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawJumping(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    5 * HeroRectWidth,     //   - This rectangle specifies
                    HeroRectOffset * 4,           //	   where "inside" the texture
                    HeroRectWidth,             //     to get pixels (We don't want to
                    HeroRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        // Method for Drawing the hero
        public override void Draw(SpriteBatch sb)
        {
            switch (currentState)
            {
                case HeroState.WalkRight:
                {
                        DrawWalking(SpriteEffects.FlipHorizontally, sb);
                        break;
                }
                case HeroState.WalkLeft:
                {
                        DrawWalking(SpriteEffects.None, sb);
                        break;
                }
                case HeroState.Attack:
                {
                       DrawAttack(SpriteEffects.None, sb);
                       break;
                }
                case HeroState.Jumping:
                {
                        DrawJumping(SpriteEffects.None, sb);
                       break;
                }

            }
            //DrawIdle(SpriteEffects.None, sb);
            //DrawWalking(SpriteEffects.None, sb);
            //DrawAttack(SpriteEffects.None, sb);
            //DrawGetUp(SpriteEffects.None, sb);
        }

        protected override bool CheckCollision(List<Hitbox> objects)
        {
            return false;
        }
    }
}
