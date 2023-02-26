using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionTraslado : MonoBehaviour
{

    //"destino" será el objeto con todas las piezas destino
    public Datos_Guardados destino;
    //"numeroDestino" corresponde al numero de archivo de guardado al que se accederá
    public NumeroVariable numeroDestino;

    //Se adjuntan en Unity todas las piezas conectadas
    public GameObject BF, BM, B1, B2, M1, M2, MA;

    //los distintos flags que se usarán para ir de una parte a otra
    public int flagStart=0, F_BF, F_BM, F_B1, F_B2, F_M1, F_M2, F_MA;

    //Activada por el boton en canvas
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

        //Empieza el movimiento
        flagStart = 1;
    }

    public void Stop()
    {
        flagStart = 0;
    }

    public void Update()
    {
        if (flagStart == 1)
        {

            BF.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, 3f) * Time.deltaTime);
        }
    }

}
