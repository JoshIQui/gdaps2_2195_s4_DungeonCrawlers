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
    // Enum used to differentiate the many different tile types
    enum TileType
    {
        Floor,
        Spikes,
        Stairs,
        PlatformEdge,
        Platform,
        Divider,
        HalfTile,
        DoubleSide,
        FullCorner,
        StairTriangle,
        BlackBlock,
        Enemy,
        None
    }
    class Tile
    {
        // Fields
        //private bool isDamageable;
        private Texture2D asset;
        private Hitbox position;
        private TileType type;
        private float rotation;
        private SpriteEffects flipSprite;
        private int spriteNumWidth;
        private int spriteNumHeight;

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
        public Tile(Texture2D asset, Hitbox position, TileType type, float rotation, SpriteEffects flipSprite, int spriteNumWidth, int spriteNumHeight)
        {
            this.asset = asset;
            this.position = position;
            this.type = type;
            this.rotation = rotation;
            this.flipSprite = flipSprite;
            this.spriteNumWidth = spriteNumWidth;
            this.spriteNumHeight = spriteNumHeight;
        }

        // Method that draws the asset depending on the tile type
        public void Draw(SpriteBatch sb)
        {
            if (position == null)
            {
                return;
            }
            // Rotated Tiles that are not 64x64 must be accomodated
            int adjustmentX = 32;
            int adjustmentY = 32;
            if (type == TileType.HalfTile)
            {
                if (rotation == (float)Math.PI / 2)
                {
                    adjustmentX = 0;
                }
                else if (rotation == (float)Math.PI)
                {
                    adjustmentY = 0;
                }
            }
                sb.Draw(
                asset,                                                      // - The texture to draw
                new Vector2(position.ScreenPositionX + adjustmentX,
                            position.WorldPositionY + adjustmentY),         // - The location to draw on the screen
                new Rectangle(                                              // - The "source" rectangle
                    spriteNumWidth * offset,                                // - This rectangle specifies where "inside" the texture
                    offset * spriteNumHeight,
                    width,                                                  // to get pixels (We don't want to draw the whole thing)
                    height),  
                Color.White,                                                // - The color
                rotation,                                                   // - Rotation
                //Vector2.Zero,
                new Vector2(width / 2, height / 2),                         // - Origin inside the image (center of tile)
                1.0f,                                                       // - Scale (100% - no change)
                flipSprite,                                                 // - Can be used to flip the image
                0);                                                         // - Layer depth (unused)

        }        
    }
}
