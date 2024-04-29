using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GnomoNormalIA : GnomoIA
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

    //Barra de Stamina
    public float stamina;
    private float _staminaMax;
    public float rapidezRecuperacion;
    public float gastoStamina;
    public float gastoStaminaCorriendo;
    [SerializeField] BarraVida barraDeStamina;


    //Condiciones
    bool agotado;
    bool puedeAtacar;
    bool enHuerta;
    public bool test;

    //Objetos
    public GameObject huerta;

    // Start is called before the first frame update

    //int mascara = 1 << 6;
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        puedeAtacar = true;
        temporizador = 0.1f;
        //stamina = 100f;
        _staminaMax = stamina;
        barraDeStamina.setVida(stamina);
        agotado = false;
        velocidad = agente.speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Temporizador para hacer da�o cada x tiempo
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

        // Esta lista almacenar� el resultado de llamar a OverlapSphere
        Collider[] listaChoques;

        listaChoques = Physics.OverlapSphere(transform.position, radio, mascara);

        // se obtiene el enemigo m�s cercano al gnomo
        Transform enemigoMasCercano = obtenerPosEnemigoMasCercano(listaChoques);

        //Si no est� agotado y hay enemigos en el radio de acci�n se va a perseguirlo
        if (!agotado)
        {
            if (enemigoMasCercano != null)
            {
                agente.speed = velocidad;
                agente
                    .SetDestination(enemigoMasCercano.GetComponent<EnemigoIA>().transform.position);
            }

            // Mientras lo persigue se va agotando ligeramente
            if (agente.speed > 0)
            {
                
                stamina -= gastoStaminaCorriendo * agente.speed / agente.speed;
            }
        }

        // Si est� agotado se va a la huerta a reponer energ�as
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
        if (other.gameObject.tag == "Enemigo")
        {
            EnemigoIA enemigo = other.gameObject.GetComponent<EnemigoIA>();
                if (!agotado && puedeAtacar)
                {
                    enemigo.takeDamage(danio);
                    //Debug.Log("aTAQUE");
                    puedeAtacar = false;
                }
            stamina -= gastoStamina;
            barraDeStamina.actualizarBarraDeVida(stamina);
            //if (test) { Debug.Log("ACTUALIZADO " + stamina); }

            if (stamina <= 0)
            {
                agotado = true;
                stamina = 0f;
                agente.speed = velocidad / 2;
            }
        }

        if (other.gameObject.tag == "Huerta" && agotado)
        {
            if (!enHuerta)
            {
                agente.speed = velocidad / 4;
            }
            enHuerta = true;
            
            if (stamina < 100f)
            {
                stamina += rapidezRecuperacion;
                barraDeStamina.actualizarBarraDeVida(stamina, _staminaMax);
            }
            else
            {
                agotado = false;
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
