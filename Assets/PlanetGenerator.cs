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
        var planet = Instantiate(PlanetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        planet.name = planet.name + 0;
            
        var controller = planet.GetComponentInChildren<PlanetController>();
        Planets.Add(controller);
    }

    void Start()
    {
        Ships = FindObjectsByType<ShipController>(FindObjectsSortMode.None).ToList();

        foreach (var ship in Ships)
        {
            ship.transform.position = new Vector3(0, 3200, 0);
            ship.OrbitalBody.Velocity = new Vector3(Planets[0].OrbitalBody.Velocity.x + 10, 0, 0);
            
            ship.OrbitalBody.AddBodyToSystem(Planets[0].OrbitalBody);
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