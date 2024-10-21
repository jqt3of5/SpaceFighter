using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using Screen = UnityEngine.Device.Screen;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    public float throttle = 0;

    //are we throttling up, down, or neutral?
    private float throttleDir = 0;
   
    public float primaryThrust = .100f;
    public float secondaryThrust = .050f;
    public float rotationTorque = .1f;
    public float max_rotation_speed = 3;
    
    private Vector2 _screen;
    private Vector2 _mouse;

    public Rigidbody RigidBody { get; set; }
    public BodyState BodyState { get; set; }
    
    public Vector3 ThrustVector = Vector3.zero;
    public Vector3 AngularTorque = Vector3.zero;
    
    //TODO: force vectoring
    //TODO: Battery power and power consumption
    //TODO: total velocity delta

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
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
        AngularTorque.x = _mouse.y * rotationTorque; 
        AngularTorque.y = -_mouse.x * rotationTorque; 
    }
    
    void FixedUpdate()
    {
        //Throttle up in .5 seconds
        throttle += 2 * throttleDir * Time.fixedDeltaTime;
        
        throttle = Math.Clamp(throttle, 0, 1);
        
        if (throttle >= 0)
            ThrustVector.z = primaryThrust * throttle;
        else if (throttleDir < 0)
            ThrustVector.z = -secondaryThrust;
        else
            ThrustVector.z = 0;

        var worldThrus = transform.TransformDirection(ThrustVector);
        RigidBody.AddForce(worldThrus, ForceMode.Acceleration);

        RigidBody.AddTorque(AngularTorque.x * transform.right , ForceMode.Acceleration);
        RigidBody.AddTorque(AngularTorque.y * transform.up , ForceMode.Acceleration);
        RigidBody.AddTorque(AngularTorque.z * transform.forward, ForceMode.Acceleration);
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
        ThrustVector.x = secondaryThrust * vector.x;
        ThrustVector.y = secondaryThrust * vector.y;
    }
    
    public void OnRoll(InputValue value)
    {
        AngularTorque.z = rotationTorque * value.Get<float>();
    }

}