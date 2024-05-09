using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[SelectionBase]
public class EnemigoIA : EntidadesIA
{
    // Navmesh
    protected NavMeshAgent agente;
    Transform destino;

    //Vida
    [Header("Variables Enemigo IA")]
    public float velocidad;
    public float vida;
    float vidaMax;

    // Puntos que devuelve al ser derrotado
    public float dineroADevolver;
    public float puntosComidaARobar;

    //Barra de vida
    [SerializeField] BarraVida barraDeVida;

    //Condiciones
    public bool _destinoCompletado;
    //public bool test;

    //Valores parámetros
    [Tooltip("Dadas unas unidades de metros, la IA parará antes del destino")]
    public float distanciaAParar;
    [Tooltip("Dadas unas unidades de metros, el gnomo desaparecerá del mapa tras alejarse del destino")]
    public float distanciaDespawn;

    /*
    [Header("Destinos")]
    [Tooltip("La IA escoge el destino más cercano entre los mismos tipos de destinos. En este caso" +
        " se escogerá el granero más cercano")]
    public GameObject[] listaGraneros;
    */

    // Start is called before the first frame update
    private void Awake()
    {

    }

    public void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        destino = obtenerPosGraneroMasCercano();
        agente.destination = destino.position;
        agente.stoppingDistance = distanciaAParar;
        vidaMax = vida;
        agente.speed = velocidad;
        barraDeVida.setVida(vida);
        _destinoCompletado = false;
    }
    
    public void takeDamage(float damage)
    {
        vida -= damage;
        barraDeVida.actualizarBarraDeVida(vida, vidaMax);
        if (vida <= 0)
        {
            morir();
        }
    }

    void morir()
    {
        GameManager.Instance.aniadirDinero(dineroADevolver);
        GameManager.Instance.aniadirMuertes();
        Destroy(this.gameObject);
    }

    void despawn()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        checkPathComplete();
        checkMissionCompleted();
    }

    // Si el objetivo ha sido cumplido, hacer despawn tras haberse alejado de la granja
    private void checkMissionCompleted()
    {
        if (_destinoCompletado && Vector3.Distance(transform.position, 
            SpawnManager.Instance.centroMundo.position) > distanciaDespawn)
        {
            despawn();
        }
    }

    // Si se ha completado el destino principal, hacer que se marche y se aleje del mapa
    private void checkPathComplete()
    {
        if (!_destinoCompletado && !agente.pathPending && agente.remainingDistance < distanciaAParar)
        {
            float distanciaCentroMundo = Vector3.Distance(transform.position, 
                SpawnManager.Instance.centroMundo.position);

            _destinoCompletado = true;
            GameManager.Instance.quitarDinero(puntosComidaARobar);
            
            //destino.gameObject.GetComponent<GraneroEntity>().almacenamiento -= puntosComidaARobar;
            dineroADevolver += puntosComidaARobar;
            // Roba comida y esos puntos de comida se añaden a los puntos que devuelve al morir

            /*Vector3 dirToGoal = transform.position
                - (transform.forward * 
                (distanciaDespawn - distanciaCentroMundo + 50f));*/
            Vector3 dirToGoal = /*transform.position -*/
                SpawnManager.Instance.puntoAleatorioEnAnillo(
                    SpawnManager.Instance.centroMundo.position,
                    SpawnManager.Instance.distanciaMinimaSpawn + 20f,
                    SpawnManager.Instance.radio + 20f);
            // Una vez complete el destino irá a un punto aleatorio del anillo que comprende el spawn
            // de enemigos, y al irse a esta zona desaparecerá

            Vector3 newPos = /*transform.position + */dirToGoal;
            
            //Importante: hay que tener en cuenta que si el destino está fuera de la zona del navmesh
            // el agente no funcionará correctamente y se parará por el camino sin desaparecer
            newPos = new Vector3(newPos.x, destino.position.y ,newPos.z);
            
            agente.SetDestination(newPos);

        }
        /*if (test)
        {
            Debug.Log("TEST : " + (_agente.remainingDistance < distanciaAParar) + _agente.pathPending);
        }*/
    }

    public float getVidaMax()
    {
        return vidaMax;
    }

    public BarraVida getBarraDeVida()
    {
        return barraDeVida;
    }
}
