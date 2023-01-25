using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoReal : MonoBehaviour
{
    public Component[] rigidArray;
    // Start is called before the first frame update
    void Start()
    {
        rigidArray = GetComponentsInChildren<Rigidbody>();
    }

    public void DescongelarRobot()
    {
        foreach (Rigidbody r in rigidArray)
        {
            r.constraints = RigidbodyConstraints.None;
        }
    }

}
