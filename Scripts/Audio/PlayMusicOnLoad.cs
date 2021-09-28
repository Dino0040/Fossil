using UnityEngine;
namespace Fossil
{
    public class PlayMusicOnLoad : MonoBehaviour
    {
        public string musicName;

        private void Start()
        {
            MusicManager musicManager = null;
            GlobalReferenceProvider.Fill(ref musicManager);
            musicManager.FadeThenPlay(musicName);
        }
    }
}