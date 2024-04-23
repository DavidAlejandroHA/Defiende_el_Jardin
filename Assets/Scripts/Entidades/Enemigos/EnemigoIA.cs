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
    bool destinoCompletado;

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
        destinoCompletado = false;

        barraDeVida = barraDeVidaObj.GetComponent<BarraVida>();
        barraDeVida.actualizarBarraDeVida(vida, vidaMax); // para asegurar que se actualiza
        // como es debido al volver a empezar
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
        /*GameManager.Instance.aniadirDinero(dineroADevolver);
        GameManager.Instance.aniadirPuntos(dineroADevolver);
        GameManager.Instance.aniadirMuertes();*/
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (agente.pathStatus != NavMeshPathStatus.PathComplete)
            Debug.Log("a");*/
        checkPathComplete();
        //Debug.Log(Vector3.Distance(transform.position, destino.transform.position));
        checkMissionCompleted();
    }

    private void checkMissionCompleted()
    {
        if (destinoCompletado && Vector3.Distance(transform.position, destino.transform.position) > distanciaDespawn)
        {
            morir();
        }
    }

    private void checkPathComplete()
    {
        if (!destinoCompletado && !_agente.pathPending && _agente.remainingDistance < distanciaAParar)
        {
            //Debug.Log("a");
            destinoCompletado = true;
            Vector3 dirToPlayer = transform.position - destino.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            newPos = newPos * 3;
            newPos = new Vector3(newPos.x, destino.transform.position.y ,newPos.z);
            _agente.SetDestination(newPos);

        }
    }

    /*public NavMeshAgent getAgente()
    {
        return _agente;
    }*/

    public float getVidaMax()
    {
        return vidaMax;
    }
}
