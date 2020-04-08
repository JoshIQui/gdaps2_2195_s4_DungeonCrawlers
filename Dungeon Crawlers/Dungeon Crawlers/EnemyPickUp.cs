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
    class EnemyPickUp : GameObject
    {
        // Boolean will check if the pick up is active
        private bool pickedUp;

        // Constants for rectangle in the spritesheet
        const int EnemyRectOffsetWalk = 48;   // How far down in the image are the frames? FOR THE RUN
        const int EnemyRectHeight = 45;     // The height of a single frame
        const int EnemyRectWidth = 88;     // The width of a single frame
        const int OffsetX = 50;

        // Property for pick-up activeness
        public bool PickedUp
        {
            get { return pickedUp; }
        }

        // Constructor
        public EnemyPickUp(Texture2D asset, Hitbox position)
            : base(asset, position)
        {
            pickedUp = false;
        }

        // Method that checks if the object is picked up or not
        public void Logic(Player target)
        {
            if(target.Position.Box.Intersects(position.Box) && !pickedUp)
            {
                pickedUp = true;
                target.NumEnemies++;
            }
        }

        // Method for Drawing Object
        public override void Draw(SpriteBatch sb)
        {
            DrawPickUp(SpriteEffects.None, sb);
        }

        // Method for Drawing the Enemy Pick-up
        private void DrawPickUp(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.ScreenPositionX - OffsetX, position.WorldPositionY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    0 * EnemyRectWidth,     //   - This rectangle specifies
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

        public override void Update(GameTime gameTime)
        {

        }
        public override void CheckCollision(List<Hitbox> objects)
        {
            
        }
    }
}
