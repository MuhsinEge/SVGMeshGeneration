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
    private float _smoothRotationVelocity; // Velocity for smoothing rotation

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isTouched)
        {
            if (_rotationAmount == 0) return;
            // Calculate the target rotation based on the accumulated rotation amount
            Quaternion targetRotation = Quaternion.AngleAxis(_rotationAmount * sensitivity, Vector3.forward);

            // Smoothly interpolate the current rotation to the target rotation
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
                // Accumulate the rotation amount based on the mouse movement
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
            // Smoothly decay the rotation amount when the mouse button is release
            _lastTouchedPositionX = 0f; // Reset to avoid sudden rotation when the mouse is pressed again
        }
    }
}
