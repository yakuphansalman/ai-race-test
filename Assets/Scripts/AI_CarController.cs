using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_CarController : MonoBehaviour
{
    public SO_Car _carPreferences;

    private float _speed, _rotationSpeed;
    private float _horizontalMultiplier,_verticalMultiplier;
    private float[] _objectDeltaMagnitudes;

    public Vector3 showChecker;
    private Vector3 _activeDelta;
    private Vector3[] _objectDeltas;
 
    private Ray[] _rays;
    private RaycastHit[] _hits;


    private void Start()
    {
        _speed = _carPreferences.Speed;
        
        _rays = new Ray[5];
        _hits = new RaycastHit[5];
        _objectDeltas = new Vector3[5];
        _objectDeltaMagnitudes = new float[5];
    }

    private void Update()
    {

        Move();

        CastRays();

        CheckObstacles();
    }
    private void Move()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }
    private void CastRays()
    {
        for (int i = 0; i < 5; i++)
        {
            Physics.Raycast(_rays[i], out _hits[i], 4f);
        }

        #region Ray Directions
        for (int i = 0; i < 5; i++)
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
            if (i == 2)//Left
            {
                _verticalMultiplier = 0;
                _horizontalMultiplier = -1;
            }
            if (i == 3)//Front Right
            {
                _verticalMultiplier = 1;
                _horizontalMultiplier = 1;
            }
            if (i == 4)
            {
                _verticalMultiplier = 1;
                _horizontalMultiplier = -1;
            }
            _rays[i] = new Ray(transform.position, (transform.forward * _verticalMultiplier + transform.right * _horizontalMultiplier).normalized);
            if (_activeDelta == _objectDeltas[i])
            {
                Debug.DrawRay(_rays[i].origin, _rays[i].direction * 4, new Color(255, 0, 0));
            }
            else
            {
                Debug.DrawRay(_rays[i].origin, _rays[i].direction * 4, new Color(255, 255, 0));
            }

        }
        #endregion

    }
    private void CheckObstacles()
    {
        
        #region Scenarios

        #region Mutual Scenario
        for (int i = 0; i < 5; i++)
        {
            if (_hits[i].collider != null)
            {
                if (_hits[i].collider.gameObject.CompareTag("Obstacle"))
                {
                    _objectDeltas[i] = _hits[i].collider.transform.localPosition - transform.localPosition;
                    _objectDeltaMagnitudes[i] = _objectDeltas[i].magnitude;
                    if (Mathf.Min(_objectDeltaMagnitudes) == _objectDeltaMagnitudes[i])
                    {
                        _activeDelta = _objectDeltas[i];
                    }
                    transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
                    #region Unmutual Scenarios

                    if (_objectDeltas[i] == _activeDelta)
                    {
                        #region Front Scenario
                        if (i == 0)
                        {
                            int m1;
                            Debug.Log("Current Scenario is Front");
                            if (_objectDeltaMagnitudes[1] > _objectDeltaMagnitudes[2] || _objectDeltaMagnitudes[3] > _objectDeltaMagnitudes[4])// Check that right delta grater than left delta
                            {
                                m1 = 1;
                            }
                            else if(_objectDeltaMagnitudes[1] <= _objectDeltaMagnitudes[2] || _objectDeltaMagnitudes[3] <= _objectDeltaMagnitudes[4])
                            {
                                m1 = -1;
                            }
                            else
                            {
                                m1 = 1;
                            }
                            _speed = _carPreferences.Speed / _objectDeltaMagnitudes[i];
                            _rotationSpeed = _carPreferences.RotationSpeed / _objectDeltaMagnitudes[i] * m1;
                            
                        }
                        #endregion

                        #region Left And Right Scenarios

                        if (i == 1)//Right
                        {
                            Debug.Log("Current Scenario is Right");
                            _rotationSpeed = _carPreferences.RotationSpeed / _objectDeltaMagnitudes[i] * -1;
                        }
                        if (i == 2)//Left
                        {
                            Debug.Log("Current Scenario is Left");
                            _rotationSpeed = _carPreferences.RotationSpeed / _objectDeltaMagnitudes[i];
                        }

                        #endregion

                        #region Front Right And Front Left Scenarios

                        if (i == 3)//Front Right
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed / _objectDeltaMagnitudes[i] * -1;
                        }
                        if (i == 4)//Front Left
                        {
                            _rotationSpeed = _carPreferences.RotationSpeed / _objectDeltaMagnitudes[i];
                        }

                        #endregion
                    }

                    #endregion
                }
            }
            else
            {
                _objectDeltaMagnitudes[i] = Random.Range(7f, 10f);
                //Check when hit that chosen is none and set its object magnitude value as random float value 
                //The reason of doing this is the conditions at 121 and 125 has to be like normal values unlike 0f or 1000f
                //If the value is 0f , condition at 107 can not be defined
            }

        }
        #endregion

        #endregion

        
    }
}
