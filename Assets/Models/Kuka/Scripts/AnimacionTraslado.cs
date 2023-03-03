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
    public Vector3 D_BM, D_B1, D_B2, D_M1, D_M2, D_MA;

    //Coordenadas de Origen del objeto actual
    public Quaternion origen;

    /*"speed" es la velocidad con la cual se moverá el robot y se guarda el valor en "speedInicial",
    ya que speed varía dependiendo de los intentos hechos por el robot para encontrar la posición objetivo.
    "tolerancia" es el margen de error para encontrar dicha posición.*/
    public float speed=1, speedInicial, tolerancia;

    //los distintos flags que se usarán para movilizar cada parte, para reducir la velocidad en casos de error y abortar el proceso
    public int flagStart = 0, flagError, flagAbort, flagAnimacion;
    public float  C_BM, C_B1, C_B2, C_M1, C_M2, C_MA;

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
            //Empieza el movimiento
            D_BM = new Vector3(0f, 0f, 0f);
            D_B1 = new Vector3(0f, 0f, 0f);
            D_B2 = new Vector3(0f, 0f, 0f); 
            D_M1 = new Vector3(0f, 0f, 0f); 
            D_M2 = new Vector3(0f, 0f, 0f);
            D_MA = new Vector3(0f, 0f, 0f);
            origen = F_BM.GetComponent<Transform>().rotation;
            flagError = 0;
            flagAbort = 0;
            speedInicial = speed;
            flagStart = 1;
            StartCoroutine("CalcularDistancia");
        }
        else
        {
            Debug.Log("ERROR, el destino no existe");
        }
    }

    public void Stop()
    {
        D_BM = new Vector3(0f, 0f, 0f);
        D_B1 = new Vector3(0f, 0f, 0f);
        D_B2 = new Vector3(0f, 0f, 0f);
        D_M1 = new Vector3(0f, 0f, 0f);
        D_M2 = new Vector3(0f, 0f, 0f);
        D_MA = new Vector3(0f, 0f, 0f);
        flagStart = 0;
        flagError = 0;
        flagAbort = 0;
        flagAnimacion = 0;
        speedInicial = speed;
    }

    //public void Update()
    IEnumerator CalcularDistancia()
    {
        while (flagStart != 7)
        {

            switch (flagStart)
            {
                //Se mueve la base
                case 1:
                    calcular(F_BM.GetComponent<Transform>().rotation.eulerAngles.y, destino.BaseMovil.eulerAngles.y, 2, F_BM.GetComponent<Transform>(), F_B1.GetComponent<Transform>(), 190, D_BM.z, new Vector3(0f, 0f, speed), destino.BaseMovil, 0f, 0f);
                    break;
                //Se mueve brazo 1
                case 2:
                    calcular(F_B1.GetComponent<Transform>().rotation.eulerAngles.x, destino.Brazo1.eulerAngles.x, 3, F_B1.GetComponent<Transform>(), F_B2.GetComponent<Transform>(), 200, D_B1.y, new Vector3(0f, speed, 0f), destino.Brazo1, F_B1.GetComponent<Transform>().rotation.eulerAngles.z, destino.Brazo1.eulerAngles.z);
                    break;
                //Se mueve brazo 2
                case 3:
                    calcular(F_B2.GetComponent<Transform>().rotation.eulerAngles.x, destino.Brazo2.eulerAngles.x, 4, F_B2.GetComponent<Transform>(), F_M1.GetComponent<Transform>(), 270, D_B2.y, new Vector3(0f, speed, 0f), destino.Brazo2, F_B2.GetComponent<Transform>().rotation.eulerAngles.z, destino.Brazo2.eulerAngles.z);
                    break;
                //Se mueve muneca 1
                case 4:
                    calcular(F_M1.GetComponent<Transform>().rotation.eulerAngles.y, destino.Muneca1.eulerAngles.y, 5, F_M1.GetComponent<Transform>(), F_M2.GetComponent<Transform>(), 360, D_M1.x, new Vector3(speed, 0f, 0f), destino.Muneca1, 0f, 0f);
                    break;
                //Se mueve muneca 2
                case 5:
                    calcular(F_M2.GetComponent<Transform>().rotation.eulerAngles.x, destino.Muneca2.eulerAngles.x, 6, F_M2.GetComponent<Transform>(), F_MA.GetComponent<Transform>(), 235, D_M2.y, new Vector3(0f, speed, 0f), destino.Muneca2, F_M2.GetComponent<Transform>().rotation.eulerAngles.z, destino.Muneca2.eulerAngles.z);
                    break;
                //Se mueve mano
                case 6:
                    calcular(F_MA.GetComponent<Transform>().rotation.eulerAngles.y, destino.Mano.eulerAngles.y, 7, F_MA.GetComponent<Transform>(), F_BM.GetComponent<Transform>(), 190, D_MA.x, new Vector3(speed, 0f, 0f), destino.Mano, 0, 0);
                    break;
                default:
                    break;
            }
        }
        C_BM = Mathf.Abs(D_BM.z);
        C_B1 = Mathf.Abs(D_B1.y);
        C_B2 = Mathf.Abs(D_B2.y);
        C_M1 = Mathf.Abs(D_M1.x);
        C_M2 = Mathf.Abs(D_M2.y);
        C_MA = Mathf.Abs(D_MA.x);
        speed = speedInicial;
        flagStart = 0;
        flagAnimacion = 1;
        yield return null;
    }

    public bool compare(float puntoInicio, float puntoFin, float verificadorInicio, float verificadorFin)
    {
        if ( (puntoInicio < (puntoFin + tolerancia) && puntoInicio > (puntoFin - tolerancia)) && (verificadorInicio < (verificadorFin + 40) && verificadorInicio > (verificadorFin - 40)))
        {
            return true;
        }
        return false;
    }

    public void calcular(float eulerInicio, float eulerFin, int sgteFlag, Transform objeto1, Transform objeto2, int rangoMax, float distancia, Vector3 vectorMove, Quaternion cargado, float v1, float v2)
    {
        //Si no ha encontrado la posición después de 5 intentos, disminuye velocidad
        if (flagError > 5)
        {
            speed = speed/2;
            flagError = 0;
        }
        //Si es True, termina la función y continua con el siguiente objeto
        if (compare(eulerInicio, eulerFin, v1 , v2) || flagAbort == 25)
        {
            origen = objeto2.rotation;
            objeto1.rotation = cargado;
            flagError = 0;
            flagAbort = 0;
            speed = speedInicial;
            flagStart = sgteFlag;
        }
        else
        {
            //Busca el punto en un rango en de valores
            if (distancia > -rangoMax && distancia < rangoMax)
            {
                objeto1.Rotate(vectorMove);
                switch (flagStart)
                {
                    case 1:
                        D_BM = vectorMove + D_BM;
                        break;
                    case 2:
                        D_B1 = vectorMove + D_B1;
                        break;
                    case 3:
                        D_B2 = vectorMove + D_B2;
                        break;
                    case 4:
                        D_M1 = vectorMove + D_M1;
                        break;
                    case 5:
                        D_M2 = vectorMove + D_M2;
                        break;
                    case 6:
                        D_MA = vectorMove + D_MA;
                        break;
                }
                
            }
            else
            {
                //Si despues de buscar en un sentido no encuentra el punto, cambia a la siguiente dirección
                switch (flagStart)
                {
                    case 1:
                        D_BM = new Vector3(0f, 0f, 0f);
                        break;
                    case 2:
                        D_B1 = new Vector3(0f, 0f, 0f);
                        break;
                    case 3:
                        D_B2 = new Vector3(0f, 0f, 0f);
                        break;
                    case 4:
                        D_M1 = new Vector3(0f, 0f, 0f);
                        break;
                    case 5:
                        D_M2 = new Vector3(0f, 0f, 0f);
                        break;
                    case 6:
                        D_MA = new Vector3(0f, 0f, 0f);
                        break;
                }
                objeto1.rotation = origen;
                flagError++;
                flagAbort++;
                speed = -speed;
            }
        }
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
                if (D_BM.z>0)
                {
                    R_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, speed*10) * Time.deltaTime);
                }
                else
                {
                    R_BM.GetComponent<Transform>().Rotate(new Vector3(0, 0, -speed*10) * Time.deltaTime);
                }
                C_BM= C_BM - speed * 10 * Time.deltaTime;
            }
            //else
            //{
            //    R_BM.GetComponent<Transform>().rotation = F_BM.GetComponent<Transform>().rotation;
            //}
            if (C_B1 > 0)
            {
                if (D_B1.y > 0)
                {
                    R_B1.GetComponent<Transform>().Rotate(new Vector3(0, speed*10, 0) * Time.deltaTime);
                }
                else
                {
                    R_B1.GetComponent<Transform>().Rotate(new Vector3(0, -speed*10, 0) * Time.deltaTime);
                }
                C_B1 = C_B1 - speed * 10 * Time.deltaTime;
            }
            //else
            //{
            //    //R_B1.GetComponent<Transform>().rotation = F_B1.GetComponent<Transform>().rotation;
            //}
            if (C_B2 > 0)
            {
                if (D_B2.y > 0)
                {
                    R_B2.GetComponent<Transform>().Rotate(new Vector3(0, speed*10, 0) * Time.deltaTime);
                }
                else
                {
                    R_B2.GetComponent<Transform>().Rotate(new Vector3(0, -speed*10, 0) * Time.deltaTime);
                }
                C_B2 = C_B2 - speed * 10 * Time.deltaTime;
            }
            //else
            //{
            //    //R_B2.GetComponent<Transform>().rotation = F_B2.GetComponent<Transform>().rotation;
            //}
            if (C_M1 > 0)
            {
                if (D_M1.x > 0)
                {
                    R_M1.GetComponent<Transform>().Rotate(new Vector3(speed * 10, 0, 0) * Time.deltaTime);
                }
                else
                {
                    R_M1.GetComponent<Transform>().Rotate(new Vector3(-speed * 10, 0, 0) * Time.deltaTime);
                }
                C_M1 = C_M1 - speed * 10 * Time.deltaTime;
            }
            if (C_M2 > 0)
            {
                if (D_M2.y > 0)
                {
                    R_M2.GetComponent<Transform>().Rotate(new Vector3(0, speed * 10, 0) * Time.deltaTime);
                }
                else
                {
                    R_M2.GetComponent<Transform>().Rotate(new Vector3(0, -speed * 10, 0) * Time.deltaTime);
                }
                C_M2 = C_M2 - speed * 10 * Time.deltaTime;
            }
            if (C_MA > 0)
            {
                if (D_MA.x > 0)
                {
                    R_MA.GetComponent<Transform>().Rotate(new Vector3(speed * 10, 0, 0) * Time.deltaTime);
                }
                else
                {
                    R_MA.GetComponent<Transform>().Rotate(new Vector3(-speed * 10, 0, 0) * Time.deltaTime);
                }
                C_MA = C_MA - speed * 10 * Time.deltaTime;
            }
        }
    }
}

