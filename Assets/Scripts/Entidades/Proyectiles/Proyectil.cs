using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float tiempoDespawn;
    private float _danio;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, tiempoDespawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemigo")
        {
            other.GetComponent<EnemigoIA>().takeDamage(_danio);
            //Debug.Log("A");
            Destroy(this.gameObject);
        } else if (other.gameObject.tag != "Gnomo" && other.gameObject.tag != "PiezaCuerpo" && other.gameObject.tag != "Proyectil"
            && other.gameObject.tag != "Huerta")
            // Si no es un gnomo o un proyectil con lo que ha chocado entonces se destruye el proyectil
        {
            Destroy(this.gameObject);
        }
    }

    public void setDanio(float danio)
    {
        _danio = danio;
    }
}
