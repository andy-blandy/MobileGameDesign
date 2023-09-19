//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""82e7da29-c6b9-45e7-8d2c-486d524445db"",
            ""actions"": [
                {
                    ""name"": ""Swipe"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c2f7cd5e-f0f7-4db9-850f-fc7a3673e8e8"",
                    ""expectedControlType"": ""Touch"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Analog"",
                    ""type"": ""Value"",
                    ""id"": ""1c88e617-b8e9-4404-8950-790dbcf19d8f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bba59103-acec-47b7-ada7-f92a34dc3f6a"",
                    ""path"": ""<Touchscreen>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Swiping"",
                    ""action"": ""Swipe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abbbec01-f502-4ac7-9bfe-07cd0ec38f36"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Analog"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Swiping"",
            ""bindingGroup"": ""Swiping"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Swipe = m_Gameplay.FindAction("Swipe", throwIfNotFound: true);
        m_Gameplay_Analog = m_Gameplay.FindAction("Analog", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Swipe;
    private readonly InputAction m_Gameplay_Analog;
    public struct GameplayActions
    {
        private @InputActions m_Wrapper;
        public GameplayActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Swipe => m_Wrapper.m_Gameplay_Swipe;
        public InputAction @Analog => m_Wrapper.m_Gameplay_Analog;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Swipe.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwipe;
                @Swipe.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwipe;
                @Swipe.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwipe;
                @Analog.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAnalog;
                @Analog.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAnalog;
                @Analog.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAnalog;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Swipe.started += instance.OnSwipe;
                @Swipe.performed += instance.OnSwipe;
                @Swipe.canceled += instance.OnSwipe;
                @Analog.started += instance.OnAnalog;
                @Analog.performed += instance.OnAnalog;
                @Analog.canceled += instance.OnAnalog;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_SwipingSchemeIndex = -1;
    public InputControlScheme SwipingScheme
    {
        get
        {
            if (m_SwipingSchemeIndex == -1) m_SwipingSchemeIndex = asset.FindControlSchemeIndex("Swiping");
            return asset.controlSchemes[m_SwipingSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnSwipe(InputAction.CallbackContext context);
        void OnAnalog(InputAction.CallbackContext context);
    }
}
