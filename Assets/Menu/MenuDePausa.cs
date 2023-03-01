using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDePausa : MonoBehaviour
{
    public CanvasGroup grupo;

    //Intercambiar Visibilidad
    void Start()
    {
        grupo = GetComponent<CanvasGroup>();
    }
    
    public void Mostrar()
    {
        grupo.alpha = 1;
        grupo.interactable = true;
        grupo.blocksRaycasts = true;
    }

    public void Ocultar()
    {
        grupo.alpha = 0;
        grupo.interactable = false;
        grupo.blocksRaycasts = false;
    }


    public void Pausar()
    {
        Time.timeScale = 0;
    }

    public void Reanudar()
    {
        Time.timeScale = 1;
    }

}
