using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_CarController : MonoBehaviour
{
    public SO_Car _carPreferences;

    private float _speed;
    private float _rotationSpeed;
    private float _horizontalMultiplier,_verticalMultiplier;

    public Vector3 showChecker;

    private Ray[] _rays;
    private RaycastHit[] _hits;


    private void Start()
    {
        _speed = _carPreferences.Speed;
        _rotationSpeed = _carPreferences.RotationSpeed;
        
        _rays = new Ray[8];
        _hits = new RaycastHit[8];
    }

    private void Update()
    {
        Move();

        CheckObstacles();
    }
    private void Move()
    {

        transform.position += transform.forward * _speed * Time.deltaTime;
    }
    private void CheckObstacles()
    {
        //Cast the rays and hits
        for (int i = 0; i < 8; i++)
        {
            Physics.Raycast(_rays[i], out _hits[i], 5f);
        }

        #region Ray Directions
        for (int i = 0; i < 8; i++)
        {
            if (i == 0)
            {
                _verticalMultiplier = 1;
                _horizontalMultiplier = 0;
            }
            if (i == 1)
            {
                _verticalMultiplier = 0;
                _horizontalMultiplier = 1;
            }
            if (i == 2)
            {
                _verticalMultiplier = -1;
                _horizontalMultiplier = 0;
            }
            if (i == 3)
            {
                _verticalMultiplier = 0;
                _horizontalMultiplier = -1;
            }
            if (i == 4)
            {
                _verticalMultiplier = 1;
                _horizontalMultiplier = 1;
            }
            if (i == 5)
            {
                _verticalMultiplier = 1;
                _horizontalMultiplier = -1;
            }
            if (i == 6)
            {
                _verticalMultiplier = -1;
                _horizontalMultiplier = -1;
            }
            if (i == 7)
            {
                _verticalMultiplier = -1;
                _horizontalMultiplier = 1;
            }
            _rays[i] = new Ray(transform.position, (transform.forward * _verticalMultiplier + transform.right * _horizontalMultiplier).normalized);

        }
        #endregion

        #region Scenarios
        #region Mutual Scenario
        for (int i = 0; i < 8; i++)
        {
            if (_hits[i].collider != null)
            {
                if (_hits[i].collider.gameObject.CompareTag("Obstacle"))
                {
                    Vector3 checker = _hits[i].collider.transform.localPosition - transform.localPosition;
                    transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
                    #region Unmutual Scenarios
                    bool onCross = false;
                    #region Front Scenario
                    if (i == 0)
                    { 

                    }
                    #endregion

                    #region Cross Scenario
                    if (i == 4 || i == 5)
                    {
                        if (_hits[i].collider != null)
                        {
                            onCross = true;
                        }
                    }
                    #endregion

                    #endregion
                }
            }

        }
        #endregion
        #endregion
    }
}
