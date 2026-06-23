using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class  MassPoint : MonoBehaviour
{
    // Public Variables
    private float mass = 1;
    public Vector2 velocity;
    public Vector2 acceleration;
    
    
    // Verlet integration
    public Vector2 force;
    private Vector2 currentPosition;
    private Vector2 previousPosition;

    private void Start()
    {
        currentPosition = transform.position;
        previousPosition = currentPosition;
    }
    
    private void FixedUpdate()
    {
        //ApplyGravity();
        
        // Movement based of Verlet integration (implementation help from ChatGPT)
        Vector2 acceleration = force / mass;
        Vector2 velocity = currentPosition - previousPosition;
        Vector2 newPosition = currentPosition + velocity +  acceleration * Time.deltaTime;
        
        previousPosition = currentPosition;
        currentPosition = newPosition;
        
        transform.position = currentPosition;
        force = Vector2.zero; 
    }

    private void ApplyGravity()
    {
        velocity.y -= 9.81f * Time.deltaTime;
        transform.position += (Vector3)(velocity * Time.deltaTime) ;
    }

    public void AddForce(Vector2 _force)
    {
        force += _force;
    }
}