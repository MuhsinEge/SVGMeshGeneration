using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TubeRotator : MonoBehaviour
{
    public float dragThreshold = 3f;
    public float sensitivity = 1f;
    public float rotationSmoothTime = 0.1f; // Smoothing time for rotation
    Rigidbody _rigidBody;
    private bool _isTouched;
    private float _lastTouchedPositionX;
    private float _rotationAmount;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isTouched)
        {
            if (_rotationAmount == 0) return;
            Quaternion targetRotation = Quaternion.AngleAxis(_rotationAmount * sensitivity, Vector3.forward);
            _rigidBody.MoveRotation(Quaternion.Slerp(_rigidBody.rotation, targetRotation, rotationSmoothTime));
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float touchedPositionX = Input.mousePosition.x;

            if (_isTouched == false)
            {
                _lastTouchedPositionX = touchedPositionX;
                _isTouched = true;
                return;
            }

            _isTouched = true;
            var dragDistance = touchedPositionX - _lastTouchedPositionX;

            if (Mathf.Abs(dragDistance) > dragThreshold)
            {
                var rotationModifier = Mathf.Clamp(dragDistance * sensitivity * Time.deltaTime, -2.5f, 2.5f);
                _rotationAmount += rotationModifier;

                _lastTouchedPositionX = touchedPositionX;
            }
            else
            {
                
                _lastTouchedPositionX = touchedPositionX;
            }
        }
        else
        {
            _isTouched = false;
            _lastTouchedPositionX = 0f;
        }
    }
}
