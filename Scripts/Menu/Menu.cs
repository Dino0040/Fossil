using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Fossil
{
    public class Menu : MonoBehaviour
    {
        public GameObject buttonPreset;
        public GameObject sliderPreset;
        public GameObject togglePreset;

        public EventSystem eventSystem;

        List<System.Action> menuChain;
        List<IMenuCanvasItem> currentOpenCanvasMenuItems;

        MenuCategory currentMenuRoot;

        private void Awake()
        {
            GlobalReferenceProvider.Register(typeof(Menu), this);
            currentOpenCanvasMenuItems = new List<IMenuCanvasItem>();
            menuChain = new List<System.Action>();
        }

        public MenuCategory GetCurrentMenuRoot()
        {
            return currentMenuRoot;
        }

        void CloseCanvasMenuItems()
        {
            foreach (IMenuCanvasItem item in currentOpenCanvasMenuItems)
            {
                item.Close();
            }
        }

        void GoBack()
        {
            menuChain.RemoveAt(menuChain.Count - 1);
            System.Action action = menuChain[menuChain.Count - 1];
            menuChain.RemoveAt(menuChain.Count - 1);
            action();
        }

        public void ShowMenuIfNoneShown(MenuCategory menuCategory)
        {
            if (GetCurrentMenuRoot() == null)
            {
                ShowMenu(menuCategory);
            }
        }

        void ShowMenu(MenuCategory menuCategory, bool clearMenuChain)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CloseCanvasMenuItems();
            if (clearMenuChain)
            {
                menuChain.Clear();
            }
            menuChain.Add(() => ShowMenu(menuCategory));

            currentOpenCanvasMenuItems.Clear();
            MenuButton button;

            if (menuChain.Count > 1)
            {
                button = Instantiate(buttonPreset, transform).GetComponent<MenuButton>();
                button.Init("<=", GoBack);
                currentOpenCanvasMenuItems.Add(button);

                if (eventSystem != null)
                {
                    eventSystem.SetSelectedGameObject(button.gameObject);
                }
            }
            else
            {
                currentMenuRoot = menuCategory;
            }

            foreach (MenuItem settingsItem in menuCategory.items)
            {
                if (settingsItem.hideForPlatform.Contains(Application.platform))
                {
                    continue;
                }
                switch (settingsItem)
                {
                    case MenuCategory newSettingsCategory:
                        button = Instantiate(buttonPreset, transform).GetComponent<MenuButton>();
                        button.Init(newSettingsCategory.displayName, () => ShowMenu(newSettingsCategory, false));
                        currentOpenCanvasMenuItems.Add(button);
                        break;
                    case SettingsAction newSettingsAction:
                        button = Instantiate(buttonPreset, transform).GetComponent<MenuButton>();
                        button.Init(newSettingsAction.displayName, () => newSettingsAction.Run(this));
                        currentOpenCanvasMenuItems.Add(button);
                        break;
                    case FloatSettingsValue floatSettingsValue:
                        MenuSlider slider = Instantiate(sliderPreset, transform).GetComponent<MenuSlider>();
                        slider.Init(floatSettingsValue.displayName, x => { floatSettingsValue.SetValue(x); }, floatSettingsValue.GetValue(), floatSettingsValue.min, floatSettingsValue.max, floatSettingsValue.stepsize);
                        currentOpenCanvasMenuItems.Add(slider);
                        break;
                    case ExclusiveSettingsValue exclusiveSettingsValue:
                        button = Instantiate(buttonPreset, transform).GetComponent<MenuButton>();
                        button.Init(exclusiveSettingsValue.displayName + ": " + exclusiveSettingsValue.GetValue().displayName, () => ShowExclusiveOptions(exclusiveSettingsValue));
                        currentOpenCanvasMenuItems.Add(button);
                        break;
                    case BoolSettingsValue boolSettingsValue:
                        MenuButton toggleButton = Instantiate(togglePreset, transform).GetComponent<MenuButton>();
                        toggleButton.SetToggleStatus(boolSettingsValue.GetValue());
                        toggleButton.Init(boolSettingsValue.displayName, () => { boolSettingsValue.SetValue(!boolSettingsValue.GetValue()); toggleButton.SetToggleStatus(boolSettingsValue.GetValue()); });
                        currentOpenCanvasMenuItems.Add(toggleButton);
                        break;
                    default:
                        Debug.LogError("Menu does not know how to handle type of " + settingsItem + "!");
                        break;
                }
            }
        }

        public void ShowMenu(MenuCategory menuCategory)
        {
            ShowMenu(menuCategory, true);
        }

        void ShowExclusiveOptions(ExclusiveSettingsValue exclusiveSettingsValue)
        {

            CloseCanvasMenuItems();
            menuChain.Add(() => ShowExclusiveOptions(exclusiveSettingsValue));

            currentOpenCanvasMenuItems = new List<IMenuCanvasItem>();
            MenuButton button;

            button = Instantiate(buttonPreset, transform).GetComponent<MenuButton>();
            button.Init("<=", GoBack);
            currentOpenCanvasMenuItems.Add(button);
            if (eventSystem != null)
            {
                eventSystem.SetSelectedGameObject(button.gameObject);
            }

            foreach (ExclusiveOption option in exclusiveSettingsValue.GetPossibleValues())
            {
                button = Instantiate(buttonPreset, transform).GetComponent<MenuButton>();
                button.Init(option.displayName, () => { exclusiveSettingsValue.SetValue(option); GoBack(); });
                currentOpenCanvasMenuItems.Add(button);
            }

        }

        public void CloseMenu()
        {
            currentMenuRoot = null;
            CloseCanvasMenuItems();
            currentOpenCanvasMenuItems.Clear();
            menuChain.Clear();
        }

        public void CloseMenu(MenuCategory menuCategory)
        {
            if (menuCategory == GetCurrentMenuRoot())
            {
                CloseMenu();
            }
        }
    }
}