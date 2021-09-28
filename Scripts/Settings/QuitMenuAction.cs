using UnityEngine;
namespace Fossil
{
    [CreateAssetMenu(fileName = "QuitMenuAction", menuName = "Fossil/Menu/Actions/Quit")]
    public class QuitMenuAction : SettingsAction
    {
        public override void Run(Menu menu)
        {
            Application.Quit();
        }
    }
}