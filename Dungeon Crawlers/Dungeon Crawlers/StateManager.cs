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
        Pause,
        Help,
        GameOver,
        Win
    }
    class StateManager
    {
        // ---------------------
        // Fields
        // ---------------------

        private GameState currentState;
        private static StateManager mgrInstance;

        // ---------------------
        // Properties
        // ---------------------

        // Add a public static "Instance" property
        public static StateManager Instance
        {
            get
            {
                // Returns a reference to the single
                // static instance (creating it if needed)
                if (mgrInstance == null)
                {
                    mgrInstance = new StateManager();
                }
                return mgrInstance;
            }
        }
        public GameState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        // ---------------------
        // Constructor
        // ---------------------
        private StateManager()
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

                case GameState.Pause:
                    currentState = GameState.Pause;
                    break;

                case GameState.Help:
                    currentState = GameState.Help;
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
