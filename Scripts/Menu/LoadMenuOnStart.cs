using UnityEngine;
namespace Fossil
{
    public class LoadMenuOnStart : MonoBehaviour
    {
        public MenuCategory menuCategory;

        Menu menu;
        void Start()
        {
            GlobalReferenceProvider.Fill(ref menu);
            menu.ShowMenu(menuCategory);
        }
    }
}