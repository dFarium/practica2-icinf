using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Este script se encarga de controlar la velocidad del motor del HingeJoint usando como parametro el slider mostrado en simulacion
public class Rotaciones : MonoBehaviour{

    public Text textoVelocidad; //Se obtiene el valor que aparece al lado del Slider


    //Para rotar a izquierda
    public void RotarIZQ(){
        float velocidad = float.Parse(textoVelocidad.text)*40+10;
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.force = 10000;
        motor.targetVelocity = velocidad*-1;
        hinge.motor = motor; //Importante esta linea, asi se le indica a unity que tiene que usar todos los valores antes asignados en la funcion
        hinge.useMotor = true;
    }

    //Para rotar a derecha
    public void RotarDER(){
        float velocidad = float.Parse(textoVelocidad.text)*40+10;
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.force = 10000;
        motor.targetVelocity = velocidad;
        hinge.motor = motor; //Importante esta linea, asi se le indica a unity que tiene que usar todos los valores antes asignados en la funcion
        hinge.useMotor = true;
    }

    //Dejar de rotar
    public void StopRotar(){
        var hinge = GetComponent<HingeJoint>();
        var motor = hinge.motor;
        motor.force = 10000;
        motor.targetVelocity = 0;
        hinge.motor = motor; //Importante esta linea, asi se le indica a unity que tiene que usar todos los valores antes asignados en la funcion
        hinge.useMotor = true;
    }

    //Congela el eje Y de la pieza del robot (componente Rigidody)
    public void CongelarYRobot()
    {
        var rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    //Congela el eje Z de la pieza del robot (componente Rigidody)
    public void CongelarZRobot()
    {
        var rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

}
