namespace Anomaly.Utils
{
    public class AInput
    {
        public static bool IsPressed(AKeyCode key)
        {
            return Device.GetButton((int)key).wasPressedThisFrame;
        }

        public static bool IsHeld(AKeyCode key)
        {
            return Device.GetButton((int)key).isPressed;
        }

        public static bool IsReleased(AKeyCode key)
        {
            return Device.GetButton((int)key).wasReleasedThisFrame;
        }
    }
}