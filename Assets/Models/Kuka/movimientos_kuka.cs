using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //Libreria que usa unity para trabajar Texto desde la UI del canvas

//Este script se encarga de controlar la velocidad del motor del HingeJoint usando como parametro el slider mostrado en simulacion
public class movimientos_kuka : MonoBehaviour
{

    public Text textoVelocidad; //Se obtiene el valor que aparece al lado del Slider

    public void RotarIZQ()
    {
        float velocidad = float.Parse(textoVelocidad.text) * 40 + 10; //El valor se pasa de string a float, y se le multiplica por 40 y se suma 10
        var hinge = GetComponent<HingeJoint>(); //Se obtiene el componente HingeJoint del objeto al cual esta asociado el script
        var motor = hinge.motor;
        motor.force = 10000; //La fuerza que tiene el motor, para que no sea afectado por la gravedad
        motor.targetVelocity = velocidad * -1;//Para que gire hacia la izquierda
        hinge.motor = motor; //Importante esta linea, asi se le indica a unity que tiene que usar todos los valores antes asignados en la funcion
        hinge.useMotor = true;
    }

    public void RotarDER()
    {
        float velocidad = float.Parse(textoVelocidad.text) * 40 + 10;//El valor se pasa de string a float, y se le multiplica por 40 y se suma 10
        var hinge = GetComponent<HingeJoint>(); //Se obtiene el componente HingeJoint del objeto al cual esta asociado el script
        var motor = hinge.motor;
        motor.force = 10000; //La fuerza que tiene el motor, para que no sea afectado por la gravedad
        motor.targetVelocity = velocidad; //Para que gire hacia la izquierda
        hinge.motor = motor; //Importante esta linea, asi se le indica a unity que tiene que usar todos los valores antes asignados en la funcion
        hinge.useMotor = true;
    }

    public void StopRotar()
    {
        var hinge = GetComponent<HingeJoint>(); //Se obtiene el componente HingeJoint del objeto al cual esta asociado el script
        var motor = hinge.motor;
        motor.force = 10000; //La fuerza que tiene el motor, para que no sea afectado por la gravedad
        motor.targetVelocity = 0; //Simplemente se setea a 0 para que se deje de mover
        hinge.motor = motor; //Importante esta linea, asi se le indica a unity que tiene que usar todos los valores antes asignados en la funcion
        hinge.useMotor = true;
    }

    //Congela las piezas del robot que no estan siendo utilizadas
    public void CongelarYRobot()
    {
        var rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    public void CongelarZRobot()
    {
        var rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

}
