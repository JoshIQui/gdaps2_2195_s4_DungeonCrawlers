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
        //LeftStairs,
        //RightStairs,
        PlatformEdge,
        //LeftLedge,
        //RightLedge,
        Platform,
        Divider,
        HalfTile,
        //TopHalfTile,
        //BottomHalfTile,
        SlantCorner,
        //TopLeftSlantCorner,
        //TopRightSlantCorner,
        //BottomRightSlantCorner,
        //BottomLeftSlantCorner,
        FullCorner,
        //TopLeftFullCorner,
        //TopRightFullCorner,
        //BottomRightFullCorner,
        //BottomLeftFullCorner,
        StairTriangle,
        //TopLeftTriangle,
        //TopRightTriangle,
        //BottomRightTriangle,
        //BottomLeftTriangle,
        BlackBlock,
        Enemy,
        None
    }
    class Tile
    {
        // Fields
        protected bool isDamageable;
        protected Texture2D asset;
        protected Hitbox position;
        protected TileType type;

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
        public Tile(Texture2D asset, Hitbox position, TileType type)
        {
            this.asset = asset;
            this.position = position;
            this.type = type;
        }

        // Method that draws the asset depending on the tile type
        public void Draw(SpriteBatch sb, int spriteNumWidth, int spriteNumHeight, SpriteEffects flipSprite, Single rotation)
        {
            sb.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    spriteNumWidth * width,     //   - This rectangle specifies
                    offset * spriteNumHeight,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                rotation,                              // - Rotation
                new Vector2(width / 2, height / 2),                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)

            /*
            switch (type)
            {
                case TileType.Spikes:
                    DrawSpikes(SpriteEffects.None, sb);
                    break;
                case TileType.Floor:
                    DrawFloor(SpriteEffects.None, sb);
                    break;
                case TileType.LeftStairs:
                    DrawStairs(SpriteEffects.FlipHorizontally, sb);
                    break;
                case TileType.RightStairs:
                    DrawStairs(SpriteEffects.None, sb);
                    break;
                case TileType.TopHalfTile:
                    DrawHalfBlock(SpriteEffects.None, sb);
                    break;
                case TileType.BottomHalfTile:
                    DrawHalfBlock(SpriteEffects.FlipVertically, sb);
                    break;
                case TileType.Divider:
                    DrawDivider(SpriteEffects.None,sb);
                    break;
                case TileType.TopLeftSlantCorner:
                    DrawSlantCorner(SpriteEffects.None, sb);
                    break;
                case TileType.TopRightSlantCorner:
                    DrawSlantCorner(SpriteEffects.FlipHorizontally, sb);
                    break;
                case TileType.BottomLeftFullCorner:
                    DrawFullCorner(SpriteEffects.None, sb);
                    break;
                case TileType.BottomRightFullCorner:
                    DrawFullCorner(SpriteEffects.FlipHorizontally, sb);
                    break;
                case TileType.BlackBlock:
                    DrawBlackBlock(SpriteEffects.None, sb);
                    break;
                case TileType.TopLeftTriangle:
                    DrawTriangle(SpriteEffects.None, sb);
                    break;
                case TileType.TopRightTriangle:
                    DrawTriangle(SpriteEffects.FlipHorizontally, sb);
                    break;
                case TileType.Platform:
                    DrawPlatform(SpriteEffects.None, sb);
                    break;
                case TileType.LeftLedge:
                    DrawLedge(SpriteEffects.FlipHorizontally, sb);
                    break;
                case TileType.RightLedge:
                    DrawLedge(SpriteEffects.None, sb);
                    break;
            }
            */
        }

        // ---------------------------
        // Methods for Drawing Tiles
        // ---------------------------
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
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        private void DrawSpikes(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    5 * width,     //   - This rectangle specifies
                    offset * 1,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawStairs(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    0 * width,     //   - This rectangle specifies
                    offset * 1,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawHalfBlock(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    1 * width + 15,     //   - This rectangle specifies
                    offset * 0,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawDivider(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    2 * width + 30,     //   - This rectangle specifies
                    offset * 0,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawSlantCorner(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    3 * width + 45,     //   - This rectangle specifies
                    offset * 0,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawFullCorner(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    4 * width + 60,     //   - This rectangle specifies
                    offset * 0,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawBlackBlock(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    5 * width + 75,     //   - This rectangle specifies
                    offset * 0,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawTriangle(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    1 * width + 15,     //   - This rectangle specifies
                    offset * 1,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawPlatform(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    2 * width + 30,     //   - This rectangle specifies
                    offset * 1,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
        private void DrawLedge(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                asset,                    // - The texture to draw
                new Vector2(position.BoxX, position.BoxY),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    3 * width + 45,     //   - This rectangle specifies
                    offset * 1,           //	   where "inside" the texture
                    width,             //     to get pixels (We don't want to
                    height),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }
    }
}
