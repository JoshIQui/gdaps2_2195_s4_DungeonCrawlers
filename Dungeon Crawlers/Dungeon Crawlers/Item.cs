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
    class Item : GameObject
    {
        private int width;
        private int height;

        public Item(Texture2D asset, Hitbox position, int screenWidth, int screenHeight)
            : base(asset, position)
        {

            this.width = screenWidth;
            this.height = screenHeight;
        }
        public override void Update(GameTime gametime)
        {

        }
        // Method for Drawing the items
        public override void Draw(SpriteBatch sb)
        {

        }
        public bool Intersect (Rectangle target)
        {
            if (position.Box.Intersects(target))
            {
                return true;
            }
            return false;
        }
        protected override bool CheckCollision(List<Hitbox> objects)
        {
            return false;
        }
    }
}
