using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class traslacion : MonoBehaviour
{
    public GameObject cubito;
    public Vector3 componente;
    public float x, y, z;


    // Update is called once per frame
    void Update()
    {
        componente = cubito.GetComponent<Transform>().eulerAngles;
        x = GetObjectRotationX();
        y = GetObjectRotationY();
        z = GetObjectRotationZ();

    }

    float GetObjectRotationZ()
    {
        if (componente.z > 180)
        {
            return componente.z - 360;
        }
        else
        {
            return componente.z;
        }
    }

    float GetObjectRotationX()
    {
        if (componente.x > 180)
        {
            return componente.x - 360;
        }
        else
        {
            return componente.x;
        }
    }

    float GetObjectRotationY()
    {
        if (componente.y > 180)
        {
            return componente.y - 360;
        }
        else
        {
            return componente.y;
        }
    }
}