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
    public GameObject R_BF, R_BM, R_B1, R_B2, R_M1, R_M2, R_MA;
    public GameObject F_BF, F_BM, F_B1, F_B2, F_M1, F_M2, F_MA;

    //Distancia recorrida por cada objeto
    public float D_BM, D_B1, D_B2, D_M1, D_M2, D_MA;
    public float C_BM, C_B1, C_B2, C_M1, C_M2, C_MA;

    /* "speedTest" es la velocidad con la cual se calculará la distancia del punto A al B de cada pieza del robot.
     * "margen" es el margen de error para encontrar cada posición.*/
    public float speedTest = 1, margen;

    //los distintos flags que se usarán para reducir la velocidad en casos de error o abortar el proceso y movilizar cada parte.
    public int flagAbort, flagAnimacion;

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
            for (int i = 0; i < 1; i++)
            {
                StartCoroutine("CalcularDistancia");
            }

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

    IEnumerator CalcularDistancia()
    {
        //Empezamos con la Base -------------------------------------------------------------------------------------------------------------------------------------------------
        while (compare(F_BM.GetComponent<Transform>().rotation.eulerAngles.y, destino.BaseMovil.eulerAngles.y, 0, 0) && flagAbort < 10)
        {
            //Busca el punto en un rango en de valores
            if (D_BM >= -190 && D_BM <= 190)
            {
                //F_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, speedTest));
                //D_BM = (new Vector3(0, 0, speedTest)).z + D_BM;
                F_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, speedTest) * Time.deltaTime);
                D_BM = (new Vector3(0, 0, speedTest) * Time.deltaTime).z + D_BM;
                yield return null;
            }
            else
            {
                speedTest = speedTest / 2;
                speedTest = -speedTest;
                F_BM.GetComponent<Transform>().rotation = R_BM.GetComponent<Transform>().rotation;
                D_BM = 0;
                flagAbort++;
            }
        }
        if (flagAbort >= 10)
        {
            F_BM.GetComponent<Transform>().rotation = destino.BaseMovil;
            D_BM = 400;
        }
        //F_BM.GetComponent<Transform>().rotation = destino.BaseMovil;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve brazo 1 ------------------------------------------------------------------------------------------------------------------------------------------------------
        F_B1.GetComponent<Transform>().rotation = R_B1.GetComponent<Transform>().rotation;
        F_B2.GetComponent<Transform>().rotation = R_B2.GetComponent<Transform>().rotation;
        F_M1.GetComponent<Transform>().rotation = R_M1.GetComponent<Transform>().rotation;
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;
        while (compare(F_B1.GetComponent<Transform>().rotation.eulerAngles.x, destino.Brazo1.eulerAngles.x, F_B1.GetComponent<Transform>().rotation.eulerAngles.z, destino.Brazo1.eulerAngles.z) && flagAbort < 10)
        {
            //Busca el punto en un rango en de valores
            if (D_B1 >= -200 && D_B1 <= 200)
            {
                //F_B1.GetComponent<Transform>().Rotate(new Vector3(0, speedTest, 0));
                //D_B1 = (new Vector3(0, speedTest, 0)).y + D_B1;
                F_B1.GetComponent<Transform>().Rotate(new Vector3(0, speedTest, 0) * Time.deltaTime);
                D_B1 = (new Vector3(0, speedTest, 0) * Time.deltaTime).y + D_B1;
                yield return null;
            }
            else
            {
                speedTest = speedTest / 2;
                speedTest = -speedTest;
                F_B1.GetComponent<Transform>().rotation = R_B1.GetComponent<Transform>().rotation;
                D_B1 = 0;
                flagAbort++;
            }
        }
        if (flagAbort >= 10)
        {
            F_B1.GetComponent<Transform>().rotation = destino.Brazo1;
            D_B1 = 400;
        }
        //F_B1.GetComponent<Transform>().rotation = destino.Brazo1;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve Brazo 2 ------------------------------------------------------------------------------------------------------------------------------------------------------
        F_B2.GetComponent<Transform>().rotation = R_B2.GetComponent<Transform>().rotation;
        F_M1.GetComponent<Transform>().rotation = R_M1.GetComponent<Transform>().rotation;
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;
        while (compare(F_B2.GetComponent<Transform>().rotation.eulerAngles.x, destino.Brazo2.eulerAngles.x, F_B2.GetComponent<Transform>().rotation.eulerAngles.z, destino.Brazo2.eulerAngles.z) && flagAbort < 10)
        {
            //Busca el punto en un rango en de valores
            if (D_B2 >= -280 && D_B2 <= 280)
            {
                //F_B2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest, 0));
                //D_B2 = (new Vector3(0, speedTest, 0)).y + D_B2;
                F_B2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest, 0) * Time.deltaTime);
                D_B2 = (new Vector3(0, speedTest, 0) * Time.deltaTime).y + D_B2;
                yield return null;
            }
            else
            {
                speedTest = speedTest / 2;
                speedTest = -speedTest;
                F_B2.GetComponent<Transform>().rotation = R_B2.GetComponent<Transform>().rotation;
                D_B2 = 0;
                flagAbort++;
            }
        }
        if (flagAbort >= 10)
        {
            D_B2 = 400;
            F_B2.GetComponent<Transform>().rotation = destino.Brazo2;
        }
        //F_B2.GetComponent<Transform>().rotation = destino.Brazo2;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve Muneca 1 -----------------------------------------------------------------------------------------------------------------------------------------------------
        F_M1.GetComponent<Transform>().rotation = R_M1.GetComponent<Transform>().rotation;
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;
        while (compare(F_M1.GetComponent<Transform>().rotation.eulerAngles.y, destino.Muneca1.eulerAngles.y, 0, 0) && flagAbort < 10)
        {
            //Busca el punto en un rango en de valores
            if (D_M1 >= -190 && D_M1 <= 190)
            {
                //F_M1.GetComponent<Transform>().Rotate(new Vector3(speedTest, 0, 0));
                //D_M1 = (new Vector3(speedTest, 0, 0)).x + D_M1;
                F_M1.GetComponent<Transform>().Rotate(new Vector3(speedTest, 0, 0) * Time.deltaTime);
                D_M1 = (new Vector3(speedTest, 0, 0) * Time.deltaTime).x + D_M1;
                yield return null;
            }
            else
            {
                speedTest = speedTest / 2;
                speedTest = -speedTest;
                F_M1.GetComponent<Transform>().rotation = R_M1.GetComponent<Transform>().rotation;
                D_M1 = 0;
                flagAbort++;
            }
        }
        if (flagAbort >= 10)
        {
            F_M1.GetComponent<Transform>().rotation = destino.Muneca1;
            D_M1 = 400;
        }
        //F_M1.GetComponent<Transform>().rotation = destino.Muneca1;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve Muneca 2 ------------------------------------------------------------------------------------------------------------------------------------------------------
        F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;
        while (compare(F_M2.GetComponent<Transform>().rotation.eulerAngles.x, destino.Muneca2.eulerAngles.x, F_M2.GetComponent<Transform>().rotation.eulerAngles.z, destino.Muneca2.eulerAngles.z) && flagAbort < 10)
        {
            //Busca el punto en un rango en de valores
            if (D_M2 >= -240 && D_M2 <= 240)
            {
                //F_M2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest, 0));
                //D_M2 = (new Vector3(0, speedTest, 0)).y + D_M2;
                F_M2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest, 0) * Time.deltaTime);
                D_M2 = (new Vector3(0, speedTest, 0) * Time.deltaTime).y + D_M2;
                yield return null;
            }
            else
            {
                speedTest = speedTest / 2;
                speedTest = -speedTest;
                F_M2.GetComponent<Transform>().rotation = R_M2.GetComponent<Transform>().rotation;
                D_M2 = 0;
                flagAbort++;
            }
        }
        if (flagAbort >= 10)
        {
            F_M2.GetComponent<Transform>().rotation = destino.Muneca2;
            D_M2 = 400;
        }
        //F_M2.GetComponent<Transform>().rotation = destino.Muneca2;
        speedTest = Mathf.Abs(speedTest * Mathf.Pow(2, flagAbort));
        flagAbort = 0;

        //Se mueve Mano -----------------------------------------------------------------------------------------------------------------------------------------------------
        F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;
        while (compare(F_MA.GetComponent<Transform>().rotation.eulerAngles.y, destino.Mano.eulerAngles.y, 0, 0) && flagAbort < 10)
        {
            //Busca el punto en un rango en de valores
            if (D_MA >= -190 && D_MA <= 190)
            {
                //F_MA.GetComponent<Transform>().Rotate(new Vector3(speedTest, 0, 0));
                //D_MA = (new Vector3(speedTest, 0, 0)).x + D_MA;
                F_MA.GetComponent<Transform>().Rotate(new Vector3(speedTest, 0, 0) * Time.deltaTime);
                D_MA = (new Vector3(speedTest, 0, 0) * Time.deltaTime).x + D_MA;
                yield return null;
            }
            else
            {
                speedTest = speedTest / 2;
                speedTest = -speedTest;
                F_MA.GetComponent<Transform>().rotation = R_MA.GetComponent<Transform>().rotation;
                D_MA = 400;
                flagAbort++;
            }
        }
        if (flagAbort >= 10)
        {
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
        flagAnimacion = 1;
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

    public void Update()
    {
        if (C_BM < 0 && C_B1 < 0 && C_B2 < 0 && C_M1 < 0 && C_M2 < 0 && C_MA < 0)
        {
            flagAnimacion = 0;
        }
        if (flagAnimacion == 1)
        {
            if (C_BM > 0)
            {
                if (D_BM > 0)
                {
                    R_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, speedTest * 10) * Time.deltaTime);
                }
                else
                {
                    R_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, -speedTest * 10) * Time.deltaTime);
                }
                C_BM = C_BM - speedTest * 10 * Time.deltaTime;
            }
            //else
            //{
            //    R_BM.GetComponent<Transform>().rotation = F_BM.GetComponent<Transform>().rotation;
            //}
            if (C_B1 > 0)
            {
                if (D_B1 > 0)
                {
                    R_B1.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                }
                else
                {
                    R_B1.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);
                }
                C_B1 = C_B1 - speedTest * 10 * Time.deltaTime;
            }
            //else
            //{
            //    //R_B1.GetComponent<Transform>().rotation = F_B1.GetComponent<Transform>().rotation;
            //}
            if (C_B2 > 0)
            {
                if (D_B2 > 0)
                {
                    R_B2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                }
                else
                {
                    R_B2.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);
                }
                C_B2 = C_B2 - speedTest * 10 * Time.deltaTime;
            }
            //else
            //{
            //    //R_B2.GetComponent<Transform>().rotation = F_B2.GetComponent<Transform>().rotation;
            //}
            if (C_M1 > 0)
            {
                if (D_M1 > 0)
                {
                    R_M1.GetComponent<Transform>().Rotate(new Vector3(speedTest * 10, 0, 0) * Time.deltaTime);
                }
                else
                {
                    R_M1.GetComponent<Transform>().Rotate(new Vector3(-speedTest * 10, 0, 0) * Time.deltaTime);
                }
                C_M1 = C_M1 - speedTest * 10 * Time.deltaTime;
            }
            if (C_M2 > 0)
            {
                if (D_M2 > 0)
                {
                    R_M2.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                }
                else
                {
                    R_M2.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);
                }
                C_M2 = C_M2 - speedTest * 10 * Time.deltaTime;
            }
            if (C_MA > 0)
            {
                if (D_MA > 0)
                {
                    R_MA.GetComponent<Transform>().Rotate(new Vector3(speedTest * 10, 0, 0) * Time.deltaTime);
                }
                else
                {
                    R_MA.GetComponent<Transform>().Rotate(new Vector3(-speedTest * 10, 0, 0) * Time.deltaTime);
                }
                C_MA = C_MA - speedTest * 10 * Time.deltaTime;
            }
        }
    }
}

