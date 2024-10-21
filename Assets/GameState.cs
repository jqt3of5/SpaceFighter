using System.Collections.Generic;
using UnityEngine;

public class OrbitalMath
{
    public const double G = 6.67430E-11;
    /// <summary>
    /// Mu is the standard gravitational parameter
    /// </summary>
    /// <param name="mass"> mass in Kg</param>
    /// <returns>Mu</returns>
    public static double CalcMu(double mass) => G * mass;
    /// <summary>
    /// Returns the mass of the object from the standard gravitational parameter
    /// </summary>
    /// <param name="mu"></param>
    /// <returns></returns>
    public static double CalcMass(double mu) => mu / G;
    
    /// <summary>
    /// Calculate the orbital period of an object given the semimajor axis and the mass of the object it's orbiting
    /// </summary>
    /// <param name="a"></param>
    /// <param name="mass"></param>
    /// <returns></returns>
    public static double CalcOrbitalPeriod(double a, double mass)
    {
        return 2 * Mathd.PI * Mathd.Sqrt(a * a * a / (G * mass));
    }

    public static Vector3d BaryCenter(params (Vector3d position, double mass) [] bodies)
    {
        var mass = 0.0;
        var center = Vector3d.zero;

        foreach (var (p, m) in bodies)
        {
            center += m * p;
            mass += m;
        }

        return center / mass;
    }
    /// <summary>
    /// Calculate the gravity vector between two objects relative to the reference object 
    /// </summary>
    /// <param name="referencePosition"></param>
    /// <param name="referenceMu"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector3d CalcGravityVector(Vector3d referencePosition, double referenceMu, Vector3d position)
    {
        var gravityVector = referencePosition - position;
        var gravityDirection = gravityVector.normalized;
        var distance = gravityVector.magnitude;
        
        var acc = referenceMu / (distance * distance);

        return gravityDirection * acc;
    }
    
    public static double SemiMinorAxis(double a, double e) => a * Mathd.Sqrt(1 - e * e);
    public static double Periapsis(double a, double e) => a * (1 - e);
    public static double Apopsis(double a, double e) => a * (1 + e);
    
    /// <summary>
    /// Calcualates the distance from the foci of an ellipse given the semimajor axis, eccentricity and angle from the periapsis 
    /// </summary>
    /// <param name="a">SemimajorAxis</param>
    /// <param name="e">Eccentricity</param>
    /// <param name="theta">Angle in radians from the periapsis</param>
    /// <returns></returns>
    public static double DistanceFromFoci(double a, double e, double theta) => a * (1 - e*e)/ (1 + e * Mathd.Cos(theta)); 
  
    public static Vector3 PointAtTheta(Vector3d e, Vector3d h, double a, double theta)
    {
        var r = DistanceFromFoci(a, e.magnitude, theta);
        var r_vec = e.normalized * r;
        
        var deg = Mathd.Rad2Deg * theta;
        
        Quaternion pointQuaternion = Quaternion.AngleAxis((float)deg, (Vector3)h);
        
        return pointQuaternion * (Vector3)r_vec;
    }
        
    /// <summary>
    /// Calculates the various orbital elements given the planet, ship velocity and ship position
    /// Source: https://orbital-mechanics.space/classical-orbital-elements/orbital-elements-and-the-state-vector.html
    /// </summary>
    /// <param name="referenceVelocity">The velocity of the reference body, the one we're orbiting. In world space</param>
    /// <param name="referencePosition">The position of the reference body, the one we're orbiting. In world space</param>
    /// <param name="referencePlane">The plane of reference, usually the plane of the orbit of the reference body. But may also be the equatorial plane of the reference body</param>
    /// <param name="referenceMu">The standard gravitational parameter of the reference body. equal to G * Mass</param>
    /// <param name="bodyVelocity">The velocity of the body who's elements we are calculating. In world space</param>
    /// <param name="bodyPosition">The position of the body who's elements we are calculating. In world space</param>
    /// <returns>
    /// h_vec - orbital angular momentum
    /// e_vec - the eccentricity vector, pointing to the periapsis
    /// i - inclination angle
    /// Omega - Right ascention of ascending node
    /// omega - argument of periapsis
    /// nu - true anomaly
    /// a - the semimajor axis of the ellipse
    /// </returns>
    public static (Vector3d h_vec, Vector3d e_vec, double i, double Omega, double omega, double nu, double a) 
        CalcOrbitalElements(
            Vector3d referenceVelocity, Vector3d referencePosition, Vector3d referencePlane, double referenceMu, 
            Vector3d bodyVelocity, Vector3d bodyPosition)
    {
    
        //Relative position vector
        var r_vec = bodyPosition - referencePosition; 
    
        //It's supposed to be the matlab norm()
        var r = r_vec.magnitude;

        //Relative velocity vector
        var v_vec = bodyVelocity - referenceVelocity;
        //It's supposed to be the matlab norm()
        var v = v_vec.magnitude;

        //semimajor axis via the vis viva equation
        var a = -1 / (v * v / referenceMu - 2/r);

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
        var k = referencePlane;
        //node line
        var n_vec = Vector3d.Cross(k, h_vec);
        var n = n_vec.magnitude;
    
        //Right ascension of ascending node
        var Omega = 2 * Mathd.PI - Mathd.Acos(n_vec.y / n);

        //Eccentricity
        var e_vec = Vector3d.Cross(v_vec, h_vec) / referenceMu - r_vec / r;
    
        var e = e_vec.magnitude;

        //Argument of periapsis
        var omega = 2 * Mathd.PI * -Mathd.Acos(Vector3d.Dot(n_vec, e_vec) / (n * e));

        //True anomaly
        var nu = Mathd.Acos(Vector3d.Dot(e_vec, r_vec) / (e * r));

        return (h_vec, e_vec, i, Omega, omega, nu, a);
    }
 
}
public class BodyState 
{
    //Metadata Properties
    //====================================================
    public string Name;
    public Color Color = Color.magenta;
    //====================================================
    
    //Physical Properties
    //====================================================
    public double Diameter = 1;

    public Vector3 EulerAngularAcceleration = Vector3.zero;
    public Vector3 EulerAngularVelocity = Vector3.zero;
    public Vector3 EulerRotation = Vector3.zero;
   
    public Vector3d Position = Vector3d.zero;
    public Vector3d Velocity = Vector3d.zero;
    public Vector3d Gravity = Vector3d.zero;
    public Vector3d Acceleration = Vector3d.zero;
        
    /// <summary>
    /// Standard gravitation parameter
    /// </summary>
    public double Mu;
    
    public double Mass
    {
        get => OrbitalMath.CalcMass(Mu);
        set => Mu = OrbitalMath.CalcMu(value);
    } 
    //====================================================
        
    //Orbital properties
    //====================================================
    public Vector3d OrbitalPlane = Vector3d.up;
    public Vector3d EccentricityVector;
        
    public double Eccentricity;
    
    public double SemimajorAxis;
    public double SemiminorAxis;

    public double Periapsis;
    public double Apoppsis;

    public double OrbitalPeriod;
    public double TrueAnomaly;

    public BodyState Orbiting;
    //====================================================

    public void UpdateGravity(Vector3d gravity)
    {
        Gravity = gravity;
    }
    public void UpdatePosition(float deltaTime)
    {
        Velocity += (Acceleration + Gravity) * deltaTime;
        Position += Velocity * deltaTime;
    
        EulerAngularVelocity += EulerAngularAcceleration * deltaTime;
        EulerRotation += EulerAngularVelocity * deltaTime;     
    }
    
    public void UpdateElements(BodyState referenceBody)
    {
        var elements = OrbitalMath.CalcOrbitalElements(
            referenceBody.Velocity, referenceBody.Position, referenceBody.OrbitalPlane, referenceBody.Mu,
            Velocity, Position);
        
        // Stored values
        // ==============================
        EccentricityVector = elements.e_vec;
        SemimajorAxis = elements.a;
        TrueAnomaly = elements.nu;
        OrbitalPlane = elements.h_vec.normalized;
        Eccentricity = elements.e_vec.magnitude;
        // ==============================
        
        // Calculated values TODO: make them lazy?
        // ==============================
        SemiminorAxis = OrbitalMath.SemiMinorAxis(SemimajorAxis, Eccentricity);

        Periapsis = OrbitalMath.Periapsis(SemimajorAxis, Eccentricity);
        Apoppsis = OrbitalMath.Apopsis(SemimajorAxis, Eccentricity);
        
        OrbitalPeriod = OrbitalMath.CalcOrbitalPeriod(SemimajorAxis, referenceBody.Mass);
        // ==============================
    }

    /// <summary>
    /// Calculates the distance from the center of gravity to the orbital path at a given angle (relative to the periapsis) 
    /// </summary>
    /// <param name="theta">Angle in radians</param>
    /// <returns></returns>
    public Vector3 PointAtTheta(double theta) 
    {
        if (Orbiting == null)
        {
            return Vector3.zero;
        }

        return OrbitalMath.PointAtTheta(EccentricityVector, OrbitalPlane, SemimajorAxis, theta);
    }
}

public class GameState : MonoBehaviour
{
    public List<BodyState> OrbitingBodies = new ();

    public List<BodyState> InitialBodies = new()
    {
        new (){Name = "Sun", Mass = 1.989E30, Diameter = 1.39E9, Position = Vector3d.zero, Velocity = Vector3d.zero, Color = Color.blue},
        new () {Name = "PlanetX", Mass = 6.39E24, Diameter = 12.75e6, Position = 149E9 * Vector3d.forward, Velocity = 29.8e3 * Vector3d.left, Color = Color.magenta},
        new () {Name = "PlanetY", Mass = 6.39E24, Diameter = 12.75e6, Position = -100E9 * Vector3d.forward, Velocity = -29.8e3 * Vector3d.left, Color = Color.green},
        new () {Name = "Player", Mass = 1000, Diameter = 100, Position = 150E9 * Vector3d.forward, Velocity = 29.8e3 * Vector3d.left, Color = Color.red},
    };
    
    private void Awake()
    {
        foreach (var body in InitialBodies)
        {
            OrbitingBodies.Add(body);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }
    
    void FixedUpdate()
    {
        foreach (var body in OrbitingBodies)
        {
            var gravity = Vector3d.zero;
            var maxGravity = 0.0;
            var maxReference = body; 
            foreach (var b in OrbitingBodies)
            {
                if (body == b)
                {
                    continue;
                }

                var g = OrbitalMath.CalcGravityVector(b.Position, b.Mu, body.Position);
                var gravMag = g.magnitude; 
                if (gravMag > maxGravity)
                {
                    maxGravity = gravMag;
                    maxReference = b;
                }

                gravity += g;
            }

            body.Orbiting = maxReference;
            
            body.UpdateGravity(gravity);
            
            //TODO: if (IsKinematic)
            body.UpdatePosition(Time.fixedDeltaTime); 
            
            //This is only needed when the object is being rendered.
            //Can probably update this every 5 seconds... and then animate between states
            body.UpdateElements(maxReference); 
        }
    }
    
    // Update is called once per frame
    void Update()
    {
    }

}