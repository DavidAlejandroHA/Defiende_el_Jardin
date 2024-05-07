using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntidadesIA : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //En desuso
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
        return gameObjMasCercano; // puede llegar a ser nulo si no hay nada al rededor, hay que tenerlo en cuenta                  
    }

    protected Transform obtenerPosGameObjMasCercano(string tag)
    {
        Transform gameObjMasCercano = null;
        float menorDistancia = Mathf.Infinity;

        GameObject[] listaGameObj = GameObject.FindGameObjectsWithTag(tag);
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
        return gameObjMasCercano; // puede llegar a ser nulo si no hay nada al rededor, hay que tenerlo en cuenta         
    }
}
