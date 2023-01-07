using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumeroVariable : MonoBehaviour
{
    //Script usado para los numeros de guardado
    public int numeroSave;
    public int numeroLoad;
    public Text numSave;//Texto representativo de que numero de guardado es
    public Text numLoad;//Texto representativo de que numero de cargado es

    public void aumentarSave(){
        numeroSave++;
        numSave.text = numeroSave.ToString();
    }

    public void disminuirSave(){
        if (numeroSave>1){
            numeroSave = numeroSave-1;
            numSave.text = numeroSave.ToString();
        }
    }

    public void aumentarLoad(){
        numeroLoad++;
        numLoad.text = numeroLoad.ToString();
    }

    public void disminuirLoad(){
        if (numeroLoad>1){
            numeroLoad = numeroLoad-1;
            numLoad.text = numeroLoad.ToString();
        }
    }
}
