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
    class Player : GameObject
    {
        // Fields
        private int health;
        private int numEnemies;

        // Properties
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int NumEnemies
        {
            get { return numEnemies; }
            set { numEnemies = value; }
        }

        // Constructor
        public Player(Texture2D asset, Hitbox position, int screenWidth, int screenHeight, int health = 100, int numEnemies = 0)
            : base(asset, position, graphics)
        {
            this.asset = asset;
            this.position = position;
            this.graphics = graphics;
            this.health = health;
            this.numEnemies = numEnemies;
        }

        // Methods

        // Method for Player Updates in game
        public override void Update(GameTime gametime)
        {

        }

        // Method for Drawing the Player
        public override void Draw(SpriteBatch sb)
        {
            
        }

        // Method for Collision Checks for player
        protected override bool CheckCollision(List<Hitbox> objects)
        {
            return false;
        }


    }
}
