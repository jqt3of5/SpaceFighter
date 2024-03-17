using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlanetController : MonoBehaviour
{
    public const double G = 6.67430E-11f;
    
    /// <summary>
    /// Standard gravitation parameter
    /// Units are .1km^2 * .1km/s^2
    ///
    /// Mars is 6776km in diameter, so the surface is ~ 3000 km from the center. At the surface of an equivalent planet
    /// I want the acceleration to be .050 km/s2.
    /// Ex: Earth is 3.986E5
    /// </summary>
    public double Mu = 50.0 * 3000 * 3000 / 1000;
    
    public double Mass;
    
    public LineRenderer LineRenderer;
    public OrbitalBodyComponent OrbitalBody { get; set; }

    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
        OrbitalBody = GetComponent<OrbitalBodyComponent>();
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
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}