using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawlers
{
    class Flag
    {
        // Declare field and property for flag hitbox
        private Hitbox position;

        public Hitbox Position
        {
            get { return position; }
        }

        // Constructor
        public Flag(Hitbox position)
        {
            this.position = position;
        }
    }
}
