namespace Sokoban.Service.Models
{
    public record StarTresholdLookup(int One, int Two, int Three)
    {
        public int GetNumberOfAcquiredStars(int bestCompletionMoveCount)
        {
            if (bestCompletionMoveCount >= Three)
            {
                return 3;
            }

            if (bestCompletionMoveCount >= Two)
            {
                return 2;
            }

            if (bestCompletionMoveCount >= One)
            {
                return 1;
            }

            return 0;
        }
    };
}
