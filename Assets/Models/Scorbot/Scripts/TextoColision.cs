using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextoColision : MonoBehaviour
{
    public Text textofull;
    public Text textoshort;
    public List<string> lista = new List<string>();
    
    public int addToLista(string objeto)
    {
        string textofinal= "";
        if (lista.Contains(objeto))
        {
            return lista.IndexOf(objeto);
        }

        lista.Add(objeto);
        foreach (string fila in lista)
        {
            textofinal = textofinal + fila + "\n";
        }
        //Debug.Log(textofinal + "FIN\n");
        textofull.text = textofinal;
        return lista.IndexOf(objeto);
    }

    public int removerOfLista(string objeto)
    {
        string textofinal = "";
        if (!lista.Contains(objeto))
        {
            return -1;
        }

        lista.Remove(objeto);

        foreach (string fila in lista)
        {
            textofinal = textofinal + fila + "\n";
        }
        textofull.text = textofinal;
        return -1;
    }

    private void Update()
    {
        if(lista.Count == 0)
        {
            textoshort.text = "Sin colisiones";
            textoshort.color = Color.green;
            textofull.text = "No hay colisiones que mostrar";
            textofull.color = Color.green;
        }
        else
        {
            textoshort.text = "Colisiones!";
            textoshort.color = Color.red;
            textofull.color = Color.red;
        }
    }

    
}
