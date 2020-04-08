using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Dungeon_Crawlers
{
    class Camera
    {
        // Fields
        private static Rectangle position;
        private Hitbox playerPosition;
        private Vector2 center;

        // Properties
        public static int WorldPositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }

        // Private Constructor
        public Camera(Hitbox playerPosition, int width, int height)
        {
            center = new Vector2(width / 2, height / 2);
            position = new Rectangle(0, 0, width, height);
            this.playerPosition = playerPosition;
        }

        // Methods
        public void Update()
        {
            center.X = playerPosition.WorldPositionX + playerPosition.Box.Width / 2;
            if ((center.X - position.Width / 2) > 0)
            {
                position.X = (int)(center.X - position.Width / 2);
            }
        }

    }
}
