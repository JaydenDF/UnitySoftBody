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
    
    public List<MassPoint> massPoints = new List<MassPoint>();
    public List<Spring> springs = new List<Spring>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //MassPointPlacer();
        MassPointDrawer();
        SpringDrawer();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void CalculateVolume()
    {
        
    }
}
