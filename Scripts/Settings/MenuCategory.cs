using NaughtyAttributes;
using UnityEngine;
namespace Fossil
{
    [CreateAssetMenu(fileName = "MenuCategory", menuName = "Fossil/Menu/Category")]
    public class MenuCategory : MenuItem
    {
        [ReorderableList]
        public MenuItem[] items;
    }
}