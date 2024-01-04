using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    //parametros movimiento camara
    [SerializeField] private float _moveSpeed; //velocidad movimiento (WASD)
    [SerializeField] private float _maxDistanceFromOrigin; //max distancia del (0,0,0) a la que se puede mover

    [SerializeField] private float _zoomSpeed; //velocidad de zoom (rueda raton)
    [SerializeField] private float _minYPos; //lo mas abajo que llega con el zoom
    [SerializeField] private float _maxYPos; //lo mas arriba que llega con el zoom

    [SerializeField] private float _rotationSpeed; //velocidd de rotacion (QE)

    private int _yInput = 0;
    private Vector2 _xzInput = Vector2.zero;
    private int _rotationInput = 0;
    private Transform _cam;

    private void Awake()
    {//la camara como tal es el hijo del objeto de este script (Camera Axis)
        _cam = GetComponentInChildren<Camera>().transform; 
    }

    public void Zoom(InputAction.CallbackContext context)
    {//evento cuando se mueve la rueda del raton, se guarda en yInput como int, puede ser -1, 0 o 1
        _yInput = Mathf.Clamp(Mathf.RoundToInt(context.ReadValue<Vector2>().y), -1, 1);
    }

    public void Move(InputAction.CallbackContext context)
    {//evento de pulsar WASD, se guarda el vector de input normalizado
        _xzInput = context.ReadValue<Vector2>().normalized;
    }

    public void Rotate(InputAction.CallbackContext context)
    {//evento de pulsar QE, se guarda el valor -1, 0 o 1
        _rotationInput = Mathf.RoundToInt(context.ReadValue<float>());
    }

    private void Update()
    {
        float unscaledDt = Time.deltaTime / GameManager.Instance.TimeScale;

        Vector3 newPos = transform.position;

        //movimiento con el input (del padre, no de la camara sola)
        newPos += 
            transform.right * _xzInput.x * _moveSpeed * unscaledDt + 
            transform.forward * _xzInput.y * _moveSpeed * unscaledDt;
        newPos.y = 0; //se cambia la y de la nueva pos a 0 para calcular la distancia a la que esta del origen
        newPos = Vector3.ClampMagnitude(newPos, _maxDistanceFromOrigin); //para evitar salirse de la max distancia
        
        transform.position = newPos;

        //zoom con el input (en el zoom se mueve la camara en vez del padre)

        //se clampea para evitar salirse de los limites del zoom
        //newPos.y = Mathf.Clamp(_cam.position.y - _yInput * _zoomSpeed * unscaledDt, _minYPos, _maxYPos);

        newPos = _cam.position + _cam.forward * _yInput * _zoomSpeed * unscaledDt;
        if(newPos.y > _minYPos && newPos.y < _maxYPos) _cam.position = newPos;

        //si hay input de movimiento, no se rota la camara (para evitar transformaciones raras)
        if (_xzInput.sqrMagnitude > 0) return; 


        //rotacion (del padre, no de la camara, para que la camara rote pero sobre el eje del (0,0,0)
        Vector3 newRot = transform.rotation.eulerAngles;
        newRot.y -= _rotationInput * _rotationSpeed * unscaledDt;

        transform.rotation = Quaternion.Euler(newRot);

    }
}
