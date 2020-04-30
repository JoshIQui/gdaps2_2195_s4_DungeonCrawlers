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
        private const int enemyWidth = 72;
        private const int enemyHeight = 90;
        private List<Tile> tiles;
        private TileType type = TileType.None;
        private int spriteNumWidth;
        private int spriteNumHeight;
        private float rotation = 0;
        private SpriteEffects flipSprite = SpriteEffects.None;
        private List<Hitbox> tileHitboxes;
        private List<Hitbox> enemyHitboxes;
        private List<EnemyPickUp> enemyPickUps;
        private const int MaxLevelSize = 3648;
        private bool flagged = false;

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

        public List<Hitbox> TileHitBoxes
        {
            get
            { return tileHitboxes; }
        }

        public List<EnemyPickUp> EnemyPickUps
        {
            get { return enemyPickUps; }
        }

        public int LevelWidth
        {
            get
            {
                int levelWidth = 0;
                foreach(Hitbox h in tileHitboxes)
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
            tileHitboxes = new List<Hitbox>();
            enemyPickUps = new List<EnemyPickUp>();
            enemyHitboxes = new List<Hitbox>();
        }

        // Static Instance
        public static TileManager mgrInstance;

        // Methods
        public void LoadLevel(Texture2D tileTextures, Texture2D charTextures, string filename, int sequenceNum)
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(filename);

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
                        if (characters[j] == '1' || characters[j] == '!' || characters[j] == 'Q' || characters[j] == 'A' || characters[j] == 'Z')
                        {
                            type = TileType.Floor;
                            spriteNumWidth = 0;
                            spriteNumHeight = 0;
                        }
                        // ~~~~~~~~~~~~Half Tiles ~~~~~~~~~~~~
                        else if (characters[j] == '2' || characters[j] == '@' || characters[j] == 'W' || characters[j] == 'S' || characters[j] == 'X')
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
                        // ~~~~~~~~~~~ Double Side ~~~~~~~~~~~~
                        else if (characters[j] == '4' || characters[j] == 'R' || characters[j] == 'F' || characters[j] == 'V')
                        {
                            type = TileType.DoubleSide;
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
                        else if (characters[j] == '9' || characters[j] == '(' || characters[j] == 'O')
                        {
                            type = TileType.Platform;
                            spriteNumWidth = 2;
                            spriteNumHeight = 1;
                        }
                        //~~~~~~~~~~ Platform Edge ~~~~~~~~~~~~~
                        else if (characters[j] == '0' || characters[j] == ')' || characters[j] == 'P' || characters[j] == 'p')
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
                            rotation = (float)Math.PI / 2;
                        }
                        //~~~~~~~~~~ Rotation 180 Degrees ~~~~~~~~~~
                        else if (characters[j] == 'A' || characters[j] == 'S' || characters[j] == 'F' || characters[j] == 'G')
                        {
                            rotation = (float)Math.PI;
                        }
                        //~~~~~~~~~~ Rotation 270 Degrees ~~~~~~~~~~
                        else if (characters[j] == 'Z' || characters[j] == 'X' || characters[j] == 'V' || characters[j] == 'B')
                        {
                            rotation = (float)(3 * Math.PI / 2);
                        }
                        //~~~~~~~~~~~~~ No Rotation ~~~~~~~~~~~~~~
                        else
                        {
                            rotation = 0;
                        }


                        //~~~~~~~~~~~~ Flip Horizontally ~~~~~~~~~~~~
                        if (characters[j] == 'U' || characters[j] == 'I' || characters[j] == 'O' || characters[j] == 'P' || characters[j] == 'p')
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


                        //~~~~~~~~~~~~~~~~~ Flagged ~~~~~~~~~~~~~~~~~~~~~
                        if (characters[j] == '!' || characters[j] == ')' || characters[j] == 'p' || characters[j] == '@' || characters[j] == '(')
                        {
                            flagged = true;
                        }
                        else
                        {
                            flagged = false;
                        }

                        Hitbox position = null;
                        Hitbox flagBox = null;
                        // Tiles that are 64x64
                        if (type == TileType.Floor || type == TileType.Divider || type == TileType.DoubleSide || type == TileType.FullCorner
                            || type == TileType.BlackBlock || type == TileType.Stairs)
                        {
                            position = new Hitbox(new Rectangle(j * tileWidth + (sequenceNum * MaxLevelSize), i * tileHeight, tileWidth, tileHeight), BoxType.Collision);

                            if (flagged)
                            {
                                flagBox = new Hitbox(position.Box, BoxType.Flag);
                            }
                        }
                        // Tiles that are 64x32
                        else if (type == TileType.HalfTile || type == TileType.Platform)
                        {                            
                            position = new Hitbox(new Rectangle(j * tileWidth + (sequenceNum * MaxLevelSize), i * tileHeight, tileWidth, shortTileHeight), BoxType.Collision);
                            
                            if (rotation == (float)Math.PI / 2)
                            {
                                position.Box = new Rectangle(position.WorldPositionX + shortTileHeight, position.WorldPositionY, shortTileHeight, tileWidth);
                            }
                            else if (rotation == (float)Math.PI)
                            {
                                position.Box = new Rectangle(position.WorldPositionX, position.WorldPositionY + shortTileHeight, tileWidth, shortTileWidth);
                            }
                            else if (rotation == (float)(3 * Math.PI / 2))
                            {
                                position.Box = new Rectangle(position.WorldPositionX, position.WorldPositionY, shortTileHeight, tileWidth);
                            }

                            if (flagged)
                            {
                                flagBox = new Hitbox(position.Box, BoxType.Flag);
                            }
                        }
                        // Tiles that are 48x32
                        else if (type == TileType.PlatformEdge)
                        {
                            position = new Hitbox(new Rectangle(j * tileWidth + (sequenceNum * MaxLevelSize), i * tileHeight, shortTileWidth, shortTileHeight), BoxType.Collision);

                            if (flagged)
                            {
                                flagBox = new Hitbox(position.Box, BoxType.Flag);
                            }
                        }
                        // Tiles that are 64x26
                        else if (type == TileType.Spikes)
                        {
                            position = new Hitbox(new Rectangle(j * tileWidth + (sequenceNum * MaxLevelSize), i * tileHeight, tileWidth, spikesHeight), BoxType.Hurtbox);
                        }
                        else if (type == TileType.StairTriangle)
                        {
                            position = new Hitbox(new Rectangle(j * tileWidth + (sequenceNum * MaxLevelSize), i * tileHeight, 15, 15), BoxType.Collision);
                        }
                        else if (type == TileType.Enemy)
                        {
                            position = new Hitbox(new Rectangle(j * tileWidth + (sequenceNum * MaxLevelSize), i * tileHeight - 26, enemyWidth, enemyHeight), BoxType.Hitbox);
                        }

                        if (position != null && type != TileType.Enemy)
                        {
                            tileHitboxes.Add(position);
                            if (flagBox != null)
                            {
                                tileHitboxes.Add(flagBox);
                            }
                            tiles.Add(new Tile(tileTextures, position, type, rotation, flipSprite, spriteNumWidth, spriteNumHeight));
                        }
                        else if (type == TileType.Enemy)
                        {
                            enemyHitboxes.Add(position);
                            enemyPickUps.Add(new EnemyPickUp(charTextures, position));
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
                foreach (EnemyPickUp e in enemyPickUps)
                {
                    if (e.PickedUp == false)
                    {
                        e.Draw(sb);
                    }
                }
        }
    }
}
