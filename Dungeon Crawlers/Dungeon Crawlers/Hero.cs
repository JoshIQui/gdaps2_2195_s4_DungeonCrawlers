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
    class Hero : GameObject, IHaveAI
    {
        // Fields
        Texture2D asset;
        Rectangle position;
        private int health;
        private int width;
        private int height;
        public int Health
        {
            get { return health; }
        }

        public Hero(Texture2D asset, Rectangle position, GraphicsDeviceManager graphics, int screenWidth, int screenHeight, int health = 100)
            :base (asset,position,graphics)
        {
            this.health = health;
            this.width = screenWidth;
            this.health = screenHeight;
        }
        protected override bool CheckCollision(List<Hitbox> objects)
        {

            return false;
        }
    }
}
