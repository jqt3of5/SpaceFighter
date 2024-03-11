using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlanetController : MonoBehaviour
{
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
        
    }
    
    void FixedUpdate()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}