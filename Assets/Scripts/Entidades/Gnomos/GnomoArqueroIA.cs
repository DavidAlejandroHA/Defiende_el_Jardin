using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GnomoArqueroIA : GnomoIA
{
    // Navmesh
    /*private NavMeshAgent _agente;
    private float velocidad;

    // Variables
    public float radio;
    public bool mostrarAreaDeAccion;
    public float danio;
    public float cooldown;
    [SerializeField] private float temporizador;
    */

    //Barra de Municion
    public int municion;
    private int _municionMax;
    public float rapidezRecuperacion;
    //public float gastoStamina;
    //public float gastoStaminaCorriendo;
    [SerializeField] BarraVida barraDeMunicion;
    
    //Condiciones
    bool municionAgotada;
    bool puedeAtacar;
    bool enHuerta;

    //Objetos
    public GameObject huerta;

    // Start is called before the first frame update

    //int mascara = 1 << 6;
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        puedeAtacar = true;
        temporizador = 0.1f;
        _municionMax = municion;
        barraDeMunicion.setVida(municion);
        municionAgotada = false;
        velocidad = agente.speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Temporizador para hacer daño cada x tiempo
        temporizador -= Time.deltaTime;
        if (temporizador <= 0)
        {
            temporizador = cooldown;
        }

        // Se dispara un proyectil cada vez que se reinicia el temporizador
        if (temporizador == cooldown)
        {
            puedeAtacar = true;
        }

        // Esta lista almacenará el resultado de llamar a OverlapSphere
        Collider[] listaChoques;

        listaChoques = Physics.OverlapSphere(transform.position, radio, mascara);

        // se obtiene el enemigo más cercano al gnomo
        Transform enemigoMasCercano = obtenerPosEnemigoMasCercano(listaChoques);

        //Si no está agotado y hay enemigos en el radio de acción se va a perseguirlo
        if (!municionAgotada)
        {
            if (enemigoMasCercano != null)
            {
                agente.speed = velocidad;
                agente
                    .SetDestination(enemigoMasCercano.GetComponent<EnemigoIA>().transform.position);
            }
        }

        // Si está agotado se va a la huerta a reponer energías
        else if (agente.destination != huerta.transform.position)
        {
            agente
                    .SetDestination(huerta.transform.position);
        }
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

    private void OnCollisionStay(Collision collision)
    {
        
    }

    // Cuando hace contacto continuamente con el enemigo empieza a golpearle por turnos
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Huerta" && municionAgotada)
        {
            if (!enHuerta)
            {
                agente.speed = velocidad / 4;
            }
            enHuerta = true;

            float progresoSumado = barraDeMunicion.sumarVida(rapidezRecuperacion);

            if (progresoSumado < _municionMax)
            {
                
            }
            else
            {
                barraDeMunicion.actualizarBarraDeVida(_municionMax);
                // Ajusta al 100% exactamente en caso de poder llegado a haber un exceso puntual
                municion = _municionMax;
                municionAgotada = false;
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
