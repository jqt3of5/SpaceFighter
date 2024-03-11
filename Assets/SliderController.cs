using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderController : MonoBehaviour
{
    private ShipController _ship;
    // Start is called before the first frame update
    void Start()
    {
        _ship = GameObject.Find("ShipCube").GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
