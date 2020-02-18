using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawlers
{
    class Hero : GameObject, IHaveAI
    {
        protected override bool CheckCollision(List<Hitbox> objects)
        {
            return false;
        }
    }
}
