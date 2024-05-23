using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    GameObject objetoAColocar;
    GameObject objetoCopiado;
    float precioAsignadoAlObjeto;
    //List<GameObject> piezasModificadas;
    bool objetoSiendoArrastrado;

    [SerializeField] TextMeshProUGUI textoPuntosCompra;
    [SerializeField] TextMeshProUGUI textoPuntosReservas;

    string textoPuntosCompraOriginal;
    string textoPuntosReservasOriginal;

    PlayerInput playerInput;

    int mascaraSuelo = 1 << 7;
    public static UIManager Instance { get; private set; }
    
    
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
        objetoSiendoArrastrado = false;
        playerInput = GetComponent<PlayerInput>();

        textoPuntosCompraOriginal = textoPuntosCompra.text;
        Debug.Log(textoPuntosCompraOriginal);
        textoPuntosReservasOriginal = textoPuntosReservas.text;

        actualizarTextoPuntosCompra();
        actualizarTextoPuntosReservas();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playerInput.actions["Ejes"].ReadValue<Vector2>());
        comprobarColocarObjetos();
    }

    public void designarObjeto(GameObject gObj)
    {
        objetoAColocar = gObj;
    }

    public void generarObjeto()
    {
        if (!objetoSiendoArrastrado) // Para que solo se pueda generar un objeto al mismo tiempo
                                     // hasta que no se coloque
        {
            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit golpeRayo;
            bool colisionConRayo = Physics.Raycast(rayo, out golpeRayo, 100000f, mascaraSuelo);
            objetoCopiado = Object.Instantiate(objetoAColocar,
                    !colisionConRayo ? objetoAColocar.transform.position : golpeRayo.point,
                    objetoAColocar.transform.rotation);
            objetoCopiado.GetComponent<GnomoIA>().imagenCirculo.SetActive(true);
            objetoCopiado.SetActive(true);
            habilitarScripts(objetoCopiado, false);
            habilitarCompontentesIA(objetoCopiado, false);

            // Se deshabilitan todas las scripts para que no inicie su comportamiento hasta que
            // no sea activado y puesto en el mundo

            //seleccionarObjeto();
            objetoSiendoArrastrado = true;
        }
    }

    /*
    void seleccionarObjeto()
    {
        piezasModificadas = asignarColor(new Color32(49, 255, 255, 255), 
            objetoCopiado, "PiezaCuerpo");
    }
    */

    void habilitarScripts(GameObject gameObj, bool trueOrFalse)
    {
        MonoBehaviour[] scripts = gameObj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = trueOrFalse;
        }
    }

    void habilitarCompontentesIA(GameObject gameObj, bool trueOrFalse)
    {
        gameObj.GetComponent<NavMeshAgent>().enabled = trueOrFalse;
        gameObj.GetComponent<Animator>().enabled = trueOrFalse;
        gameObj.GetComponent<BoxCollider>().enabled = trueOrFalse;
        gameObj.GetComponent<Rigidbody>().isKinematic = !trueOrFalse;
    }

    void comprobarColocarObjetos()
    {
        if (objetoSiendoArrastrado)
        {
            if (playerInput.actions["CancelarColocarObjeto"].ReadValue<float>() > 0)
            {
                Destroy(objetoCopiado);
                objetoSiendoArrastrado = false;
                return;
            } // Si se cancela la colocación del objeto se destruye la copia y se ignora el resto
            

            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit golpeRayo;
            if (Physics.Raycast(rayo, out golpeRayo, 100000f, mascaraSuelo))
            {
                objetoCopiado.gameObject.transform.position = golpeRayo.point;
            }

            if (/*Input.GetMouseButtonDown(0)*/
            playerInput.actions["ColocarObjetos"].ReadValue<float>() > 0
                /* && EventSystem.current.IsPointerOverGameObject()*/)
            {
                //asignarColor(new Color32(255, 255, 255, 255), objetoCopiado, "PiezaCuerpo");
                objetoCopiado.GetComponent<GnomoIA>().imagenCirculo.SetActive(false);
                GameManager.Instance.quitarPuntosComidaCompra(precioAsignadoAlObjeto);
                habilitarScripts(objetoCopiado, true);
                habilitarCompontentesIA(objetoCopiado, true);
                
                objetoSiendoArrastrado = false;
            }
        }
    }

    // Devuelve la lista de los objetos a los que se les ha cambiado el color con
    // los colores originales
    /*List<GameObject> asignarColor(Color32 color, GameObject gameObj, string tag)
    {
        List<GameObject> listaPiezas = new List<GameObject>();

        foreach (Transform child in gameObj.transform)
        {
            if (child.tag == tag)
            {
                listaPiezas.Add(child.gameObject);
            }
        }

        List<GameObject> listaPiezasCopia = listaPiezas;
        foreach (GameObject gObj in listaPiezas)
        {
            foreach (Material mat in gObj.GetComponent<Renderer>().materials)
            {
                //mat.color = new Color32(49, 255, 255, 255);
                mat.color = color;
            }
        }

        return listaPiezasCopia;
    }*/

    public void actualizarTextoPuntosCompra()
    {
        textoPuntosCompra.text = textoPuntosCompraOriginal + 
            GameManager.Instance.getPuntosComidaCompra() + "$";
    }

    public void actualizarTextoPuntosReservas()
    {
        textoPuntosReservas.text = textoPuntosReservasOriginal + 
            GameManager.Instance.getPuntosComidaReservas() + "$";
    }

    public void setObjetoAColocar(GameObject gObj)
    {
        objetoAColocar = gObj;
    }
    
    public void setPrecioAsignadoAlObjeto(float precio)
    {
        precioAsignadoAlObjeto = precio;
    }
}
