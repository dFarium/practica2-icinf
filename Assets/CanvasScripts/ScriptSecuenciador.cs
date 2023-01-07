using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Este script se encarga de secuenciar todos las posiciones de guardado indicadas en la barra de input, debajo del boton "Secuenciador"
public class ScriptSecuenciador : MonoBehaviour
{
    public TestSave cargar;
    public Text textoSecuencia;
    public string[] corregido;
    public Button boton;
    public Button botonTomar;

    public void test()
    {
        //cambiamos el texto que ingreso el usuario a string 
        string transicion;
        transicion = textoSecuencia.text.ToString();
        //cambiamos el string a una lista de objetos
        corregido = transicion.Split('-');//Cambia de un string 1-2-3 a un array
        StartCoroutine("CargadoProgresivo");   //Se llama a la corrutina

    }

    IEnumerator CargadoProgresivo()//Realiza el cambiado progresivo
    {
        int i;
        cambiarBoton();
        for (i = 0; i < corregido.Length; i++)
        {
            float numero = 1F;
            cargar.loadData(int.Parse(corregido[i]));
            yield return new WaitForSecondsRealtime(numero);//Espera 1 segundo
        }
        cambiarBoton();
    }

    public void cambiarBoton()//Desactiva los botones mientras se esta ejecutando la secuencia
    {
        boton.interactable = !boton.interactable;
        botonTomar.interactable = !botonTomar.interactable;
    }
}
