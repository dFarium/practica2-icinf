using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbrirCerrarAcordeon : MonoBehaviour
{
    
    public Text textoBoton;
    public bool boton;
    public CanvasGroup corto,full;
    // Start is called before the first frame update

    public void abrirCerrar()
    {
        if (boton)
        {
            textoBoton.text = "Cerrar";
            corto.alpha = 0;
            corto.interactable = false;
            corto.blocksRaycasts = false;
            full.alpha = 1;
            full.interactable = true;
            full.blocksRaycasts = true;
            boton = false;
        }
        else
        {
            textoBoton.text = "Abrir";
            corto.alpha = 1;
            corto.interactable = true;
            corto.blocksRaycasts = true;
            full.alpha = 0;
            full.interactable = false;
            full.blocksRaycasts = false;
            boton = true;
        }
    }

}
