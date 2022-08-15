using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Sokoban.Service;

namespace Sokoban.Monogame.Android
{
    internal class SoundPlayer : IRequiringLoadContent, IGameEventHandler
    {
        private SoundEffectLookup soundEffects;

        public void LoadContent(ContentManager contentManager)
        {
            soundEffects = new SoundEffectLookup(
                contentManager.LoadSoundEffect(nameof(SoundEffectLookup.Completion)),
                contentManager.LoadSoundEffect(nameof(SoundEffectLookup.Teleport)),
                contentManager.LoadSoundEffect(nameof(SoundEffectLookup.MovementBlockedByWall)),
                contentManager.LoadSoundEffect(nameof(SoundEffectLookup.TrashBagMovement)),
                contentManager.LoadSoundEffect(nameof(SoundEffectLookup.TrashEnteringTrashCan)));

            var music = contentManager.LoadSound<Song>("Music");
            MediaPlayer.Play(music);
        }

        public void OnCompletion()
        {
            soundEffects.Completion.Play();
        }

        public void OnMovementBlockedByWall()
        {
            soundEffects.MovementBlockedByWall.Play();
        }

        public void OnTeleport()
        {
            soundEffects.Teleport.Play();
        }

        public void OnTrashBagMovement()
        {
            soundEffects.TrashBagMovement.Play();
        }

        public void OnTrashEnteringTrashCan()
        {
            soundEffects.TrashEnteringTrashCan.Play();
        }

        public void OnTrashLeavingTrashCan()
        {
        }

        private record SoundEffectLookup(
            SoundEffect Completion,
            SoundEffect Teleport,
            SoundEffect MovementBlockedByWall,
            SoundEffect TrashBagMovement,
            SoundEffect TrashEnteringTrashCan);
    }

    internal static class ContentManagerSoundEffectExtensions
    {
        public static TSound LoadSound<TSound>(this ContentManager contentManager, string name)
        {
            return contentManager
                .Load<TSound>($"Sounds/{name}");
        }

        public static SoundEffect LoadSoundEffect(this ContentManager contentManager, string name)
        {
            return contentManager
                .LoadSound<SoundEffect>(name);
        }
    }
}
