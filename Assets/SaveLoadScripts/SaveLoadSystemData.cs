using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveLoadSystemData
{
    public static void SaveData<T>(T data, string path, string fileName)
    {
        //Crear variable con la carpeta del archivo
        string fullPath = Application.dataPath + "/" + path;
        //bool para verificar si existe el archivo 
        bool checkFolderExit = Directory.Exists(fullPath);
        if (checkFolderExit == false)
        {
            //Crea la carpeta si no existe
            Directory.CreateDirectory(fullPath);
        }

        //crea el archivo .Json con el nombre proporcionado por el Script "TestSave"
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(fullPath + fileName + ".json", json);

        //Mensaje para comprobar que el programa no tuvo errores
        Debug.Log("GUARDADO EXITOSAMENTE EN: " + fullPath + fileName);
    }

    public static T LoadData<T>(string path, string fileName)
    {
        //Se crea la dirección del archivo
        string fullPath = Application.dataPath + "/" + path + fileName + ".json";
        //Se verifica si existe
        if (File.Exists(fullPath))
        {
            //Leemos el Texto
            string textJson = File.ReadAllText(fullPath);
            //De Json a un Obj que Unity entiende
            var obj = JsonUtility.FromJson<T>(textJson);
            //Mensaje para comprobar que el programa no tuvo errores
            Debug.Log(fileName + " CARGADO EXITOSAMENTE");
            return obj;
        }
        else
        {
            //avisa que no encontró el archivo
            Debug.Log(fileName +" NO ENCONTRADO");
            return default;
        }
    }

}
