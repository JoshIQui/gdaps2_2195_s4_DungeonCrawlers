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
    abstract class GameObject
    {
        // Fields
        protected bool isDamageable;
        protected Texture2D asset;
        protected Hitbox position;

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
        public GameObject(Texture2D asset, Hitbox position)
        {
            this.asset = asset;
            this.position = position;
        }
        // Methods
        abstract protected bool CheckCollision(List<Hitbox> objects);

        // Method that draws the asset
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position.Box, Color.White);
        }

        // Method that updates object
        public abstract void Update(GameTime gameTime);

    }
}
