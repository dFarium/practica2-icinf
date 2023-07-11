using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VentanaEmergente : MonoBehaviour
{
    // Referencia al GameObject de la ventana de opciones
    public GameObject ventanaEmergente;

    [SerializeField]
    private Canvas canvas;

    // Metodo para abrir y cerrar la ventana de opciones
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

    // Metodo para cerrar la ventana de opciones
    public void CerrarVentanaOpciones()
    {
        ventanaEmergente.SetActive(false);
    }

    public void DragWindow(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            null,
            out position);

        transform.position = canvas.transform.TransformPoint(position);
    }

}

