using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TestSave : MonoBehaviour
{
    /*Este script funciona de la siguiente manera: 
     * Este script funciona en conjunto con el script "SaveLoadSystemData" y Canvas.
     * Al iniciar crea un archivo llamado "Guardado 0".
     * Cada vez que se guarda o carga una posicion, se llama al archivo "Guardado X", con X el numero que lee desde Canvas, el cual 
     *   va desde el 1, hasta el infinito.
     * Si intenta cargar una posicion, y el archivo json no existe, creara un archivo json con el nombre de la posicion en la que esta 
     *   (de esta forma, no se moveria el brazo y tampoco generaria un error al cargar).
     * Para resetear el cubo o resetear todo, el script llama a los datos almacenados en "Guardado 0".
     */

    //Se utiliza para generar el nombre en donde guardar los datos (el cual seria "Guardado numeroSave")
    public NumeroVariable numeroSave;
    //Se utiliza para generar el nombre en donde cargar los datos (el cual seria "Guardado numeroLoad")
    public NumeroVariable numeroLoad;
    //Se crea un objeto tipo Data, para leer sus componentes
    public Data data;
    //Da informacion de si el objeto esta actualmente tomado
    public PickUpObject estado;

    //Carpeta en la que se encuentran los archivos de guardado
    public const string pathData = "Data/";
    //El nombre con el cual se crearan los archivos json
    public string namefileData = "Guardado ";


    //Se ejecuta al iniciar el programa
    private void Start()
    {
        //Se crea una variable para ver si existe o no el "Guardado 0"
        var dataFound = SaveLoadSystemData.LoadData<Data>(pathData, "Guardado 0");
        if (dataFound != null)
        {
            //Si existe, llena data con los archivos encontrados
            data = dataFound;
        }
        else
        {
            //Si no existe, crea el archivo json "Guardado 0"
            namefileData = "Guardado ";
            data = new Data();
            guardarData(0);
        }
    }

    //Carga los datos de un "Guardado X", con X el numero que se aprecia abajo a la derecha en el canvas (al lado izq del boton "Cargar")
    public void loadData(int numeroGuardado)
    {
        //Crea el nombre del archivo 
        namefileData = "Guardado " + numeroGuardado;
        //Se crea una variable para ver si existe o no 
        var dataFound = SaveLoadSystemData.LoadData<Data>(pathData, namefileData);
        if (dataFound != null)
        {
            //Si existe, llena data con los archivos encontrados
            data = dataFound;
        }
        else
        {
            //Si no existe, crea un archivo json con los datos actuales del brazo robot
            namefileData = "Guardado ";
            data = new Data();
            guardarData(numeroGuardado);
        }
        //A continuacion se cargan datos de posicion y rotacion de cada pieza del brazo robotico, cada pieza tiene un "Tag" con el que se identifica
        //Carga los datos de posicion y rotacion de la Base encontrados en el archivo json 
        GameObject.FindGameObjectWithTag("Base").transform.position = data.Base;
        GameObject.FindGameObjectWithTag("Base").transform.rotation = data.BaseR;

        //Carga los datos de posicion y rotacion del Hombro encontrados en el archivo json 
        GameObject.FindGameObjectWithTag("Hombro").transform.position = data.Hombro;
        GameObject.FindGameObjectWithTag("Hombro").transform.rotation = data.HombroR;

        //Carga los datos de posicion y rotacion del Brazo encontrados en el archivo json 
        GameObject.FindGameObjectWithTag("Brazo").transform.position = data.Brazo;
        GameObject.FindGameObjectWithTag("Brazo").transform.rotation = data.BrazoR;

        //Carga los datos de posicion y rotacion del Antebrazo encontrados en el archivo json 
        GameObject.FindGameObjectWithTag("Antebrazo").transform.position = data.Antebrazo;
        GameObject.FindGameObjectWithTag("Antebrazo").transform.rotation = data.AntebrazoR;

        //Carga los datos de posicion y rotacion de la Muñeca encontrados en el archivo json 
        GameObject.FindGameObjectWithTag("Muñeca").transform.position = data.Muñeca;
        GameObject.FindGameObjectWithTag("Muñeca").transform.rotation = data.MuñecaR;

        //Carga los datos de posicion y rotacion de la Mano encontrados en el archivo json 
        GameObject.FindGameObjectWithTag("Mano").transform.position = data.Mano;
        GameObject.FindGameObjectWithTag("Mano").transform.rotation = data.ManoR;

        //Carga los datos de posicion y rotacion del Cubo 1 encontrados en el archivo json 
        GameObject.FindGameObjectWithTag("Cubo 1").transform.position = data.Cubo_1;
        GameObject.FindGameObjectWithTag("Cubo 1").transform.rotation = data.Cubo_1R;

        //Carga los datos de posicion y rotacion del Cubo 2 encontrados en el archivo json 
        GameObject.FindGameObjectWithTag("Cubo 2").transform.position = data.Cubo_2;
        GameObject.FindGameObjectWithTag("Cubo 2").transform.rotation = data.Cubo_2R;

        //Revisa si hay actualmente esta tomado el objeto, y si discrepa con el dato almacenado en "Data" sobre si esta tomado o no
        if (estado.objetoTomado!=data.objetoTomado) {
        //Si el objeto esta tomado y en esa posicion cargada NO esta tomado
        //o si el objeto NO esta tomado y en esa posicion cargada SI esta tomado, entonces:

            //Le quita la gravedad al objeto
            GameObject.FindGameObjectWithTag("UbbCube").GetComponent<Rigidbody>().useGravity = false;    

            //Le carga la posicion y rotacion al cubo, encontrada en el archivo json
            GameObject.FindGameObjectWithTag("UbbCube").transform.position = data.cube;
            GameObject.FindGameObjectWithTag("UbbCube").transform.rotation = data.cubeR;

            //Ejecuta la funcion "Esperar"
            StartCoroutine("Esperar");

            //Le devolvemos la gravedad al objeto
            GameObject.FindGameObjectWithTag("UbbCube").GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            // Si el objeto esta tomado y carga a una posicion tomada, o si
            // el objeto NO esta tomado y carga a una posicion donde No esta tomado

            //Le carga la posicion y rotacion al cubo, encontrada en el archivo json
            GameObject.FindGameObjectWithTag("UbbCube").transform.position = data.cube;
            GameObject.FindGameObjectWithTag("UbbCube").transform.rotation = data.cubeR;
        }
    }

    public void guardarData(int numeroGuardado)
    {
        //Genera el nombre con el cual se generará el archivo Json
        namefileData = "Guardado " + numeroGuardado;
       
        //Guarda todas las posiciones de todas las piezas del brazo robot
        data.Base = GameObject.FindGameObjectWithTag("Base").transform.position;
        data.Hombro = GameObject.FindGameObjectWithTag("Hombro").transform.position;
        data.Brazo = GameObject.FindGameObjectWithTag("Brazo").transform.position;
        data.Antebrazo = GameObject.FindGameObjectWithTag("Antebrazo").transform.position;
        data.Muñeca = GameObject.FindGameObjectWithTag("Muñeca").transform.position;
        data.Mano = GameObject.FindGameObjectWithTag("Mano").transform.position;
        data.Cubo_1 = GameObject.FindGameObjectWithTag("Cubo 1").transform.position;
        data.Cubo_2 = GameObject.FindGameObjectWithTag("Cubo 2").transform.position;

        //Guarda todas las rotaciones de todas las piezas del brazo robot
        data.BaseR = GameObject.FindGameObjectWithTag("Base").transform.rotation;
        data.HombroR = GameObject.FindGameObjectWithTag("Hombro").transform.rotation;
        data.BrazoR = GameObject.FindGameObjectWithTag("Brazo").transform.rotation;
        data.AntebrazoR = GameObject.FindGameObjectWithTag("Antebrazo").transform.rotation;
        data.MuñecaR = GameObject.FindGameObjectWithTag("Muñeca").transform.rotation;
        data.ManoR = GameObject.FindGameObjectWithTag("Mano").transform.rotation;
        data.Cubo_1R = GameObject.FindGameObjectWithTag("Cubo 1").transform.rotation;
        data.Cubo_2R = GameObject.FindGameObjectWithTag("Cubo 2").transform.rotation;

        //Guarda las posiciones y rotaciones del cubo con el cual interactua el robot
        data.cube = GameObject.FindGameObjectWithTag("UbbCube").transform.position;
        data.cubeR = GameObject.FindGameObjectWithTag("UbbCube").transform.rotation;
        //Guarda el estado actual del cubo (Tomado/ No tomado)
        data.objetoTomado = estado.objetoTomado;
        //ejecuta la funcion SaveData para guardar los datos
        SaveData();
    }

    private void SaveData()
    {
        //ejecuta la funcion SaveData del script "SaveLoadSystemData" para guardar los datos
        SaveLoadSystemData.SaveData(data, pathData, namefileData);
    }

    public void resetObjeto()
    {
        //Crea el nombre del archivo
        namefileData = "Guardado "+0;
        //Con el nombre, podemos buscarlo con la funcion LoadData del script "SaveLoadSystemData" para cargar los datos
        var dataFound = SaveLoadSystemData.LoadData<Data>(pathData, namefileData);
        //Se actualizan los datos de data (nuestra variable a la hora de cargar y guardar datos)
        data = dataFound;

        //Le carga la posicion y rotacion al cubo originales del objeto (donde se encontraba al iniciar el programa)
        GameObject.FindGameObjectWithTag("UbbCube").transform.position = data.cube;
        GameObject.FindGameObjectWithTag("UbbCube").transform.rotation = data.cubeR;
    }

    public void reset(){
        //Para resetear el brazo robot, se cargan todos los datos del archivo "Guardado 0"
        loadData(0);
    }

    public void LoadButtonData()
    {
        //Ejecuta la funcion loadData, obteniendo el numero de Canvas
        loadData(numeroLoad.numeroLoad);
    }

    public void SaveButtonData()
    {
        //Ejecuta la funcion guardarData, obteniendo el numero de Canvas
        guardarData(numeroSave.numeroSave);
    }

    IEnumerator Esperar()
    {
        //Espera un tiempo antes de ejecutar la funcion pickButton

        //Se define el tiempo
        float numero = 0.1F;
        //El programa espera ese tiempo
        yield return new WaitForSecondsRealtime(numero);
        //Toma o suelta el objeto
        estado.pickButton();
    }
}
