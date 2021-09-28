using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace Fossil
{
    public class SceneLoader : MonoBehaviour
    {
        [System.Serializable]
        public struct NamedTransition
        {
            public string name;
            public Transition transition;
        }

        public Transition defaultTransition;
        public NamedTransition[] transitions;
        Dictionary<string, Transition> transitionDictionary;
        bool transitioning;
        public string defaultSceneName;
        Scene? currentLoadedScene = null;

        public UnityEvent beforeSceneUnload;
        public UnityEvent afterSceneLoad;

        private void Awake()
        {
            GlobalReferenceProvider.Register(typeof(SceneLoader), this);
        }

        void Start()
        {
            transitionDictionary = new Dictionary<string, Transition>();
            foreach (NamedTransition namedTransition in transitions)
            {
                transitionDictionary.Add(namedTransition.name, namedTransition.transition);
            }

            if (SceneManager.sceneCount == 1)
            {
                LoadScene(defaultSceneName);
            }
            else
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    if (SceneManager.GetSceneAt(i) != gameObject.scene)
                        currentLoadedScene = SceneManager.GetSceneAt(i);
                }
                SceneManager.SetActiveScene((Scene)currentLoadedScene);
            }
        }

        public void LoadScene(string name, string transitionName)
        {
            Transition transition;
            if (!transitionDictionary.TryGetValue(transitionName, out transition))
            {
                transition = defaultTransition;
                Debug.LogWarning("Transition " + transitionName + " does not exist!");
            }
            LoadScene(name, transition);
        }

        public void LoadScene(string name, Transition transition)
        {
            if (!transitioning)
            {
                StartCoroutine(LoadSceneRoutine(name, transition));
            }
        }

        public void LoadScene(string name)
        {
            LoadScene(name, defaultTransition);
        }

        public void ReloadScene()
        {
            LoadScene(currentLoadedScene.Value.name);
        }

        public void ReloadScene(string transitionName)
        {
            LoadScene(currentLoadedScene.Value.name, transitionName);
        }

        public void ReloadScene(Transition transition)
        {
            LoadScene(currentLoadedScene.Value.name, transition);
        }

        IEnumerator LoadSceneRoutine(string name, Transition transition)
        {
            transitioning = true;

            if (currentLoadedScene != null)
            {
                if (transition != null)
                {
                    yield return transition.StartTransition();
                }
            }

            if (currentLoadedScene != null)
            {
                beforeSceneUnload.Invoke();
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync((Scene)currentLoadedScene);
                yield return unloadOperation;
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            yield return loadOperation;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
            afterSceneLoad.Invoke();

            if (currentLoadedScene != null)
            {
                if (transition != null)
                {
                    yield return transition.EndTransition();
                }
            }

            currentLoadedScene = SceneManager.GetSceneByName(name);

            transitioning = false;
        }
    }
}