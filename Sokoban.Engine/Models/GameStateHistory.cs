namespace Sokoban.Engine.Models
{
    internal class GameStateHistory
    {
        private readonly Stack<GameState> all;
        private readonly Stack<GameState> undone = new();

        public GameStateHistory(GameState currentState)
        {
            Current = currentState;
            all = new(new[] { currentState });
        }

        public GameState Current { get; private set; }

        public void Add(GameState @new)
        {
            all.Push(@new);
            Current = @new;
            undone.Clear();
        }

        public bool CanUndo()
        {
            return all.Count > 1;
        }

        public void Undo()
        {
            if (!CanUndo())
            {
                throw new InvalidOperationException("The game is in its initial state");
            }

            var newUndone = all.Pop();
            undone.Push(newUndone);
            Current = newUndone;
        }

        public bool CanRedo()
        {
            return undone.Count > 0;
        }

        public void Redo()
        {
            if (!CanRedo())
            {
                throw new InvalidOperationException("Nothing to redo");
            }

            var newRedone = undone.Pop();
            all.Push(newRedone);
            Current = newRedone;
        }

        public void Clear()
        {
            undone.Clear();
            Current = all.First();
            all.Clear();
            all.Push(Current);
        }

        public int Count => all.Count;
    }
}
