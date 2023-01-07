using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//En este script se crean las variables que despues se guararan en el archivo json

[System.Serializable]
public class Data
{
    //Creamos las variables del Cubo (Posición, rotación y si esta o no tomado)
    public Vector3 cube;
    public Quaternion cubeR;
    public int objetoTomado;

    //Creamos las variables de posicion de cada pieza del brazo robot
    public Vector3 Base;
    public Vector3 Hombro;
    public Vector3 Brazo;
    public Vector3 Antebrazo;
    public Vector3 Muñeca;
    public Vector3 Mano;
    public Vector3 Cubo_1;
    public Vector3 Cubo_2;

    //Creamos las variables de rotacion de cada pieza del brazo robot
    public Quaternion BaseR;
    public Quaternion HombroR;
    public Quaternion BrazoR;
    public Quaternion AntebrazoR;
    public Quaternion MuñecaR;
    public Quaternion ManoR;
    public Quaternion Cubo_1R;
    public Quaternion Cubo_2R;
}
