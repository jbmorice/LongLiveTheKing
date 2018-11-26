using System;
using UnityEngine;

namespace LLtK
{
    public class InputHandler : MonoBehaviour
    {
        public enum InputMode
        {
            MouseKeyboard,
            Controller,
            Touch
        }

        public InputMode CurrentInputMode;

        public event Action<Vector3> MoveCameraEvent;
        public event Action<float> ZoomEvent;
        public event Action ClickEvent;

        public KeyCode MoveCameraUpKeyCode = KeyCode.UpArrow;
        public KeyCode MoveCameraDownKeyCode = KeyCode.DownArrow;
        public KeyCode MoveCameraLeftKeyCode = KeyCode.LeftArrow;
        public KeyCode MoveCameraRightKeyCode = KeyCode.RightArrow;

        public float ScreenEdgeSize = 25f;

        private void Start()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
            CurrentInputMode = InputMode.MouseKeyboard;
            #endif

            #if UNITY_XBOXONE || UNITY_PS4
            CurrentInputMode = InputMode.Controller;
            #endif

            #if UNITY_IPHONE || UNITY_ANDROID
            CurrentInputMode = InputMode.Touch;
            #endif
        }

        private void Update()
        {
            switch (CurrentInputMode)
            {
                case InputMode.MouseKeyboard:
                    HandleMouseKeyboardInput();
                    break;

                case InputMode.Controller:
                    //HandleControllerInput();
                    break;

                case InputMode.Touch:
                    //HandleTouchInput();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleMouseKeyboardInput()
        {
            if (MoveCameraEvent != null)
            {
                if (Input.GetKey(MoveCameraUpKeyCode)) MoveCameraEvent(Vector3.forward);
                if (Input.GetKey(MoveCameraDownKeyCode)) MoveCameraEvent(Vector3.back);
                if (Input.GetKey(MoveCameraLeftKeyCode)) MoveCameraEvent(Vector3.left);
                if (Input.GetKey(MoveCameraRightKeyCode)) MoveCameraEvent(Vector3.right);

                if (Input.mousePosition.y > Screen.height - ScreenEdgeSize) MoveCameraEvent(Vector3.forward);
                if (Input.mousePosition.y < ScreenEdgeSize) MoveCameraEvent(Vector3.back);
                if (Input.mousePosition.x < ScreenEdgeSize) MoveCameraEvent(Vector3.left);
                if (Input.mousePosition.x > Screen.width - ScreenEdgeSize) MoveCameraEvent(Vector3.right);
            }

            if (ZoomEvent != null)
            {
                if (Input.mouseScrollDelta.y < 0f || Input.mouseScrollDelta.y > 0f) ZoomEvent(Input.mouseScrollDelta.y);
            }
        }
    }
}