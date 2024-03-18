using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

[Flags]
public enum VectorComponent
{
    X = 1<<0,
    Y = 1<<1,
    Z = 1<<2,
    All = X | Y | Z
}
public class OrbitalBodyComponent: MonoBehaviour
{
    public Rigidbody rigidBody;
    
    /// <summary>
    /// The total gravity vector acting on the body, as calculated from the current system 
    /// </summary>
    public Vector3d Gravity = Vector3d.zero;

    public Vector3d Eccentricity = Vector3d.zero;

    public double TrueAnomaly = 0;

    private Vector3 _previousVelocity;
    public Vector3 Acceleration;
    private Vector3 _previousAngularVelocity;
    public Vector3 AngularAcceleration;

    [CanBeNull] private GameState PrimaryGravitationalBody = null;

    public void AddBodyToSystem(GameState body)
    {
        PrimaryGravitationalBody = body;
    }

    public Vector3d CalcGravity(GameState body2)
    {
        var gravityVector = new Vector3d(body2.transform.position) - transform.position;
        var gravityDirection = gravityVector.normalized;
        var distance = gravityVector.magnitude;
                
        var acc = body2.Mu / (distance * distance);

        return gravityDirection * acc;
    }

    private (Vector3d h_vec, Vector3d e_vec, double i, double Omega, double omega, double nu) OrbitalElements(GameState body1, Vector3d body2velocity, Vector3d body2position)
    {
        //Source: https://orbital-mechanics.space/classical-orbital-elements/orbital-elements-and-the-state-vector.html
        
        //Relative position vector
        var r_vec = body1.transform.position - body2position; 
        
        //It's supposed to be the matlab norm()
        var r = r_vec.magnitude;

        //Relative velocity vector
        var v_vec = body2velocity;// - body1.velocity;
        //It's supposed to be the matlab norm()
        var v = v_vec.magnitude;

        //radial velocity
        var v_r = Vector3d.Dot(r_vec / r, v_vec);

        //azimuthal velocity
        var v_p = Mathd.Sqrt(v * v - v_r * v_r);

        //Orbital angular momentum
        var h_vec = Vector3d.Cross(r_vec, v_vec);

        var h = h_vec.magnitude;

        //The inclination using the z component 
        var i = Mathd.Acos(h_vec.z / h);
        
        //Reference plane of the planet
        var k = new Vector3d(0, 0, 1);
        //node line
        var n_vec = Vector3d.Cross(k, h_vec);
        var n = n_vec.magnitude;
        
        //Right ascension of ascending node
        var Omega = 2 * Mathf.PI - Math.Acos(n_vec.y / n);

        //Eccentricity
        var e_vec = Vector3d.Cross(v_vec, h_vec) / body1.Mu - r_vec / r;
        
        var e = e_vec.magnitude;

        //Argument of periapsis
        var omega = 2 * Mathf.PI * -Mathd.Acos(Vector3d.Dot(n_vec, e_vec) / (n * e));

        //True anomaly
        var nu = Mathd.Acos(Vector3d.Dot(e_vec, r_vec) / (e * r));

        return (h_vec, e_vec, i, Omega, omega, nu);
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    
    void FixedUpdate()
    {
        Acceleration = rigidBody.velocity - _previousVelocity;
        _previousVelocity = rigidBody.velocity;
        
        AngularAcceleration = rigidBody.angularVelocity - _previousAngularVelocity;
        _previousAngularVelocity = rigidBody.angularVelocity;

        if (PrimaryGravitationalBody == null)
        {
            return;
        }
        
        var gravity = CalcGravity(PrimaryGravitationalBody);
        
        Gravity = gravity; 
        var worldGravity = transform.TransformDirection((Vector3)Gravity);
        rigidBody.AddForce(worldGravity, ForceMode.Acceleration);
        
        var (h_vec, e_vec,i, Omega, omega, nu) =  OrbitalElements(PrimaryGravitationalBody, new Vector3d(rigidBody.velocity), new Vector3d(transform.position));
        
        Eccentricity = e_vec;
        TrueAnomaly = nu;
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}