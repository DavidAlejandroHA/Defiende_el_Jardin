using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GnomoIA : MonoBehaviour
{
    // Navmesh
    private NavMeshAgent _agente;
    private Transform destino;

    // Variables
    public float radio;
    public float danio;
    public float cooldown;
    private float temporizador;
    bool puedeAtacar;
    //Barra de municion
    //[SerializeField] GameObject barraDeVidaObj;
    //[SerializeField] BarraVida barraDeVida;
    // Start is called before the first frame update

    int mascara = 1 << 6;
    void Start()
    {
        _agente = GetComponent<NavMeshAgent>();
        puedeAtacar = true;
        temporizador = 0.1f;
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

        //Si hay enemigos en el radio de acción se va a perseguirlo
        if (enemigoMasCercano != null)
        {
            _agente
                .SetDestination(enemigoMasCercano.GetComponent<EnemigoIA>().transform.position);
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
        Gizmos.color = new Color(255, 192, 203); // Color rosa
        Gizmos.DrawWireSphere(transform.position, radio);
        //Gizmos.DrawRay(transform.position, transform.forward);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (puedeAtacar && collision.gameObject.tag == "Enemigo")
        {
            collision.gameObject.GetComponent<EnemigoIA>().takeDamage(danio);
            Debug.Log("aTAQUE");
            puedeAtacar = false;
        }
    }
}
