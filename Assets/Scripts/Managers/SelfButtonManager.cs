using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfButtonManager : MonoBehaviour
{
    public float precio;
    public Color colorDeshabilitado = new Color32(150, 150, 150, 255); // gris
    public Color colorHabilitado = new Color32(255, 255, 255, 255); // blanco

    // Start is called before the first frame update
    void Start()
    {
        gestionarColorBoton();
        this.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
    }

    // Update is called once per frame
    void Update()
    {
        gestionarColorBoton();
    }

    void gestionarColorBoton()
    {
            if (GameManager.Instance.getPuntosComidaCompra() < precio)
            {
                this.GetComponent<Button>().enabled = false;
                this.GetComponent<Button>().image.color = colorDeshabilitado;
            }
            else
            {
                this.GetComponent<Button>().enabled = true;
                this.GetComponent<Button>().image.color = colorHabilitado;
            }
    }

    /*public void restarPrecio()
    {
        GameManager.Instance.aniadirComidaReservas(precio);
    }*/

    public void asignarPrecio()
    {
        GameUIManager.Instance.setPrecioAsignadoAlObjeto(precio);
    }
}
