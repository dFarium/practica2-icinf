using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traslacion : MonoBehaviour
{

    //Se cargara el archivo [numeroDestino] y se definira "destino" como la posicion final del robot
    public NumeroVariable numeroDestino;
    public Datos_Guardados destino;

    //Las piezas con una R inicial corresponden a el robot visible por el usuario
    public GameObject R_BF, R_BM, R_B1, R_B2, R_M1, R_M2, R_MA;
    //Las piezas con una F inicial corresponden a un robot invisible por el usuario, y es usado para realizar calculos
    public GameObject F_BF, F_BM, F_B1, F_B2, F_M1, F_M2, F_MA;

    //Distancia recorrida por cada pieza del robot
    public float D_BM, D_B1, D_B2, D_M1, D_M2, D_MA;

    //Valor absoluto de distancia recorrida por cada pieza del robot
    public float C_BM, C_B1, C_B2, C_M1, C_M2, C_MA;

    /* "speedReal" es la velocidad inicial para las pruebas
    "speedTest" es la velocidad con la cual se calculara la distancia del punto A al B de cada pieza del robot, disminuira por cada error
    "margen" es el margen de error para encontrar cada posicion*/
    public float speedReal, speedTest = 1, margen;

    //los distintos flags que se usar�n para reducir la velocidad en casos de error o abortar el proceso y movilizar cada parte.
    public int flagAbort, stop;

    //Activada por el boton en canvas
    public void MoverDireccion()
    {
        var dataFound = SaveLoadKuka.LoadData<Datos_Guardados>("Posicion " + numeroDestino.numeroDir);
        if (dataFound != null)
        {
            //Si existe, actualiza la variable "destino" con los valores encontrados
            destino = dataFound;
            Debug.Log("Cargando destino para animar : Posicion " + numeroDestino.numeroDir);
            //Se reinician las variables
            D_BM = 0;
            D_B1 = 0;
            D_B2 = 0;
            D_M1 = 0;
            D_M2 = 0;
            D_MA = 0;
            speedTest = speedReal;
            flagAbort = 0;
            stop = 0;
            //Calcula el movimiento que debe realizar
            StartCoroutine("CalcularDistancia");
            //Ejecuta el movimiento
            if (stop!=1) StartCoroutine("MovimientoSimple");
        }
        else
        {
            Debug.Log("ERROR, el destino no existe");
        }
    }

    //Funcion usada para debug
    public void Stop()
    {
        stop = 1;
    }

    IEnumerator CalcularDistancia()
    {
        //Calcula el movimiento de la Base -------------------------------------------------------------------------------------------------------------------------------------------------
        while (compare(F_BM.GetComponent<Transform>().localEulerAngles.y, destino.BaseMovil.eulerAngles.y, 0, 0) && flagAbort < 20)
        {
            distancia(F_BM, R_BM, 190, new Vector3(0, 0, speedTest), D_BM, 1);
        }
        if (flagAbort >= 20)
        {
            D_BM = 400;
        }
        F_BM.GetComponent<Transform>().rotation = destino.BaseMovil;
        speedTest = speedReal;
        flagAbort = 0;

        //Calcula el movimiento del brazo 1 ------------------------------------------------------------------------------------------------------------------------------------------------------
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
            D_B1 = 0;
        }
        F_B1.GetComponent<Transform>().rotation = destino.Brazo1;
        speedTest = speedReal;
        flagAbort = 0;

        //Calcula el movimiento del Brazo 2 ------------------------------------------------------------------------------------------------------------------------------------------------------
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
            D_B2 = 0;
        }
        F_B2.GetComponent<Transform>().rotation = destino.Brazo2;
        speedTest = speedReal;
        flagAbort = 0;

        //Calcula el movimiento de la Muneca 1 -----------------------------------------------------------------------------------------------------------------------------------------------------
        F_M1.GetComponent<Transform>().rotation = R_M1.GetComponent<Transform>().rotation;
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;

        while (compare(F_M1.GetComponent<Transform>().localEulerAngles.y, destino.Muneca1.eulerAngles.y, 0, 0) && flagAbort < 20)
        {
            distancia(F_M1, R_M1, 190, new Vector3(0, speedTest, 0), D_M1, 4);
        }
        if (flagAbort >= 20)
        {
            D_M1 = 0;
        }
        F_M1.GetComponent<Transform>().rotation = destino.Muneca1;
        speedTest = speedReal;
        flagAbort = 0;

        //Calcula el movimiento de la Muneca 2 ------------------------------------------------------------------------------------------------------------------------------------------------------
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;

        while (compare(F_M2.GetComponent<Transform>().localEulerAngles.x, destino.Muneca2.eulerAngles.x, 0, 0) && flagAbort < 20)
        {
            distancia(F_M2, R_M2, 230, new Vector3(0, speedTest, 0), D_M2, 5);
        }
        if (flagAbort >= 20)
        {
            D_M2 = 0;
        }
        speedTest = speedReal;
        flagAbort = 0;

        //Calcula el movimiento de la Mano -----------------------------------------------------------------------------------------------------------------------------------------------------
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;

        while (compare(F_MA.GetComponent<Transform>().localEulerAngles.y, destino.Mano.eulerAngles.y, 0, 0) && flagAbort < 20)
        {
            distancia(F_MA, R_MA, 190, new Vector3(speedTest, 0, 0), D_MA, 6);
        }
        if (flagAbort >= 20)
        {
            D_MA = 0;
        }
        F_MA.GetComponent<Transform>().rotation = destino.Mano;
        speedTest = speedReal;
        flagAbort = 0;

        //Cada movimiento se mide en distancia y se calcula el valor absoluto
        C_BM = Mathf.Abs(D_BM);
        C_B1 = Mathf.Abs(D_B1);
        C_B2 = Mathf.Abs(D_B2);
        C_M1 = Mathf.Abs(D_M1);
        C_M2 = Mathf.Abs(D_M2);
        C_MA = Mathf.Abs(D_MA);
        yield return null;
    }
    //Funcion que compara el punto de inicio y el de llegada
    public bool compare(float puntoInicio, float puntoFin, float verificadorInicio, float verificadorFin)
    {
        if ((puntoInicio < (puntoFin + margen) && puntoInicio > (puntoFin - margen)) && (verificadorInicio < (verificadorFin + 40) && verificadorInicio > (verificadorFin - 40)))
        {
            return false;
        }
        return true;
    }

    //Funcion que calcula la distancia
    public void distancia(GameObject RobotIntermedio, GameObject RobotReal, float rangoMax, Vector3 moveVector, float distancia, int piezaActual)
    {
        //Busca el punto en un rango en de valores
        if (distancia >= -rangoMax && distancia <= rangoMax)
        {
            //Caso correcto, mueve el robot
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
            //caso contrario, disminuye la velocidad de prueba y cambia el sentido
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

    //Ejecuta el movimiento
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
        //Calcula distancia de una forma más simple
        StartCoroutine("SimpleDistancia");
        //Ejecuta  la distancia calculada
        StartCoroutine("MovimientoCompleto");
        yield return null;
    }
    IEnumerator SimpleDistancia()
    {
        D_BM = destino.BaseMovil.eulerAngles.y - R_BM.GetComponent<Transform>().localEulerAngles.y;
        D_B1 = destino.Brazo1.eulerAngles.x - R_B1.GetComponent<Transform>().localEulerAngles.x;
        D_B2 = destino.Brazo2.eulerAngles.x - R_B2.GetComponent<Transform>().localEulerAngles.x;
        D_M1 = destino.Muneca1.eulerAngles.y - R_M1.GetComponent<Transform>().localEulerAngles.y;
        D_M2 = destino.Muneca2.eulerAngles.x - R_M2.GetComponent<Transform>().localEulerAngles.x;
        D_MA = destino.Mano.eulerAngles.y - R_MA.GetComponent<Transform>().localEulerAngles.y;

        //Si es mayor a 181, se realiza la resta para que realice una vuelta menor
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

    //Ejecuta  la distancia calculada
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
        //Finalmente carga las posiciones guardadas para una mayor presición
        R_BM.GetComponent<Transform>().rotation = destino.BaseMovil;
        R_B1.GetComponent<Transform>().rotation = destino.Brazo1;
        R_B2.GetComponent<Transform>().rotation = destino.Brazo2;
        R_M1.GetComponent<Transform>().rotation = destino.Muneca1;
        R_M2.GetComponent<Transform>().rotation = destino.Muneca2;
        R_MA.GetComponent<Transform>().rotation = destino.Mano;
        yield return null;
    }
}

