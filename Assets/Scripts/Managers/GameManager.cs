using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int vidas;
    public float dinero;
    public float puntos;
    public float tiempoRestante;
    public int enemigosMuertos;

    bool partidaActiva = true;

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

    // Start is called before the first frame update
    void Start()
    {
        // Se empieza con 10 puntos para hacer posible defenderse al jugador
        //aniadirDinero(10f);
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (partidaActiva)
        {
            tiempoRestante -= Time.deltaTime;

        }

        if (tiempoRestante <= 0)
        {
            terminarPartida();
            ganarPartida();
        }

    }

    public void aniadirMuertes()
    {
        enemigosMuertos++;
    }
    public void aniadirDinero(float dinero)
    {
        this.dinero += dinero;
        //UIManager.Instance.actualizarTextoDinero();
        //ButtonManager.Instance.resetImagesColor();
    }

    public void quitarDinero(float dinero)
    {
        this.dinero -= dinero;
        //ButtonManager.Instance.checkEnoughMoney(dinero);
        //UIManager.Instance.actualizarTextoDinero();
        //ButtonManager.Instance.resetImagesColor();
    }

    /*public void quitarVida()
    {
        //puntos += 5;
        //TODO: Finalizar juego al perder todas las vidas
        if (vidas <= 0)
        {
            terminarPartida();
            perderPartida();
        }
    }*/

    public void aniadirPuntos(float puntos)
    {
        this.puntos += puntos;
    }

    public bool getPartidaActiva()
    {
        return partidaActiva;
    }

    public void terminarPartida()
    {
        partidaActiva = false;
        Time.timeScale = 0f;
    }

    void ganarPartida()
    {
        //UIManager.Instance.mostrarPanelGanar();
    }
    void perderPartida()
    {
        //UIManager.Instance.mostrarPanelPerder();
    }

    public Vector3 puntoAleatorioEnAnillo(Vector3 origen, float minRadio, float maxRadio)
    {
        Vector2 origen2D = new Vector2(origen.x, origen.z);
        Vector2 randomDirection = (Random.insideUnitCircle).normalized;

        float randomDistance = Random.Range(minRadio, maxRadio);

        Vector2 point = origen2D + randomDirection * randomDistance;

        return new Vector3(point.x, origen.y, point.y);
    }
}
