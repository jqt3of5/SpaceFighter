using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class CameraViewController : MonoBehaviour
{
    private Canvas _canvas;
    private Camera _camera;

    private Dictionary<PlanetController, TextMeshProUGUI> _texts = new Dictionary<PlanetController, TextMeshProUGUI>();
    private PlanetController[] _planets;
    private ShipController _player;
    
    public TextMeshProUGUI LabelPrefab;
    private TextMeshProUGUI _yawTarget;
    private TextMeshProUGUI _yawRate;
    private TextMeshProUGUI _panTarget;
    private TextMeshProUGUI _panRate;
    private TextMeshProUGUI _pitchTarget;
    private TextMeshProUGUI _pitchRate;

    private BoxCollider _boxCollider; 
    private GraphicRaycaster _raycaster;
    private TextMeshProUGUI _prograde;

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _camera = GetComponent<Camera>();
        _player = FindObjectOfType<ShipController>();
        _planets = FindObjectsByType<PlanetController>(FindObjectsSortMode.None);

        _yawTarget = GameObject.Find("YawTarget").GetComponent<TextMeshProUGUI>();
        _yawRate = GameObject.Find("YawRate").GetComponent<TextMeshProUGUI>();
        _panTarget = GameObject.Find("PanTarget").GetComponent<TextMeshProUGUI>();
        _panRate = GameObject.Find("PanRate").GetComponent<TextMeshProUGUI>();
        _pitchTarget = GameObject.Find("PitchTarget").GetComponent<TextMeshProUGUI>();
        _pitchRate = GameObject.Find("PitchRate").GetComponent<TextMeshProUGUI>();

        _boxCollider = _canvas.GetComponent<BoxCollider>();
        _raycaster = GetComponent<GraphicRaycaster>();
        
        _prograde = Instantiate(LabelPrefab, _canvas.transform, false);

        foreach (var planet in _planets)
        {
            _texts[planet] = Instantiate(LabelPrefab, _canvas.transform, false);
            _texts[planet].text = planet.name;
        }
    }
  
    void Update()
    {
        transform.position = _player.transform.position;
        transform.rotation = _player.transform.rotation;
        
        _yawRate.text = $"{_player.Rigidbody.angularVelocity.z:F} /s";
        _yawTarget.text = $"{_player.Rigidbody.rotation.z:F}";
        
        _pitchRate.text = $"{_player.Rigidbody.angularVelocity.x:F} /s";
        _pitchTarget.text = $"{_player.Rigidbody.rotation.x:F}";
        
        _panRate.text = $"{_player.Rigidbody.angularVelocity.y:F} /s";
        _panTarget.text = $"{_player.Rigidbody.rotation.y:F}";

        var ray = new Ray(_player.transform.position, _player.Rigidbody.velocity);

        if (_boxCollider.Raycast(ray, out var hits, 50))
        {
            _prograde.rectTransform.anchoredPosition = hits.point;
            Debug.Log(hits.collider?.name);
            Debug.Log(hits.point);
        }
        
        foreach (var planet in _planets)
        {
            var position = planet.transform.position;
            var screenPoint = _camera.WorldToScreenPoint(position);
        
            screenPoint.z = 0; 
            _texts[planet].rectTransform.anchoredPosition = screenPoint;
            
            var r = position - _player.transform.position;
            var velocity = Vector3.Dot(_player.Rigidbody.velocity, r.normalized);
            _texts[planet].text = $"{planet.name}\n{r.magnitude:F}km\n{velocity:F}km/s";
        }
    }
}