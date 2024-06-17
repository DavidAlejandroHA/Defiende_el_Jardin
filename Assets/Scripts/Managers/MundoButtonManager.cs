using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MundoButtonManager : MonoBehaviour
{
    //public string desbloqueoMundoNombre;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("DesbloqueoMundo2"));
        if (PlayerPrefs.GetInt("DesbloqueoMundo2") > 0)
        {
            this.gameObject.GetComponent<Button>().enabled= true;
        }
        else
        {
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.GetComponent<Button>().image.color = new Color32(154, 154, 154, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
