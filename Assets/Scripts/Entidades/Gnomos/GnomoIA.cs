using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GnomoIA : MonoBehaviour
{
    // Navmesh
    //[Header("NavMesh")]
    protected NavMeshAgent agente;
    protected float velocidad;

    // Variables
    [Header("Variables Gnomo IA")]
    public float radio;
    public bool mostrarAreaDeAccion;
    //public float danio;
    public float cooldown;
    /*[SerializeField]*/ public float temporizador;

    // Start is called before the first frame update

    protected int mascara = 1 << 6;
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    protected Transform obtenerPosEnemigoMasCercano(Collider[] listaChoques)
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

    protected bool comprobarQueNoHayObstaculos(Transform enemigoMasCercano)
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
}
