using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EsconderVentana : MonoBehaviour
{
    // Referencia al GameObject de la ventana
    public GameObject ventanaEmergente;

    // Metodo para abrir y cerrar la ventana
    public void SwitchVentanaOpciones()
    {
        if (ventanaEmergente.activeSelf)
        {
            ventanaEmergente.SetActive(false);
        }
        else
        {
            ventanaEmergente.SetActive(true);
        }
    }

    // Metodo para cerrar la ventana
    public void CerrarVentanaOpciones()
    {
        ventanaEmergente.SetActive(false);
    }
}
