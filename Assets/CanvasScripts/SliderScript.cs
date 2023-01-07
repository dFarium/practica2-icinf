using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Este script se encarga de utilizar la funcion onValueChanged par actualizar los valores
public class SliderScript : MonoBehaviour
{

    [SerializeField] private Slider _slider;
    [SerializeField] private Text _sliderText;
    public int v;
    // Start is called before the first frame update

    void Start()
    {
        //Se usa este script porque el onValueChanged del inspector esta broken, pero escencialmente es lo mismo, se llama cada vez que el numero cambia
        _slider.onValueChanged.AddListener((v)=>{
            v =v/4;
            _sliderText.text = v.ToString();
        });
    }
}
