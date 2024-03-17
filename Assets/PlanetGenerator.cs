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
        planet.name = planet.name;
            
        var controller = planet.GetComponent<PlanetController>();
        Planets.Add(controller);
    }

    void Start()
    {
        Ships = FindObjectsByType<ShipController>(FindObjectsSortMode.None).ToList();

        foreach (var ship in Ships)
        {
            ship.OrbitalBody.AddBodyToSystem(Planets[0]);
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