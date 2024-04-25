using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GnomoIA : MonoBehaviour
{
    // Navmesh
    private NavMeshAgent _agente;
    private float velocidad;

    // Variables
    public float radio;
    public bool mostrarAreaDeAccion;
    public float danio;
    public float cooldown;
    [SerializeField] private float temporizador;
    

    //Barra de Stamina
    public float stamina;
    private float _staminaMax;
    public float rapidezRecuperacion;
    public float gastoStamina;
    [SerializeField] BarraVida barraDeStamina;


    //Condiciones
    bool agotado;
    bool puedeAtacar;
    bool enHuerta;

    //Objetos
    public GameObject huerta;

    // Start is called before the first frame update

    int mascara = 1 << 6;
    void Start()
    {
        _agente = GetComponent<NavMeshAgent>();
        puedeAtacar = true;
        temporizador = 0.1f;
        stamina = 100f;
        _staminaMax = stamina;
        agotado = false;
        velocidad = _agente.speed;
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
        if (!agotado)
        {
            if (enemigoMasCercano != null)
            {
                _agente.speed = velocidad;
                _agente
                    .SetDestination(enemigoMasCercano.GetComponent<EnemigoIA>().transform.position);
            }
        }
        // Si está agotado se va a la huerta a reponer energías
        else if (_agente.destination != huerta.transform.position)
        {
            _agente
                    .SetDestination(huerta.transform.position);
        }
    }

    Transform obtenerPosEnemigoMasCercano(Collider[] listaChoques)
    {
        Transform enemigoMasCercano = null;
        float menorDistancia = Mathf.Infinity;

        // Se comprueba y elige el enemigo con menor distancia
        if (listaChoques.Length > 0)
        {
            foreach (Collider choque in listaChoques)
            {
                float distanciaActual = Vector3.Distance(transform.position, choque.transform.position);
                if (distanciaActual < menorDistancia)
                {
                    /* Se detectan enemigos dentro del radio de acción pero hay que comprobar que
                     * no hay muros por delante*/
                    if (comprobarQueNoHayObstaculos(choque.transform))
                    {
                        menorDistancia = distanciaActual;
                        enemigoMasCercano = choque.transform;
                    }
                }
            }
        }
        return enemigoMasCercano; // puede llegar a ser nulo si no hay nada al rededor, hay que                    
    }                               // tenerlo en cuenta

    private bool comprobarQueNoHayObstaculos(Transform enemigoMasCercano)
    {
        bool enemigoVisible = true;
        if (enemigoMasCercano != null)
        {
            /* Primero intenté esta parte con raycastall pero al final solo funciono con un linecast, 
             * lo dejo como nota por si acaso*/

            RaycastHit hit;
            if (Physics.Linecast(transform.position, enemigoMasCercano.transform.position, out hit))
            {
                if (hit.transform.tag != "Proyectil" && hit.collider.gameObject.tag != "Enemigo")
                {
                    enemigoVisible = false;
                }
            }
        }

        return enemigoVisible;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemigo")
        {
            EnemigoIA enemigo = other.gameObject.GetComponent<EnemigoIA>();
                if (!agotado && puedeAtacar)
                {
                    enemigo.takeDamage(danio);
                    Debug.Log("aTAQUE");
                    puedeAtacar = false;
                }
            stamina -= gastoStamina;
            barraDeStamina.actualizarBarraDeVida(stamina, _staminaMax);

            if (stamina <= 0)
            {
                agotado = true;
                stamina = 0f;
                _agente.speed = velocidad / 2;
            }
        }

        if (other.gameObject.tag == "Huerta")
        {
            if (!enHuerta)
            {
                _agente.speed = _agente.speed / 4;
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Huerta")
        {
            enHuerta = false;
        }
    }
}
