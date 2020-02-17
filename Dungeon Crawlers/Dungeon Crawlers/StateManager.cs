using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawlers
{
    //The different parts of the game's state
    enum GameState
    {
        Title,
        Instructions,
        Game,
        GameOver,
        Win
    }
    class StateManager
    {
        // ---------------------
        // Fields
        // ---------------------

        private GameState currentState;

        // ---------------------
        // Properties
        // ---------------------

        public GameState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        // ---------------------
        // Constructor
        // ---------------------
        public StateManager()
        {
            this.currentState = GameState.Title;
        }

        // ---------------------
        // Methods
        // ---------------------

        //Changes the state of the game to the parameter
        public void ChangeState(GameState newState)
        {
            switch (newState)
            {
                case GameState.Title:
                    currentState = GameState.Title;
                    break;

                case GameState.Instructions:
                    currentState = GameState.Instructions;
                    break;

                case GameState.Game:
                    currentState = GameState.Game;
                    break;

                case GameState.GameOver:
                    currentState = GameState.GameOver;
                    break;

                case GameState.Win:
                    currentState = GameState.Win;
                    break;
            }
        }

        //ADD LOGIC HERE
        public void DecideState()
        {
            GameState decidedState = GameState.Title;

            this.ChangeState(decidedState);
        }
    }
}
 