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

    public Vector3 recorrido , finalidad ,debug4;

    public Quaternion origen;

    public float speed=10;

    //los distintos flags que se usarán para ir de una parte a otra
    public int flagStart=0;

    public void Start()
    {
        recorrido = new Vector3(0f, 0f, 0f);
    }

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
        recorrido = new Vector3(0f, 0f, 0f);
        origen = BM.GetComponent<Transform>().rotation;
        flagStart = 1;
    }

    public void Stop()
    {
        recorrido = new Vector3(0f, 0f, 0f);
        flagStart = 0;
    }

    public void Update()
    {
        switch (flagStart)
        {
            //Se mueve la base
            case 1:
                debug4 = BM.GetComponent<Transform>().rotation.eulerAngles;
                calcular(BM.GetComponent<Transform>().rotation.eulerAngles.y, destino.BaseMovil.eulerAngles.y, 2, BM.GetComponent<Transform>(), B1.GetComponent<Transform>(), 190, recorrido.z, new Vector3(0f, 0f, speed), destino.BaseMovil, 0f,0f);
                break;
            //Se mueve brazo 1
            case 2:
                debug4 = B1.GetComponent<Transform>().rotation.eulerAngles;
                calcular(B1.GetComponent<Transform>().rotation.eulerAngles.x, destino.Brazo1.eulerAngles.x, 3, B1.GetComponent<Transform>(), B2.GetComponent<Transform>(), 200, recorrido.y, new Vector3(0f, speed, 0f), destino.Brazo1, B1.GetComponent<Transform>().rotation.eulerAngles.z, destino.Brazo1.eulerAngles.z);
                break;
            //Se mueve brazo 2
            case 3:
                debug4 = B2.GetComponent<Transform>().rotation.eulerAngles;
                calcular(B2.GetComponent<Transform>().rotation.eulerAngles.x, destino.Brazo2.eulerAngles.x, 4, B2.GetComponent<Transform>(), M1.GetComponent<Transform>(), 270, recorrido.y, new Vector3(0f, speed, 0f), destino.Brazo2, B2.GetComponent<Transform>().rotation.eulerAngles.z, destino.Brazo2.eulerAngles.z);
                break;
            //Se mueve muneca 1
            case 4:
                debug4 = M1.GetComponent<Transform>().rotation.eulerAngles;
                calcular(M1.GetComponent<Transform>().rotation.eulerAngles.y, destino.Muneca1.eulerAngles.y, 5, M1.GetComponent<Transform>(), M2.GetComponent<Transform>(), 360, recorrido.x, new Vector3(speed, 0f, 0f), destino.Muneca1,0f,0f);
                break;
            //Se mueve muneca 2
            case 5:
                debug4 = M2.GetComponent<Transform>().rotation.eulerAngles;
                calcular(M2.GetComponent<Transform>().rotation.eulerAngles.x, destino.Muneca2.eulerAngles.x, 6, M2.GetComponent<Transform>(), MA.GetComponent<Transform>(), 235, recorrido.y, new Vector3(0f, speed, 0f), destino.Muneca2, M2.GetComponent<Transform>().rotation.eulerAngles.z, destino.Muneca2.eulerAngles.z);
                break;
            //Se mueve mano
            case 6:
                debug4 = MA.GetComponent<Transform>().rotation.eulerAngles;
                finalidad = destino.Mano.eulerAngles;
                calcular(MA.GetComponent<Transform>().rotation.eulerAngles.y, destino.Mano.eulerAngles.y, 0, MA.GetComponent<Transform>(), BM.GetComponent<Transform>(), 190, recorrido.x, new Vector3(speed, 0f, 0f), destino.Mano,0,0);
                break;
            default:
                break;
        }

    }

    //public bool compare(float puntoInicio, float puntoFin)
    public bool compare(float puntoInicio, float puntoFin, float verificadorInicio, float verificadorFin)
    {
        debug1 = puntoInicio;
        debug2 = puntoFin;
        if ( (puntoInicio < (puntoFin + 0.1) && puntoInicio > (puntoFin - 0.1)) && (verificadorInicio < (verificadorFin + 40) && verificadorInicio > (verificadorFin - 40)))
        {
            return true;
        }
        return false;
    }

    public void calcular(float eulerInicio, float eulerFin, int sgteFlag, Transform objeto1, Transform objeto2, int rangoMax, float distancia, Vector3 vectorMove, Quaternion cargado, float v1, float v2)
    {
        if (compare(eulerInicio, eulerFin, v1 , v2))
        {
            origen = objeto2.rotation;
            objeto1.rotation = cargado;
            flagStart = sgteFlag;
        }
        else
        {
            if (distancia > -rangoMax && distancia < rangoMax)
            {
                objeto1.Rotate(vectorMove * Time.deltaTime);
                recorrido = vectorMove * Time.deltaTime + recorrido;
            }
            else
            {
                recorrido = new Vector3(0f, 0f, 0f);
                objeto1.rotation = origen;
                speed = -speed;
            }
        }
    }


}
