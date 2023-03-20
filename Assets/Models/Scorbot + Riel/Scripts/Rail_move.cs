using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rail_move : MonoBehaviour
{
    private Vector3 PosIni;
    private Vector3 Move = new Vector3(0, 0, 0);
    private Vector3 FinalPos;
    public Text textoVelocidad;
    public float speed, limIZQ, limDER;
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
            speed = float.Parse(textoVelocidad.text);
            if ((Move.x > 0 && transform.position.x < limDER) || (Move.x < 0 && transform.position.x > limIZQ))
            {
                transform.position = transform.position + Move * speed * Time.deltaTime;
            }
        }
    }
}

