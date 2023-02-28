using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumeroVariable : MonoBehaviour
{
    //Script usado para los numeros de guardado
    public int numeroSave;
    public int numeroLoad;
    public int numeroDir;
    public Text numSave;//Texto representativo de que numero de guardado es
    public Text numLoad;//Texto representativo de que numero de cargado es
    public Text numDir;//Texto representativo de que numero de direccion final es

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
        if (numeroLoad>0){
            numeroLoad = numeroLoad-1;
            numLoad.text = numeroLoad.ToString();
        }
    }

    public void aumentarDir()
    {
        numeroDir++;
        numDir.text = numeroDir.ToString();
    }

    public void disminuirDir()
    {
        if (numeroDir > 0)
        {
            numeroDir = numeroDir - 1;
            numDir.text = numeroDir.ToString();
        }
    }
}
