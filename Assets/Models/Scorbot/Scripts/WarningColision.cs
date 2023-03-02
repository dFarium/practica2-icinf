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


    //Se obtiene el nombre del gameobject al cual esta asociado el script
    private void Start()
    {
        nombre = gameObject.name;
        tag = gameObject.tag;
        //Debug.Log(nombre);
    }

    //Se ejecuta mientras colisionen objetos
    private void OnTriggerStay(Collider other)
    {
        //Ignorar colision dentro del mismo robot 
        if (nombre == "Hombro" && other.name == "Brazo") return;
        if (nombre == "Brazo" && other.name == "Hombro") return;
        if (nombre == "Brazo" && other.name == "Antebrazo") return;
        if (nombre == "Antebrazo" && other.name == "Brazo") return;
        if (nombre == "Antebrazo" && other.name == "Mano") return;
        if (nombre == "Mano" && other.name == "Antebrazo") return;

        //Ignorar warning colision entre GameObjects con el tag "IgnorarColision" y "Cubo 1"
        if (tag == "IgnorarColision" && other.tag == "Cubo 1") return;


        
        //Cambiar canvasgroup "WarningSystem" para que sea visible, ademas añade a lista los objetos que colisionan
        grupo.alpha = 1;
        grupo.interactable = true;
        grupo.blocksRaycasts = true;
        indice = texto.addToLista(other.name + " ("+ nombre +")");
    }


    //Se ejecuta cuando dejan de colisionar objetos entre si
    private void OnTriggerExit(Collider other)
    {
        //Cambiar canvasgroup "WarningSystem" para que sea invisible, ademas quita de la  lista los objetos que salen de colision
        grupo.alpha = 0;
        grupo.interactable = false;
        grupo.blocksRaycasts = false;
        indice = texto.removerOfLista(other.name + " (" + nombre + ")");
    }
    
}
