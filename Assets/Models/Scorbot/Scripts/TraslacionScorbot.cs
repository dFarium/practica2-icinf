using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraslacionScorbot : MonoBehaviour
{

    //"destino" ser� el objeto con todas las piezas destino
    public Datos_Guardados_Scorbot destino;
    //"numeroDestino" corresponde al numero de archivo de guardado al que se acceder�
    public NumeroVariable numeroDestino;

    //Se adjuntan en Unity todas las piezas conectadas
    public GameObject RH, RBR, RAB, RMU, RMA;
    public GameObject FH, FBR, FAB, FMU, FMA;

    //Distancia recorrida por cada objeto
    public float DH, DBR, DAB, DMU, DMA;

    //Valor absoluto de distancia recorrida por cada objeto
    public float CH, CBR, CAB, CMU, CMA;

    /* "speedTest" es la velocidad con la cual se calcular� la distancia del punto A al B de cada pieza del robot.
     * "margen" es el margen de error para encontrar cada posici�n.*/
    public float speedReal, speedTest = 1, margen;

    //los distintos flags que se usar�n para reducir la velocidad en casos de error o abortar el proceso y movilizar cada parte.
    public int flagAbort, stop;

    //Activada por el boton en canvas
    public void MoverDireccion()
    {
        //Se crea una variable para ver si existe o no 
        var dataFound = SaveLoadScorbot.LoadData<Datos_Guardados_Scorbot>("scorbot " + numeroDestino.numeroDir);
        if (dataFound != null)
        {
            //Si existe, llena data con los archivos encontrados
            destino = dataFound;
            Debug.Log("Cargando destino para animar : scorbot " + numeroDestino.numeroDir);
            //Empieza el movimiento
            DH = 0;
            DBR = 0;
            DAB = 0;
            DMU = 0;
            DMA = 0;
            flagAbort = 0;
            speedTest = speedReal;
            stop = 0;
            StartCoroutine("CalcularDistancia");
            if (stop != 1) StartCoroutine("MovimientoSimple");
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

    IEnumerator CalcularDistancia()
    {
        //Se mueve Hombro ------------------------------------------------------------------------------------------------------------------------------------------------------
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

        //Se mueve Brazo ------------------------------------------------------------------------------------------------------------------------------------------------------
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

        //Se mueve Antebrazo -----------------------------------------------------------------------------------------------------------------------------------------------------
        FAB.GetComponent<Transform>().rotation = RAB.GetComponent<Transform>().rotation;
        FMU.GetComponent<Transform>().rotation = RMU.GetComponent<Transform>().rotation;
        FMA.GetComponent<Transform>().rotation = RMA.GetComponent<Transform>().rotation;

        while (compare(FAB.GetComponent<Transform>().localEulerAngles.x, destino.Antebrazo.eulerAngles.x, 0, 0) && flagAbort < 20)
        {
            distancia(FAB, RAB, 280, new Vector3(speedTest, 0, 0), DAB, 3);
        }
        if (flagAbort >= 20)
        {
            Debug.Log("Antebrazo alcanzo limite");
            DAB = 0;
        }
        FAB.GetComponent<Transform>().rotation = destino.Antebrazo;
        speedTest = speedReal;
        flagAbort = 0;

        //Se mueve Muñeca ------------------------------------------------------------------------------------------------------------------------------------------------------
        FMU.GetComponent<Transform>().rotation = RMU.GetComponent<Transform>().rotation;
        FMA.GetComponent<Transform>().rotation = RMA.GetComponent<Transform>().rotation;

        while (compare(FMU.GetComponent<Transform>().localEulerAngles.z, destino.ConjuntoMano.eulerAngles.z, 0, 0) && flagAbort < 20)
        {
            distancia(FMU, RMU, 190, new Vector3(0, 0, speedTest), DMU, 4);
        }
        if (flagAbort >= 20)
        {
            Debug.Log("Muñeca alcanzo limite");
            DMU = 0;
        }
        FMU.GetComponent<Transform>().rotation = destino.ConjuntoMano;
        speedTest = speedReal;
        flagAbort = 0;

        //Se mueve Mano -----------------------------------------------------------------------------------------------------------------------------------------------------
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

        CH = Mathf.Abs(DH);
        CBR = Mathf.Abs(DBR);
        CAB = Mathf.Abs(DAB);
        CMU = Mathf.Abs(DMU);
        CMA = Mathf.Abs(DMA);
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


    IEnumerator MovimientoSimple()
    {
        while (CH > 0 || CBR > 0 || CAB > 0 || CMU > 0 || CMA > 0)
        {
            //Movimiento Hombro
            if (CH > 0)
            {
                if (DH > 0) RH.GetComponent<Transform>().Rotate(new Vector3(0, speedTest * 10,0) * Time.deltaTime);
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
        StartCoroutine("SimpleDistancia");
        StartCoroutine("MovimientoCompleto");
        yield return null;
    }
    IEnumerator SimpleDistancia()
    {
        DH = destino.Hombro.eulerAngles.y - RH.GetComponent<Transform>().localEulerAngles.y;
        //Debug.Log("DIF RH " + destino.BaseMovil.eulerAngles.y + " y " + RH.GetComponent<Transform>().localEulerAngles.y + " = " + DH);
        DBR = destino.Brazo.eulerAngles.z - RBR.GetComponent<Transform>().localEulerAngles.z;
        //Debug.Log("DIF RBR " + destino.Brazo1.eulerAngles.y + " y " + RBR.GetComponent<Transform>().localEulerAngles.y + " = " + DBR);
        DAB = destino.Antebrazo.eulerAngles.x - RAB.GetComponent<Transform>().localEulerAngles.x;
        //Debug.Log("DIF RAB " + destino.Brazo2.eulerAngles.y + " y " + RAB.GetComponent<Transform>().localEulerAngles.y + " = " + DAB);
        DMU = destino.ConjuntoMano.eulerAngles.z - RMU.GetComponent<Transform>().localEulerAngles.z;
        //Debug.Log("DIF RMU " + destino.Muneca1.eulerAngles.y + " y " + RMU.GetComponent<Transform>().localEulerAngles.y + " = " + DMU);
        DMA = destino.mano.eulerAngles.y - RMA.GetComponent<Transform>().localEulerAngles.y;
        //Debug.Log("DIF RMA " + destino.Muneca2.eulerAngles.y + " y " + RMA.GetComponent<Transform>().localEulerAngles.y + " = " + D_M2);


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
        RH.GetComponent<Transform>().rotation = destino.Hombro;
        RBR.GetComponent<Transform>().rotation = destino.Brazo;
        RAB.GetComponent<Transform>().rotation = destino.Antebrazo;
        RMU.GetComponent<Transform>().rotation = destino.ConjuntoMano;
        RMA.GetComponent<Transform>().rotation = destino.mano;
        yield return null;
    }
}

