using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float cooldown = 1f;
    float temporizador;
    public GameObject enemigo;
    float distanciaMinimaSpawn = 20f;

    public float radio;
    float centro;
    // Start is called before the first frame update

    Vector3 posCentroMundo = new Vector3(0f, 0.5f, 0f);
    void Start()
    {
        if (radio > 48f)
        {
            radio = 48f;
        } // Se asegura que el enemigo no se pueda generar fuera del mapa y llegar a dar error en caso de que la
        // distancia introducida de rango sea excesiva

        centro = radio / 2;
        temporizador = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        temporizador -= Time.deltaTime;
        if (temporizador <= 0)
        {
            //float radio = Random.value * 5;

            /* GameObject nuevoEnemigo = Instantiate(enemigo, posCentroMundo - new Vector3(radio / 2, 0f, radio / 2)
             + new Vector3(Random.value * radio, 0f, Random.value * radio), Quaternion.identity);*/
            GameObject nuevoEnemigo = Instantiate(enemigo,
                puntoAleatorioEnAnillo(posCentroMundo, distanciaMinimaSpawn, radio)
                , Quaternion.identity);

            nuevoEnemigo.transform.LookAt(posCentroMundo);
            nuevoEnemigo.SetActive(true);
            temporizador = cooldown;
        }
    }

    // Dado un origen y un radio mínimo y máximo que formarán un anillo, se devolverá un punto aleatorio dentro de este
    public Vector3 puntoAleatorioEnAnillo(Vector3 origen, float minRadio, float maxRadio)
    {
        Vector2 origen2D = new Vector2(origen.x, origen.z);
        Vector2 randomDirection = (Random.insideUnitCircle).normalized;

        float randomDistance = Random.Range(minRadio, maxRadio);

        Vector2 point = origen2D + randomDirection * randomDistance;

        return new Vector3(point.x, origen.y, point.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(20, 192, 203);
        Gizmos.DrawWireSphere(posCentroMundo, radio);
        //Gizmos.DrawRay(transform.position, transform.forward);
    }

}
