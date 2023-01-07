using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Este script se encarga de indicarle a la InteractionZone si el objeto se puede tomar
public class pickupable : MonoBehaviour
{
    public bool isPickable = true;//Se le indica que el objeto se puede tomar

    private void OnTriggerEnter(Collider other)//OnTriggerEnter se activa si la InteractionZone entra al objeto
    {
        if (other.tag == "InteractionZone")
        {
            other.GetComponentInParent<PickUpObject>().ObjectToPickUp = this.gameObject; //Se le indica a la InteractionZone que lo puede tomar

        }
    }

    private void OnTriggerExit(Collider other)//OnTriggerEnter se activa si la InteractionZone sale del objeto
    {
        if (other.tag == "InteractionZone")
        {
            other.GetComponentInParent<PickUpObject>().ObjectToPickUp = null; //Se le indica a la InteractionZone ya no que lo puede tomar
        }
    }
}