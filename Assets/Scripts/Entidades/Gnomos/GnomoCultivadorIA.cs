using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GnomoCultivadorIA : GnomoIA
{
    //Barra de Stamina
    [Header("Stats de Cosecha")]
    public float capacidadCosecha;
    private float _cosechaMax;
    public float rapidezCosecha;
    [SerializeField] BarraVida barraDeCosecha;

    //Condiciones
    bool necesitaCultivar;
    bool enHuerta;
    //public bool test;
    
    //Objetos
    /*[Header("Destinos")]
    public GameObject[] listaHuertas;*/
    //GameObject[] huerta;

    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        temporizador = 0f;
        //stamina = 100f;
        _cosechaMax = capacidadCosecha;
        capacidadCosecha = 0f;
        barraDeCosecha.setVida(capacidadCosecha);
        necesitaCultivar = true;
        agente.speed = velocidad;
        agente.SetDestination(obtenerPosHuertaMasCercana().position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (mostrarAreaDeAccion)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radio);
            //Gizmos.DrawRay(transform.position, transform.forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Granero" && !necesitaCultivar)
        {
            GameManager.Instance.aniadirDinero(capacidadCosecha);
            capacidadCosecha = 0f;
            necesitaCultivar = true;
            agente.destination = obtenerPosHuertaMasCercana().position;
        }
    }

    // Cuando hace contacto continuamente con el enemigo empieza a golpearle por turnos
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Huerta" && necesitaCultivar)
        {
            if (!enHuerta)
            {
                agente.speed = velocidad / 4;
            }
            enHuerta = true;
            
            if (capacidadCosecha < _cosechaMax)
            {
                capacidadCosecha += rapidezCosecha;
                barraDeCosecha.actualizarBarraDeVida(capacidadCosecha, _cosechaMax);
            }
            else
            {
                capacidadCosecha = _cosechaMax;
                necesitaCultivar = false;
                agente.SetDestination(obtenerPosGraneroMasCercano().position);
                agente.speed = velocidad;
            }
        }
    }

    /* Al salir de la huerta se marca un booleano para que posteriormente se vuelva a usar
     a la hora de reducir la velocidad al entrar en la huerta */
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Huerta")
        {
            enHuerta = false;
        }
    }
}
