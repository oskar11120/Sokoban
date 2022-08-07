namespace Sokoban.Service.Models
{
    public record LevelInfo(string LevelId, StarTresholdLookup StartTresholds, int? BestCompletionMoveCount);
}
