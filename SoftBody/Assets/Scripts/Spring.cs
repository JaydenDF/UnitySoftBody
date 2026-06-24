using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Spring : MonoBehaviour
{
    // Public variables
    public MassPoint mpA;
    public MassPoint mpB;
    public float stifness;
    public float restLength;
    public float damping;
    
    public Material lineMaterial;
    public float lineWidth = 0.1f;
    public LineRenderer lineRenderer;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (mpA == null || mpB == null || lineRenderer == null)
            return;
        
        lineRenderer.SetPosition(0, mpA.transform.position);
        lineRenderer.SetPosition(1, mpB.transform.position);
        SpringPhysics();
    }

    // SpringPhysics function has been made with help of ChatGPT
    public void SpringPhysics()
    {
        Vector2 posA = mpA.transform.position;
        Vector2 posB = mpB.transform.position;

        Vector2 delta = posB - posA;
        float length = delta.magnitude;

        if (length == 0)
            return;

        Vector2 dir = delta.normalized;

        float extension = length - restLength;

        Vector2 springForce = dir * (extension * stifness);

        Vector2 relativeVelocity = mpB.GetVelocity() - mpA.GetVelocity();
        Vector2 dampingForce = dir * (Vector2.Dot(relativeVelocity, dir) * damping);

        Vector2 totalForce = springForce + dampingForce;

        mpA.AddForce(totalForce);
        mpB.AddForce(-totalForce );
    }

    public void CreateSpring(MassPoint _mpA, MassPoint _mpB, float _stifness,
        float _restLength, float _damping, Material _lineMaterial)
    {
        mpA = _mpA;
        mpB = _mpB;
        stifness = _stifness;
        restLength = _restLength;
        damping = _damping;
        lineMaterial = _lineMaterial;
        
        InitializeLineRenderer();
    }
    
    private void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
}
