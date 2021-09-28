using UnityEngine;
namespace Fossil
{
    public class PauseTrigger : MonoBehaviour
    {
        public MenuCategory pauseMenuCategory;
        public string pauseButtonName = "Pause";

        IPauseManager pauseManager;
        Menu menu;

        private void Start()
        {
            GlobalReferenceProvider.Fill(ref pauseManager);
            GlobalReferenceProvider.Fill(ref menu);
        }

        private void Update()
        {
            if (Input.GetButtonDown(pauseButtonName))
            {
                if (pauseManager.PauseState == PauseState.running && menu.GetCurrentMenuRoot() == null)
                {
                    pauseManager.Pause();
                    menu.ShowMenu(pauseMenuCategory);
                }
                if (pauseManager.PauseState == PauseState.paused && menu.GetCurrentMenuRoot() == pauseMenuCategory)
                {
                    pauseManager.Unpause();
                    menu.CloseMenu();
                }
            }
        }
    }
}