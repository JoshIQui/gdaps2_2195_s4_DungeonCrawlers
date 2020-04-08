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
        private string duration;

        //IMPLEMENT ANIMATION FIELDS HERE

        // ---------------------
        // Properties
        // ---------------------

        public Rectangle Box
        {
            get { return this.box; }
            set { this.box = value; }
        }

        public int WorldPositionX
        {
            get { return box.X; }
            set { box.X = value; }
        }

        public int WorldPositionY
        {
            get { return box.Y; }
            set { box.Y = value; }
        }

        public int ScreenPositionX
        {
            get { return box.X - Camera.WorldPositionX; }
        }

        public BoxType BoxType
        {
            get { return this.boxType; }
        }

        // ---------------------
        // Constructor
        // ---------------------

        public Hitbox(Rectangle box, BoxType boxType, string duration = null)
        {
            this.box = box;
            this.boxType = boxType;
            this.duration = duration;
        }

        // ---------------------
        // Methods
        // ---------------------

        //Method is not fully implemented
        public void CountDuration()
        {
            if (duration != "0")
            {
                int frameDuration;

                if (int.TryParse(duration, out frameDuration))
                {
                    if (frameDuration!= 0)
                    {
                        frameDuration -= 1;
                    }
                }

                duration = "" + frameDuration;
            }
        }
    }
}
