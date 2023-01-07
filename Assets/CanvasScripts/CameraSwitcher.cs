using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraSwitcher : MonoBehaviour
{
     //Esta funcion en general permite seleccionar camaras para que se cambien en un orden en especifico,para ver mas, revisen el GameObject CameraSwitcher
    public GameObject[] Cameras;
     
    int currentCam;
     
    void Start()//Setea la primera camara
    {
        currentCam = 0;
        setCam(currentCam);
    }    

    public void setCam(int idx){//Setea el resto de camaras
        for(int i = 0; i < Cameras.Length; i++){
            if(i == idx){
                Cameras[i].SetActive(true);
            }else{
                Cameras[i].SetActive(false);
            }
        }
    }
     
    public void toggleCam(){//Cambia entre camaras
        currentCam++;
        if(currentCam > Cameras.Length-1)
            currentCam = 0;
        setCam(currentCam);
    }
}
