using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public GameObject entidad;
    float vidaMax;
    /*[SerializeField] private */
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookRotation = Camera.main.transform.rotation;
        transform.rotation = lookRotation;
    }

    /* Actualiza la barra de vida dada la vida que se entrega en el método respecto a
    la vida máxima ya definida*/
    public void actualizarBarraDeVida(float vidaActual)
    {
        slider = this.GetComponent<Slider>();
        actualizarBarraDeVida(vidaActual, vidaMax);
    }

    // Actualiza la barra de vida dada una vida actual y una vida máxima
    public void actualizarBarraDeVida(float vidaActual, float vidaMax)
    {
        slider = this.GetComponent<Slider>();
        slider.value = vidaActual / vidaMax;
    }

    // Suma puntos al slider y retorna el nuevo valor que este tiene
    public float sumarVida(float vidaASumar)
    {
        slider.value = slider.value + vidaASumar;
        return slider.value;
    }

    // Establece la vida/munición máxima
    public void setVida(float vida)
    {
        vidaMax = vida;
    }
}
