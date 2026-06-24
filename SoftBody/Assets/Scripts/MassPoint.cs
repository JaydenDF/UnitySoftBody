using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class  MassPoint : MonoBehaviour
{
    // Public Variables
    public float mass = 0.1f;
    
    // Verlet integration
    public Vector2 force;
    private Vector2 currentPosition;
    private Vector2 previousPosition;

    private void Start()
    {
        mass = 1;
        currentPosition = transform.position;
        previousPosition = currentPosition;
    }
    
    private void FixedUpdate()
    {
        // Movement based of Verlet integration (implementation help from ChatGPT)
        Vector2 acceleration = force / mass;
        Vector2 velocity = currentPosition - previousPosition;
        Vector2 newPosition = currentPosition + velocity +  acceleration * Time.deltaTime * Time.deltaTime;
        
        previousPosition = currentPosition;
        currentPosition = newPosition;
        
        transform.position = currentPosition;
        force = Vector2.zero; 
    }

    public void AddForce(Vector2 _force)
    {
        force += _force;
    }
    
    public void ResolveCollision(Vector2 normal, float restitution = 0.99f)
    {
        Vector2 velocity = currentPosition - previousPosition;
        velocity = Vector2.Reflect(velocity, normal) * restitution;
        previousPosition = currentPosition - velocity;
    }
    
    public Vector2 GetVelocity()
    {
        return currentPosition - previousPosition;
    }
}