using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traslacion : MonoBehaviour
{

    //"destino" ser� el objeto con todas las piezas destino
    public Datos_Guardados destino;
    //"numeroDestino" corresponde al numero de archivo de guardado al que se acceder�
    public NumeroVariable numeroDestino;

    //Se adjuntan en Unity todas las piezas conectadas
    public GameObject R_BF, R_BM, R_B1, R_B2, R_M1, R_M2, R_MA;
    public GameObject F_BF, F_BM, F_B1, F_B2, F_M1, F_M2, F_MA;

    //Distancia recorrida por cada objeto
    public float D_BM, D_B1, D_B2, D_M1, D_M2, D_MA;
    public float C_BM, C_B1, C_B2, C_M1, C_M2, C_MA;

    /* "speedTest" es la velocidad con la cual se calcular� la distancia del punto A al B de cada pieza del robot.
     * "margen" es el margen de error para encontrar cada posici�n.*/
    public float speedTest = 1, margen;

    //los distintos flags que se usar�n para reducir la velocidad en casos de error o abortar el proceso y movilizar cada parte.
    public int flagAbort, flagAnimacion;

    public int debug;
    //Activada por el boton en canvas
    public void MoverDireccion()
    {
        //Se crea una variable para ver si existe o no 
        var dataFound = SaveLoadKuka.LoadData<Datos_Guardados>("Posicion " + numeroDestino.numeroDir);
        if (dataFound != null)
        {
            //Si existe, llena data con los archivos encontrados
            destino = dataFound;
            Debug.Log("Cargando destino para animar : Posicion " + numeroDestino.numeroDir);
            //Empieza el movimiento
            D_BM = 0;
            D_B1 = 0;
            D_B2 = 0;
            D_M1 = 0;
            D_M2 = 0;
            D_MA = 0;
            flagAbort = 0;
            flagAnimacion = 0;
            StartCoroutine("CalcularDistancia");
            StartCoroutine("Movimiento");
        }
        else
        {
            Debug.Log("ERROR, el destino no existe");
        }
    }

    public void Stop()
    {
        flagAnimacion = 0;
    }

    IEnumerator Espera()
    {
        yield return new WaitForSeconds(1);
    }

    IEnumerator CalcularDistancia()
    {
        D_BM = destino.BaseMovil.eulerAngles.y - R_BM.GetComponent<Transform>().localEulerAngles.y;
        D_B1 = destino.Brazo1.eulerAngles.x - R_B1.GetComponent<Transform>().localEulerAngles.x;
        D_B2 = destino.Brazo2.eulerAngles.x - R_B2.GetComponent<Transform>().localEulerAngles.x;
        D_M1 = destino.Muneca1.eulerAngles.y - R_M1.GetComponent<Transform>().localEulerAngles.y;
        D_M2 = destino.Muneca2.eulerAngles.x - R_M2.GetComponent<Transform>().localEulerAngles.x;
        D_MA = destino.Mano.eulerAngles.y - R_MA.GetComponent<Transform>().localEulerAngles.y;

        if (Mathf.Abs(D_BM) > 181)
        {
            if (D_BM > 0) D_BM = 360 - D_BM;
            else D_BM = -360 - D_BM;
        }
        if (Mathf.Abs(D_B1) > 181) 
        {
            if (D_B1 > 0) D_B1 = 360 - D_B1;
            else D_B1 = -360 - D_B1;
        }
        if (Mathf.Abs(D_B2) > 181) 
        {
            if (D_B2 > 0) D_B2 = 360 - D_B2;
            else D_B2 = -360 - D_B2;
        }
        if (Mathf.Abs(D_M1) > 181) 
        {
            if (D_M1 > 0) D_M1 = 360 - D_M1;
            else D_M1 = -360 - D_M1;
        }
        if (Mathf.Abs(D_M2) > 181) 
        {
            if (D_M2 > 0) D_M2 = 360 - D_M2;
            else D_M2 = -360 - D_M2;
        }
        if (Mathf.Abs(D_MA) > 181) 
        {
            if (D_MA > 0) D_MA = 360 - D_MA;
            else D_MA = -360 - D_MA;
        }

        C_BM = Mathf.Abs(D_BM);
        C_B1 = Mathf.Abs(D_B1);
        C_B2 = Mathf.Abs(D_B2);
        C_M1 = Mathf.Abs(D_M1);
        C_M2 = Mathf.Abs(D_M2);
        C_MA = Mathf.Abs(D_MA);
        yield return null;
    }

    public bool compare(float puntoInicio, float puntoFin, float verificadorInicio, float verificadorFin)
    {
        if ((puntoInicio < (puntoFin + margen) && puntoInicio > (puntoFin - margen)) && (verificadorInicio < (verificadorFin + 40) && verificadorInicio > (verificadorFin - 40)))
        {
            return false;
        }
        return true;
    }

    public void distancia(GameObject RobotIntermedio, GameObject RobotReal, float rangoMax, Vector3 moveVector, float distancia, int piezaActual)
    {
        //Busca el punto en un rango en de valores
        if (distancia >= -rangoMax && distancia <= rangoMax)
        {
            RobotIntermedio.GetComponent<Transform>().Rotate(moveVector);
            switch (piezaActual)
            {
                case 1:
                    D_BM = moveVector.z + D_BM;
                    if (D_BM == distancia) flagAbort = 20;
                    break;
                case 2:
                    D_B1 = moveVector.y + D_B1;
                    if (D_B1 == distancia) flagAbort = 20;
                    break;
                case 3:
                    D_B2 = moveVector.y + D_B2;
                    if (D_B2 == distancia) flagAbort = 20;
                    break;
                case 4:
                    D_M1 = moveVector.y + D_M1;
                    if (D_M1 == distancia) flagAbort = 20;
                    break;
                case 5:
                    D_M2 = moveVector.y + D_M2;
                    if (D_M2 == distancia) flagAbort = 20;
                    break;
                case 6:
                    D_MA = moveVector.x + D_MA;
                    if (D_MA == distancia) flagAbort = 20;
                    break;
            }
        }
        else
        {
            speedTest = speedTest / 2;
            speedTest = -speedTest;
            RobotIntermedio.GetComponent<Transform>().rotation = RobotReal.GetComponent<Transform>().rotation;
            switch (piezaActual)
            {
                case 1:
                    D_BM = 0;
                    break;
                case 2:
                    D_B1 = 0;
                    break;
                case 3:
                    D_B2 = 0;
                    break;
                case 4:
                    D_M1 = 0;
                    break;
                case 5:
                    D_M2 = 0;
                    break;
                case 6:
                    D_MA = 0;
                    break;
            }
            flagAbort= flagAbort+1;
        }
    }


    IEnumerator Movimiento()
    {
        while(C_BM > 0 || C_B1 > 0 || C_B2 > 0 || C_M1 > 0 || C_M2 > 0 || C_MA > 0)
        {
            //Movimiento Base
            if (C_BM > 0)
            {
                if (D_BM > 0) R_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, speedTest * 10) * Time.deltaTime);
                else R_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, -speedTest * 10) * Time.deltaTime);

                C_BM = C_BM - speedTest * 10 * Time.deltaTime;
            }
            //else R_BM.GetComponent<Transform>().rotation = destino.BaseMovil;
            //Movimiento Brazo 1
            if (C_B1 > 0)
            {
                if (D_B1 > 0) R_B1.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_B1.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                C_B1 = C_B1 - speedTest * 10 * Time.deltaTime;
            }
            //else R_B1.GetComponent<Transform>().rotation = destino.Brazo1;
            //Movimiento Brazo 2
            if (C_B2 > 0)
            {
                if (D_B2 > 0) R_B2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_B2.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                C_B2 = C_B2 - speedTest * 10 * Time.deltaTime;
            }
            //else R_B2.GetComponent<Transform>().rotation = destino.Brazo2;
            //Movimiento Muneca 1
            if (C_M1 > 0)
            {
                if (D_M1 > 0) R_M1.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_M1.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);
                
                C_M1 = C_M1 - speedTest * 10 * Time.deltaTime;
            }
            //else R_M1.GetComponent<Transform>().rotation = destino.Muneca1;
            //Movimiento Muneca 2
            if (C_M2 > 0)
            {
                if (D_M2 > 0) R_M2.GetComponent<Transform>().Rotate(new Vector3( 0, speedTest * 10, 0) * Time.deltaTime);
                else R_M2.GetComponent<Transform>().Rotate(new Vector3( 0, -speedTest * 10, 0) * Time.deltaTime);

                C_M2 = C_M2 - speedTest * 10 * Time.deltaTime;
            }
            //else R_M2.GetComponent<Transform>().rotation = destino.Muneca2;
            //Movimiento Mano
            if (C_MA > 0)
            {
                if (D_MA > 0) R_MA.GetComponent<Transform>().Rotate(new Vector3(speedTest * 10, 0, 0) * Time.deltaTime);
                else R_MA.GetComponent<Transform>().Rotate(new Vector3(-speedTest * 10, 0, 0) * Time.deltaTime);

                C_MA = C_MA - speedTest * 10 * Time.deltaTime;
            }
            //else R_MA.GetComponent<Transform>().rotation = destino.Mano;
            yield return null;
        }
        yield return null;
        //F_B1.GetComponent<Transform>().rotation = R_B1.GetComponent<Transform>().rotation;
        //F_B2.GetComponent<Transform>().rotation = R_B2.GetComponent<Transform>().rotation;
        //F_M1.GetComponent<Transform>().rotation = R_M1.GetComponent<Transform>().rotation;
        //F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        //F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;
    }
}

