using UnityEngine;
using UnityEngine.EventSystems;
namespace Fossil
{
    public class EventSystemDefault : MonoBehaviour
    {
        void Start()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}