using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapGenerator : MonoBehaviour
{
    public double Zoom = -1000;
    public GameState GameState;
    public GameObject PlanetPrefab;
    public Camera Camera; 
    
    public List<(GameObject, Transform, OrbitRender, BodyState)> Planets = new ();

    public (GameObject, Transform, OrbitRender, BodyState) ReferencePlanet;
    
    void Start()
    {
        foreach (var state in GameState.OrbitingBodies)
        {
            var rotation = state.EulerRotation;
            
            var obj = Instantiate(PlanetPrefab, Vector3.zero, Quaternion.Euler(rotation.x, rotation.y, rotation.z), this.transform);
            
            obj.name = state.Name;
            
            var orbit = obj.GetComponentInChildren<OrbitRender>();
            orbit.bodyState = state;

            var planet = obj.transform.GetChild(1);
            Planets.Add((obj, planet, orbit, state));
        }  
        
        //TODO: Changeable
        ReferencePlanet = Planets.First();
    }

    void Update()
    {
        //We want to push the world so the center is on the surface of the planet/object. Not the Center of the object.  
        // var offsetToSurface = (Camera.transform.position - ReferencePlanet.Item2.position).normalized * ((float)ReferencePlanet.Item5.Diameter * (float)Scale);
        
        double Sigmoid(double x, double a = 1) => 1 / (1 + Math.Exp(-x / a));
        
        foreach (var (obj, planet, orbit, state) in Planets)
        {
            orbit.Scale = Sigmoid(Zoom,1);
            orbit.lineWidth = (float)Sigmoid(Zoom, 10);

            var diameter = Mathd.Log(state.Diameter) * Sigmoid(Zoom, 10); 
            
            planet.localScale = (float)diameter * new Vector3(1,1,1);
            planet.transform.localPosition = (Vector3)((state.Position - ReferencePlanet.Item4.Position) * orbit.Scale);// - offsetToSurface);
        }
           
        // TODO: rotate around the surface of the planet.
        transform.RotateAround(Vector3.zero, Vector3.down, _rotate.x);
        transform.RotateAround(Vector3.zero, Vector3.right, _rotate.y);
    }

    private Vector2 _rotate = Vector2.zero;
    
    public void OnZoom(InputValue value)
    {
        Zoom += value.Get<float>()/1000;
    }
    
    public void OnRotate(InputValue value)
    {
        _rotate = 2 * value.Get<Vector2>();
    }
}