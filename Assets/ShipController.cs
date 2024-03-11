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
   
    public float primaryThrust = 1000;
    public float secondaryThrust = 50;
    public float rotationTorque = 10;
    public float max_rotation_speed = 100;
    
    private Vector2 _screen;
    private Vector2 _mouse;

    private Vector3 ThrustVector = Vector3.zero;
    private Vector3 Angulartorque = Vector3.zero;
    
    public OrbitalBodyComponent OrbitalBody { get; set; }

    //TODO: force vectoring, adjustable throttle, moments and gyroscopic abilities, center of mass etc

    private void Awake()
    {
        OrbitalBody = GetComponent<OrbitalBodyComponent>();
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
        Angulartorque.x = _mouse.y * rotationTorque; 
        Angulartorque.y = -_mouse.x * rotationTorque; 
    }
    void FixedUpdate()
    {
        //Throttle up in .5 seconds
        throttle += 2 * throttleDir * Time.fixedDeltaTime;

        throttle = MathEx.Cap(throttle, 1, 0);

        if (throttle >= 0)
            ThrustVector.z = primaryThrust * throttle;
        else if (throttleDir < 0)
            ThrustVector.z = -secondaryThrust;
        else
            ThrustVector.z = 0;

        //Impulse forces are applied for only a single loop
        //Add the thrust vector force
        OrbitalBody.AddForce(ThrustVector, space: Space.Self, ForceMode.Impulse);
        OrbitalBody.AddMoment(Angulartorque, ForceMode.Impulse);
    }

    public void OnThrust(InputValue value)
    {
        var vector = value.Get<Vector3>();
        
        throttleDir = vector.z;
        ThrustVector.x = secondaryThrust * vector.x;
        ThrustVector.y = secondaryThrust * vector.y;
    }
    
    public void OnRoll(InputValue value)
    {
        Angulartorque.z = rotationTorque * value.Get<float>();
    }
}