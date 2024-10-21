using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    /// <summary>
    /// Used to map the BodyState over to the RigidBody. And vice versa 
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class OrbitRender : MonoBehaviour
    {
        public double Scale = 1;
        
        public LineRenderer lineRenderer;
        
        public BodyState bodyState;
        
        public float textSize = 12f;
        public float lineWidth = 1f;
        public const int lineResolution = 200;

        public double Eccentricity = 0;
        public double TrueAnomaly = 0;
        public Vector3 OrbitalPlane = Vector3.zero;
        public double SemiMajorAxis = 0;
        public double OrbitalPeriod = 0;

        public GameObject CanvasPrefab;
        public GameObject LabelPrefab;

        private TextMeshProUGUI apopsisText;
        private TextMeshProUGUI periapsisText;
        
        private Canvas apopsisCanvas;
        private Canvas periapsisCanvas;

        private Camera Camera;
        private void Awake()
        {
            Camera = GameObject.Find("MainCamera").GetComponent<Camera>();
            
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.useWorldSpace = false;
            //
            // var canvas1 = Instantiate(CanvasPrefab, this.transform);
            // apopsisCanvas = canvas1.GetComponent<Canvas>();
            //
            // var label1 = Instantiate(LabelPrefab, apopsisCanvas.transform);
            // apopsisText = label1.GetComponent<TextMeshProUGUI>();
            // apopsisText.text = "";
            //
            // var canvas2 = Instantiate(CanvasPrefab, this.transform);
            // periapsisCanvas = canvas2.GetComponent<Canvas>();
            //
            // var label2 = Instantiate(LabelPrefab, periapsisCanvas.transform);
            // periapsisText = label2.GetComponent<TextMeshProUGUI>();
            // periapsisText.text = "";
        }

        private void Start()
        {
        
        }

        private void FixedUpdate()
        {
            Eccentricity = bodyState.Eccentricity;
            TrueAnomaly = bodyState.TrueAnomaly;
            OrbitalPlane = (Vector3)bodyState.OrbitalPlane;
            OrbitalPeriod = bodyState.OrbitalPeriod;
            SemiMajorAxis = bodyState.SemimajorAxis;
        }

        public void Update()
        {
            //TODO: Respond to Camera events?
            var cameraTransform = Camera.transform;
            
            // apopsisCanvas.transform.LookAt(cameraTransform);
            // periapsisCanvas.transform.LookAt(cameraTransform);
            //
            // //TODO: Texxt doesn't change?
            // // textSize = (float)Math.Clamp(Math.Log(Scale), 0, 100);
            // // periapsisCanvas.transform.localScale = new Vector3(1,1,1) * textSize;
            // // apopsisCanvas.transform.localScale = new Vector3(1,1,1) * textSize;
            //
            // // Move the canvas to be towards the camera just a bit
            // var periapsis = ((cameraTransform.localPosition - periapsisCanvas.transform.localPosition).normalized) + bodyState.PointAtTheta(0) * (float)Scale;
            // periapsisCanvas.transform.localPosition = (Vector3)periapsis;
            //
            // //TODO: Not all orbits have a periapsis
            // periapsisText.text = $"Periapsis";
            //
            // if (bodyState.Eccentricity < 1)
            // {
            //     // Move the canvas to be towards the camera just a bit
            //     var apopsis = ((cameraTransform.localPosition - apopsisCanvas.transform.localPosition).normalized) + bodyState.PointAtTheta(Math.PI) * (float)Scale;
            //     apopsisCanvas.transform.localPosition = (Vector3)apopsis;
            //     apopsisText.text = $"Apopsis";
            // }
            
            // lineWidth = (float)Math.Clamp(Math.Log(distance), 0, 100);
 
            lineRenderer.startColor = bodyState.Color;
            lineRenderer.endColor = bodyState.Color;

            // lineWidth = (float)Scale;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
 
            // lineRenderer.widthCurve = AnimationCurve.EaseInOut(0, lineWidth, lineResolution, lineWidth);
            
            lineRenderer.positionCount = 0;
            var baryCenter = OrbitalMath.BaryCenter((bodyState.Position, bodyState.Mass), (bodyState.Orbiting.Position, bodyState.Orbiting.Mass));
            for (int i = 0, j = 0; i < lineResolution + 1; i++)
            {
                var rad = (double)i / lineResolution * 2.0 * Mathd.PI;

                //Calculate the distance from the barycenter at a particular angle
                var position = (baryCenter + bodyState.PointAtTheta(rad)) * Scale;
                
                //TODO: Unsure hwere to set this
                // if (position.magnitude > 10000)
                // {
                // continue;
                // }

                //Only draw the lines that are close
                lineRenderer.positionCount = j+1;
                //Draw the line
                lineRenderer.SetPosition(j, (Vector3)position);
                j += 1;
            }
        }
    }
}