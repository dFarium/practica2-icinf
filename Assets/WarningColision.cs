using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningColision : MonoBehaviour
{
    public CanvasGroup grupo;
    public int tolerancia;
    public int colisiones = 0;
    public TextoColision texto;
    public int indice;
    public string nombre;


    private void Start()
    {
        nombre = gameObject.name;
        tag = gameObject.tag;
        Debug.Log(nombre);
    }
    private void OnTriggerStay(Collider other)
    {
        if (nombre == "Hombro" && other.name == "Brazo") return;
        if (nombre == "Brazo" && other.name == "Hombro") return;
        if (nombre == "Brazo" && other.name == "Antebrazo") return;
        if (nombre == "Antebrazo" && other.name == "Brazo") return;
        if (nombre == "Antebrazo" && other.name == "Mano") return;
        if (nombre == "Mano" && other.name == "Antebrazo") return;

        if (tag == "IgnorarColision" && other.tag == "Cubo 1") return;

        if (colisiones > tolerancia)
        {
            if(other.tag != "Objeto")
            {
                //Debug.Log(other.tag);
                //Debug.Log(other.name + " ESTA COLISIONANDO");
                grupo.alpha = 1;
                grupo.interactable = true;
                grupo.blocksRaycasts = true;
                indice = texto.addToLista(other.name + " ("+ nombre +")");
            } 
        }
        else
        {
            colisiones++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        grupo.alpha = 0;
        grupo.interactable = false;
        grupo.blocksRaycasts = false;
        indice = texto.removerOfLista(other.name + " (" + nombre + ")");
    }
    
}
