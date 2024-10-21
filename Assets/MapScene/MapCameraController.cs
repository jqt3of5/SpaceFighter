using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCameraController : MonoBehaviour
{
    public TextMeshProUGUI LabelPrefab;
    public Canvas CanvasPrefab;
    public GameState GameState;
    
    // Start is called before the first frame update
    void Start()
    {
        // var projected = Player.PlanetRigidBody.bodyState.Position + Player.PlanetRigidBody.bodyState.Velocity;
        // var screenPoint = Camera.WorldToScreenPoint((Vector3)projected);
        // screenPoint.z = 0;
        // _prograde.rectTransform.anchoredPosition = screenPoint;
        //
        // foreach (var planet in State.OrbitingBodies)
        // {
        //     var position = planet.Position;
        //     
        //     var r = position - Player.transform.position;
        //     var velocity = Vector3.Dot((Vector3)Player.PlanetRigidBody.bodyState.Velocity, (Vector3)r.normalized);
        //     
        //     _target.text = $"{velocity:F3} km/s";
        // }

    }
    
}