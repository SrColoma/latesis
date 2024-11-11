using System;
using System.Collections.Generic;
using UnityEngine;

public class MediatorController : MonoBehaviour
{
    // Singleton para asegurar que exista solo una instancia de MediatorController
    public static MediatorController Instance { get; private set; }

    // Diccionario que almacena eventos junto con sus listeners
    private Dictionary<string, Action<object[]>> eventDictionary;

    private void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Asegura que este objeto no se destruya al cambiar de escena
            eventDictionary = new Dictionary<string, Action<object[]>>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para registrar un nuevo evento en el sistema
    public void RegisterEvent(string eventID)
    {
        if (!eventDictionary.ContainsKey(eventID))
        {
            eventDictionary[eventID] = null; // Inicializamos la entrada en el diccionario
            Debug.Log("Evento registrado: " + eventID);
        }
    }

    // Método para suscribir un listener a un evento
    public void SubscribeEvent(string eventID, Action<object[]> listener)
    {
        if (eventDictionary.ContainsKey(eventID))
        {
            eventDictionary[eventID] += listener; // Añadimos el listener al evento
            Debug.Log("Listener suscrito al evento: " + eventID);
        }
        else
        {
            Debug.LogError("Evento no registrado: " + eventID);
        }
    }

    // Método para emitir un evento con argumentos
    public void EmitEvent(string eventID, object[] arguments)
    {
        if (eventDictionary.ContainsKey(eventID) && eventDictionary[eventID] != null)
        {
            Debug.Log("Emitiendo evento: " + eventID + " con argumentos: " + string.Join(", ", arguments));
            eventDictionary[eventID].Invoke(arguments); // Llamamos a todos los listeners suscritos
        }
        else
        {
            Debug.LogWarning("No hay listeners para el evento: " + eventID);
        }
    }
}
