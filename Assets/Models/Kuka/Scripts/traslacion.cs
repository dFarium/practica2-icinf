using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traslacion : MonoBehaviour
{

    //"destino" será el objeto con todas las piezas destino
    public Datos_Guardados destino;
    //"numeroDestino" corresponde al numero de archivo de guardado al que se accederá
    public NumeroVariable numeroDestino;

    //Se adjuntan en Unity todas las piezas conectadas
    public GameObject R_BF, R_BM, R_B1, R_B2, R_M1, R_M2, R_MA;
    public GameObject F_BF, F_BM, F_B1, F_B2, F_M1, F_M2, F_MA;

    //Distancia recorrida por cada objeto
    public float D_BM, D_B1, D_B2, D_M1, D_M2, D_MA;

    //Valor absoluto de distancia recorrida por cada objeto
    public float C_BM, C_B1, C_B2, C_M1, C_M2, C_MA;

    /* "speedTest" es la velocidad con la cual se calculará la distancia del punto A al B de cada pieza del robot.
     * "margen" es el margen de error para encontrar cada posición.*/
    public float speedTest = 1, margen;

    //los distintos flags que se usarán para reducir la velocidad en casos de error o abortar el proceso y movilizar cada parte.
    public int flagAbort, stop;

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
            stop = 0;
            StartCoroutine("CalcularDistancia");
            if(stop!=1) StartCoroutine("MovimientoSimple");
        }
        else
        {
            Debug.Log("ERROR, el destino no existe");
        }
    }

    public void Stop()
    {
        stop = 1;
    }

    IEnumerator Espera()
    {
        yield return new WaitForSeconds(1);
    }

    IEnumerator CalcularDistancia()
    {
        //Empezamos con la Base -------------------------------------------------------------------------------------------------------------------------------------------------
        while (compare(F_BM.GetComponent<Transform>().localEulerAngles.y, destino.BaseMovil.eulerAngles.y, 0, 0) && flagAbort < 20)
        {
            distancia(F_BM, R_BM, 190, new Vector3(0, 0, speedTest), D_BM, 1);
        }
        if (flagAbort >= 20)
        {
            //F_BM.GetComponent<Transform>().rotation = destino.BaseMovil;
            D_BM = 400;
        }
        F_BM.GetComponent<Transform>().rotation = destino.BaseMovil;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve brazo 1 ------------------------------------------------------------------------------------------------------------------------------------------------------
        F_B1.GetComponent<Transform>().rotation = R_B1.GetComponent<Transform>().rotation;
        F_B2.GetComponent<Transform>().rotation = R_B2.GetComponent<Transform>().rotation;
        F_M1.GetComponent<Transform>().rotation = R_M1.GetComponent<Transform>().rotation;
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;

        while (compare(F_B1.GetComponent<Transform>().localEulerAngles.x, destino.Brazo1.eulerAngles.x, F_B1.GetComponent<Transform>().localEulerAngles.z, destino.Brazo1.eulerAngles.z) && flagAbort < 20)
        {
            distancia(F_B1, R_B1, 200, new Vector3(0, speedTest, 0), D_B1, 2);
        }
        if (flagAbort >= 20)
        {
            //F_B1.GetComponent<Transform>().rotation = destino.Brazo1;
            D_B1 = 0;
        }
        F_B1.GetComponent<Transform>().rotation = destino.Brazo1;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve Brazo 2 ------------------------------------------------------------------------------------------------------------------------------------------------------
        F_B2.GetComponent<Transform>().rotation = R_B2.GetComponent<Transform>().rotation;
        F_M1.GetComponent<Transform>().rotation = R_M1.GetComponent<Transform>().rotation;
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;

        while (compare(F_B2.GetComponent<Transform>().localEulerAngles.x, destino.Brazo2.eulerAngles.x, F_B2.GetComponent<Transform>().localEulerAngles.z, destino.Brazo2.eulerAngles.z) && flagAbort < 20)
        {
            distancia(F_B2, R_B2, 280, new Vector3(0, speedTest, 0), D_B2, 3);
        }
        if (flagAbort >= 20)
        {
            //F_B2.GetComponent<Transform>().rotation = destino.Brazo2;
            D_B2 = 0;
        }
        F_B2.GetComponent<Transform>().rotation = destino.Brazo2;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve Muneca 1 -----------------------------------------------------------------------------------------------------------------------------------------------------
        F_M1.GetComponent<Transform>().rotation = R_M1.GetComponent<Transform>().rotation;
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;

        while (compare(F_M1.GetComponent<Transform>().localEulerAngles.y, destino.Muneca1.eulerAngles.y, 0, 0) && flagAbort < 20)
        {
            distancia(F_M1, R_M1, 190, new Vector3(0, speedTest, 0), D_M1, 4);
        }
        if (flagAbort >= 20)
        {
            //F_M1.GetComponent<Transform>().rotation = destino.Muneca1;
            D_M1 = 0;
        }
        F_M1.GetComponent<Transform>().rotation = destino.Muneca1;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve Muneca 2 ------------------------------------------------------------------------------------------------------------------------------------------------------
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;

        while (compare(F_M2.GetComponent<Transform>().localEulerAngles.x, destino.Muneca2.eulerAngles.x, 0, 0) && flagAbort < 20)
        {
            distancia(F_M2, R_M2, 230, new Vector3(0, speedTest, 0), D_M2, 5);
        }
        if (flagAbort >= 20)
        {
            //F_M2.GetComponent<Transform>().rotation = destino.Muneca2;
            D_M2 = 0;
        }
        //F_M2.GetComponent<Transform>().rotation = destino.Muneca2;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve Mano -----------------------------------------------------------------------------------------------------------------------------------------------------
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;

        while (compare(F_MA.GetComponent<Transform>().localEulerAngles.y, destino.Mano.eulerAngles.y, 0, 0) && flagAbort < 20)
        {
            distancia(F_MA, R_MA, 190, new Vector3(speedTest, 0, 0), D_MA, 6);
        }
        if (flagAbort >= 20)
        {
            //F_MA.GetComponent<Transform>().rotation = destino.Mano;
            D_MA = 0;
        }
        F_MA.GetComponent<Transform>().rotation = destino.Mano;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

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


    IEnumerator MovimientoSimple()
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
            //Movimiento Brazo 1
            if (C_B1 > 0)
            {
                if (D_B1 > 0) R_B1.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_B1.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                C_B1 = C_B1 - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Brazo 2
            if (C_B2 > 0)
            {
                if (D_B2 > 0) R_B2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_B2.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                C_B2 = C_B2 - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Muneca 1
            if (C_M1 > 0)
            {
                if (D_M1 > 0) R_M1.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_M1.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);
                
                C_M1 = C_M1 - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Muneca 2
            if (C_M2 > 0)
            {
                if (D_M2 > 0) R_M2.GetComponent<Transform>().Rotate(new Vector3( 0, speedTest * 10, 0) * Time.deltaTime);
                else R_M2.GetComponent<Transform>().Rotate(new Vector3( 0, -speedTest * 10, 0) * Time.deltaTime);

                C_M2 = C_M2 - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Mano
            if (C_MA > 0)
            {
                if (D_MA > 0) R_MA.GetComponent<Transform>().Rotate(new Vector3(speedTest * 10, 0, 0) * Time.deltaTime);
                else R_MA.GetComponent<Transform>().Rotate(new Vector3(-speedTest * 10, 0, 0) * Time.deltaTime);

                C_MA = C_MA - speedTest * 10 * Time.deltaTime;
            }
            yield return null;
        }
        StartCoroutine("SimpleDistancia");
        StartCoroutine("MovimientoCompleto");
        yield return null;
    }
    IEnumerator SimpleDistancia()
    {
        D_BM = destino.BaseMovil.eulerAngles.y - R_BM.GetComponent<Transform>().localEulerAngles.y;
        Debug.Log("DIF BM " + destino.BaseMovil.eulerAngles.y + " y " + R_BM.GetComponent<Transform>().localEulerAngles.y +" = "+ D_BM);
        D_B1 = destino.Brazo1.eulerAngles.x - R_B1.GetComponent<Transform>().localEulerAngles.x;
        Debug.Log("DIF B1 " + destino.Brazo1.eulerAngles.y + " y " + R_B1.GetComponent<Transform>().localEulerAngles.y + " = " + D_B1);
        D_B2 = destino.Brazo2.eulerAngles.x - R_B2.GetComponent<Transform>().localEulerAngles.x;
        Debug.Log("DIF B2 " + destino.Brazo2.eulerAngles.y + " y " + R_B2.GetComponent<Transform>().localEulerAngles.y + " = " + D_B2);
        D_M1 = destino.Muneca1.eulerAngles.y - R_M1.GetComponent<Transform>().localEulerAngles.y;
        Debug.Log("DIF M1 " + destino.Muneca1.eulerAngles.y + " y " + R_M1.GetComponent<Transform>().localEulerAngles.y + " = " + D_M1);
        D_M2 = destino.Muneca2.eulerAngles.x - R_M2.GetComponent<Transform>().localEulerAngles.x;
        Debug.Log("DIF M2 " + destino.Muneca2.eulerAngles.y + " y " + R_M2.GetComponent<Transform>().localEulerAngles.y + " = " + D_M2);
        D_MA = destino.Mano.eulerAngles.y - R_MA.GetComponent<Transform>().localEulerAngles.y;
        Debug.Log("DIF MA " + destino.Mano.eulerAngles.y + " y " + R_MA.GetComponent<Transform>().localEulerAngles.y + " = " + D_MA);

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

    IEnumerator MovimientoCompleto()
    {
        while (C_BM > 0 || C_B1 > 0 || C_B2 > 0 || C_M1 > 0 || C_M2 > 0 || C_MA > 0)
        {
            //Movimiento Base
            if (C_BM > 0)
            {
                if (D_BM > 0) R_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, speedTest * 10) * Time.deltaTime);
                else R_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, -speedTest * 10) * Time.deltaTime);

                C_BM = C_BM - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Brazo 1
            if (C_B1 > 0)
            {
                if (D_B1 > 0) R_B1.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_B1.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                C_B1 = C_B1 - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Brazo 2
            if (C_B2 > 0)
            {
                if (D_B2 > 0) R_B2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_B2.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                C_B2 = C_B2 - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Muneca 1
            if (C_M1 > 0)
            {
                if (D_M1 > 0) R_M1.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_M1.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                C_M1 = C_M1 - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Muneca 2
            if (C_M2 > 0)
            {
                if (D_M2 > 0) R_M2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else R_M2.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                C_M2 = C_M2 - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Mano
            if (C_MA > 0)
            {
                if (D_MA > 0) R_MA.GetComponent<Transform>().Rotate(new Vector3(speedTest * 10, 0, 0) * Time.deltaTime);
                else R_MA.GetComponent<Transform>().Rotate(new Vector3(-speedTest * 10, 0, 0) * Time.deltaTime);

                C_MA = C_MA - speedTest * 10 * Time.deltaTime;
            }
            yield return null;
        }
        R_BM.GetComponent<Transform>().rotation = destino.BaseMovil;
        R_B1.GetComponent<Transform>().rotation = destino.Brazo1;
        R_B2.GetComponent<Transform>().rotation = destino.Brazo2;
        R_M1.GetComponent<Transform>().rotation = destino.Muneca1;
        R_M2.GetComponent<Transform>().rotation = destino.Muneca2;
        R_MA.GetComponent<Transform>().rotation = destino.Mano;
        yield return null;
    }
}

