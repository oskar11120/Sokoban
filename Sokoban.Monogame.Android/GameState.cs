using Sokoban.Engine;

namespace Sokoban.Monogame.Android
{
    internal class GameState
    {
        private Game currentlySelectedGame = null;
        private bool isInLevelSelectionScreen = false;
        private bool isInMenuScreen = true;

        public void ReplaceSelectedGame(Game newGame)
        {
            isInLevelSelectionScreen = false;
            currentlySelectedGame = newGame;
        }

        public void OpenLevelSelectionScreen()
        {
            isInMenuScreen = false;
            isInLevelSelectionScreen = true;
        }

        public void OpenMenuScreen()
        {
            isInLevelSelectionScreen = false;
            isInMenuScreen = true;
        }

        public void OpenCurrentGameScreen()
        {
            if (currentlySelectedGame is null)
            {
                throw new InvalidOperationException($"{nameof(currentlySelectedGame)} was null");
            }

            isInLevelSelectionScreen = false;
            isInMenuScreen = false;
        }

        public void Switch(Action ifInMenuScreen, Action ifInLevelSelectionScreen, Action<Game> ifInCurrentGameScreen)
        {
            if (isInMenuScreen)
            {
                ifInMenuScreen();
            }
            else if (isInLevelSelectionScreen)
            {
                ifInLevelSelectionScreen();
            }
            else if (currentlySelectedGame is not null)
            {
                ifInCurrentGameScreen(currentlySelectedGame);
            }
            else
            {
                throw new InvalidOperationException("Invalid game state");
            }
        }
    }
}
