using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//Clase en la cual se  crean variables a fin de guardarlo en un txt
public class Datos_Guardados
{
    //Creamos las variables de rotacion de cada pieza del brazo robot
    public Quaternion BaseFija;
    public Quaternion BaseMovil;
    public Quaternion Brazo1;
    public Quaternion Brazo2;
    public Quaternion Muneca1;
    public Quaternion Muneca2;
    public Quaternion Mano;

    //Creamos las variables de posicion y rotacion del cubo
    public Quaternion cubo;
    public Vector3 PosCubo;
}
