using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AI_CarController : MonoBehaviour
{
    public SO_Car _carPreferences;

    private int _rayCount = 5;
    [SerializeField] private int _activeScenario;

    private float _speed, _rotationSpeed;
    private float _horizontalMultiplier,_verticalMultiplier;
    private float[] _objectDeltaMagnitudes;

    private Vector3[] _objectDeltas;
 
    private Ray[] _rays;
    private RaycastHit[] _hits;


    private void Start()
    {
        _speed = _carPreferences.Speed;
        
        _rays = new Ray[_rayCount];
        _hits = new RaycastHit[_rayCount];
        _objectDeltas = new Vector3[_rayCount];
        _objectDeltaMagnitudes = new float[_rayCount];
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
        for (int i = 0; i < _rayCount; i++)
        {
            Physics.Raycast(_rays[i], out _hits[i], 4f);
        }

        #region Ray Directions
        for (int i = 0; i < _rayCount; i++)
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
            if (i == 4)//Front Left
            {
                _verticalMultiplier = 1;
                _horizontalMultiplier = -1;
            }
            _rays[i] = new Ray(transform.position, (transform.forward * _verticalMultiplier + transform.right * _horizontalMultiplier).normalized);

            if (i == _activeScenario)
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
        for (int i = 0; i < _rayCount; i++)
        {
            _objectDeltaMagnitudes[i] = _objectDeltas[i].magnitude;
            if (_objectDeltaMagnitudes[i] == Mathf.Min(_objectDeltaMagnitudes))
            {
                _activeScenario = i;
            }
            if (_objectDeltaMagnitudes[_activeScenario] == float.NaN)
            {
                _activeScenario = Random.Range(0, 6);
            }
            if (_hits[i].collider != null)
            {
                if (_hits[i].collider.gameObject.CompareTag("Obstacle"))
                {
                    _objectDeltas[i] = _hits[i].collider.transform.localPosition - transform.localPosition;
                    transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
                    #region Unmutual Scenarios

                    if (i == _activeScenario)
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
                            _rotationSpeed = _carPreferences.RotationSpeed / (Mathf.Pow(_objectDeltaMagnitudes[i],2) * m1);
                            
                        }
                        #endregion

                        #region Left And Right Scenarios

                        if (i == 1)//Right
                        {
                            Debug.Log("Current Scenario is Right");
                            _rotationSpeed = _carPreferences.RotationSpeed / (Mathf.Pow(_objectDeltaMagnitudes[i], 2) * -1);
                        }
                        if (i == 2)//Left
                        {
                            Debug.Log("Current Scenario is Left");
                            _rotationSpeed = _carPreferences.RotationSpeed / Mathf.Pow(_objectDeltaMagnitudes[i], 2);
                        }

                        #endregion

                        #region Front Right And Front Left Scenarios

                        if (i == 3)//Front Right
                        {
                            Debug.Log("Current Scenario is Front Right");
                            _rotationSpeed = _carPreferences.RotationSpeed / (Mathf.Pow(_objectDeltaMagnitudes[i], 2) * -1);
                        }
                        if (i == 4)//Front Left
                        {
                            Debug.Log("Current Scenario is Front Left");
                            _rotationSpeed = _carPreferences.RotationSpeed / Mathf.Pow(_objectDeltaMagnitudes[i], 2);
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {
                    _objectDeltas[i] = new Vector3(float.NaN, float.NaN, float.NaN);
                }
            }
            else
            {
                _objectDeltas[i] = new Vector3(float.NaN, float.NaN, float.NaN);
            }
        }
        #endregion

        #endregion

        
    }
}
