using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadKuka : MonoBehaviour
{
    //Valores leidos desde el Canvas
    public NumeroVariable numeroSave;
    public NumeroVariable numeroLoad;

    //Se creara un object que guarde los datos relevantes de las piezas
    public Datos_Guardados piezas; //reemplazado por Data data

    //Carpeta en la que se encuentran los archivos de guardado
    public const string carpeta = "Kuka-Data/";
    //El nombre con el cual se crearan los archivos json
    public string namefileData;

    //Se ejecuta al iniciar el programa
    private void Start()
    {
        //Se crea una variable para ver si existe o no la "Posicion 0"
        var dataFound = LoadData<Datos_Guardados>("Posicion 0");

        if (dataFound != null)
        {
            //Si existe, llena data con los archivos encontrados
            piezas = dataFound;
        }
        else
        {
            //Si no existe, crea el archivo json "Posicion 0"
            namefileData = "Posicion ";
            piezas = new Datos_Guardados();
            guardarDatos(0);
        }
    }

    public void guardarDatos(int numeroGuardado)
    {
        //Genera el nombre con el cual se generar� el archivo Json
        namefileData = "Posicion " + numeroGuardado;

        //Guarda todas las rotaciones de todas las piezas del kuka
        piezas.BaseFija = GameObject.FindGameObjectWithTag("K-BaseFija").transform.rotation;
        piezas.BaseMovil = GameObject.FindGameObjectWithTag("K-BaseMovil").transform.rotation;
        piezas.Brazo1 = GameObject.FindGameObjectWithTag("K-Brazo1").transform.rotation;

        //ejecuta la funcion SaveData para guardar los datos
        SaveData(piezas, namefileData);
    }

    public void cargarDatos(int numeroGuardado)
    {
        //Se crea una variable para ver si existe o no 
        var dataFound = LoadData<Datos_Guardados>("Guardado " + numeroGuardado);
        if (dataFound != null)
        {
            //Si existe, llena data con los archivos encontrados
            piezas = dataFound;
        }
        else
        {
            //Si no existe, crea un archivo json con los datos actuales del brazo robot
            namefileData = "Guardado ";
            piezas= new Datos_Guardados();
            guardarDatos(numeroGuardado);
        }
        
        //A continuacion se cargan datos de rotacion de cada pieza del kuka
        /*
            AAAAAAAAAAAAAAA
         */
    }

    public static void SaveData<T>(T data, string fileName)
    {
        //Se crea la direcci�n del archivo
        string fullPath = Application.dataPath + "/" + carpeta;
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

        //Mensaje para comprobar que el programa no tuvo errores
        Debug.Log("GUARDADO EXITOSAMENTE EN: " + fullPath + fileName);
    }

    public static T LoadData<T>(string fileName)
    {
        //Se crea la direcci�n del archivo
        string fullPath = Application.dataPath + "/" + carpeta + fileName + ".json";
        //Se verifica si existe
        if (File.Exists(fullPath))
        {
            //Leemos el Texto
            string textJson = File.ReadAllText(fullPath);
            //De Json a un Obj que Unity entiende
            var obj = JsonUtility.FromJson<T>(textJson);
            //Mensaje para comprobar que el programa no tuvo errores
            Debug.Log(fileName + "ROTACIONES KUKA CARGADAS EXITOSAMENTE");
            return obj;
        }
        else
        {
            //No encontr� el archivo
            Debug.Log(fileName + " NO ENCONTRADO");
            return default;
        }
    }
}
