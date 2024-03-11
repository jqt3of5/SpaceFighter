using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetGenerator : MonoBehaviour
{
    public GameObject PlanetPrefab;

    public List<PlanetController> Planets = new();
    public List<ShipController> Ships = new();
    // Start is called before the first frame update
    private void Awake()
    {
        // for (int i = 0; i < 2; ++i)
        // {
        
        var planet = Instantiate(PlanetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        planet.name = planet.name + 0;
            
        var controller = planet.GetComponentInChildren<PlanetController>();
        Planets.Add(controller);

        // controller.accPerM2 = defaultAccPerM2;
            
        var planet1 = Instantiate(PlanetPrefab, new Vector3(0, 0, 2000), Quaternion.identity);
        planet1.name = planet.name + 1;
            
        var controller1 = planet1.GetComponentInChildren<PlanetController>();
        Planets.Add(controller1);

        controller1.OrbitalBody.Mu = controller.OrbitalBody.Mu/10;
        //Initial velocity for orbiting
        controller1.OrbitalBody.Velocity = new Vector3(Mathf.Sqrt(controller.OrbitalBody.Mu/1000f),0,0);
        
        controller1.OrbitalBody.AddBodyToSystem(controller.OrbitalBody);
        controller.OrbitalBody.AddBodyToSystem(controller1.OrbitalBody);
        // }  
    }

    void Start()
    {
        Ships = FindObjectsByType<ShipController>(FindObjectsSortMode.None).ToList();

        foreach (var ship in Ships)
        {
            ship.transform.position = new Vector3(0, 500, 2000);
            ship.OrbitalBody.Velocity = new Vector3(Planets[1].OrbitalBody.Velocity.x + 10, 0, 0);
            
            ship.OrbitalBody.AddBodyToSystem(Planets[1].OrbitalBody);
        }
        
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}