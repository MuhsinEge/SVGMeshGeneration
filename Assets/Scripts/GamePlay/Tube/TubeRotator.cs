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
    private float _lastTouchedPositionY;
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
            float touchedPositionY = Input.mousePosition.y;

            if (_isTouched == false)
            {
                _lastTouchedPositionX = touchedPositionX;
                _lastTouchedPositionY = touchedPositionY;
                _isTouched = true;
                return;
            }

            _isTouched = true;
            var dragDistanceX = touchedPositionX - _lastTouchedPositionX;
            var dragDistanceY = touchedPositionY - _lastTouchedPositionY;

            var dragDistance = Mathf.Abs(dragDistanceX) > Mathf.Abs(dragDistanceY) ? dragDistanceX : dragDistanceY;

            if (Mathf.Abs(dragDistance) > dragThreshold)
            {
                var rotationModifier = Mathf.Clamp(dragDistance * sensitivity * Time.deltaTime, -2.5f, 2.5f);
                _rotationAmount += rotationModifier;
                _lastTouchedPositionY = touchedPositionY;
                _lastTouchedPositionX = touchedPositionX;
            }
            else
            {
                _lastTouchedPositionY = touchedPositionY;
                _lastTouchedPositionX = touchedPositionX;
            }
        }
        else
        {
            _isTouched = false;
            _lastTouchedPositionY = 0f;
            _lastTouchedPositionX = 0f;
        }
    }
}
