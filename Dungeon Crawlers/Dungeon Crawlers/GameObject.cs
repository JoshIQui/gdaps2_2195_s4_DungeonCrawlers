using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawlers
{
    abstract class GameObject
    {
        // Fields
        protected bool isDamageable;

        // Properties

        // Constructor

        // Methods
        abstract protected bool CheckCollision(List<Hitbox> objects);

    }
}
