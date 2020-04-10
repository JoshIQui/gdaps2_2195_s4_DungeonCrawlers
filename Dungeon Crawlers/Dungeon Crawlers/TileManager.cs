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
        private List<Hitbox> hitBoxes;

        // Properties
        public static TileManager Instance
        {
            get
            {
                if (mgrInstance == null)
                {
                    mgrInstance = new TileManager();
                }
                return mgrInstance;
            }
        }

        public List<Hitbox> HitBoxes
        {
            get
            {
                return hitBoxes;
            }
        }

        public int LevelWidth
        {
            get
            {
                int levelWidth = 0;
                foreach(Hitbox h in hitBoxes)
                {
                    if (h.WorldPositionX > levelWidth)
                    {
                        levelWidth = h.WorldPositionX;
                    }
                }
                return levelWidth + 64;
            }
        }

        // Constructor
        private TileManager()
        {
            tiles = new List<Tile>();
            hitBoxes = new List<Hitbox>();
        }

        // Static Instance
        public static TileManager mgrInstance;

        // Methods
        public void LoadLevel(Texture2D asset)
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("testLevel.txt");

                string line;
                if ((line = reader.ReadLine()) != null)
                {
                    string[] dimensions = line.Split(',');
                    numTilesWidth = int.Parse(dimensions[0]);
                    numTilesHeight = int.Parse(dimensions[1]);
                }
                for (int i = 0; i < numTilesHeight; i++)
                {
                    string row = reader.ReadLine();
                    char[] characters = row.ToCharArray();
                    for (int j = 0; j < numTilesWidth; j++)
                    {

                        // ~~~~~~~~~~~~~ Floors ~~~~~~~~~~~~~~
                        if (characters[j] == '1' || characters[j] == 'Q' || characters[j] == 'A' || characters[j] == 'Z')
                        {
                            type = TileType.Floor;
                            spriteNumWidth = 0;
                            spriteNumHeight = 0;
                        }
                        // ~~~~~~~~~~~~Half Tiles ~~~~~~~~~~~~
                        else if (characters[j] == '2' || characters[j] == 'W' || characters[j] == 'S' || characters[j] == 'X')
                        {
                            type = TileType.HalfTile;
                            spriteNumWidth = 1;
                            spriteNumHeight = 0;
                        }
                        // ~~~~~~~~~~~~ Divider ~~~~~~~~~~~~~
                        else if (characters[j] == '3' || characters[j] == 'E')
                        {
                            type = TileType.Divider;
                            spriteNumWidth = 2;
                            spriteNumHeight = 0;
                        }
                        // ~~~~~~~~~~~ Slant Corner ~~~~~~~~~~~~
                        else if (characters[j] == '4' || characters[j] == 'R' || characters[j] == 'F' || characters[j] == 'V')
                        {
                            type = TileType.SlantCorner;
                            spriteNumWidth = 3;
                            spriteNumHeight = 0;
                        }
                        //~~~~~~~~~~~~ Full Corner ~~~~~~~~~~~~~~~
                        else if (characters[j] == '5' || characters[j] == 'T' || characters[j] == 'G' || characters[j] == 'B')
                        {
                            type = TileType.FullCorner;
                            spriteNumWidth = 4;
                            spriteNumHeight = 0;
                        }
                        //~~~~~~~~~~~ Black Space ~~~~~~~~~~~~~~
                        else if (characters[j] == '6')
                        {
                            type = TileType.BlackBlock;
                            spriteNumWidth = 5;
                            spriteNumHeight = 0;
                        }
                        //~~~~~~~~~~~~~~ Stairs ~~~~~~~~~~~~~~~~
                        else if (characters[j] == '7' || characters[j] == 'U')
                        {
                            type = TileType.Stairs;
                            spriteNumWidth = 0;
                            spriteNumHeight = 1;
                        }
                        //~~~~~~~~~~~ Stair Corner ~~~~~~~~~~~~~~
                        else if (characters[j] == '8' || characters[j] == 'I')
                        {
                            type = TileType.StairTriangle;
                            spriteNumWidth = 1;
                            spriteNumHeight = 1;
                        }
                        //~~~~~~~~~~~~~ Platform ~~~~~~~~~~~~~~
                        else if (characters[j] == '9' || characters[j] == 'O')
                        {
                            type = TileType.Platform;
                            spriteNumWidth = 2;
                            spriteNumHeight = 1;
                        }
                        //~~~~~~~~~~ Platform Edge ~~~~~~~~~~~~~
                        else if (characters[j] == '0' || characters[j] == 'P')
                        {
                            type = TileType.PlatformEdge;
                            spriteNumWidth = 3;
                            spriteNumHeight = 1;
                        }
                        //~~~~~~~~~~~~~ Spikes ~~~~~~~~~~~~~~~~~
                        else if (characters[j] == '-' || characters[j] == '[')
                        {
                            type = TileType.Spikes;
                            spriteNumWidth = 4;
                            spriteNumHeight = 1;
                        }
                        //~~~~~~~~~~~~~ Enemy ~~~~~~~~~~~~~~~~
                        else if (characters[j] == '=' || characters[j] == '+' || characters[j] == ']')
                        {
                            type = TileType.Enemy;
                        }
                        //~~~~~~~~~~~ Blank Space ~~~~~~~~~~~~~
                        else if (characters[j] == '~')
                        {
                            type = TileType.None;
                        }


                        //~~~~~~~~~~ Rotation 90 Degrees ~~~~~~~~~~~
                        if (characters[j] == 'Q' || characters[j] == 'W' || characters[j] == 'E' || characters[j] == 'R' || characters[j] == 'T')
                        {
                            rotation = (Single)Math.PI / 2;
                        }
                        //~~~~~~~~~~ Rotation 180 Degrees ~~~~~~~~~~
                        else if (characters[j] == 'A' || characters[j] == 'S' || characters[j] == 'F' || characters[j] == 'G')
                        {
                            rotation = (Single)Math.PI;
                        }
                        //~~~~~~~~~~ Rotation 270 Degrees ~~~~~~~~~~
                        else if (characters[j] == 'Z' || characters[j] == 'X' || characters[j] == 'V' || characters[j] == 'B')
                        {
                            rotation = (Single)(3 * Math.PI / 2);
                        }
                        //~~~~~~~~~~~~~ No Rotation ~~~~~~~~~~~~~~
                        else
                        {
                            rotation = 0;
                        }


                        //~~~~~~~~~~~~ Flip Horizontally ~~~~~~~~~~~~
                        if (characters[j] == 'U' || characters[j] == 'I' || characters[j] == 'O' || characters[j] == 'P')
                        {
                            flipSprite = SpriteEffects.FlipHorizontally;
                        }
                        //~~~~~~~~~~~~~ Flip Vertically ~~~~~~~~~~~~~~
                        else if (characters[j] == '[')
                        {
                            flipSprite = SpriteEffects.FlipVertically;
                        }
                        //~~~~~~~~~~~~~~~ No Transformation ~~~~~~~~~~~~~~
                        else
                        {
                            flipSprite = SpriteEffects.None;
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

                        if (position != null)
                        {
                            hitBoxes.Add(position);
                            tiles.Add(new Tile(asset, position, type, rotation, flipSprite, spriteNumWidth, spriteNumHeight));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            if (reader != null)
            {
                reader.Close();
            }
        }

        public void DrawLevel(SpriteBatch sb)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(sb);
            }
        }
    }
}
