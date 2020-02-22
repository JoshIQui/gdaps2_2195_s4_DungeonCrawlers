using Microsoft.Xna.Framework;

namespace Dungeon_Crawlers
{
    enum BoxType
    {
        Hitbox,
        Hurtbox,
        Collision
    }

    class Hitbox
    {
        // ---------------------
        // Fields
        // ---------------------

        private Rectangle box;
        private BoxType boxType;
        private bool hasDuration;

        //IMPLEMENT ANIMATION FIELDS HERE

        // ---------------------
        // Properties
        // ---------------------

        public Rectangle Box
        {
            get { return this.box; }
            set { this.box = value; }
        }

        public int BoxX
        {
            get { return box.X; }
            set { box.X = value; }
        }

        public int BoxY
        {
            get { return box.Y; }
            set { box.Y = value; }
        }

        public BoxType BoxType
        {
            get { return this.boxType; }
        }

        // ---------------------
        // Constructor
        // ---------------------

        public Hitbox(Rectangle box, BoxType boxType)
        {
            this.box = box;
            this.boxType = boxType;
        }
    }
}
