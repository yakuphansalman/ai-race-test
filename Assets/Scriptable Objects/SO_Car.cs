using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Car Preferences" , menuName = "Car Preferences")]
public class SO_Car : ScriptableObject
{
    [SerializeField] private float _speed, _rotationSpeed;

    public float Speed => _speed;
    public float RotationSpeed => _rotationSpeed;
}
