using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[SelectionBase]
public class EnemigoIA : MonoBehaviour
{
    // Navmesh
    private NavMeshAgent _agente;
    Transform destino;

    //Vida
    [Header("Variables Enemigo IA")]
    public float vida;
    float vidaMax;

    // Puntos que devuelve al ser derrotado
    public float dineroADevolver;

    //Barra de vida
    [SerializeField] BarraVida barraDeVida;

    //Condiciones
    private bool _destinoCompletado;
    public bool test;

    //Valores parámetros
    [Tooltip("Dadas unas unidades de metros, la IA parará antes del destino")]
    public float distanciaAParar;
    [Tooltip("Dadas unas unidades de metros, el gnomo desaparecerá del mapa tras alejarse del destino")]
    public float distanciaDespawn;

    [Header("Destinos")]
    [Tooltip("La IA escoge el destino más cercano entre los mismos tipos de destinos. En este caso" +
        " se escogerá el granero más cercano")]
    public GameObject[] listaGraneros;

    // Start is called before the first frame update
    private void Awake()
    {

    }

    public void Start()
    {
        _agente = GetComponent<NavMeshAgent>();
        destino = obtenerPosGraneroMasCercano(listaGraneros);
        _agente.destination = destino.position;
        _agente.stoppingDistance = distanciaAParar;
        vidaMax = vida;
        barraDeVida.setVida(vida);
        _destinoCompletado = false;
        //barraDeVida = barraDeVidaObj.GetComponent<BarraVida>();
        //barraDeVida.actualizarBarraDeVida(vida, vidaMax); // para asegurar que se actualiza
        // como es debido al volver a empezar
    }

    // 
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
        /*GameManager.Instance.aniadirDinero(dineroADevolver);
        GameManager.Instance.aniadirPuntos(dineroADevolver);
        GameManager.Instance.aniadirMuertes();*/
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
        if (_destinoCompletado && Vector3.Distance(transform.position, destino.transform.position) > distanciaDespawn)
        {
            morir();
        }
    }

    // Si se ha completado el destino principal, hacer que se marche y se aleje del mapa
    private void checkPathComplete()
    {
        if (!_destinoCompletado && !_agente.pathPending && _agente.remainingDistance < distanciaAParar)
        {
            _destinoCompletado = true;
            Vector3 dirToGoal = transform.position - destino.transform.position;
            Vector3 newPos = transform.position + dirToGoal;
            newPos = newPos * 4;
            newPos = new Vector3(newPos.x, destino.transform.position.y ,newPos.z);
            _agente.SetDestination(newPos);

        }
        if (test)
        {
            Debug.Log("TEST : " + (_agente.remainingDistance < distanciaAParar) + _agente.pathPending);
        }
    }

    protected Transform obtenerPosGameObjMasCercano(GameObject[] listaGameObj)
    {
        Transform gameObjMasCercano = null;
        float menorDistancia = Mathf.Infinity;

        // Se comprueba y elige la huerta con menor distancia
        if (listaGameObj.Length > 0)
        {
            foreach (GameObject gameObj in listaGameObj)
            {
                float distanciaActual = Vector3.Distance(transform.position, gameObj.transform.position);
                if (distanciaActual < menorDistancia)
                {
                    menorDistancia = distanciaActual;
                    gameObjMasCercano = gameObj.transform;
                }
            }
        }
        return gameObjMasCercano; // puede llegar a ser nulo si no hay nada al rededor, hay que tenerlo
                                  // en cuenta
    }

    protected Transform obtenerPosGraneroMasCercano(GameObject[] listaGraneros)
    {
        return obtenerPosGameObjMasCercano(listaGraneros);
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
