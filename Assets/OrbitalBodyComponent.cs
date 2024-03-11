using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    private Vector3 up = Vector3.up;
    private Vector3 down = Vector3.down;
    private Vector3 left = Vector3.left;
    private Vector3 right = Vector3.right;
    private Vector3 forward = Vector3.forward;
    private Vector3 back = Vector3.back;

    public const float G = 6.67430E-11f;

    private Vector3 _previousVelocity = Vector3.zero; 
    public Vector3 ActualAcceleration = Vector3.zero;
    /// <summary>
    /// The current acceleration as determined by the forces acting on the body
    /// </summary>
    public Vector3 Acceleration = Vector3.zero;
    
    /// <summary>
    /// The total gravity vector acting on the body, as calculated from the current system 
    /// </summary>
    public Vector3 Gravity = Vector3.zero;
    
    
    private Vector3 _previousAngularVelociy = Vector3.zero; 
    public Vector3 ActualAngularAcceleration = Vector3.zero;
    /// <summary>
    /// Radians per second per second
    /// </summary>
    public Vector3 AngularAcceleration = Vector3.zero;
    
    /// <summary>
    /// Radians per second 
    /// </summary>
    public Vector3 AngularVelocity = Vector3.zero;
    
    /// <summary>
    /// In world units
    /// </summary>
    public Vector3 Velocity = Vector3.zero;
    
    /// <summary>
    /// Relative to the center of the object
    /// </summary>
    public Vector3 CenterOfMass = Vector3.zero;
    
    /// <summary>
    /// The mass of the object in Kg. 
    /// </summary>
    public float Mass = 0;
    
    /// <summary>
    /// Standard gravitation parameter
    /// Units are km^2 * km/s^2
    ///
    /// Mars is 6776km in diameter, so the surface is ~ 3000 km from the center. At the surface of an equivalent planet
    /// I want the acceleration to be 50 m/s2.
    /// Ex: Earth is 3.986E5
    /// </summary>
    public int Mu = 50 * 3000 * 3000;

    public Vector3 Eccentricity = Vector3.zero;

    public float TrueAnomaly = 0;

    public Rigidbody RigidBody; 

    /// <summary>
    /// </summary>
    public IEnumerable<Vector3> LagrangePoint = new List<Vector3>();
    
    private HashSet<OrbitalBodyComponent> BodiesInSystem = new();

    //TODO: Can this be optimized?
    public Vector3 SystemCenterOfMass => (
                                             transform.position +
                                             CenterOfMass +
                                             BodiesInSystem.Aggregate(Vector3.zero,
                                                 (a, b) => a + b.transform.position + b.CenterOfMass))
                                         / (Mass + BodiesInSystem.Aggregate(0f, (a,b) => a + b.Mass));
    
    public void AddBodyToSystem(OrbitalBodyComponent body)
    {
        BodiesInSystem.Add(body);
    }

    public void RemoveBodyFromSystem(OrbitalBodyComponent body)
    {
        BodiesInSystem.Remove(body);
    }

    public void ClearForces()
    {
        Acceleration.x = 0;
        Acceleration.y = 0;
        Acceleration.z = 0;
        AngularAcceleration.x = 0;
        AngularAcceleration.y = 0;
        AngularAcceleration.z = 0;
    }
    public Vector3 CalcGravity(OrbitalBodyComponent body2)
    {
        var gravityVector = body2.transform.position - transform.position;
        var gravityDirection = gravityVector.normalized;
        var distance = gravityVector.magnitude;
                
        var acc = body2.Mu / (distance * distance);

        return gravityDirection * acc;
    }

    private (Vector3 h_vec, Vector3 e_vec, float i, float Omega, float omega, float nu) OrbitalElements(OrbitalBodyComponent body1, Vector3 body2velocity, Vector3 body2position)
    {
        //Source: https://orbital-mechanics.space/classical-orbital-elements/orbital-elements-and-the-state-vector.html
        
        //Relative position vector
        var r_vec = body1.transform.position - body2position; 
        
        //It's supposed to be the matlab norm()
        var r = r_vec.magnitude;

        //Relative velocity vector
        var v_vec = body2velocity - body1.Velocity;
        //It's supposed to be the matlab norm()
        var v = v_vec.magnitude;

        //radial velocity
        var v_r = Vector3.Dot(r_vec / r, v_vec);

        //azimuthal velocity
        var v_p = Mathf.Sqrt(v * v - v_r * v_r);

        //Orbital angular momentum
        var h_vec = Vector3.Cross(r_vec, v_vec);

        var h = h_vec.magnitude;

        //The inclination using the z component 
        var i = Mathf.Acos(h_vec.z / h);
        
        //Reference plane of the planet
        var k = new Vector3(0, 0, 1);
        //node line
        var n_vec = Vector3.Cross(k, h_vec);
        var n = n_vec.magnitude;
        
        //Right ascension of ascending node
        var Omega = 2 * Mathf.PI - Mathf.Acos(n_vec.y / n);

        //Eccentricity
        var e_vec = Vector3.Cross(v_vec, h_vec) / body1.Mu - r_vec / r;
        
        var e = e_vec.magnitude;

        //Argument of periapsis
        var omega = 2 * Mathf.PI * -Mathf.Acos(Vector3.Dot(n_vec, e_vec) / (n * e));

        //True anomaly
        var nu = Mathf.Acos(Vector3.Dot(e_vec, r_vec) / (e * r));

        return (h_vec, e_vec, i, Omega, omega, nu);
    }

    // public void AddCouple(Vector3 couple, Vector3 atVector, Space space = Space.World)
    // {
    //     if (space == Space.World)
    //     {
    //         //Convert to local
    //         couple = transform.InverseTransformVector(couple);
    //     }
    //     
    //     atVector = atVector - CenterOfMass;
    //     //Calculate the moment - in local coordinates
    //     //timess 2 because it's a coupled force
    //     var moment = 2*Vector3.Cross(couple, atVector);
    //     AddMoment(moment);
    // }
    //
    // public void AddCouple(Vector3 couple, float distance, Space space = Space.World)
    // {
    //     if (space == Space.World)
    //     {
    //         //Convert to local
    //         couple = transform.InverseTransformVector(couple);
    //     }
    //     
    //     //Calculate the moment - in local coordinates
    //     //timess 2 because it's a coupled force
    //     var moment = 2*Vector3.Cross(couple, right*distance);
    //     AddMoment(moment);
    // }

    public void AddMoment(Vector3 moment, ForceMode mode = ForceMode.Impulse)
    {
        switch (mode)
        {
            case ForceMode.Acceleration:
                AngularAcceleration += moment;
                break;
            case ForceMode.Force:
                AngularAcceleration += moment / Mass;
                break;
            case ForceMode.Impulse:
                AngularVelocity += moment / Mass * Time.fixedDeltaTime;
                break;
            case ForceMode.VelocityChange:
                AngularVelocity += moment;
                break; 
        }
    }
    
    public void AddForce(Vector3 force, Space space = Space.World, ForceMode mode = ForceMode.Impulse)
    {
        if (space == Space.Self)
        {
            force = transform.TransformVector(force);
        }
        
        switch (mode)
        {
            case ForceMode.Acceleration:
                Acceleration += force;
                break;
            case ForceMode.Force:
                force = force / Mass;
                Acceleration += force;
                break;
            case ForceMode.Impulse:
                force = force / Mass;
                Velocity += force*Time.fixedDeltaTime;
                break;
            case ForceMode.VelocityChange:
                Velocity += force;
                break;
        }
    }
    
    public void AddForce(Vector3 force, Vector3 atVector, Space space = Space.World, ForceMode mode = ForceMode.Impulse)
    {
        if (space == Space.World)
        {
            //Convert to local
            force = transform.InverseTransformVector(force);
        }
        
        //Center of mass is the center of rotation. atVector is always local
        atVector = atVector - CenterOfMass;

        //TODO: I doubt this works :/
        //Find the component of the force that would cause the object to translate
        var at_norm = atVector.normalized;
        var vector = Vector3.Dot(force, at_norm) * at_norm;
        
        //Transform it back
        vector = transform.TransformVector(vector);
        
        switch (mode)
        {
            case ForceMode.Acceleration:
                Acceleration += vector;
                break;
            case ForceMode.Force:
                vector = vector / Mass;
                Acceleration += vector;
                
                //Calculate the moment - in local coordinates
                AngularAcceleration += Vector3.Cross(force, atVector) / Mass;
                
                break;
            case ForceMode.Impulse:
                vector = vector / Mass;
                Velocity += vector*Time.fixedDeltaTime;
                
                //Calculate the moment - in local coordinates
                AngularVelocity += Vector3.Cross(force, atVector) / Mass * Time.fixedDeltaTime;
                break;
            case ForceMode.VelocityChange:
                Velocity += vector;
                break;
        }
    }

    //TODO: Instead of having to redo the calculations each time, we can just read from the provider
    // public void AddForceProvider(IForceProvider provider)
    // {
        
    // }
    
    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Calculate the typical mass of the planet if it is unset
        if (Mass == 0)
        {
            Mass = Mu / G * 1000;
        }
        else if (Mu == 0)
        {
            Mu = (int)(G / 1000 * Mass);
        }
        
    }
    
    void FixedUpdate()
    {
        Gravity = Vector3.zero;
        foreach (var body in BodiesInSystem)
        {
            Gravity += CalcGravity(body);
        }

        Velocity += (Gravity + Acceleration) * Time.fixedDeltaTime;
        transform.Translate(Velocity*Time.fixedDeltaTime, Space.World);

        ActualAcceleration = (Velocity - _previousVelocity)/Time.fixedDeltaTime;
        _previousVelocity = Velocity;
        
        AngularVelocity += AngularAcceleration * Time.fixedDeltaTime;
        transform.Rotate(AngularVelocity * Time.fixedDeltaTime);
        
        ActualAngularAcceleration = (AngularVelocity - _previousAngularVelociy)/Time.fixedDeltaTime;
        _previousAngularVelociy = AngularVelocity;

        if (!BodiesInSystem.Any())
        {
            return;
        }
        
        var (h_vec, e_vec,i, Omega, omega, nu) =  OrbitalElements(BodiesInSystem.First(), Velocity, transform.position);

        Eccentricity = e_vec;
        TrueAnomaly = nu;
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}