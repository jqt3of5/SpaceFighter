using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class CameraViewController : MonoBehaviour
{
    public ShipController Player;
    public Canvas Canvas;
    public Camera Camera;
    public GameState State;

    public TextMeshProUGUI LabelPrefab;
    private TextMeshProUGUI _yawTarget;
    private TextMeshProUGUI _yawRate;
    private TextMeshProUGUI _panTarget;
    private TextMeshProUGUI _panRate;
    private TextMeshProUGUI _pitchTarget;
    private TextMeshProUGUI _pitchRate;

    private BoxCollider _boxCollider; 
    private Image _prograde;
    private TextMeshProUGUI _throttle;
    private TextMeshProUGUI _orbit;
    private TextMeshProUGUI _target;
    private TextMeshProUGUI _eccentricity;
    private TextMeshProUGUI _orbitalPeriod;
    private TextMeshProUGUI _periapsis;

    // Start is called before the first frame update
    void Start()
    {
        _yawTarget = GameObject.Find("YawTarget").GetComponent<TextMeshProUGUI>();
        _yawRate = GameObject.Find("YawRate").GetComponent<TextMeshProUGUI>();
        _panTarget = GameObject.Find("PanTarget").GetComponent<TextMeshProUGUI>();
        _panRate = GameObject.Find("PanRate").GetComponent<TextMeshProUGUI>();
        _pitchTarget = GameObject.Find("PitchTarget").GetComponent<TextMeshProUGUI>();
        _pitchRate = GameObject.Find("PitchRate").GetComponent<TextMeshProUGUI>();
        
        _throttle = GameObject.Find("ThrottleValue").GetComponent<TextMeshProUGUI>();
        _orbit = GameObject.Find("OrbitalVelocity").GetComponent<TextMeshProUGUI>();
        _target = GameObject.Find("TargetVelocity").GetComponent<TextMeshProUGUI>();
        
        _eccentricity = GameObject.Find("EccentricityValue").GetComponent<TextMeshProUGUI>();
        _orbitalPeriod = GameObject.Find("OrbitalPeriodValue").GetComponent<TextMeshProUGUI>();
        
        _periapsis = GameObject.Find("PeriapsisDistance").GetComponent<TextMeshProUGUI>();

        _boxCollider = Canvas.GetComponent<BoxCollider>();
        
        _prograde = GameObject.Find("Prograde").GetComponent<Image>();

        // foreach (var planet in _planets)
        // {
        //     _texts[planet] = Instantiate(LabelPrefab, _canvas.transform, false);
        //     _texts[planet].text = planet.Name;
        // }
    }
  
    void Update()
    {
        transform.position = Player.transform.position;
        transform.rotation = Player.transform.rotation;
        
        _yawRate.text = $"{Player.BodyState.EulerAngularVelocity.z:F} /s";
        _yawTarget.text = $"{Player.BodyState.EulerRotation.z:F}";
        
        _pitchRate.text = $"{Player.BodyState.EulerAngularVelocity.x:F} /s";
        _pitchTarget.text = $"{Player.BodyState.EulerRotation.x:F}";
        
        _panRate.text = $"{Player.BodyState.EulerAngularVelocity.y:F} /s";
        _panTarget.text = $"{Player.BodyState.EulerRotation.y:F}";

        _throttle.text = $"{Player.throttle}";

        //TODO:
        _orbit.text = $"{Player.BodyState.Velocity.magnitude:F3} km/s";
        _eccentricity.text = $"{Player.BodyState.Eccentricity:F3}";
        _orbitalPeriod.text = $"{Player.BodyState.OrbitalPeriod:F3} s";
        _periapsis.text = $"{Player.BodyState.SemimajorAxis/1000:F3} km";
        
        //Project the velocity out passed the canvas
        // Debug.DrawRay(Player.PlanetRigidBody.Position, Player.PlanetRigidBody.Velocity*1000,Color.green );
        
        var projected = Player.BodyState.Position + Player.BodyState.Velocity;
        var screenPoint = Camera.WorldToScreenPoint((Vector3)projected);
        screenPoint.z = 0;
        _prograde.rectTransform.anchoredPosition = screenPoint;
        
        foreach (var planet in State.OrbitingBodies)
        {
            var position = planet.Position;
            
            var r = position - Player.transform.position;
            var velocity = Vector3.Dot((Vector3)Player.BodyState.Velocity, (Vector3)r.normalized);
            
            _target.text = $"{velocity:F3} km/s";
            
        }
    }
}