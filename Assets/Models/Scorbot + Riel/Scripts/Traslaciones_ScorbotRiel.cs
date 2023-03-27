using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traslaciones_ScorbotRiel : MonoBehaviour
{

    //Se cargara el archivo [numeroDestino] y se definira "destino" como la posicion final del robot
    public NumeroVariable numeroDestino;
    public DatosGuardadosScorbotRiel destino;

    //Las piezas con una R inicial corresponden a el robot visible por el usuario
    public GameObject Riel, RH, RBR, RAB, RMU, RMA;
    //Las piezas con una F inicial corresponden a un robot invisible por el usuario, y es usado para realizar calculos
    public GameObject FH, FBR, FAB, FMU, FMA;

    //Distancia recorrida por cada pieza del robot
    public float DRiel, DH, DBR, DAB, DMU, DMA;

    //Valor absoluto de distancia recorrida por cada pieza del robot
    public float CRiel, CH, CBR, CAB, CMU, CMA;

    /* "speedReal" es la velocidad inicial para las pruebas
    "speedTest" es la velocidad con la cual se calculara la distancia del punto A al B de cada pieza del robot, disminuira por cada error
    "margen" es el margen de error para encontrar cada posicion*/
    public float speedReal, speedTest = 1, margen;

    //los distintos flags que se usaron para reducir la velocidad en casos de error o abortar el proceso y movilizar cada parte.
    public int flagAbort, stop;

    //Activada por el boton en canvas
    public void MoverDireccion()
    {
        var dataFound = SaveLoadScorbotRiel.LoadData<DatosGuardadosScorbotRiel>("scorbot " + numeroDestino.numeroDir);
        if (dataFound != null)
        {
            //Si existe, actualiza la variable "destino" con los valores encontrados
            destino = dataFound;
            Debug.Log("Cargando destino para animar : scorbot " + numeroDestino.numeroDir);
            //Se reinician las variables
            DH = 0;
            DBR = 0;
            DAB = 0;
            DMU = 0;
            DMA = 0;
            flagAbort = 0;
            speedTest = speedReal;
            stop = 0;
            //Calcula el movimiento que debe realizar
            StartCoroutine("CalcularDistancia");
            //Ejecuta el movimiento
            if (stop != 1) StartCoroutine("MovimientoSimple");
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

    //Calcula el movimiento que debe realizar
    IEnumerator CalcularDistancia()
    {
        //Calcula el movimiento del Riel ------------------------------------------------------------------------------------------------------------------------------------------------------
        DRiel = Riel.GetComponent<Transform>().position.x - destino.PosRobot.x;

        //Calcula el movimiento del Hombro ------------------------------------------------------------------------------------------------------------------------------------------------------
        while (compare(FH.GetComponent<Transform>().localEulerAngles.y, destino.Hombro.eulerAngles.y, 0, 0) && flagAbort < 20)
        {
            distancia(FH, RH, 190, new Vector3(0, speedTest, 0), DH, 1);
        }
        if (flagAbort >= 20)
        {
            Debug.Log("Hombro alcanzo limite");
            DH = 0;
        }
        FH.GetComponent<Transform>().rotation = destino.Hombro;
        speedTest = speedReal;
        flagAbort = 0;

        //Calcula el movimiento del Brazo ------------------------------------------------------------------------------------------------------------------------------------------------------
        FBR.GetComponent<Transform>().rotation = RBR.GetComponent<Transform>().rotation;
        FAB.GetComponent<Transform>().rotation = RAB.GetComponent<Transform>().rotation;
        FMU.GetComponent<Transform>().rotation = RMU.GetComponent<Transform>().rotation;
        FMA.GetComponent<Transform>().rotation = RMA.GetComponent<Transform>().rotation;

        while (compare(FBR.GetComponent<Transform>().localEulerAngles.z, destino.Brazo.eulerAngles.z, 0, 0) && flagAbort < 20)
        {
            distancia(FBR, RBR, 160, new Vector3(0, 0, speedTest), DBR, 2);
        }
        if (flagAbort >= 20)
        {
            Debug.Log("Brazo alcanzo limite");
            DBR = 0;
        }
        FBR.GetComponent<Transform>().rotation = destino.Brazo;
        speedTest = speedReal;
        flagAbort = 0;

        //Calcula el movimiento del Antebrazo ------------------------------------------------------------------------------------------------------------------------------------------------------
        FAB.GetComponent<Transform>().rotation = RAB.GetComponent<Transform>().rotation;
        FMU.GetComponent<Transform>().rotation = RMU.GetComponent<Transform>().rotation;
        FMA.GetComponent<Transform>().rotation = RMA.GetComponent<Transform>().rotation;

        while (compare(FAB.GetComponent<Transform>().localEulerAngles.x, destino.Antebrazo.eulerAngles.x, 0, 0) && flagAbort < 10)
        {
            distancia(FAB, RAB, 280, new Vector3(speedTest, 0, 0), DAB, 3);
        }
        if (flagAbort >= 10)
        {
            Debug.Log("Antebrazo alcanzo limite");
            DAB = 0;
        }
        FAB.GetComponent<Transform>().rotation = destino.Antebrazo;
        speedTest = speedReal;
        flagAbort = 0;

        //Calcula el movimiento de la muñeca ------------------------------------------------------------------------------------------------------------------------------------------------------
        FMU.GetComponent<Transform>().rotation = RMU.GetComponent<Transform>().rotation;
        FMA.GetComponent<Transform>().rotation = RMA.GetComponent<Transform>().rotation;

        while (compare(FMU.GetComponent<Transform>().localEulerAngles.z, destino.ConjuntoMano.eulerAngles.z, 0, 0) && flagAbort < 20)
        {
            distancia(FMU, RMU, 190, new Vector3(0, 0, speedTest), DMU, 4);
        }
        if (flagAbort >= 20)
        {
            Debug.Log("Mu�eca alcanzo limite");
            DMU = 0;
        }
        FMU.GetComponent<Transform>().rotation = destino.ConjuntoMano;
        speedTest = speedReal;
        flagAbort = 0;

        //Calcula el movimiento de la mano ------------------------------------------------------------------------------------------------------------------------------------------------------
        FMA.GetComponent<Transform>().rotation = RMA.GetComponent<Transform>().rotation;

        while (compare(FMA.GetComponent<Transform>().localEulerAngles.y, destino.mano.eulerAngles.y, 0, 0) && flagAbort < 20)
        {
            distancia(FMA, RMA, 190, new Vector3(0, speedTest, 0), DMA, 5);
        }
        if (flagAbort >= 20)
        {
            Debug.Log("Mano alcanzo limite");
            DMA = 0;
        }
        FMA.GetComponent<Transform>().rotation = destino.mano;
        speedTest = speedReal;
        flagAbort = 0;

        //Cada movimiento se mide en distancia y se calcula el valor absoluto
        CRiel = Mathf.Abs(DRiel);
        CH = Mathf.Abs(DH);
        CBR = Mathf.Abs(DBR);
        CAB = Mathf.Abs(DAB);
        CMU = Mathf.Abs(DMU);
        CMA = Mathf.Abs(DMA);
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
                    DH = moveVector.y + DH;
                    if (DH == distancia) flagAbort = 20;
                    break;
                case 2:
                    DBR = moveVector.z + DBR;
                    if (DBR == distancia) flagAbort = 20;
                    break;
                case 3:
                    DAB = moveVector.x + DAB;
                    if (DAB == distancia) flagAbort = 20;
                    break;
                case 4:
                    DMU = moveVector.z + DMU;
                    if (DMU == distancia) flagAbort = 20;
                    break;
                case 5:
                    DMA = moveVector.y + DMA;
                    if (DMA == distancia) flagAbort = 20;
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
                    DH = 0;
                    break;
                case 2:
                    DBR = 0;
                    break;
                case 3:
                    DAB = 0;
                    break;
                case 4:
                    DMU = 0;
                    break;
                case 5:
                    DMA = 0;
                    break;
            }
            flagAbort = flagAbort + 1;
        }
    }

//Ejecuta el movimiento
    IEnumerator MovimientoSimple()
    {
        //Mientras la distancia a recorrder de cada pieza sea mayor a 0, mueve la pieza unos grados
        while (CH > 0 || CBR > 0 || CAB > 0 || CMU > 0 || CMA > 0 || CRiel>0)
        {
            //Movimiento Riel
            if (CRiel > 0)
            {
                if (DRiel > 0) Riel.transform.position = Riel.transform.position + new Vector3( -speedTest, 0, 0) * Time.deltaTime;
                else Riel.transform.position = Riel.transform.position + new Vector3(speedTest, 0, 0) * Time.deltaTime;

                CRiel = CRiel - speedTest * Time.deltaTime;
            }
            //Movimiento Hombro
            if (CH > 0)
            {
                if (DH > 0) RH.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else RH.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                CH = CH - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Brazo
            if (CBR > 0)
            {
                if (DBR > 0) RBR.GetComponent<Transform>().Rotate(new Vector3(0, 0, speedTest * 10) * Time.deltaTime);
                else RBR.GetComponent<Transform>().Rotate(new Vector3(0, 0, -speedTest * 10) * Time.deltaTime);

                CBR = CBR - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Antebrazo
            if (CAB > 0)
            {
                if (DAB > 0) RAB.GetComponent<Transform>().Rotate(new Vector3(speedTest * 10, 0, 0) * Time.deltaTime);
                else RAB.GetComponent<Transform>().Rotate(new Vector3(-speedTest * 10, 0, 0) * Time.deltaTime);

                CAB = CAB - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Mu�eca
            if (CMU > 0)
            {
                if (DMU > 0) RMU.GetComponent<Transform>().Rotate(new Vector3(0, 0, speedTest * 10) * Time.deltaTime);
                else RMU.GetComponent<Transform>().Rotate(new Vector3(0, 0, -speedTest * 10) * Time.deltaTime);

                CMU = CMU - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Mano
            if (CMA > 0)
            {
                if (DMA > 0) RMA.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else RMA.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                CMA = CMA - speedTest * 10 * Time.deltaTime;
            }
            yield return null;
        }
        //Calcula distancia de una forma más simple
        StartCoroutine("SimpleDistancia");
        //Ejecuta  la distancia calculada
        StartCoroutine("MovimientoCompleto");
        yield return null;
    }

    //Calcula distancia de una forma más simple
    IEnumerator SimpleDistancia()
    {
        //Calcula la diferencia de angulos entre las diversas posiciones
        DH = destino.Hombro.eulerAngles.y - RH.GetComponent<Transform>().localEulerAngles.y;
        DBR = destino.Brazo.eulerAngles.z - RBR.GetComponent<Transform>().localEulerAngles.z;
        DAB = destino.Antebrazo.eulerAngles.x - RAB.GetComponent<Transform>().localEulerAngles.x;
        DMU = destino.ConjuntoMano.eulerAngles.z - RMU.GetComponent<Transform>().localEulerAngles.z;
        DMA = destino.mano.eulerAngles.y - RMA.GetComponent<Transform>().localEulerAngles.y;

        //Si es mayor a 181, se realiza la resta para que realice una vuelta menor
        if (Mathf.Abs(DH) > 181)
        {
            if (DH > 0) DH = 360 - DH;
            else DH = -360 - DH;
        }
        if (Mathf.Abs(DBR) > 181)
        {
            if (DBR > 0) DBR = 360 - DBR;
            else DBR = -360 - DBR;
        }
        if (Mathf.Abs(DAB) > 181)
        {
            if (DAB > 0) DAB = 360 - DAB;
            else DAB = -360 - DAB;
        }
        if (Mathf.Abs(DMU) > 181)
        {
            if (DMU > 0) DMU = 360 - DMU;
            else DMU = -360 - DMU;
        }
        if (Mathf.Abs(DMA) > 181)
        {
            if (DMA > 0) DMA = 360 - DMA;
            else DMA = -360 - DMA;
        }

        CH = Mathf.Abs(DH);
        CBR = Mathf.Abs(DBR);
        CAB = Mathf.Abs(DAB);
        CMU = Mathf.Abs(DMU);
        CMA = Mathf.Abs(DMA);
        yield return null;
    }

    //Ejecuta  la distancia calculada
    IEnumerator MovimientoCompleto()
    {
        while (CH > 0 || CBR > 0 || CAB > 0 || CMU > 0 || CMA > 0)
        {
            //Movimiento Hombro
            if (CH > 0)
            {
                if (DH > 0) RH.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else RH.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                CH = CH - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Brazo
            if (CBR > 0)
            {
                if (DBR > 0) RBR.GetComponent<Transform>().Rotate(new Vector3(0, 0, speedTest * 10) * Time.deltaTime);
                else RBR.GetComponent<Transform>().Rotate(new Vector3(0, 0, -speedTest * 10) * Time.deltaTime);

                CBR = CBR - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Antebrazo
            if (CAB > 0)
            {
                if (DAB > 0) RAB.GetComponent<Transform>().Rotate(new Vector3(speedTest * 10, 0, 0) * Time.deltaTime);
                else RAB.GetComponent<Transform>().Rotate(new Vector3(-speedTest * 10, 0, 0) * Time.deltaTime);

                CAB = CAB - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Muñeca
            if (CMU > 0)
            {
                if (DMU > 0) RMU.GetComponent<Transform>().Rotate(new Vector3(0, 0, speedTest * 10) * Time.deltaTime);
                else RMU.GetComponent<Transform>().Rotate(new Vector3(0, 0, -speedTest * 10) * Time.deltaTime);

                CMU = CMU - speedTest * 10 * Time.deltaTime;
            }
            //Movimiento Mano
            if (CMA > 0)
            {
                if (DMA > 0) RMA.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10, 0) * Time.deltaTime);
                else RMA.GetComponent<Transform>().Rotate(new Vector3(0, -speedTest * 10, 0) * Time.deltaTime);

                CMA = CMA - speedTest * 10 * Time.deltaTime;
            }
            yield return null;
        }

        //Finalmente carga las posiciones guardadas para una mayor presición
        Riel.GetComponent<Transform>().position= destino.PosRobot;
        RH.GetComponent<Transform>().rotation = destino.Hombro;
        RBR.GetComponent<Transform>().rotation = destino.Brazo;
        RAB.GetComponent<Transform>().rotation = destino.Antebrazo;
        RMU.GetComponent<Transform>().rotation = destino.ConjuntoMano;
        RMA.GetComponent<Transform>().rotation = destino.mano;
        yield return null;
    }
}

