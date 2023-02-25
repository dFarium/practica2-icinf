using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionTraslado : MonoBehaviour
{

    public Datos_Guardados destino;
    public NumeroVariable numeroDestino;

    //Se adjuntan en Unity todas las piezas conectadas
    public GameObject BF, BM, B1, B2, M1, M2, MA;

    public void MoverDireccion()
    {
        //Se crea una variable para ver si existe o no 
        var dataFound = SaveLoadKuka.LoadData<Datos_Guardados>("Posicion " + numeroDestino.numeroDir);
        if (dataFound != null)
        {
            //Si existe, llena data con los archivos encontrados
            destino = dataFound;
            Debug.Log("Cargando destino " + numeroDestino.numeroDir);
        }
        else
        {
            Debug.Log("ERROR, el destino no existe");
        }

        
    }
}
