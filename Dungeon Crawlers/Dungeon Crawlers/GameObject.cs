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
        protected Rectangle position;
        protected GraphicsDeviceManager graphics;

        // Properties

        // Constructor
        public GameObject(Texture2D asset, Rectangle position, GraphicsDeviceManager graphics)
        {

        }
        // Methods
        abstract protected bool CheckCollision(List<Hitbox> objects);

        // Method that draws the asset
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.White);
        }
        // Method that updates object
        public abstract void Update(GameTime gameTime);

    }
}
