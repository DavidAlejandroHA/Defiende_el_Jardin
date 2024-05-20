using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamaraManager : MonoBehaviour
{
    public CinemachineVirtualCamera camara;
    public bool cambiandoPosCamara = true;
    public bool moviendoVertical = false;

    public static CamaraManager Instance { get; private set; }

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

    }

    // Update is called once per frame
    void Update()
    {
        if (cambiandoPosCamara)
        {
            float movX = Input.GetAxis("Mouse X");
            float movY = 0f;
            float movZ = Input.GetAxis("Mouse Y");
            
            if (moviendoVertical)
            {
                movY = movZ;
                movZ = 0f;
            }

            camara.transform.position = camara.transform.position + new Vector3(movX, movY, movZ);
            //Debug.Log(camara.transform.position + " - " + new Vector3(movX, movY, movZ));
        }
    }

    public void cambiarAModoColocarObj(bool value)
    {
        cambiandoPosCamara = !value;
    }
}
