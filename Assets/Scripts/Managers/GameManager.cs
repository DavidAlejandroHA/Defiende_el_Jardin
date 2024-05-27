using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public float puntosComidaReservas;
    float puntosComidaCompra;

    public float tiempoRestante;
    public int enemigosMuertos;

    bool partidaActiva = true;
    bool partidaTerminada = false;

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
        puntosComidaCompra = 0f;

        //UIManager.Instance.actualizarTextoPuntosCompra();
        //UIManager.Instance.actualizarTextoPuntosReservas();
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
            pausarPartida();
            ganarPartida();
        }

        if (puntosComidaReservas <= 0)
        {
            pausarPartida();
            perderPartida();
        }
    }

    public void aniadirMuertes()
    {
        enemigosMuertos++;
    }
    public void aniadirComidaReservas(float dinero)
    {
        this.puntosComidaReservas += dinero;
        UIManager.Instance.actualizarTextoPuntosReservas();
        /*if (this.puntosComidaReservas <= 0)
        {
            terminarPartida();
            perderPartida();
        }*/
    }

    public void quitarComidaReservas(float dinero)
    {
        this.puntosComidaReservas -= dinero;
        UIManager.Instance.actualizarTextoPuntosReservas();
        if (this.puntosComidaReservas <= 0)
        {
            pausarPartida();
            perderPartida();
        }
    }

    public void aniadirPuntosComidaCompra(float dinero)
    {
        this.puntosComidaCompra += dinero;
        //SelfButtonManager.actualizado = true;
        UIManager.Instance.actualizarTextoPuntosCompra();
    }

    public void quitarPuntosComidaCompra(float dinero)
    {
        this.puntosComidaCompra -= dinero;
        //SelfButtonManager.actualizado = true;
        UIManager.Instance.actualizarTextoPuntosCompra();
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

    public bool getPartidaActiva()
    {
        return partidaActiva;
    }

    public void pausarPartida()
    {
        partidaActiva = false;
        Time.timeScale = 0f;
    }

    void ganarPartida()
    {
        //UIManager.Instance.mostrarPanelGanar();
        partidaActiva = false;
        partidaTerminada = true;
        UIManager.Instance.panelMenuPausa.SetActive(false);
    }
    void perderPartida()
    {
        //UIManager.Instance.mostrarPanelPerder();
        partidaActiva = false;
        partidaTerminada = true;
        UIManager.Instance.panelMenuPausa.SetActive(false);
    }

    public Vector3 puntoAleatorioEnAnillo(Vector3 origen, float minRadio, float maxRadio)
    {
        Vector2 origen2D = new Vector2(origen.x, origen.z);
        Vector2 randomDirection = (Random.insideUnitCircle).normalized;

        float randomDistance = Random.Range(minRadio, maxRadio);

        Vector2 point = origen2D + randomDirection * randomDistance;

        return new Vector3(point.x, origen.y, point.y);
    }

    public float getPuntosComidaCompra()
    {
        return puntosComidaCompra;
    }

    public float getPuntosComidaReservas()
    {
        return puntosComidaReservas;
    }

    public void setPartidaActiva(bool activo)
    {
        partidaActiva = activo;
        if (partidaActiva)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }

    public bool getPartidaTerminada()
    {
        return partidaTerminada;
    }
}
