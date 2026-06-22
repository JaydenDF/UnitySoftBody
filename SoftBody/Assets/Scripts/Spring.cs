using UnityEngine;
using UnityEngine.UIElements;

public struct Spring 
{
    // Public variables
    public MassPoint mpA;
    public MassPoint mpB;
    public float stifness;
    public float restLength;
    public float damping;
    
    public Spring(MassPoint _mpA, MassPoint _mpB, float _stifness, float _restLength, float _damping)
    {
        mpA = _mpA;
        mpB = _mpB;
        stifness = _stifness;
        restLength = _restLength;
        damping = _damping;
    }
}
