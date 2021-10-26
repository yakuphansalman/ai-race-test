using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_CarController : MonoBehaviour
{
    public SO_Car _carPreferences;

    private float _speed, _rotationSpeed;
    private float _horizontalMultiplier,_verticalMultiplier;

    public Vector3 showChecker;

    private Ray[] _rays;
    private RaycastHit[] _hits;


    private void Start()
    {
        _speed = _carPreferences.Speed;
        
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
            Physics.Raycast(_rays[i], out _hits[i], 4f);
        }

        #region Ray Directions
        for (int i = 0; i < 8; i++)
        {
            if (i == 0)//Front
            {
                _verticalMultiplier = 1;
                _horizontalMultiplier = 0;
            }
            if (i == 1)//Right
            {
                _verticalMultiplier = 0;
                _horizontalMultiplier = 1;
            }
            if (i == 2)//Rear
            {
                _verticalMultiplier = -1;
                _horizontalMultiplier = 0;
            }
            if (i == 3)//Left
            {
                _verticalMultiplier = 0;
                _horizontalMultiplier = -1;
            }
            if (i == 4)//Front Right
            {
                _verticalMultiplier = 1;
                _horizontalMultiplier = 1;
            }
            if (i == 5)//Front Left
            {
                _verticalMultiplier = 1;
                _horizontalMultiplier = -1;
            }
            if (i == 6)//Rear Left
            {
                _verticalMultiplier = -1;
                _horizontalMultiplier = -1;
            }
            if (i == 7)//REar Right
            {
                _verticalMultiplier = -1;
                _horizontalMultiplier = 1;
            }
            _rays[i] = new Ray(transform.position, (transform.forward * _verticalMultiplier + transform.right * _horizontalMultiplier).normalized);
            Debug.DrawRay(_rays[i].origin,_rays[i].direction*4);

        }
        #endregion

        #region Scenarios

        #region Mutual Scenario
        for (int i = 0; i < 8; i++)
        {
            Vector3 checker;
            if (_hits[i].collider != null)
            {
                if (_hits[i].collider.gameObject.CompareTag("Obstacle"))
                {
                    checker = _hits[i].collider.transform.localPosition - transform.localPosition;
                    transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
                    #region Unmutual Scenarios

                    #region Front Scenario
                    if (i == 0)
                    {
                        showChecker = checker;
                        if (Mathf.Abs(checker.z) <= 3.5f)
                        {
                            _speed = _carPreferences.Speed * 0.3f;
                        }
                    }
                    #endregion

                    #region Left And Right Scenarios

                    if (i == 1)//Right
                    {
                        if (Mathf.Abs(checker.x) > 2.5f && Mathf.Abs(checker.x) <= 4f)
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed * 0.6f * -1;
                        }
                        if (Mathf.Abs(checker.x) <= 2.5f)
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed * 1.8f * -1;
                        }
                    }
                    if (i == 3)//Left
                    {
                        showChecker = checker;
                        if (Mathf.Abs(checker.x) > 2.5f && Mathf.Abs(checker.x) <= 3f)
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed * 0.6f;
                        }
                        if (Mathf.Abs(checker.x) <= 2.5f)
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed * 1.8f;
                        }
                    }

                    #endregion

                    #region Front Right And Front Left Scenarios

                    if (i == 4)//Front Right
                    {
                        if (Mathf.Abs(checker.x) > 1.5f && Mathf.Abs(checker.x) <= 3f)
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed * 0.6f;
                        }
                        if (Mathf.Abs(checker.x) <= 1.5f)
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed * 2.4f;
                        }
                    }
                    if (i == 5)//Front Left
                    {
                        if (Mathf.Abs(checker.x) > 1.5f && Mathf.Abs(checker.x) <= 3f)
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed * 0.6f * -1;
                        }
                        if (Mathf.Abs(checker.x) <= 1.5f)
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed * 2.4f * -1;
                        }
                    }

                    #endregion

                    #endregion
                }
            }
            else
            {
                foreach (RaycastHit hit in _hits)
                {
                    checker = Vector3.zero;
                    if (i == 0)
                    {
                        _rotationSpeed = 0;
                    }
                }
            }

        }
        #endregion

        #endregion
    }
}
