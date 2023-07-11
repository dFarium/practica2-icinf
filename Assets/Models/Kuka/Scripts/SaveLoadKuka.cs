using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadKuka : MonoBehaviour
{
    //Valores obtenidos desde Canvas, indican la posicion a cargar o a guardar
    public NumeroVariable numeroSave;
    public NumeroVariable numeroLoad;

    //Se creara un object que guarde los datos relevantes de las piezas
    public Datos_Guardados piezas;

    //Piezas individuales del robot, se dan como parametro desde unity
    public GameObject BF, BM, B1, B2, M1, M2, MA, CUBO;

    //Carpeta en la que se encuentran los archivos de guardado
    public const string carpeta = "Kuka-Data/";

    //El nombre, bajo el cual, se crearan los archivos de guardado
    public string namefileData;

    private void Start()
    {
        //Determina si es primera vez que se utiliza la escena o no
        var dataFound = LoadData<Datos_Guardados>("Posicion 0");
        if (dataFound != null)
        {
            //Si existe, utilizara por defecto las posiciones iniciales del robot
            piezas = dataFound;
        }
        else
        {
            // Si no existe, crea el archivo json "Posicion 0"
            // y le asigna las posiciones iniciales del robot
            namefileData = "Posicion ";
            piezas = new Datos_Guardados();
            guardarDatos(0);
        }
    }

    /*
    funcion que guarda los datos de la posicion actual del robot (valor obtenido desde canvas),
    utilizando la clase "Datos_Guardados", guarda cada valor en una variable y crea
    el archivo "Posicion [Valor_Actual]"
    */
    public void guardarDatos(int numeroGuardado)
    {
        namefileData = "Posicion " + numeroGuardado;

        //Guarda todas las rotaciones de todas las piezas
        piezas.BaseFija = BF.GetComponent<Transform>().rotation;
        piezas.BaseMovil = BM.GetComponent<Transform>().rotation;
        piezas.Brazo1 = B1.GetComponent<Transform>().rotation;
        piezas.Brazo2 = B2.GetComponent<Transform>().rotation;
        piezas.Muneca1 = M1.GetComponent<Transform>().rotation;
        piezas.Muneca2 = M2.GetComponent<Transform>().rotation;
        piezas.Mano = MA.GetComponent<Transform>().rotation;
        piezas.cubo = CUBO.GetComponent<Transform>().rotation;
        piezas.PosCubo = CUBO.GetComponent<Transform>().position;
        //ejecuta la funcion SaveData para guardar los datos
        SaveData(piezas, namefileData);
    }

    //Si existe un archivo con la posición a cargar, carga los datos guardados de dicha posicion
    public void cargarDatos(int numeroGuardado)
    {
        var dataFound = LoadData<Datos_Guardados>("Posicion " + numeroGuardado);
        if (dataFound != null)
        {
            //Si existe, actualiza la variable "piezas" con los valores encontrados
            piezas = dataFound;
            //A continuacion se cargan datos cada pieza del robot
            BF.GetComponent<Transform>().rotation = piezas.BaseFija;
            BM.GetComponent<Transform>().rotation = piezas.BaseMovil;
            B1.GetComponent<Transform>().rotation = piezas.Brazo1;
            B2.GetComponent<Transform>().rotation = piezas.Brazo2;
            M1.GetComponent<Transform>().rotation = piezas.Muneca1;
            M2.GetComponent<Transform>().rotation = piezas.Muneca2;
            MA.GetComponent<Transform>().rotation = piezas.Mano;
            CUBO.GetComponent<Transform>().rotation = piezas.cubo;
            CUBO.GetComponent<Transform>().position = piezas.PosCubo;
        }
        else
        {
            //Si no existe, manda un aviso por consola
            Debug.Log("ERROR, el destino no existe");
        }
    }
    //Reinicia la posicion del cubo
    public void cargarCubo()
    {
        //Actualizamos la variable para cargar la posición 0
        var dataFound = LoadData<Datos_Guardados>("Posicion 0");
        if (dataFound != null)
        {
            //Si existe, actualiza la variable "piezas" con los valores encontrados
            piezas = dataFound;
            //A continuacion se cargan los datos del cubo
            CUBO.GetComponent<Transform>().rotation = piezas.cubo;
            CUBO.GetComponent<Transform>().position = piezas.PosCubo;
        }
        else
        {
            //Si no existe, manda un aviso por consola
            Debug.Log("ERROR, el destino no existe");
        }
    }

    //Funcion que guarda en memoria las posiciones del robot
    public static void SaveData<T>(T data, string fileName)
    {
        //Se crea la ruta donde guardar el archivo
        string fullPath = Application.dataPath + "/Data/" + carpeta;
        //bool para verificar si existe la ruta
        bool checkFolderExit = Directory.Exists(fullPath);
        if (checkFolderExit == false)
        {
            //Crea la carpeta si no existe
            Directory.CreateDirectory(fullPath);
        }

        //crea el archivo .Json con el nombre proporcionado
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(fullPath + fileName + ".json", json);
        Debug.Log("GUARDADO EXITOSAMENTE EN: " + fullPath + fileName);
    }

   //Funcion que lee desde memoria las posiciones del robot
    public static T LoadData<T>(string fileName)
    {
        //Construimos la ruta
        string fullPath = Application.dataPath + "/Data/" + carpeta + fileName + ".json";
        //Se verifica si existe
        if (File.Exists(fullPath))
        {
            //Leemos el Texto
            string textJson = File.ReadAllText(fullPath);
            //De Json a un Obj que Unity entiende
            var obj = JsonUtility.FromJson<T>(textJson);
            Debug.Log(fileName + " ROTACIONES KUKA CARGADAS EXITOSAMENTE");
            return obj;
        }
        else
        {
            //Si no existe, manda un aviso por consola
            Debug.Log(fileName + " NO ENCONTRADO");
            return default;
        }
    }

    //Funcion ejecutada por el boton de canvas
    public void LoadButtonData()
    {
        cargarDatos(numeroLoad.numeroLoad);
    }

    //Funcion ejecutada por el boton de canvas
    public void SaveButtonData()
    {
        guardarDatos(numeroSave.numeroSave);
    }
}
