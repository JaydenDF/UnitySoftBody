using System.Collections.Generic;
using UnityEngine;

public class SoftBodyController : MonoBehaviour
{
    public int pointCount = 10;
    public float radius;
    public float mass = 1;
    public GameObject massPointPrefab;
    
    public GameObject springPrefab;
    public float springStifness;
    public float springRestLength;
    public float springDamping;
    public Material springLineMaterial;

    private float desiredArea;
    public float scaleCoefficient = 1f;
    
    public List<MassPoint> massPoints = new List<MassPoint>();
    public List<Spring> springs = new List<Spring>();

    public float gravity = 9.81f;
    public float bounceDamping = 0.99f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //MassPointPlacer();
        MassPointDrawer();
        SpringDrawer();
        
        // Store desired area
        desiredArea = CalculateCurrentArea() + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //ApplyGravity();
        Dilation();
    }

    public void MassPointDrawer()
    {
        radius = 1f / (2f * Mathf.Sin(Mathf.PI / pointCount));

        for (int i = 0; i < pointCount; i++)
        {
            float angle = (2f * Mathf.PI * i) / pointCount;
            
            GameObject massPointSpawn = Instantiate(massPointPrefab,
                new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0.0f), 
                Quaternion.identity);
            
            massPointSpawn.transform.parent = transform;
            
            massPoints.Add(massPointSpawn.GetComponent<MassPoint>());
            
            Debug.Log($"Point {i}: {massPoints[i]}");
        }
    }

    public void SpringDrawer()
    {
        for (int i = 0; i < pointCount; i++)
        {
            GameObject springSpawn = Instantiate(springPrefab);
            springSpawn.transform.parent = transform;
            springs.Add(springSpawn.GetComponent<Spring>());
            
            if (i == pointCount - 1)
            {
                springSpawn.GetComponent<Spring>().CreateSpring(
                    massPoints[i],
                    massPoints[0],
                    springStifness,
                    springRestLength,
                    springDamping,
                    springLineMaterial);
            }
            else
            {
                springSpawn.GetComponent<Spring>().CreateSpring(
                    massPoints[i],
                    massPoints[i + 1],
                    springStifness,
                    springRestLength,
                    springDamping,
                    springLineMaterial);
            }
            
        
            Debug.Log($"spring {i}: {springs[i]}");
        }
    }

    public float CalculateCurrentArea()
    {
        float area = 0f;
        foreach (Spring spring in springs)
        {
            Vector2 mp1 = spring.mpA.transform.position;
            Vector2 mp2 = spring.mpB.transform.position;
            
            area += ((mp1.y + mp2.y) * (mp1.x - mp2.x)) * 0.5f;
        }
        
        return Mathf.Abs(area);
    }

    public void Dilation()
    {
        float areaDif =  desiredArea - CalculateCurrentArea();
        float scalingFactor = scaleCoefficient * areaDif;

        MassPoint mp1;
        MassPoint mp2;

        for (int i = 0; i < massPoints.Count; i++)
        {
            if (i == 0)
            {
                //First massPoint
                mp1 = massPoints[massPoints.Count - 1];
                mp2 = massPoints[i + 1];
            }
            else if (i == massPoints.Count - 1)
            {
                // Last massPoint
                mp1 = massPoints[i - 1];
                mp2 = massPoints[0];
            } 
            else
            {
                // Every other massPoint
                mp1 = massPoints[i - 1];
                mp2 = massPoints[i + 1];
            }

            Vector2 secantLine = mp1.transform.position - mp2.transform.position;
            Vector2 rotatedSecantLine = new Vector2(-secantLine.y, secantLine.x);
            rotatedSecantLine.Normalize();
            Vector2 dilationForce = rotatedSecantLine * scalingFactor;
            
            massPoints[i].AddForce(dilationForce);
        }
    }

    private void ApplyGravity()
    {
        for (int i = 0; i < massPoints.Count; i++)
        {
            Vector2 gravityForce = Vector2.down * gravity * mass;
            massPoints[i].AddForce(gravityForce);
        }
    }
}
