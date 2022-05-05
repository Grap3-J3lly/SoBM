//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Misc/InputControl.inputactions
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

public partial class @InputControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControl"",
    ""maps"": [
        {
            ""name"": ""CharacterControls"",
            ""id"": ""2b8f93ab-d4c8-4760-be96-a6300980e2fc"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""4408e005-faa4-4d70-81a7-f7b2e52246be"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""7eb40396-098e-4ef1-a4c1-013752905093"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Place"",
                    ""type"": ""Button"",
                    ""id"": ""506d2286-408e-4d77-bfb7-6a05760bb46e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""74c31c73-853a-4414-8fb9-54e5a03faef8"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""7f4732aa-80b0-463a-86f5-40f88ea1596b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d76bff3a-bf44-40fd-afeb-c695fba206fc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a561ca95-2e8e-40ec-a974-89f7adc9e3d1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""799b550b-0b57-443a-9d4b-b2175cb0f796"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d3217eb1-ae5e-4c38-b566-ed00c08dc40d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e04857ca-760f-4bef-ac12-1c6a0b6d759c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86f03892-a1f2-410f-b058-634556b6108b"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Place"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CameraControls"",
            ""id"": ""44a0e657-524b-4be5-8885-7ce21ff3ea3d"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1298891f-a7f0-477d-8ecc-d2efd124c7d6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""5a812378-a53d-4e37-b365-bad1ee063d36"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""0d1553dc-f658-42e2-b332-762c452243a6"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""653ccf1f-dcc0-4b46-91a4-ac5b195489d0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b49fd802-7272-4e9f-8774-8a381a55d7ce"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f5f5d8f4-b25f-4e8b-b39e-53039f467fa1"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""TouchControls"",
            ""id"": ""8fae96cc-5829-4984-a6b0-a83383a56c35"",
            ""actions"": [
                {
                    ""name"": ""PrimaryContact"",
                    ""type"": ""Button"",
                    ""id"": ""5867a9a3-c435-432c-804d-c42c5acb05d7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PrimaryPosition"",
                    ""type"": ""Value"",
                    ""id"": ""60c18b41-c30a-4017-8a72-21d7d5f9a6a5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SecondaryContact"",
                    ""type"": ""Button"",
                    ""id"": ""eb66e249-a80d-462a-807f-f652846ba354"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondaryPosition"",
                    ""type"": ""Value"",
                    ""id"": ""07f7be9b-3312-4830-ad60-825b8e6c45e0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""01ac9637-bfaf-4505-af53-a38997e52146"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""PrimaryContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48a96d02-e478-472d-bbe8-cec1973d5646"",
                    ""path"": ""<Touchscreen>/primaryTouch/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""PrimaryPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77e4d838-7cc9-4393-9a3d-7736e95e987c"",
                    ""path"": ""<Touchscreen>/touch1/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""SecondaryContact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51f27128-20a0-459e-b968-106c66c98648"",
                    ""path"": ""<Touchscreen>/touch1/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PrimaryControlScheme"",
                    ""action"": ""SecondaryPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PrimaryControlScheme"",
            ""bindingGroup"": ""PrimaryControlScheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // CharacterControls
        m_CharacterControls = asset.FindActionMap("CharacterControls", throwIfNotFound: true);
        m_CharacterControls_Move = m_CharacterControls.FindAction("Move", throwIfNotFound: true);
        m_CharacterControls_Interact = m_CharacterControls.FindAction("Interact", throwIfNotFound: true);
        m_CharacterControls_Place = m_CharacterControls.FindAction("Place", throwIfNotFound: true);
        // CameraControls
        m_CameraControls = asset.FindActionMap("CameraControls", throwIfNotFound: true);
        m_CameraControls_Rotate = m_CameraControls.FindAction("Rotate", throwIfNotFound: true);
        m_CameraControls_Zoom = m_CameraControls.FindAction("Zoom", throwIfNotFound: true);
        // TouchControls
        m_TouchControls = asset.FindActionMap("TouchControls", throwIfNotFound: true);
        m_TouchControls_PrimaryContact = m_TouchControls.FindAction("PrimaryContact", throwIfNotFound: true);
        m_TouchControls_PrimaryPosition = m_TouchControls.FindAction("PrimaryPosition", throwIfNotFound: true);
        m_TouchControls_SecondaryContact = m_TouchControls.FindAction("SecondaryContact", throwIfNotFound: true);
        m_TouchControls_SecondaryPosition = m_TouchControls.FindAction("SecondaryPosition", throwIfNotFound: true);
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

    // CharacterControls
    private readonly InputActionMap m_CharacterControls;
    private ICharacterControlsActions m_CharacterControlsActionsCallbackInterface;
    private readonly InputAction m_CharacterControls_Move;
    private readonly InputAction m_CharacterControls_Interact;
    private readonly InputAction m_CharacterControls_Place;
    public struct CharacterControlsActions
    {
        private @InputControl m_Wrapper;
        public CharacterControlsActions(@InputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_CharacterControls_Move;
        public InputAction @Interact => m_Wrapper.m_CharacterControls_Interact;
        public InputAction @Place => m_Wrapper.m_CharacterControls_Place;
        public InputActionMap Get() { return m_Wrapper.m_CharacterControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterControlsActions instance)
        {
            if (m_Wrapper.m_CharacterControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnMove;
                @Interact.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnInteract;
                @Place.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnPlace;
                @Place.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnPlace;
                @Place.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnPlace;
            }
            m_Wrapper.m_CharacterControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Place.started += instance.OnPlace;
                @Place.performed += instance.OnPlace;
                @Place.canceled += instance.OnPlace;
            }
        }
    }
    public CharacterControlsActions @CharacterControls => new CharacterControlsActions(this);

    // CameraControls
    private readonly InputActionMap m_CameraControls;
    private ICameraControlsActions m_CameraControlsActionsCallbackInterface;
    private readonly InputAction m_CameraControls_Rotate;
    private readonly InputAction m_CameraControls_Zoom;
    public struct CameraControlsActions
    {
        private @InputControl m_Wrapper;
        public CameraControlsActions(@InputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_CameraControls_Rotate;
        public InputAction @Zoom => m_Wrapper.m_CameraControls_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlsActions instance)
        {
            if (m_Wrapper.m_CameraControlsActionsCallbackInterface != null)
            {
                @Rotate.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnRotate;
                @Zoom.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnZoom;
            }
            m_Wrapper.m_CameraControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
            }
        }
    }
    public CameraControlsActions @CameraControls => new CameraControlsActions(this);

    // TouchControls
    private readonly InputActionMap m_TouchControls;
    private ITouchControlsActions m_TouchControlsActionsCallbackInterface;
    private readonly InputAction m_TouchControls_PrimaryContact;
    private readonly InputAction m_TouchControls_PrimaryPosition;
    private readonly InputAction m_TouchControls_SecondaryContact;
    private readonly InputAction m_TouchControls_SecondaryPosition;
    public struct TouchControlsActions
    {
        private @InputControl m_Wrapper;
        public TouchControlsActions(@InputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @PrimaryContact => m_Wrapper.m_TouchControls_PrimaryContact;
        public InputAction @PrimaryPosition => m_Wrapper.m_TouchControls_PrimaryPosition;
        public InputAction @SecondaryContact => m_Wrapper.m_TouchControls_SecondaryContact;
        public InputAction @SecondaryPosition => m_Wrapper.m_TouchControls_SecondaryPosition;
        public InputActionMap Get() { return m_Wrapper.m_TouchControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchControlsActions set) { return set.Get(); }
        public void SetCallbacks(ITouchControlsActions instance)
        {
            if (m_Wrapper.m_TouchControlsActionsCallbackInterface != null)
            {
                @PrimaryContact.started -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnPrimaryContact;
                @PrimaryContact.performed -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnPrimaryContact;
                @PrimaryContact.canceled -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnPrimaryContact;
                @PrimaryPosition.started -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnPrimaryPosition;
                @PrimaryPosition.performed -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnPrimaryPosition;
                @PrimaryPosition.canceled -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnPrimaryPosition;
                @SecondaryContact.started -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnSecondaryContact;
                @SecondaryContact.performed -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnSecondaryContact;
                @SecondaryContact.canceled -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnSecondaryContact;
                @SecondaryPosition.started -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnSecondaryPosition;
                @SecondaryPosition.performed -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnSecondaryPosition;
                @SecondaryPosition.canceled -= m_Wrapper.m_TouchControlsActionsCallbackInterface.OnSecondaryPosition;
            }
            m_Wrapper.m_TouchControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PrimaryContact.started += instance.OnPrimaryContact;
                @PrimaryContact.performed += instance.OnPrimaryContact;
                @PrimaryContact.canceled += instance.OnPrimaryContact;
                @PrimaryPosition.started += instance.OnPrimaryPosition;
                @PrimaryPosition.performed += instance.OnPrimaryPosition;
                @PrimaryPosition.canceled += instance.OnPrimaryPosition;
                @SecondaryContact.started += instance.OnSecondaryContact;
                @SecondaryContact.performed += instance.OnSecondaryContact;
                @SecondaryContact.canceled += instance.OnSecondaryContact;
                @SecondaryPosition.started += instance.OnSecondaryPosition;
                @SecondaryPosition.performed += instance.OnSecondaryPosition;
                @SecondaryPosition.canceled += instance.OnSecondaryPosition;
            }
        }
    }
    public TouchControlsActions @TouchControls => new TouchControlsActions(this);
    private int m_PrimaryControlSchemeSchemeIndex = -1;
    public InputControlScheme PrimaryControlSchemeScheme
    {
        get
        {
            if (m_PrimaryControlSchemeSchemeIndex == -1) m_PrimaryControlSchemeSchemeIndex = asset.FindControlSchemeIndex("PrimaryControlScheme");
            return asset.controlSchemes[m_PrimaryControlSchemeSchemeIndex];
        }
    }
    public interface ICharacterControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnPlace(InputAction.CallbackContext context);
    }
    public interface ICameraControlsActions
    {
        void OnRotate(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
    public interface ITouchControlsActions
    {
        void OnPrimaryContact(InputAction.CallbackContext context);
        void OnPrimaryPosition(InputAction.CallbackContext context);
        void OnSecondaryContact(InputAction.CallbackContext context);
        void OnSecondaryPosition(InputAction.CallbackContext context);
    }
}
