using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Tooltip("Frecuencia inicial con la que los enemigos aparecen. Aumenta con el paso del tiempo hasta llegar" +
        " al final de la partida")]
    public float cooldown;
    public float cooldownInicial;
    public float cooldownMinimoReducido;
    public float reducirCooldown;
    float temporizador;
    public GameObject enemigo;
    [Tooltip("Distancia más cercana desde la que pueden generarse los enemigos")]
    public float distanciaMinimaSpawn;

    public float radio;
    float max_radio;
    float centro;
    // Start is called before the first frame update

    //Vector3 posCentroMundo = new Vector3(0f, 0.5f, 0f);
    public Transform centroMundo;
    Vector3 posCentroMundo;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        max_radio = radio;
        if (radio > max_radio)
        {
            radio = max_radio;
        } // Se asegura que el enemigo no se pueda generar fuera del mapa y llegar a dar error en caso de que la
        // distancia introducida de rango sea excesiva

        //Se escoje un granero al azar desde donde se hará spawn
        /*GameObject[] graneros = GameObject.FindGameObjectsWithTag("Granero");
        posCentroMundo = graneros[Random.Range(0, graneros.Length)].transform.position;*/

        posCentroMundo = centroMundo.position;
        centro = radio / 2;
        temporizador = cooldown + cooldownInicial;
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
            if (cooldown > cooldownMinimoReducido)
            {
                cooldown -= reducirCooldown;
            }
            temporizador = cooldown;
            Debug.Log(cooldown);
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
        Gizmos.color = new Color(120, 192, 203);
        Gizmos.DrawWireSphere(posCentroMundo + new Vector3(0, 1f, 0f), radio);
        Gizmos.DrawWireSphere(posCentroMundo + new Vector3(0, 1f, 0f), distanciaMinimaSpawn);
        //Gizmos.DrawRay(transform.position, transform.forward);
    }

}
