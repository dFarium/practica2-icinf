using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningColision : MonoBehaviour
{
    public MeshCollider[] robot;
    public CanvasGroup grupo;
    // Start is called before the first frame update
    void Start()
    {
        robot = GetComponentsInChildren<MeshCollider>();
    }

    // Update is called once per frame
    void Update()

    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " ESTA COLISIONANDO");
        grupo.alpha = 1;
        grupo.interactable = true;
        grupo.blocksRaycasts = true;
    }

    private void OnTriggerExit(Collider other)
    {
        grupo.alpha = 0;
        grupo.interactable = false;
        grupo.blocksRaycasts = false;
    }
}
