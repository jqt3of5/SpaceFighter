using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Screen = UnityEngine.Device.Screen;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ShipController : MonoBehaviour
{
    public float throttle = 0;

    //are we throttling up, down, or neutral?
    private float throttleDir = 0;
   
    public float primaryThrust = 100;
    public float secondaryThrust = 50;
    public float rotationTorque = .1f;
    public float max_rotation_speed = 100;
    
    private Vector2 _screen;
    private Vector2 _mouse;

    private Vector3 _thrustVector = Vector3.zero;
    public Vector3 ThrustVector
    {
        get => _thrustVector;
        set => _thrustVector = value;
    }
    
    private Vector3 _angularTorque = Vector3.zero;
    
    public Vector3 Angulartorque
    {
        get => _angularTorque;
        set => _angularTorque = value;
    }
    
    public OrbitalBodyComponent OrbitalBody { get; set; }
    public Rigidbody Rigidbody { get; set; }

    //TODO: force vectoring, adjustable throttle, moments and gyroscopic abilities, center of mass etc

    private void Awake()
    {
        OrbitalBody = GetComponent<OrbitalBodyComponent>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _screen = new Vector2(Screen.width, Screen.height) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        //In percentage of screen
        _mouse = (_screen - Mouse.current.position.ReadValue())/_screen;
        
        //Scale the acceleration ad velocity based on mouse position
        _angularTorque.x = _mouse.y * rotationTorque; 
        _angularTorque.y = -_mouse.x * rotationTorque; 
    }
    void FixedUpdate()
    {
        //Throttle up in .5 seconds
        throttle += 2 * throttleDir * Time.fixedDeltaTime;
        
        throttle = MathEx.Cap(throttle, 1, 0);
        
        if (throttle >= 0)
            _thrustVector.z = primaryThrust * throttle;
        else if (throttleDir < 0)
            _thrustVector.z = -secondaryThrust;
        else
            _thrustVector.z = 0;

        var worldThrus = transform.TransformDirection(ThrustVector);
        Rigidbody.AddForce(worldThrus, ForceMode.Acceleration);

        Rigidbody.AddTorque(Angulartorque.x * transform.right , ForceMode.Acceleration);
        Rigidbody.AddTorque(Angulartorque.y * transform.up , ForceMode.Acceleration);
        Rigidbody.AddTorque(Angulartorque.z * transform.forward, ForceMode.Acceleration);
        
        // Debug.Log(Angulartorque);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Output the Collider's GameObject's name
        Debug.Log(collision.collider.name);
    }
    
    public void OnThrust(InputValue value)
    {
        var vector = value.Get<Vector3>();
        
        throttleDir = vector.z;
        _thrustVector.x = secondaryThrust * vector.x;
        _thrustVector.y = secondaryThrust * vector.y;
    }
    
    public void OnRoll(InputValue value)
    {
        _angularTorque.z = rotationTorque * value.Get<float>();
    }
}