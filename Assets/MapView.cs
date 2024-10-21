//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/MapView.inputactions
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

public partial class @MapView: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MapView()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MapView"",
    ""maps"": [
        {
            ""name"": ""ShipManeuver"",
            ""id"": ""a4b93bf0-1749-47ae-9803-b068b6630733"",
            ""actions"": [
                {
                    ""name"": ""Thrust"",
                    ""type"": ""Value"",
                    ""id"": ""d1fa7bcf-0b4a-43b6-bc79-c8f48a386234"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Value"",
                    ""id"": ""529e92e1-200a-47e3-b0cf-61b0eda3ac82"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""3D Vector"",
                    ""id"": ""a92f1cd1-140b-4d68-80eb-f7fe83a79f78"",
                    ""path"": ""3DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thrust"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""82175dfd-b1ec-4a9c-805c-927170cf4a28"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0c58efe0-63f2-4a2d-b964-3b962fce8bd2"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f6d5ee18-55e0-40cf-986a-bf1b6a582d4c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0cdced53-f78e-45be-a700-73adafdb917d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""forward"",
                    ""id"": ""268f2179-197d-420e-b45c-db8ba33e17d1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""backward"",
                    ""id"": ""c72137dd-a7d8-4462-a27e-466f1458f24d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Thrust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""90f91c25-be16-4a84-b4d7-86dd0c179ba4"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0cb51862-6822-412f-bcb6-fa884668f045"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8833d9c2-abbd-4404-b070-0732f7d996f5"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Default"",
            ""bindingGroup"": ""Default"",
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
                }
            ]
        }
    ]
}");
        // ShipManeuver
        m_ShipManeuver = asset.FindActionMap("ShipManeuver", throwIfNotFound: true);
        m_ShipManeuver_Thrust = m_ShipManeuver.FindAction("Thrust", throwIfNotFound: true);
        m_ShipManeuver_Roll = m_ShipManeuver.FindAction("Roll", throwIfNotFound: true);
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

    // ShipManeuver
    private readonly InputActionMap m_ShipManeuver;
    private List<IShipManeuverActions> m_ShipManeuverActionsCallbackInterfaces = new List<IShipManeuverActions>();
    private readonly InputAction m_ShipManeuver_Thrust;
    private readonly InputAction m_ShipManeuver_Roll;
    public struct ShipManeuverActions
    {
        private @MapView m_Wrapper;
        public ShipManeuverActions(@MapView wrapper) { m_Wrapper = wrapper; }
        public InputAction @Thrust => m_Wrapper.m_ShipManeuver_Thrust;
        public InputAction @Roll => m_Wrapper.m_ShipManeuver_Roll;
        public InputActionMap Get() { return m_Wrapper.m_ShipManeuver; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShipManeuverActions set) { return set.Get(); }
        public void AddCallbacks(IShipManeuverActions instance)
        {
            if (instance == null || m_Wrapper.m_ShipManeuverActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ShipManeuverActionsCallbackInterfaces.Add(instance);
            @Thrust.started += instance.OnThrust;
            @Thrust.performed += instance.OnThrust;
            @Thrust.canceled += instance.OnThrust;
            @Roll.started += instance.OnRoll;
            @Roll.performed += instance.OnRoll;
            @Roll.canceled += instance.OnRoll;
        }

        private void UnregisterCallbacks(IShipManeuverActions instance)
        {
            @Thrust.started -= instance.OnThrust;
            @Thrust.performed -= instance.OnThrust;
            @Thrust.canceled -= instance.OnThrust;
            @Roll.started -= instance.OnRoll;
            @Roll.performed -= instance.OnRoll;
            @Roll.canceled -= instance.OnRoll;
        }

        public void RemoveCallbacks(IShipManeuverActions instance)
        {
            if (m_Wrapper.m_ShipManeuverActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IShipManeuverActions instance)
        {
            foreach (var item in m_Wrapper.m_ShipManeuverActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ShipManeuverActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ShipManeuverActions @ShipManeuver => new ShipManeuverActions(this);
    private int m_DefaultSchemeIndex = -1;
    public InputControlScheme DefaultScheme
    {
        get
        {
            if (m_DefaultSchemeIndex == -1) m_DefaultSchemeIndex = asset.FindControlSchemeIndex("Default");
            return asset.controlSchemes[m_DefaultSchemeIndex];
        }
    }
    public interface IShipManeuverActions
    {
        void OnThrust(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
    }
}
