using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CamaraManager : MonoBehaviour
{
    public CinemachineVirtualCamera camara;
    public Camera camaraPrincipal;
    public bool cambiandoRotacionCamara;

    PlayerInput playerInput;
    [Range(0.1f, 1f)]
    public float sensibilidadMovimiento;
    [Range(0f, 100f)]
    public float velocidad;
    public float velocidadMaxima;

    public static CamaraManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cambiandoRotacionCamara = false;
    }

    // Update is called once per frame
    void Update()
    {
        camara.transform.position = camaraPrincipal.transform.position;
        // Se "ancla" la posición de la cámara principal con la de la cámara del cinemachine,
        // se evita así que pueda atravesar el terreno usando el CinemachineCollider
        
        if (cambiandoRotacionCamara)
        {
            Vector2 movGeneral = playerInput.actions["RotacionCamara"].ReadValue<Vector2>();
            float movX = movGeneral.x * sensibilidadMovimiento; //Input.GetAxis("Mouse X");
            float movY = movGeneral.y * sensibilidadMovimiento; //Input.GetAxis("Mouse Y");

            Vector3 rotate = new Vector3(-movY, movX, 0);
            camara.transform.eulerAngles = camara.transform.eulerAngles + rotate;
        }
        leerMovimientoCamara();
        leerVelocidadCamara();
    }

    void leerMovimientoCamara()
    {
        Vector2 movH = playerInput.actions["MovimientoHorizontalCam"].ReadValue<Vector2>();
        Vector2 movV = playerInput.actions["MovimientoVerticalCam"].ReadValue<Vector2>();
        if (movH != Vector2.zero)
        {
            camara.transform.Translate(new Vector3(movH.x, 0, movH.y) * velocidad * Time.deltaTime);
        }
        if (movV != Vector2.zero)
        {
            camara.transform.Translate(new Vector3(0, movV.y, 0) * velocidad * Time.deltaTime);
        }
    }

    void leerVelocidadCamara()
    {
        float vRueda = playerInput.actions["CambiarVelocidadCamara"].ReadValue<float>()/120f;
        if(vRueda != 0)
        {
            if (!((velocidad <= 0 && vRueda < 0 || velocidad >= velocidadMaxima && vRueda > 0)))
            { // Si es menor o igual a 0 o mayor o igual a 100 deja de sumar velocidad
                velocidad += vRueda;
            }
        }

        float vMandoAxis = playerInput.actions["CambiarVelocidadCamaraMando"].ReadValue<Vector2>().y/40;
        if (vMandoAxis != 0)
        {
            if (!((velocidad <= 0 && vMandoAxis < 0 || velocidad >= velocidadMaxima && vMandoAxis > 0)))
            { // Si es menor o igual a 0 o mayor o igual a 100 deja de sumar velocidad
                velocidad += vMandoAxis;
            }
        }

    }

    public void activarRotacionCamara(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Current Action Map: " + playerInput.currentActionMap.ToString());
            cambiandoRotacionCamara = true;
        }
        if (context.canceled)
        {
            cambiandoRotacionCamara = false;
        }
    }

    public void cambiarAModoColocarObj(bool value)
    {
        cambiandoRotacionCamara = !value;
    }
}
