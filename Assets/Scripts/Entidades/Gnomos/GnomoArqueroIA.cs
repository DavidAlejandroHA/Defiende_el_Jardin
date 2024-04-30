using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GnomoArqueroIA : GnomoIA
{
    // Navmesh
    /*private NavMeshAgent _agente;
    private float velocidad;

    // Variables
    public float radio;
    public bool mostrarAreaDeAccion;
    public float danio;
    public float cooldown;
    [SerializeField] private float temporizador;
    */

    [Header("Stats de munición")]
    //Barra de Municion
    public int cantidadMunicion;
    private int _municionMax;
    public float rapidezRecuperacion;
    public float velocidadLanzamiento;
    //public float gastoStamina;
    //public float gastoStaminaCorriendo;
    [SerializeField] BarraVida barraDeMunicion;

    //Ataque
    [Header("Ataque")]
    public float danioProyectil;

    [Header("Tipo de municion")]
    public GameObject tipoMunicion;

    //Condiciones
    bool municionAgotada;
    bool puedeAtacar;
    bool enHuerta;
    bool atacando;


    //Objetos
    [Header("Destinos")]
    public GameObject huerta;

    // Start is called before the first frame update

    //int mascara = 1 << 6;
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        puedeAtacar = false;
        atacando = true;
        temporizador = cooldown + 0.1f;
        _municionMax = cantidadMunicion;
        barraDeMunicion.setVida(cantidadMunicion);
        municionAgotada = false;
        velocidad = agente.speed;
        rapidezRecuperacion = rapidezRecuperacion / 10;
    }

    // Update is called once per frame
    void Update()
    {
        // Temporizador para hacer daño cada x tiempo
        if (atacando)
        {
            temporizador -= Time.deltaTime;
        }
        if (temporizador <= 0 && atacando)
        {
            temporizador = cooldown;
        }

        // Se dispara un proyectil cada vez que se reinicia el temporizador
        if (temporizador == cooldown && atacando)
        {
            puedeAtacar = true;
        }

        // Esta lista almacenará el resultado de llamar a OverlapSphere
        Collider[] listaChoques;

        listaChoques = Physics.OverlapSphere(transform.position, radio, mascara);

        // se obtiene el enemigo más cercano al gnomo
        Transform enemigoMasCercano = obtenerPosEnemigoMasCercano(listaChoques);

        //Si no está agotado y hay enemigos en el radio de acción se va a perseguirlo
        if (!municionAgotada)
        {
            if (enemigoMasCercano != null)
            {
                agente.speed = velocidad;
                Vector3 posEnemigo = enemigoMasCercano.GetComponent<EnemigoIA>().transform.position;
                agente
                    .SetDestination(posEnemigo);
                atacando = true;

                if (puedeAtacar)
                {
                    lanzarProyectil(posEnemigo);
                    puedeAtacar = false;
                }
            }
        }

        // Si está agotado se va a la huerta a reponer energías
        else if (agente.destination != huerta.transform.position)
        {
            agente.SetDestination(huerta.transform.position);
            atacando = false;
        }
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

    // Cuando hace contacto con la huerta empieza a regenerarse
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Huerta" && municionAgotada)
        {
            if (!enHuerta)
            {
                agente.speed = velocidad / 4;
            }
            enHuerta = true;

            float progresoSumado = barraDeMunicion.sumarVida(rapidezRecuperacion);
            Debug.Log(progresoSumado);
            if (progresoSumado < _municionMax)
            {
                
            }
            else
            {
                //Debug.Log("A");
                barraDeMunicion.actualizarBarraDeVida(_municionMax);
                // Ajusta al 100% exactamente en caso de poder llegado a haber un exceso puntual
                cantidadMunicion = _municionMax;
                municionAgotada = false;
                temporizador = 0.5f;
            }
        }
    }

    /* Al salir de la huerta se marca un booleano para que posteriormente se vuelva a usar
     a la hora de reducir la velocidad al entrar en la huerta */
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Huerta")
        {
            enHuerta = false;
        }
    }

    // Para hacer la trayerctoria dada una velocidad inicial (Este es el caso que se va a usar):
    //https://stackoverflow.com/questions/30290262/how-to-throw-a-ball-to-a-specific-point-on-plane
    //https://discussions.unity.com/t/input-force-is-nan-nan-nan/190950
    // Para hacer la trayectoria dada una altura máxima (lo dejo como apunte por si lo necesito
    // yo en el futuro, no lo voy a usar):
    //https://youtu.be/IvT8hjy6q4o
    public void ThrowBallAtTargetLocation(GameObject ballGameObject, Vector3 targetLocation, float initialVelocity)
    {
        Vector3 direction = (targetLocation - transform.position).normalized;
        float distance = Vector3.Distance(targetLocation, transform.position);

        Vector3 anguloSueloPos = new Vector3(targetLocation.x, transform.position.y, targetLocation.z);
        Vector3 centroPos = transform.position;
        Vector3 directionA = Vector3.Normalize(centroPos - anguloSueloPos);
        Vector3 directionB = Vector3.Normalize(centroPos - targetLocation);
        // https://stackoverflow.com/questions/49383884/find-angle-between-two-objects-while-taking-another-object-as-center-in-unity-us

        float angulo = Vector3.Angle(directionA, directionB);
           
        float firingElevationAngle = 90f -
            FiringElevationAngle(Physics.gravity.magnitude, distance, initialVelocity)
            - (angulo > 0 ? angulo : 90f);
        
        //Debug.Log(firingElevationAngle + " " + Vector3.Angle(directionA, directionB));
        Vector3 elevation = Quaternion.AngleAxis(firingElevationAngle, transform.right) * transform.up;
        float directionAngle = AngleBetweenAboutAxis(transform.forward, direction, transform.up);
        Vector3 velocity = Quaternion.AngleAxis(directionAngle, transform.up) * elevation * initialVelocity;

        // ballGameObject is object to be thrown
        ballGameObject.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
    }

    // Helper method to find angle between two points (v1 & v2) with respect to axis n
    public static float AngleBetweenAboutAxis(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    // Helper method to find angle of elevation (ballistic trajectory) required to reach distance with initialVelocity
    // Does not take wind resistance into consideration.

    /*  
     *  Si el destino a apuntar está fuera de rango (es decir, si con la velocidad inicial designada
     *  no es posible alcanzar el objetivo porque está muy lejos) el try dará un error, por lo que en ese
     *  caso se usará un ángulo por defecto de 45º
     */
    private float FiringElevationAngle(float gravity, float distance, float initialVelocity)
    {
        float angle = 45f; // Angulo por defecto
        try
        {
            angle = 0.5f * Mathf.Asin((gravity * distance) / (initialVelocity * initialVelocity))
            * Mathf.Rad2Deg;
        } catch (Exception)
        {

        }
        
        return angle;
    }

    public void lanzarProyectil(Vector3 posDestino)
    {
        GameObject proyectilInstanciado = Instantiate(tipoMunicion, transform.position +
            (transform.forward * 0.6f), transform.rotation);
        proyectilInstanciado.SetActive(true);
        proyectilInstanciado.GetComponent<Proyectil>().setDanio(danioProyectil);
        ThrowBallAtTargetLocation(proyectilInstanciado, posDestino, velocidadLanzamiento);
        
        cantidadMunicion--;
        barraDeMunicion.actualizarBarraDeVida(cantidadMunicion);
        if (cantidadMunicion <= 0)
        {
            municionAgotada = true;
        }
    }
}
