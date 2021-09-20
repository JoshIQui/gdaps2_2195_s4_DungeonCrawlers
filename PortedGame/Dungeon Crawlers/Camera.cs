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
        private int levelWidth;

        // Propertiesd
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
        /// <summary>
        /// When the player moves from the center of the screen, move the camera along with the player to create a scrolling effect
        /// </summary>
        public void Update()
        {
            levelWidth = TileManager.mgrInstance.LevelWidth;
            center.X = playerPosition.WorldPositionX;
            if ((center.X - position.Width / 2) > 0 && (center.X + position.Width / 2) < levelWidth)
            {
                position.X = (int)(center.X - position.Width / 2);
            }
        }

    }
}
