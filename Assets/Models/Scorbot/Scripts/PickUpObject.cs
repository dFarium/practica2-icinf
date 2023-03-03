using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Este script se encarga de tomar el objeto, soltarlo y detectar si esta en rango para ser tomado
public class PickUpObject : MonoBehaviour
{
    public GameObject ObjectToPickUp;
    public GameObject PickedObject;
    public Transform interactionZones;
    public Text estadoPick;
    public Text estadoBoton;
    public int objetoTomado;

    void Update() //Pregunta cada frame si puede tomar un objeto, o si ya lo tiene tomado
    {
        //Condicion si puede tomar un objeto cercano
        if (ObjectToPickUp != null && ObjectToPickUp.GetComponent<pickupable>().isPickable == true && PickedObject == null){
            estadoPick.text = "Objeto preparado para tomar";
            estadoBoton.text = "Tomar Objeto";
            objetoTomado = 0;
        }
        else
        {
            //Si es que ya tiene tomado un objeto
            if (PickedObject != null){
                estadoPick.text = "Objeto Tomado";
                estadoBoton.text = "Soltar Objeto";
                objetoTomado = 1;
            }
            //Si no encuentra nada para tomar
            else
            {
                estadoPick.text = "Ningun objeto para tomar";
                estadoBoton.text = "Esperando Objeto";
                objetoTomado = 0;

            }
        }
    }

    //Con esta funcion toma/suelta objetos
    public void pickButton(){
            //Toma objeto
            if (ObjectToPickUp != null && ObjectToPickUp.GetComponent<pickupable>().isPickable == true && PickedObject == null){
                    //Se le asigna el objeto
                    PickedObject = ObjectToPickUp;
                    //El objeto ya no es identificado como un objeto que se puede tomar
                    PickedObject.GetComponent<pickupable>().isPickable = false; 
                    //El objeto se vuelve hijo de la zona de interaccion
                    PickedObject.transform.SetParent(interactionZones);

                    //El objeto se transporta a la zona de interaccion, para que quede centrado (sin usar)
                    //PickedObject.transform.position = interactionZones.position;

                    // El objeto ya no usa gravedad, para evitar problemas con el robot
                    PickedObject.GetComponent<Rigidbody>().useGravity = false;
                    // El objeto se vuelve kinematico para poder atravesar objetos (opcional dependiendo del caso de uso) 
                    PickedObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            //Se suelta el objeto
            else if (PickedObject != null){
                    // Aqui se hace el proceso inverso a lo anterior
                    PickedObject.GetComponent<pickupable>().isPickable = true;
                    PickedObject.transform.SetParent(null);
                    PickedObject.GetComponent<Rigidbody>().useGravity = true;
                    PickedObject.GetComponent<Rigidbody>().isKinematic = false;
                    //A la interactionZone se le indica que no tiene tomado ningun objeto
                    PickedObject = null;
            }   
        }

    //Suelta el objeto si lo tiene tomado, usado al momento de resetear el objeto, fuerza el soltado del objeto
    public void resetPick(){
          if (PickedObject != null){
                    PickedObject.GetComponent<pickupable>().isPickable = true;
                    PickedObject.transform.SetParent(null);
                    PickedObject.GetComponent<Rigidbody>().useGravity = true;
                    PickedObject.GetComponent<Rigidbody>().isKinematic = false;
                    PickedObject = null; 
            }  
    } 
}

    
