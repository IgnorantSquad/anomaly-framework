using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Anomaly.Utils
{
    public class Device
    {
        public enum Type : int
        {
            Keyboard = 1 << 0,
            Mouse = 1 << 1,
            Gamepad = 1 << 2
        }

        private const int defaultShift = 8;

        public const int KeyboardShift = (int)Type.Keyboard << defaultShift;
        public const int MouseShift = (int)Type.Mouse << defaultShift;
        public const int GamepadShift = (int)Type.Gamepad << defaultShift;

        private static int supportedDevices = (int)(Type.Keyboard | Type.Mouse);

        public static ButtonControl GetButton(int key)
        {
            if (IsKeyboard(key) && IsSupported(Type.Keyboard))
            {
                return Keyboard.current[(Key)key];
            }

            if (IsMouse(key) && IsSupported(Type.Mouse))
            {
                return GetMouseButton(key);
            }

            if (IsGamepad(key) && IsSupported(Type.Gamepad))
            {
                return null;
            }

            Debug.LogError($"Not supported: {key}");
            return null;

            ButtonControl GetMouseButton(int key)
            {
                if (key == (int)AKeyCode.MouseRight) return Mouse.current.rightButton;
                if (key == (int)AKeyCode.MouseMiddle) return Mouse.current.middleButton;

                return Mouse.current.leftButton;
            }
        }


        public static bool IsKeyboard(int key)
        {
            return (key >> defaultShift) == (int)Type.Keyboard;
        }

        public static bool IsMouse(int key)
        {
            return (key >> defaultShift) == (int)Type.Mouse;
        }

        public static bool IsGamepad(int key)
        {
            return (key >> defaultShift) == (int)Type.Gamepad;
        }

        public static bool IsSupported(Type device)
        {
            return (supportedDevices & (int)device) != 0;
        }
    }
}
