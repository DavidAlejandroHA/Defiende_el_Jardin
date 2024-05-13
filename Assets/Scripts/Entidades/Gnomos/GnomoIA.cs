using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[SelectionBase]
public class GnomoIA : EntidadesIA
{
    // Navmesh
    //[Header("NavMesh")]
    protected NavMeshAgent agente;

    // Variables
    [Header("Variables Gnomo IA")]
    public float velocidad;
    public float radio;
    public bool mostrarAreaDeAccion;
    //public float danio;
    public float cooldown;
    /*[SerializeField]*/ protected float temporizador;

    public Animator animatorController;

    /*
    [Header("Destinos")]
    [Tooltip("La IA escoge el destino más cercano entre los mismos tipos de destinos. En este caso" +
        " se escogerá la huerta más cercana")]
    public GameObject[] listaHuertas;
    */

    // Start is called before the first frame update

    protected int mascara = 1 << 6;
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        animatorController.SetFloat("Velocidad", (agente.velocity.magnitude / agente.speed));
    }
    
    protected Transform obtenerPosHuertaMasCercana()
    {
        //return obtenerPosGameObjMasCercano(listaHuertas);

        return obtenerPosGameObjMasCercano("Huerta");
        // puede llegar a ser nulo si no hay nada al rededor, hay que tenerlo en cuenta               
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
            if (Physics.Linecast(transform.position, enemigoMasCercano.position, out hit))
            {
                if (hit.transform.tag != "Proyectil" && hit.collider.gameObject.tag != "Enemigo"
                    && hit.transform.tag != "Gnomo")
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
