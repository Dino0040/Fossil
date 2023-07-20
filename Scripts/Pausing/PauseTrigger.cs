using UnityEngine;
using UnityEngine.InputSystem;

namespace Fossil
{
    public class PauseTrigger : MonoBehaviour
    {
        public MenuCategory pauseMenuCategory;
        public InputActionReference inputAction;

        IPauseManager pauseManager;
        Menu menu;

        private void Start()
        {
            GlobalReferenceProvider.Fill(ref pauseManager);
            GlobalReferenceProvider.Fill(ref menu);

            inputAction.action.started += TogglePause;
            inputAction.action.Enable();
        }

        private void OnDestroy()
        {
            inputAction.action.started -= TogglePause;
            inputAction.action.Disable();
        }

        private void TogglePause(InputAction.CallbackContext _)
        {
            if (pauseManager.PauseState == PauseState.running && menu.GetCurrentMenuRoot() == null)
            {
                pauseManager.Pause();
                menu.ShowMenu(pauseMenuCategory);
            }
            else if (pauseManager.PauseState == PauseState.paused && menu.GetCurrentMenuRoot() == pauseMenuCategory)
            {
                pauseManager.Unpause();
                menu.CloseMenu();
            }
        }
    }
}