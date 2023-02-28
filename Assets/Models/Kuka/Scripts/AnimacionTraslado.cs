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

    public float debug1, debug2;

    //los distintos flags que se usarán para ir de una parte a otra
    public int flagStart=0;

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
        switch (flagStart)
        {
            //Se mueve la base
            case 1:
                if (compare(BM.GetComponent<Transform>().rotation.eulerAngles.y, destino.BaseMovil.eulerAngles.y))
                {
                    Debug.Log("Sali BM");
                    flagStart = 2;
                    //AGREGAR LOAD Posicion
                }
                BM.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, 10f) * Time.deltaTime);
                break;

            //Se mueve brazo 1
            case 2:
                if (compare(B1.GetComponent<Transform>().rotation.eulerAngles.x, destino.Brazo1.eulerAngles.x))
                {
                    Debug.Log("Sali B1");
                    flagStart = 3;
                    //AGREGAR LOAD Posicion
                }
                B1.GetComponent<Transform>().Rotate(new Vector3(0f, -10f, 0f) * Time.deltaTime);
                break;

            //Se mueve brazo 2
            case 3:
                if (compare(B2.GetComponent<Transform>().rotation.eulerAngles.x, destino.Brazo2.eulerAngles.x))
                {
                    Debug.Log("Sali B2");
                    flagStart = 4;
                    //AGREGAR LOAD Posicion
                }
                B2.GetComponent<Transform>().Rotate(new Vector3(0f, -10f, 0f) * Time.deltaTime);
                break;
            //Se mueve muneca 1
            case 4:
                if (compare(M1.GetComponent<Transform>().rotation.eulerAngles.y, destino.Muneca1.eulerAngles.y))
                {
                    Debug.Log("Sali M1");
                    flagStart = 5;
                    //AGREGAR LOAD Posicion
                }
                M1.GetComponent<Transform>().Rotate(new Vector3(10f, 0f, 0f) * Time.deltaTime);
                break;
            //Se mueve muneca 2
            case 5:
                if (compare(M2.GetComponent<Transform>().rotation.eulerAngles.x, destino.Muneca2.eulerAngles.x))
                {
                    Debug.Log("Sali M2");
                    flagStart = 6;
                    //AGREGAR LOAD Posicion
                }
                M2.GetComponent<Transform>().Rotate(new Vector3(0f, -10f, 0f) * Time.deltaTime);
                break;
            //Se mueve mano
            case 6:
                Debug.Log("Entre MA");
                if (compare(MA.GetComponent<Transform>().rotation.eulerAngles.y, destino.Mano.eulerAngles.y))
                {
                    Debug.Log("Sali MA");
                    flagStart = 0;
                    //AGREGAR LOAD Posicion
                }
                MA.GetComponent<Transform>().Rotate(new Vector3(10f, 0f, 0f) * Time.deltaTime);
                break;
            default:
                break;
        }

    }

    public bool compare(float A, float B)
    {
        debug1 = A;
        debug2 = B;
        if (A < (B + 0.1) && A > (B - 0.1))
        {
            return true;
        }
        return false;
    }
}
