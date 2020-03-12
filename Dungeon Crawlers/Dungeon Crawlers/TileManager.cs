using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawlers
{
    class TileManager
    {
        // Fields
        private int numTilesWidth;
        private int numTilesHeight;
        private const int tileWidth = 64;
        private const int tileHeight = 64;
        private const int shortTileHeight = 32;
        private const int spikesHeight = 26;
        private const int shortTileWidth = 48;
        private List<Tile> tiles;
        private TileType type = TileType.None;
        private int spriteNumWidth;
        private int spriteNumHeight;
        private Single rotation = 0;
        private SpriteEffects flipSprite = SpriteEffects.None;

        // Constructor
        public TileManager()
        {
            tiles = new List<Tile>();
        }

        // Methods
        private void LoadLevel(Texture2D asset)
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("test.txt");

                string line;
                if ((line = reader.ReadLine()) != null)
                {
                    string[] dimensions = line.Split(',');
                    numTilesWidth = int.Parse(dimensions[0]);
                    numTilesHeight = int.Parse(dimensions[1]);
                }
                for (int i = 0; i < numTilesHeight; i++)
                {
                    for (int j = 0; j < numTilesWidth; j++)
                    {
                        string row = reader.ReadLine();
                        char[] characters = row.ToCharArray();

                        foreach (char tile in characters)
                        {
                            // ~~~~~~~~~~~~~ Floors ~~~~~~~~~~~~~~
                            if (tile == '1' || tile == 'Q' || tile == 'A' || tile == 'Z')
                            {
                                type = TileType.Floor;
                            }
                            // ~~~~~~~~~~~~Half Tiles ~~~~~~~~~~~~
                            else if (tile == '2' || tile == 'W' || tile == 'S' || tile == 'X')
                            {
                                type = TileType.HalfTile;
                                spriteNumWidth = 1;
                            }
                            // ~~~~~~~~~~~~ Divider ~~~~~~~~~~~~~
                            else if (tile == '3' || tile == 'E')
                            {
                                type = TileType.Divider;
                                spriteNumWidth = 2;
                            }
                            // ~~~~~~~~~~~ Slant Corner ~~~~~~~~~~~~
                            else if (tile == '4' || tile == 'R' || tile == 'F' || tile == 'V')
                            {
                                type = TileType.SlantCorner;
                                spriteNumWidth = 3;
                            }
                            //~~~~~~~~~~~~ Full Corner ~~~~~~~~~~~~~~~
                            else if (tile == '5' || tile == 'T' || tile == 'G' || tile == 'B')
                            {
                                type = TileType.FullCorner;
                                spriteNumWidth = 4;
                            }
                            //~~~~~~~~~~~ Black Space ~~~~~~~~~~~~~~
                            else if (tile == '6')
                            {
                                type = TileType.BlackBlock;
                                spriteNumWidth = 5;
                            }
                            //~~~~~~~~~~~~~~ Stairs ~~~~~~~~~~~~~~~~
                            else if (tile == '7' || tile == 'U')
                            {
                                type = TileType.Stairs;
                                spriteNumHeight = 1;
                            }
                            //~~~~~~~~~~~ Stair Corner ~~~~~~~~~~~~~~
                            else if (tile == '8' || tile == 'I')
                            {
                                type = TileType.StairTriangle;
                                spriteNumWidth = 1;
                                spriteNumHeight = 1;
                            }
                            //~~~~~~~~~~~~~ Platform ~~~~~~~~~~~~~~
                            else if (tile == '9' || tile == 'O')
                            {
                                type = TileType.Platform;
                                spriteNumWidth = 2;
                                spriteNumHeight = 1;
                            }
                            //~~~~~~~~~~ Platform Edge ~~~~~~~~~~~~~
                            else if (tile == '0' || tile == 'P')
                            {
                                type = TileType.PlatformEdge;
                                spriteNumWidth = 3;
                                spriteNumHeight = 1;
                            }
                            //~~~~~~~~~~~~~ Spikes ~~~~~~~~~~~~~~~~~
                            else if (tile == '-' || tile == '[')
                            {
                                type = TileType.Spikes;
                                spriteNumWidth = 4;
                                spriteNumHeight = 1;
                            }
                            //~~~~~~~~~~~~~ Enemy ~~~~~~~~~~~~~~~~
                            else if (tile == '=' || tile == '+' || tile == ']')
                            {
                                type = TileType.Enemy;
                            }


                            //~~~~~~~~~~ Rotation 90 Degrees ~~~~~~~~~~~
                            if (tile == 'Q' || tile == 'W' || tile == 'E' || tile == 'R' || tile == 'T')
                            {
                                rotation = (Single)Math.PI / 2;
                            }
                            //~~~~~~~~~~ Rotation 180 Degrees ~~~~~~~~~~
                            else if (tile == 'A' || tile == 'S' || tile == 'F' || tile == 'G')
                            {
                                rotation = (Single)Math.PI;
                            }
                            //~~~~~~~~~~ Rotation 270 Degrees ~~~~~~~~~~
                            else if (tile == 'Z' || tile == 'X' || tile == 'V' || tile == 'B')
                            {
                                rotation = (Single)(3 * Math.PI / 2);
                            }


                            //~~~~~~~~~~~~ Flip Horizontally ~~~~~~~~~~~~
                            if (tile == 'U' || tile == 'I' || tile == 'O' || tile == 'P')
                            {
                                flipSprite = SpriteEffects.FlipHorizontally;
                            }
                            //~~~~~~~~~~~~~ Flip Vertically ~~~~~~~~~~~~~~
                            else if (tile == '[')
                            {
                                flipSprite = SpriteEffects.FlipVertically;
                            }


                            Hitbox position = null;
                            // Tiles that are 64x64
                            if (type == TileType.Floor || type == TileType.Divider || type == TileType.SlantCorner || type == TileType.FullCorner
                                || type == TileType.BlackBlock || type == TileType.Stairs)
                            {
                                position = new Hitbox(new Rectangle(j * tileWidth, i * tileHeight, tileWidth, tileHeight), BoxType.Collision);
                            }
                            // Tiles that are 64x32
                            else if (type == TileType.HalfTile || type == TileType.Platform)
                            {
                                position = new Hitbox(new Rectangle(j * tileWidth, i * tileHeight, tileWidth, shortTileHeight), BoxType.Collision);
                            }
                            // Tiles that are 48x32
                            else if (type == TileType.PlatformEdge)
                            {
                                position = new Hitbox(new Rectangle(j * tileWidth, i * tileHeight, shortTileWidth, shortTileHeight), BoxType.Collision);
                            }
                            // Tiles that are 64x26
                            else if (type == TileType.Spikes)
                            {
                                position = new Hitbox(new Rectangle(j * tileWidth, i * tileHeight, tileWidth, spikesHeight), BoxType.Hurtbox);
                            }
                            else if (type == TileType.StairTriangle)
                            {
                                position = new Hitbox(new Rectangle(j * tileWidth, i * tileHeight, 15, 15), BoxType.Collision);
                            }

                            tiles.Add(new Tile(asset, position, type));
                                
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }

        private void DrawLevel(SpriteBatch sb)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(sb, spriteNumWidth, spriteNumHeight, flipSprite, rotation);
            }
        }
    }
}
