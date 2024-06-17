using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GlobalUIManager : MonoBehaviour
{
    public GameObject botonPorDefecto;
    public PlayerInput playerInput;

    public bool seleccionarUIBotones = false;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        seleccionarUIBotones = false;
    }

    public static GlobalUIManager Instance { get; private set; }

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

    // Update is called once per frame
    void Update()
    {

    }

    public void activarCambioFocus(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Current Action Map: " + playerInput.currentActionMap.ToString());
            if (!seleccionarUIBotones)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(botonPorDefecto);
            }
            else
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
            seleccionarUIBotones = !seleccionarUIBotones;
        }
    }

    public void cargarEscena(string nombre)
    {

        SceneManager.LoadScene(nombre);
    }

    public void cerrarAplicacion()
    {
        Application.Quit();
    }
}
