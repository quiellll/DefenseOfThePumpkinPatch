using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxDistanceFromOrigin;

    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minYPos;
    [SerializeField] private float _maxYPos;

    [SerializeField] private float _rotationSpeed;

    private int _yInput = 0;
    private Vector2 _xzInput = Vector2.zero;
    private int _rotationInput = 0;
    private Transform _cam;

    private void Awake()
    {
        _cam = GetComponentInChildren<Camera>().transform;
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        _yInput = Mathf.Clamp(Mathf.RoundToInt(context.ReadValue<Vector2>().y), -1, 1);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _xzInput = context.ReadValue<Vector2>().normalized;
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        _rotationInput = Mathf.RoundToInt(context.ReadValue<float>());
    }

    private void Update()
    {
        Vector3 newPos = transform.position;

        //movement
        newPos += 
            transform.right * _xzInput.x * _moveSpeed * Time.deltaTime + 
            transform.forward * _xzInput.y * _moveSpeed * Time.deltaTime;
        newPos.y = 0;
        newPos = Vector3.ClampMagnitude(newPos, _maxDistanceFromOrigin);
        
        transform.position = newPos;

        //zoom
        newPos = _cam.position;
        newPos.y = 
            Mathf.Clamp(_cam.position.y - _yInput * _zoomSpeed * Time.deltaTime, _minYPos, _maxYPos);

        _cam.position = newPos;

        if (_xzInput.sqrMagnitude > 0) return;


        //rotation
        Vector3 newRot = transform.rotation.eulerAngles;
        newRot.y -= _rotationInput * _rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(newRot);


    }

}
