using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    public Vector3 velocity = new Vector3(0,0,0);

    //TODO: Matrix!
    public Vector3 primaryAcceleration = Vector3.zero;
    public Vector3 upAcceleration = Vector3.zero;
    public Vector3 strafeAcceleration = Vector3.zero;

    public Vector3 rotation = Vector3.zero; 
    public float weight = 10;

    public float primaryThrust = 100;
    public float secondaryThrust = 100;
    
    //TODO: force vectoring, adjustable throttle, moments and gyroscopic abilities, center of mass etc
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity += (primaryAcceleration + upAcceleration + strafeAcceleration) * Time.deltaTime;
        transform.Translate(velocity*Time.deltaTime, Space.World);
        
        transform.Rotate(rotation * Time.deltaTime);
    }

    public void OnForward(InputValue value)
    {
        var thrust = primaryThrust;
        if (value.Get<float>() < 0)
        {
            thrust = secondaryThrust;
        }
        
        primaryAcceleration = transform.forward * thrust / weight * value.Get<float>();
    }
    public void OnStrafe(InputValue value)
    {
        strafeAcceleration = transform.forward * secondaryThrust/ weight * value.Get<float>(); 
    }

    public void OnUp(InputValue value)
    {
        upAcceleration = transform.forward * secondaryThrust/ weight * value.Get<float>(); 
    }
    
    public void OnRoll(InputValue value)
    {
      //Modifier keys for moving camera only? 
      rotation.x = value.Get<float>()*10;
      Debug.Log(rotation.z);
    }
    
    public void OnPitch(InputValue value)
    {
        Debug.Log(value);
      rotation.x = value.Get<float>()*10;
      Debug.Log(rotation.z);
    }
    
    public void OnPan(InputValue value)
    {
      rotation.y = value.Get<float>()*10;
      Debug.Log(rotation.z);
    }
    
   
}
