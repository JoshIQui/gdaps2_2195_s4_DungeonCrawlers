﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawlers
{
    class Item : GameObject
    {
        protected override bool CheckCollision(List<Hitbox> objects)
        {
            return false;
        }
    }
}
