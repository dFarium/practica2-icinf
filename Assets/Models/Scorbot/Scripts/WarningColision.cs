using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningColision : MonoBehaviour
{
    public CanvasGroup grupo;
    public TextoColision texto;
    private int indice;
    private string nombre;
    private string robot;


    //Se obtiene el nombre del gameobject al cual esta asociado el script
    private void Start()
    {
        //Se obtienen los nombres y tags del objeto que tiene el script, ademas nombre del objeto raiz
        nombre = gameObject.name;
        tag = gameObject.tag;
        robot = transform.root.name;
    }

    //Se ejecuta mientras colisionen objetos
    private void OnTriggerStay(Collider other)
    {
        //Ignorar colision entre mismo scorbot
        if (robot == "Scorbot")
        {
            if (nombre == "Hombro" && other.name == "Brazo") return;
            if (nombre == "Brazo" && other.name == "Hombro") return;
            if (nombre == "Brazo" && other.name == "Antebrazo") return;
            if (nombre == "Antebrazo" && other.name == "Brazo") return;
            if (nombre == "Antebrazo" && other.name == "Mano") return;
            if (nombre == "Mano" && other.name == "Antebrazo") return;
        }
        
        //Ignorar colision entre mismo kuka
        if (robot == "kuka")
        {
            if (nombre == "BaseFija" && other.name == "MesaKuka") return;
            if (nombre == "MesaKuka" && other.name == "BaseFija") return;
            if (nombre == "BaseFija" && other.name == "BaseMovil") return;
            if (nombre == "BaseMovil" && other.name == "BaseFija") return;
            if (nombre == "BaseMovil" && other.name == "Brazo1") return;
            if (nombre == "Brazo1" && other.name == "BaseMovil") return;
            if (nombre == "Brazo2" && other.name == "Brazo1") return;
            if (nombre == "Brazo1" && other.name == "Brazo2") return;
            if (nombre == "Brazo2" && other.name == "Muneca1") return;
            if (nombre == "Muneca1" && other.name == "Brazo2") return;
            if (nombre == "Muneca1" && other.name == "Muneca2") return;
            if (nombre == "Muneca2" && other.name == "Muneca1") return;
            if (nombre == "Muneca2" && other.name == "Mano") return;
            if (nombre == "Mano" && other.name == "Muneca2") return;
            if (nombre == "Muneca2" && other.name == "Pinza") return;
            if (nombre == "Pinza" && other.name == "Muneca2") return;
        }

        

        //Ignorar warning colision entre GameObjects con el tag "IgnorarColision" y "Cubo 1"
        if (tag == "IgnorarColision" && other.tag == "Cubo 1") return;

        //Ignorar pickupzone
        if (nombre == "PickUpZone") return;
        if (other.name == "PickUpZone") return;
        //Ignorar InteractionZone



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
