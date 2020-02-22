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
        Texture2D asset;
        Rectangle position;
        int health;
        int numEnemies;

        // Properties
        public int Health
        {
            get { return health; }
        }

        // Constructor
        public Player(Texture2D asset, Rectangle position, GraphicsDeviceManager graphics, int screenWidth, int screenHeight, int health = 100, int numEnemies = 0)
        {
            this.asset = asset;
            this.position = position;
            this.graphics = graphics;
        }

        // Methods
        public override void Update()
        {

        }
        protected override bool CheckCollision(List<Hitbox> objects)
        {
            return false;
        }


    }
}
