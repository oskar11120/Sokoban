using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Sokoban.Service;

namespace Sokoban.Monogame.Android
{
    internal class SoundPlayer : IRequiringLoadContent, IGameEventHandler, IDisposable
    {
        private SoundEffectLookup? soundEffects = null;
        private SoundEffectLookup SoundEffects => soundEffects ?? throw new InvalidOperationException();

        public void LoadContent(ContentManager contentManager)
        {
            using var music = contentManager.Load<SoundEffect>(nameof(SoundEffectLookup.Music));
            soundEffects = new SoundEffectLookup(
                contentManager.Load<SoundEffect>(nameof(SoundEffectLookup.Completion)),
                contentManager.Load<SoundEffect>(nameof(SoundEffectLookup.Teleport)),
                contentManager.Load<SoundEffect>(nameof(SoundEffectLookup.MovementBlockedByWall)),
                contentManager.Load<SoundEffect>(nameof(SoundEffectLookup.TrashBagMovement)),
                contentManager.Load<SoundEffect>(nameof(SoundEffectLookup.TrashEnteringTrashCan)),
                music.CreateInstance());

            soundEffects.Music.IsLooped = true;
            soundEffects.Music.Play();
        }

        public void OnCompletion()
        {
            SoundEffects.Completion.Play();
        }

        public void OnMovementBlockedByWall()
        {
            SoundEffects.MovementBlockedByWall.Play();
        }

        public void OnTeleport()
        {
            SoundEffects.Teleport.Play();
        }

        public void OnTrashBagMovement()
        {
            SoundEffects.TrashBagMovement.Play();
        }

        public void OnTrashEnteringTrashCan()
        {
            SoundEffects.TrashEnteringTrashCan.Play();
        }

        public void OnTrashLeavingTrashCan()
        {
        }

        public void Dispose()
        {
            var (a, b, c, d, e, f) = SoundEffects;
            var all = new IDisposable[] { a, b, c, d, e, f };
            foreach (var effect in all)
            {
                effect.Dispose();
            }
        }

        private record SoundEffectLookup(
            SoundEffect Completion,
            SoundEffect Teleport,
            SoundEffect MovementBlockedByWall,
            SoundEffect TrashBagMovement,
            SoundEffect TrashEnteringTrashCan,
            SoundEffectInstance Music);
    }
}
