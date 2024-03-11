using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class CameraViewController : MonoBehaviour
{
    private Canvas _canvas;
    private MeshCollider _plane;
    private Camera _camera;

    private Dictionary<PlanetController, TextMeshProUGUI> _texts = new Dictionary<PlanetController, TextMeshProUGUI>();
    private PlanetController[] _planets;
    private ShipController _player;
    private TextMeshProUGUI _prograde;
    private TextMeshProUGUI _screenCenter;

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _camera = GetComponent<Camera>();
        _player = GameObject.FindObjectOfType<ShipController>();
        _planets = GameObject.FindObjectsByType<PlanetController>(FindObjectsSortMode.None);
        _plane = _canvas.GetComponent<MeshCollider>();

        var template = _canvas.GetComponentInChildren<TextMeshProUGUI>();
        foreach (var planet in _planets)
        {
            _texts[planet] = Object.Instantiate(template, _canvas.transform, false);
            _texts[planet].text = planet.name;
        }

        _prograde = Instantiate(template, _canvas.transform, false);
        _prograde.text = "X";
        
        _screenCenter = Instantiate(template, _canvas.transform, false);
        _screenCenter.text = "O";

        _screenCenter.rectTransform.anchoredPosition = _canvas.renderingDisplaySize / 2;
    }
    
  
    void Update()
    {
        var ray = new Ray(_player.transform.position, _player.OrbitalBody.Velocity);
        Debug.DrawRay(_player.transform.position, _player.OrbitalBody.Velocity*1000, Color.green);
        
        if (_plane.Raycast(ray, out var info, 100))
        {
            var hit = ray.GetPoint(info.distance);
            var screen = _camera.WorldToScreenPoint(hit);
            screen.z = 0; 
            _prograde.rectTransform.anchoredPosition = screen;
        }
        
        foreach (var planet in _planets)
        {
            var screenPoint = _camera.WorldToScreenPoint(planet.transform.position);

            screenPoint.z = 0; 
            _texts[planet].rectTransform.anchoredPosition = screenPoint;
            
            var distance = (_player.transform.position - planet.transform.position).magnitude;
            var velocity = (_player.OrbitalBody.Velocity - planet.OrbitalBody.Velocity).magnitude;
            _texts[planet].text = $"{planet.name}\n{distance:F}km\n{velocity:F}km/s";
        }
    }
}