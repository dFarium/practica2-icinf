using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//Clase en la cual se  crean variables a fin de guardarlo en un txt
public class DatosGuardadosScorbotRiel
{
    //Creamos las variables de posicion del riel
    public Vector3 PosRobot;

    //Creamos las variables de rotacion de cada pieza del brazo robot
    public Quaternion Base;
    public Quaternion Hombro;
    public Quaternion Brazo;
    public Quaternion Antebrazo;
    public Quaternion ConjuntoMano;
    public Quaternion mano;

    //Creamos las variables de posicion y rotacion del cubo
    public Quaternion cubo;
    public Vector3 PosCubo;
}
