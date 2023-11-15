using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TubeRotator :MonoBehaviour
{
    public float dragThreshold = 3f;
    public float sensitivity = 1f;
    Rigidbody _rigidBody;
    private bool _isTouched;
    private float _lastTouchedPositionX;
    private float _rotationAmount;


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!_isTouched)
        {
            return;
        }
        Quaternion q = Quaternion.AngleAxis(_rotationAmount * sensitivity, Vector3.forward);
        _rigidBody.MoveRotation(_rigidBody.transform.rotation * q);

        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _isTouched = true;
            float touchedPositionX = Input.mousePosition.x;
            if (_lastTouchedPositionX == 0f)
            {
                _lastTouchedPositionX = touchedPositionX;
                return;
            }
            else
            {
                var dragDistance = touchedPositionX - _lastTouchedPositionX;
                if(Mathf.Abs(dragDistance) > dragThreshold)
                {
                    _rotationAmount = Mathf.MoveTowards(_rotationAmount, dragDistance, Time.deltaTime * 20);
                    _lastTouchedPositionX = touchedPositionX;
                }
                else
                {
                    _rotationAmount = Mathf.MoveTowards(_rotationAmount, 0, Time.deltaTime * 100f);
                    _lastTouchedPositionX = touchedPositionX;
                }
            }
        }
        else
        {
            _isTouched = false;
            _rotationAmount = Mathf.MoveTowards(_rotationAmount, 0, Time.deltaTime * 100f);
        }
    }
}
