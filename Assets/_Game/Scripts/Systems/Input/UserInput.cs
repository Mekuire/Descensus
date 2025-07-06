using System;
using LightDI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Descensus
{
    [RequireComponent(typeof(PlayerInput))]
    public class UserInput : SystemBase
    {
        public float MoveInput => _moveAction.ReadValue<Vector2>().x;
        public bool DashWasPressed => _dashAction.WasPressedThisFrame();
        public bool DashPressed => _dashAction.IsPressed();

        public event Action PauseWasPressed;

        public event Action<ControlScheme> OnControlsChanged;

        private const string MOVE_ACTION_NAME = "Move";
        private const string DASH_ACTION_NAME = "Dash";
        private const string PAUSE_ACTION_NAME = "Pause";

        private const string GAMEPAD_SCHEME_NAME = "Gamepad";
        private const string KEYBOARD_SCHEME_NAME = "Keyboard&Mouse";

        private PlayerInput _playerInput;

        private InputAction _moveAction;
        private InputAction _dashAction;
        private InputAction _pauseAction;

        protected override void Start()
        {
            Init();
            base.Start();
        }

        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnsubscribeToActions();
            UnsubscribeFromDeviceChanged();
        }

        private void Init()
        {
            _playerInput = GetComponent<PlayerInput>();

            SetupActions();
            SubscribeToActions();
            SubscribeToDeviceChanged();
        }

        private void SetupActions()
        {
            _moveAction = _playerInput.actions[MOVE_ACTION_NAME];
            _dashAction = _playerInput.actions[DASH_ACTION_NAME];
            _pauseAction = _playerInput.actions[PAUSE_ACTION_NAME];
        }

        public void EnableInput()
        {
            if (!_playerInput) return;
            
            _playerInput.actions.Enable();
        }

        public void DisableInput()
        {
            if (!_playerInput) return;
   
            _playerInput.actions.Disable();
        }

        public void EnableAction(UserActions action)
        {
            GetInputAction(action)?.Enable();
        }

        public void DisableAction(UserActions action)
        {
            GetInputAction(action)?.Disable();
        }

        private InputAction GetInputAction(UserActions action)
        {
            return action switch
            {
                UserActions.Move => _moveAction,
                UserActions.Dash => _dashAction,
                UserActions.Pause => _pauseAction,
                _ => null
            };
        }

        private void SubscribeToActions()
        {
            _pauseAction.performed += PauseAction_performed;
        }

        private void UnsubscribeToActions()
        {
            _pauseAction.performed -= PauseAction_performed;
        }

        private void SubscribeToDeviceChanged()
        {
            _playerInput.onControlsChanged += OnInputDeviceChanged;
        }

        private void UnsubscribeFromDeviceChanged()
        {
            _playerInput.onControlsChanged -= OnInputDeviceChanged;
        }

        private void OnInputDeviceChanged(PlayerInput _)
        {
            switch (_playerInput.currentControlScheme)
            {
                case KEYBOARD_SCHEME_NAME:
                    OnControlsChanged?.Invoke(ControlScheme.KeyboardMouse);
                    Cursor.visible = true;
                    break;
                case GAMEPAD_SCHEME_NAME:
                    OnControlsChanged?.Invoke(ControlScheme.Gamepad);
                    Cursor.visible = false;
                    break;
            }
        }

        private void PauseAction_performed(InputAction.CallbackContext context) => PauseWasPressed?.Invoke();
    }
}


