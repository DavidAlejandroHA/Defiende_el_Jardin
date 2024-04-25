using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoIA : MonoBehaviour
{
    // Navmesh
    private NavMeshAgent _agente;
    public Transform destino;

    //Vida
    public float vida;
    float vidaMax;

    // Puntos que devuelve al ser derrotado
    public float dineroADevolver;

    //Barra de vida
    [SerializeField] GameObject barraDeVidaObj;
    BarraVida barraDeVida;

    //Condiciones
    public bool _destinoCompletado;
    public bool test;

    //Valores parámetros
    public float distanciaAParar;
    public float distanciaDespawn;

    // Start is called before the first frame update
    private void Awake()
    {

    }

    public void Start()
    {
        _agente = GetComponent<NavMeshAgent>();
        _agente.destination = destino.position;
        vidaMax = vida;
        _destinoCompletado = false;

        barraDeVida = barraDeVidaObj.GetComponent<BarraVida>();
        barraDeVida.actualizarBarraDeVida(vida, vidaMax); // para asegurar que se actualiza
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

    public float getVidaMax()
    {
        return vidaMax;
    }

    public BarraVida getBarraDeVida()
    {
        return barraDeVida;
    }
}
