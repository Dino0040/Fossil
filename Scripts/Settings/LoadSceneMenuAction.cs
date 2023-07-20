using NaughtyAttributes;
using UnityEngine;
namespace Fossil
{
    [CreateAssetMenu(fileName = "LoadSceneMenuAction", menuName = "Fossil/Menu/Actions/Load Scene")]
    public class LoadSceneMenuAction : SettingsAction
    {
        [Scene] public string sceneName;
        public string transitionName;

        SceneLoader sceneLoader;

        public override void Run(Menu menu)
        {
            GlobalReferenceProvider.Get(out sceneLoader);

            if(GlobalReferenceProvider.TryGet(out IPauseManager pauseManager))
            {
                pauseManager.Unpause();
            }

            if (string.IsNullOrEmpty(transitionName))
            {
                menu.CloseMenu();
                sceneLoader.LoadScene(sceneName);
            }
            else
            {
                menu.CloseMenu();
                sceneLoader.LoadScene(sceneName, transitionName);
            }
        }
    }
}