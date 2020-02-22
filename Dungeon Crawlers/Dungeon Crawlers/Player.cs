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
    class Player : GameObject
    {
        // Fields
        private int health;
        private int numEnemies;
        private KeyboardState kbState;
        private KeyboardState prevKbState;

        // Properties
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int NumEnemies
        {
            get { return numEnemies; }
            set { numEnemies = value; }
        }

        // Constructor
        public Player(Texture2D asset, Hitbox position, int screenWidth, int screenHeight, int health = 100, int numEnemies = 0)
            : base(asset, position)
        {
            this.asset = asset;
            this.position = position;
            this.health = health;
            this.numEnemies = numEnemies;
        }

        // Methods

        // Method for Player Updates in game
        public override void Update(GameTime gametime)
        {
            // Logic for Player Movement
            if(kbState.IsKeyDown(Keys.D)) // Right
            {
                position.BoxX += 2;
            }
            if(kbState.IsKeyDown(Keys.A)) // Left
            {
                position.BoxX -= 2;
            }
            if (kbState.IsKeyDown(Keys.W)) // Up
            {
                position.BoxY -= 2;
            }
            if (kbState.IsKeyDown(Keys.S)) // Crouch
            {
                
            }

            // Update previous Keyboard State
            prevKbState = kbState;
        }

        // Method for Drawing the Player
        public override void Draw(SpriteBatch sb)
        {
            
        }

        // Method for Collision Checks for player
        protected override bool CheckCollision(List<Hitbox> objects)
        {
            return false;
        }


    }
}
