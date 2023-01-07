using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTime : MonoBehaviour
{
    //Simplemente cambia la velocidad de simulacion a 0 o a 1 respectivamente
    public void FreezeButton(){
        if (Time.timeScale == 0){
            Time.timeScale = 1;
        }
        else{
            Time.timeScale = 0;
        } 
    }
}
