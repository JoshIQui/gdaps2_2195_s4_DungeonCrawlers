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
        JumpRight,
        JumpLeft
        // Add state(s) to support crouching
    }
    class Hero : GameObject, IHaveAI
    {
        // Fields  
        private int xDistace;
        private int yDistace;
        private int step;
        private int health;
        private int width;
        private int height;
        private bool jumping = false;
        private int jumpHeight = 300;
        private bool onGround = false;
        private double jumpSpd = 12;
        private double fallSpd = 1;
        const double gravityAccel = 0.5;
        Texture2D uIAsset;

        // Animation
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame

        // Constants for "source" rectangle(inside the image)
        const int WalkFrameCount = 7;       // The number of frames in the animation
        const int HeroRectOffset = 49;   // How far down in the image are the frames?
        const int HeroRectHeight = 48;     // The height of a single frame
        const int HeroNormRectWidth = 31;
        const int HeroAttackRectWidth = 48;
        const int Displacement = 14;

        HeroState currentState = HeroState.WalkRight;
        //int gravity = 9;
        bool[,] obstacle;

        List<Hitbox> hitboxes;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public bool IsJumping
        {
            get { return jumping; }
        }
       
        public Hero(Texture2D asset, Texture2D uIAsset, Hitbox position, int screenWidth, int screenHeight, int health = 100)
            :base (asset,position)
        {
            this.uIAsset = uIAsset;
            this.health = health;
            this.width = screenWidth;
            this.height = screenHeight;                       

            obstacle = new bool[height+1, width+1];
            // Initialize
            fps = 6.0;                     // Will cycle through 10 walk frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
        }
        public void logic(Player target,List<Hitbox> square)
        {
            
            //debug.X = target.Position.BoxX;
            //debug.Y = target.Position.BoxY;
            //debug.Width = 1;
            //debug.Height = 1;

            if (jumping == false)
            {
                fallSpd = fallSpd + gravityAccel;
                position.WorldPositionY += (int)fallSpd; //gravity

                for (int a = 0; a < square.Count; a++)
                {
                    if (square[a] != null)
                    {
                        if (square[a].Box.Intersects(position.Box))
                        {
                            if (square[a].BoxType == BoxType.Collision)
                            {
                                position.WorldPositionY = square[a].WorldPositionY - position.Box.Height;//
                                onGround = true;
                                fallSpd = 1;
                            }
                            if (square[a].BoxType == BoxType.Hurtbox)
                            {
                                health--;
                            }
                            
                            break; //whan its on at least 1 ground, break it.
                        }
                        else
                        {
                            onGround = false;
                        }
                    }
                }
            }

            if (jumping)
            {
                onGround = false;
                jumpSpd = jumpSpd - gravityAccel;
                position.WorldPositionY -= (int)jumpSpd;
                
                for (int a = 0; a < square.Count; a++)
                {
                    if (square[a] != null)
                    {
                        if (square[a].Box.Intersects(position.Box))
                        {
                            if (square[a].BoxType == BoxType.Collision)
                            {
                                position.WorldPositionY = square[a].WorldPositionY + square[a].Box.Height;
                            }
                            if (square[a].BoxType == BoxType.Hurtbox)
                            {
                                health--;
                            }
                            // *2 because i use 200% scaling

                        }
                    }
                }
                //jumpHeight -= 10;
            }
            if(jumpSpd <=0)
            {
                jumping = false;
                jumpSpd = 12;
            }
           
            if (position.WorldPositionY < target.Position.WorldPositionY) //going down (not needed)
            {
                
            }
            if (onGround == true) //going up (jumping)
            {
                for (int a = 0; a < square.Count; a++)
                {
                    if (square[a] != null)
                    {
                        if (square[a].Box.Intersects(position.Box))
                        {
                            if (square[a].BoxType == BoxType.Flag)
                            {
                                if (jumping == false)
                                {
                                    jumping = true;
                                    onGround = false;
                                }
                            }
                        }
                    }
                }
               
            }
            if (position.WorldPositionX < target.Position.WorldPositionX) //GO RIGHT 
            {
                position.WorldPositionX += 4;
                if (onGround == true)
                {
                    currentState = HeroState.WalkRight;
                }
                else
                {
                    currentState = HeroState.JumpRight;
                }
                for (int a = 0; a < square.Count; a++)
                {
                    if (square[a] != null)
                    {
                        if (square[a].Box.Intersects(position.Box))
                        {
                            if (square[a].BoxType == BoxType.Collision)
                            {
                                position.WorldPositionX = square[a].WorldPositionX; //- position.Box.Width;
                            }
                            if (square[a].BoxType == BoxType.Hurtbox)
                            {
                                health--;
                            }
                            /*  //Extra features that allow hero to "jump" for small obstacle
                             *  
                            if ((position.BoxY + HeroRectHeight * 2) - square[a].BoxY < square[a].Box.Height)
                            {
                                position.BoxY = square[a].BoxY - HeroRectHeight * 2;
                            }
                            else
                            {
                                position.BoxX = square[a].BoxX - HeroRectWidth * 2;// *2 because i use 200% scaling
                            }
                            */

                        }
                    }
                }
            }

            if (position.WorldPositionX > target.Position.WorldPositionX) //GO LEFT
            {
                position.WorldPositionX -= 4;
                if (onGround == true)
                {
                    currentState = HeroState.WalkLeft;
                }
                else
                {
                    currentState = HeroState.JumpLeft;
                }
                for (int a = 0; a < square.Count; a++)
                {
                    if (square[a] != null)
                    {
                        if (square[a].Box.Intersects(position.Box))
                        {
                            if (square[a].BoxType == BoxType.Collision)
                            {
                                position.WorldPositionX = square[a].WorldPositionX;// + square[a].Box.Width;
                            }
                            if (square[a].BoxType == BoxType.Hurtbox)
                            {
                                health--;
                            }
                            // *2 because i use 200% scaling

                            /* //Extra features that allow hero to "jump" for small obstacle

                            if ((position.BoxY + HeroRectHeight * 2) - square[a].BoxY < square[a].Box.Height)
                            {
                                position.BoxY = square[a].BoxY - HeroRectHeight * 2;
                            }
                            else
                            {
                                position.BoxX = square[a].BoxX + square[a].Box.Width;// *2 because i use 200% scaling
                            }
                            */

                        }
                    }
                }
            }
            if (position.Box.Intersects(target.Position.Box))// attack condition
            {
                xDistace = position.Box.Location.X - target.Position.Box.Location.X;
                yDistace = position.Box.Location.Y - target.Position.Box.Location.Y;

                if (xDistace * xDistace + yDistace * yDistace <= 100) 
                //pythagorem theory that indicatses if the location of the points is less than 10 pixel away form evey direction)
                {
                    currentState = HeroState.Attack;
                    if(target.Position.Box.Intersects(position.Box)) // If touching the player then attack the player and take damage if player is attacking
                    {
                        if(target.PlayerState == PlayerState.AttackingLeft || target.PlayerState == PlayerState.AttackingRight)
                        {
                            health--;
                            target.Health--;
                        }
                        else
                        {
                            target.Health--;
                        }
                    }
                    
                }
                //break;
            }
            if(health <= 0)
            {
                StateManager.Instance.ChangeState(GameState.Win);
            }
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
                    frame = 0;

                timeCounter -= timePerFrame;    // Remove the time we "used" - don't reset to 0
                                                // This keeps the time passed 
            }
        }
        private void DrawIdle(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            position.Box = new Rectangle(position.WorldPositionX, position.WorldPositionY, 62, 96);

            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    Displacement + (frame * HeroAttackRectWidth),     //   - This rectangle specifies
                    HeroRectOffset * 0,           //	   where "inside" the texture
                    HeroNormRectWidth,             //     to get pixels (We don't want to
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
            position.Box = new Rectangle(position.WorldPositionX, position.WorldPositionY, 62, 96);

            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    Displacement + (frame * HeroAttackRectWidth),     //   - This rectangle specifies
                    HeroRectOffset*1,           //	   where "inside" the texture
                    HeroNormRectWidth,             //     to get pixels (We don't want to
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
            if (frame == 4 || frame == 5)
            {
                position.Box = new Rectangle(position.WorldPositionX, position.WorldPositionY, 96, 96);
            }
            else
            {
                position.Box = new Rectangle(position.WorldPositionX, position.WorldPositionY, 62, 96);
            }

            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX - 14, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    frame * HeroAttackRectWidth,     //   - This rectangle specifies
                    HeroRectOffset*2,           //	   where "inside" the texture
                    HeroAttackRectWidth,             //     to get pixels (We don't want to
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
            position.Box = new Rectangle(position.WorldPositionX, position.WorldPositionY, 62, 96);

            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    Displacement + (frame * HeroAttackRectWidth),     //   - This rectangle specifies
                    HeroRectOffset * 3,           //	   where "inside" the texture
                    HeroNormRectWidth,             //     to get pixels (We don't want to
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
            position.Box = new Rectangle(position.WorldPositionX, position.WorldPositionY, 62, 96);

            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    Displacement + (5 * HeroAttackRectWidth),     //   - This rectangle specifies
                    HeroRectOffset * 4,           //	   where "inside" the texture
                    HeroNormRectWidth,             //     to get pixels (We don't want to
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
                case HeroState.JumpLeft:
                {
                       DrawJumping(SpriteEffects.None, sb);
                       break;
                }
                case HeroState.JumpRight:
                {
                       DrawJumping(SpriteEffects.FlipHorizontally, sb);
                       break;
                }



            }
            //DrawIdle(SpriteEffects.None, sb);
            //DrawWalking(SpriteEffects.None, sb);
            //DrawAttack(SpriteEffects.None, sb);
            //DrawGetUp(SpriteEffects.None, sb);

            if (health > 90)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 0, 400, 40), Color.White);
            }
            else if (health > 80 && health <= 90)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 45, 400, 40), Color.White);
            }
            else if (health > 70 && health <= 80)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 90, 400, 40), Color.White);
            }
            else if (health > 60 && health <= 70)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 135, 400, 40), Color.White);
            }
            else if (health > 50 && health <= 60)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 180, 400, 40), Color.White);
            }
            else if (health > 40 && health <= 50)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 225, 400, 40), Color.White);
            }
            else if (health > 30 && health <= 40)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 270, 400, 40), Color.White);
            }
            else if (health > 20 && health <= 30)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 315, 400, 40), Color.White);
            }
            else if (health > 10 && health <= 20)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 360, 400, 40), Color.White);
            }
            else if (health <= 10)
            {
                sb.Draw(uIAsset, new Vector2(5, 10), new Rectangle(0, 405, 400, 40), Color.White);
            }
        }

        public override void CheckCollision(List<Hitbox> objects)
        {
            
        }
    }
}
