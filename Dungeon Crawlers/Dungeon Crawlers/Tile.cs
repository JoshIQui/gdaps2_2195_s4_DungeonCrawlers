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
    class Tile
    {
        // Fields
        protected bool isDamageable;
        protected Texture2D asset;
        protected Hitbox position;

        private int width = 64;
        private int height = 64;
        private int offset = 79;

        // Properties
        public Texture2D Asset
        {
            get { return asset; }
        }

        public Hitbox Position
        {
            get { return position; }
        }

        // Constructor
        public Tile(Texture2D asset, Hitbox position)
        {
            this.asset = asset;
            this.position = position;
        }

        // Method that draws the asset
        public void Draw(SpriteBatch sb)
        {
            DrawFloor(SpriteEffects.None, sb);
        }

        private void DrawFloor(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    0 * width,     //   - This rectangle specifies
                    offset * 0,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                2.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
    }
}
