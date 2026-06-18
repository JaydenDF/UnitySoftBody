using UnityEngine;
using System.Collections.Generic;

public class SquareDrawer : MonoBehaviour
{
    [Header("Size")]
    public float width = 5f;
    public float height = 5f;

    [Header("Dot Settings")]
    public GameObject dotPrefab;

    [Header("Line Settings")]
    public Material lineMaterial;
    public float lineWidth = 0.1f;

    [Header("Physics")]
    public float gravity = -9.81f;
    public Vector2 velocity;

    // Corners stored clockwise
    public List<Vector3> corners = new List<Vector3>();

    private List<GameObject> dots = new List<GameObject>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        GenerateCorners();
        CreateDots();
        CreateLines();
    }

    void Update()
    {
        ApplyGravity();
        GenerateCorners();
        UpdateDots();
        UpdateLines();
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        Vector3 pos = transform.position;
        pos += (Vector3)(velocity * Time.deltaTime);

        // Camera bounds
        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        float minX = -screenWidth / 2f;
        float maxX = screenWidth / 2f;
        float minY = -screenHeight / 2f;
        float maxY = screenHeight / 2f;

        // Keep entire rectangle visible
        if (pos.x < minX)
        {
            pos.x = minX;
            velocity.x = 0;
        }
        else if (pos.x + width > maxX)
        {
            pos.x = maxX - width;
            velocity.x = 0;
        }

        if (pos.y < minY)
        {
            pos.y = minY;
            velocity.y = 0;
        }
        else if (pos.y + height > maxY)
        {
            pos.y = maxY - height;
            velocity.y = 0;
        }

        transform.position = pos;
    }

    void GenerateCorners()
    {
        corners.Clear();

        Vector3 bottomLeft = transform.position;
        Vector3 bottomRight = bottomLeft + new Vector3(width, 0, 0);
        Vector3 topRight = bottomLeft + new Vector3(width, height, 0);
        Vector3 topLeft = bottomLeft + new Vector3(0, height, 0);

        // Clockwise order
        corners.Add(bottomLeft);   // 0
        corners.Add(bottomRight);  // 1
        corners.Add(topRight);     // 2
        corners.Add(topLeft);      // 3
    }

    void CreateDots()
    {
        foreach (Vector3 corner in corners)
        {
            dots.Add(Instantiate(dotPrefab, corner, Quaternion.identity));
        }
    }

    void UpdateDots()
    {
        for (int i = 0; i < dots.Count; i++)
        {
            dots[i].transform.position = corners[i];
        }
    }

    void CreateLines()
    {
        for (int i = 0; i < corners.Count; i++)
        {
            GameObject lineObj = new GameObject("Edge " + i);
            lineObj.transform.parent = transform;

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.positionCount = 2;
            lr.useWorldSpace = true;

            lines.Add(lr);
        }
    }

    void UpdateLines()
    {
        for (int i = 0; i < corners.Count; i++)
        {
            Vector3 start = corners[i];
            Vector3 end = corners[(i + 1) % corners.Count];

            lines[i].SetPosition(0, start);
            lines[i].SetPosition(1, end);
        }
    }
}
