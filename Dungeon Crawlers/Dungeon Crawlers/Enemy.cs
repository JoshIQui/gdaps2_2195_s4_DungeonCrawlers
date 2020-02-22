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
    class Enemy : GameObject, IHaveAI
    {
        private int speed;
        private int health;
        private int width;
        private int height;
        
        public int Health
        {
            get { return health; }
        }

        public Enemy(Texture2D asset, Hitbox position, int screenWidth, int screenHeight, int health = 100)
            : base(asset, position)
        {
            this.health = health;
            this.width = screenWidth;
            this.health = screenHeight;
        }
        public override void Update(GameTime gametime)
        {

        }
        public override void Draw(SpriteBatch sb)
        {
            
        }

        protected override bool CheckCollision(List<Hitbox> objects)
        {
            return false;
        }
    }
}
