using System;
using UnityEngine;

[Serializable] public struct  MassPoint
{
    // Public Variables
    public Vector2 position;
    public float mass;
    public GameObject dotPrefab;
    
    
    // Read-only variables, should only be changed within this struct.
    public Vector2 Velocity { get; private set; }
    public Vector2 Force { get; private set; }
    public MassPoint(Vector2 _mpPosition, float _mpMass, GameObject _mpDot)
    {
        position = _mpPosition;
        mass = _mpMass;
        dotPrefab = _mpDot;
        Velocity = default;
        Force = default;
    }
    
    public void ApplyGravity(float deltaTime)
    {
        position.y += 9.81f * deltaTime;
        Console.WriteLine("falling, dreaming, i know you want to cry all night");
    }
}