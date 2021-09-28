using UnityEngine;
using UnityEngine.InputSystem;
namespace Fossil
{
    public class Screenshot : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.f12Key.wasPressedThisFrame)
            {
                ScreenCapture.CaptureScreenshot("Screenshot.png", 1);
            }
        }
    }
}