using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail_move : MonoBehaviour
{
    private Vector3 PosIni;
    private Vector3 Move = new Vector3(0, 0, 0);
    private Vector3 FinalPos;
    public float speed = 1;
    private int flag = 0;

    public void Izquierda()
    {
        flag = 1;
        Move.x = -1;
    }
    public void Derecha()
    {
        flag = 1;
        Move.x = 1;
    }

    public void Stop()
    {
        flag = 0;
        Move.x = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag == 1)
        {
            PosIni = transform.position;
            FinalPos = PosIni + Move * speed * Time.deltaTime;
            transform.position = FinalPos;
        }
    }
}

