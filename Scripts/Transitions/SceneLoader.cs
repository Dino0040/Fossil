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
        public bool transitionOnFirstLoad;
        Dictionary<string, Transition> transitionDictionary;
        bool transitioning;
        Scene currentLoadedScene;

        public UnityEvent beforeSceneSwitch;
        public UnityEvent afterSceneSwitch;

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

            if (defaultTransition && transitionOnFirstLoad)
            {
                defaultTransition.EndTransition();
            }

            currentLoadedScene = SceneManager.GetActiveScene();
        }

        public void LoadScene(string name, string transitionName)
        {
            if (!transitionDictionary.TryGetValue(transitionName, out Transition transition))
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
            LoadScene(currentLoadedScene.name);
        }

        public void ReloadScene(string transitionName)
        {
            LoadScene(currentLoadedScene.name, transitionName);
        }

        public void ReloadScene(Transition transition)
        {
            LoadScene(currentLoadedScene.name, transition);
        }

        IEnumerator LoadSceneRoutine(string name, Transition transition)
        {
            transitioning = true;

            beforeSceneSwitch.Invoke();

            Application.backgroundLoadingPriority = ThreadPriority.Low;
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
            loadOperation.allowSceneActivation = false;

            if (transition != null)
            {
                yield return transition.StartTransition();
            }

            Application.backgroundLoadingPriority = ThreadPriority.High;
            loadOperation.allowSceneActivation = true;
            yield return loadOperation;
            Application.backgroundLoadingPriority = ThreadPriority.Normal;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
            afterSceneSwitch.Invoke();

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