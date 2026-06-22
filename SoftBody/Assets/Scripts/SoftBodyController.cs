using System.Collections.Generic;
using UnityEngine;

public class SoftBodyController : MonoBehaviour
{
    public int pointCount = 10;
    public float radius;
    public float mass = 5;
    public GameObject dotPrefab;
    
    public List<MassPoint> massPoints = new List<MassPoint>();
    public List<Spring> springs = new List<Spring>();
    
    // Physics
    public float gravity = 9.81f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MassPointPlacer();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
    }

    public void MassPointPlacer()
    {
        radius = 1f / (2f * Mathf.Sin(Mathf.PI / pointCount));

        for (int i = 0; i < pointCount; i++)
        {
            float angle = (2f * Mathf.PI * i) / pointCount;
            // Place MassPoint
            massPoints.Add(new MassPoint(
                new Vector2(Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius), mass, dotPrefab)); 
            
            // Create and connect Visual for the MassPoint
            GameObject obj = Instantiate(dotPrefab, massPoints[i].position, Quaternion.identity);
            obj.transform.parent = transform;
            MassPointVisual visual = obj.GetComponent<MassPointVisual>();
            visual.Bind(massPoints[i]);
            
            Debug.Log($"Point {i}: {massPoints[i]}");
        }
    }

    public void SpringPlacer()
    {
        for (int i = 0; i < pointCount; i++)
        {
            if (i == pointCount - 1)
            {
                springs.Add(new Spring(massPoints[i], massPoints[0],
                    1f, 1f, 1f));
            }
            else
            {
                springs.Add(new Spring(massPoints[i], massPoints[0],
                    1f, 1f, 1f));
            }
            

            Debug.Log($"spring {i}: {springs[i]}");
        }
    }

    public void ApplyGravity()
    {
        for (int i = 0; i < massPoints.Count; i++)
        {
            MassPoint mp = massPoints[i]; // copy

            mp.ApplyGravity(Time.deltaTime);

            massPoints[i] = mp; // write back
        }
    }
}
