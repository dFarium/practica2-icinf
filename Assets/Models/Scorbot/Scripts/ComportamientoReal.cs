using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoReal : MonoBehaviour
{
    public Component[] rigidArray;

    //Obtener todos los componentes rigidbody del robot
    void Start()
    {
        rigidArray = GetComponentsInChildren<Rigidbody>();
    }

    //Quitar restricciones de rotacion todos los rigidbody del robot (Congelaciones dadas en Rotaciones.cs)
    public void DescongelarRobot()
    {
        foreach (Rigidbody r in rigidArray)
        {
            r.constraints = RigidbodyConstraints.None;
        }
    }

}
