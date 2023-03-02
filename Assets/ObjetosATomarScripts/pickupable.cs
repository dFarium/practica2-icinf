using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Este script se encarga de indicarle a la InteractionZone si el objeto se puede tomar
public class pickupable : MonoBehaviour
{
    //Se le indica que el objeto se puede tomar
    public bool isPickable = true;

    //OnTriggerEnter se activa si la InteractionZone entra a un objeto
    private void OnTriggerEnter(Collider other)
    {
        //Solo se aplica si el tag es "InteractionZone"
        if (other.tag == "InteractionZone")
        {
            //Se le indica a la InteractionZone que lo puede tomar
            other.GetComponentInParent<PickUpObject>().ObjectToPickUp = this.gameObject;

        }
    }

    //OnTriggerEnter se activa si la InteractionZone sale del objeto
    private void OnTriggerExit(Collider other)
    {
        //Solo se aplica si el tag es "InteractionZone"
        if (other.tag == "InteractionZone")
        {
            //Se le indica a la InteractionZone ya no que lo puede tomar
            other.GetComponentInParent<PickUpObject>().ObjectToPickUp = null;
        }
    }
}