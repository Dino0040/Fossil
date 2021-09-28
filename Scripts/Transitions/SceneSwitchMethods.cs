using UnityEngine;
namespace Fossil
{
    public class SceneSwitchMethods : MonoBehaviour
    {
        SceneLoader sceneLoader;

        void Start()
        {
            GlobalReferenceProvider.Fill(ref sceneLoader);
        }

        public void LoadScene(string name, string transitionName)
        {
            sceneLoader.LoadScene(name, transitionName);
        }

        public void LoadScene(string name, Transition transition)
        {
            sceneLoader.LoadScene(name, transition);
        }

        public void LoadScene(string name)
        {
            sceneLoader.LoadScene(name);
        }

        public void ReloadScene()
        {
            sceneLoader.ReloadScene();
        }

        public void ReloadScene(string transitionName)
        {
            sceneLoader.ReloadScene(transitionName);
        }

        public void ReloadScene(Transition transition)
        {
            sceneLoader.ReloadScene(transition);
        }
    }
}