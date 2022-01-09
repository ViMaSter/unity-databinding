// GENERATED AUTOMATICALLY FROM 'Assets/Samples/Unity Data Binding/1.0.0/Debugging/DebuggerPlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DebuggerPlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DebuggerPlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DebuggerPlayerInput"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""dbf9290a-44b6-47bc-bb9d-f5f646a7b9c8"",
            ""actions"": [
                {
                    ""name"": ""Toggle Debug View"",
                    ""type"": ""Button"",
                    ""id"": ""061c8109-8cfe-4d7f-8fda-a8c06b437452"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""51f52894-b193-48b7-81fe-1f0e8d31de6c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Button With Two Modifiers"",
                    ""id"": ""3ab23e83-cd49-4783-b730-4e029b82d9fd"",
                    ""path"": ""ButtonWithTwoModifiers"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Debug View"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier1"",
                    ""id"": ""f88b8611-e14b-454e-9869-fcbfa0715fcf"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Debug View"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""modifier2"",
                    ""id"": ""6db059cc-8d38-4085-b132-5e9885c533c8"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Debug View"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""22360ae4-12da-4ba0-b144-592e54594527"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Debug View"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""83cbec8c-f168-4afa-b526-aacbec62a61e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3e327056-7569-4562-9898-6fc75982c389"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""14afa6b5-b899-4129-a662-bf2b001c6d3b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a1237e4c-02fc-4af8-8441-03aae62d0ebe"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1ba78bfb-4259-4eed-8a73-cb4c66badcb0"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_ToggleDebugView = m_Default.FindAction("Toggle Debug View", throwIfNotFound: true);
        m_Default_Movement = m_Default.FindAction("Movement", throwIfNotFound: true);
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

    // Default
    private readonly InputActionMap m_Default;
    private IDefaultActions m_DefaultActionsCallbackInterface;
    private readonly InputAction m_Default_ToggleDebugView;
    private readonly InputAction m_Default_Movement;
    public struct DefaultActions
    {
        private @DebuggerPlayerInput m_Wrapper;
        public DefaultActions(@DebuggerPlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleDebugView => m_Wrapper.m_Default_ToggleDebugView;
        public InputAction @Movement => m_Wrapper.m_Default_Movement;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
            {
                @ToggleDebugView.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnToggleDebugView;
                @ToggleDebugView.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnToggleDebugView;
                @ToggleDebugView.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnToggleDebugView;
                @Movement.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_DefaultActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ToggleDebugView.started += instance.OnToggleDebugView;
                @ToggleDebugView.performed += instance.OnToggleDebugView;
                @ToggleDebugView.canceled += instance.OnToggleDebugView;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    public interface IDefaultActions
    {
        void OnToggleDebugView(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
    }
}
