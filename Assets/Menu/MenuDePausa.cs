using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDePausa : MonoBehaviour
{
    public CanvasGroup grupo;

    void Start()
    {
        grupo = GetComponent<CanvasGroup>();
    }


    //Intercambiar Visibilidad
    public void Mostrar()
    {
        grupo.alpha = 1;
        grupo.interactable = true;
        grupo.blocksRaycasts = true;
    }

    //Intercambiar Visibilidad
    public void Ocultar()
    {
        grupo.alpha = 0;
        grupo.interactable = false;
        grupo.blocksRaycasts = false;
    }

    //Las fisicas se pausan
    public void Pausar()
    {
        Time.timeScale = 0;
    }

    //Las fisicas se reanudan
    public void Reanudar()
    {
        Time.timeScale = 1;
    }

}
