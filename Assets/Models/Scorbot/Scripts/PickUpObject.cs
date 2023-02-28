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
        if (ObjectToPickUp != null && ObjectToPickUp.GetComponent<pickupable>().isPickable == true && PickedObject == null){//Condicion si puede tomar un objeto cercano
            estadoPick.text = "Objeto preparado para tomar";
            estadoBoton.text = "Tomar Objeto";
            objetoTomado = 0;
        }
        else
        {
            if(PickedObject != null){//Si es que ya tiene tomado un objeto
                estadoPick.text = "Objeto Tomado";
                estadoBoton.text = "Soltar Objeto";
                objetoTomado = 1;
            }
            else{//Si no encuentra nada para tomar
                estadoPick.text = "Ningun objeto para tomar";
                estadoBoton.text = "Esperando Objeto";
                objetoTomado = 0;

            }
        }
    }

    public void pickButton(){//Con esta funcion toma/suelta objetos
            if (ObjectToPickUp != null && ObjectToPickUp.GetComponent<pickupable>().isPickable == true && PickedObject == null){//Toma objeto
                    PickedObject = ObjectToPickUp;//Se le asigna el objeto
                    PickedObject.GetComponent<pickupable>().isPickable = false; //El objeto ya no es identificado como un objeto que se puede tomar
                    PickedObject.transform.SetParent(interactionZones);//El objeto se vuelve hijo de la zona de interaccion
                    //PickedObject.transform.position = interactionZones.position;//El objeto se transporta a la zona de interaccion, para que quede centrado
                    PickedObject.GetComponent<Rigidbody>().useGravity = false;// El objeto ya no usa gravedad, para evitar que el robot se vuelta loco
                    PickedObject.GetComponent<Rigidbody>().isKinematic = true;// El objeto se vuelve kinematico para poder atravesar objetos 
            }
            else if (PickedObject != null){//Se suelta el objeto
                    // Aqui se hace el proceso inverso a lo anterior
                    PickedObject.GetComponent<pickupable>().isPickable = true;
                    PickedObject.transform.SetParent(null);
                    PickedObject.GetComponent<Rigidbody>().useGravity = true;
                    PickedObject.GetComponent<Rigidbody>().isKinematic = false;
                    PickedObject = null; //A la interactionZone se le indica que no tiene tomado ningun objeto
            }   
        }

    public void resetPick(){//Suelta el objeto si lo tiene tomado, usado al momento de resetear el objeto, fuerza el soltado del objeto
          if (PickedObject != null){
                    PickedObject.GetComponent<pickupable>().isPickable = true;
                    PickedObject.transform.SetParent(null);
                    PickedObject.GetComponent<Rigidbody>().useGravity = true;
                    PickedObject.GetComponent<Rigidbody>().isKinematic = false;
                    PickedObject = null; 
            }  
    } 
}

    
