using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Vector3 InitPos;
    [SerializeField] Vector3 DestPos;
    private Vector3 FinalPos;
    public float movimiento;

    private void Update()
    {
        InitPos = transform.position;
        FinalPos = InitPos + DestPos * Time.deltaTime * movimiento;
        transform.position = FinalPos;
    }
}
