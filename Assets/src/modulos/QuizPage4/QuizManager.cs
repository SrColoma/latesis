using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Timeline;

[System.Serializable]
public class Respuesta
{
    public string nombre; // Nombre de la respuesta
    public Sprite sprite; // Sprite asociado
}

[System.Serializable]
public class Pregunta
{
    public string texto; // Texto de la pregunta
    public string respuestaAsociada; // Nombre de la respuesta correcta
    public AudioClip audioClip; // Clip de audio asociado
}


public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text preguntaTexto; // El texto de la pregunta
    public List<Button> botones; // Lista de los botones
    public GameObject panelpregunta; // El panel de preguntas
    public GameObject panelrespuesta; // El panel de respuestas

    [Header("Datos del Quiz")]
    public List<Pregunta> preguntas; // Lista de preguntas
    public List<Respuesta> respuestas; // Lista de respuestas (nombre y sprite)

    [Header("Señales y Eventos")]
    public SignalAsset signal; // Referencia al Signal Asset
    public GameObject receiverObject; // Objeto con el SignalReceiver

    private Pregunta preguntaActual; // Pregunta actual
    private List<Pregunta> preguntasRestantes; // Preguntas que quedan por responder
    private AudioSource audioSource;

    void Start()
    {
        // Obtener el AudioSource del mismo GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el mismo GameObject que el QuizManager.");
        }

        preguntasRestantes = new List<Pregunta>(preguntas); // Inicializar preguntas restantes
        GenerarPregunta();
    }


    void GenerarPregunta()
    {
        if (preguntasRestantes.Count == 0)
        {
            FinalizarQuiz();
            return;
        }

        // Seleccionar una pregunta aleatoria
        int index = Random.Range(0, preguntasRestantes.Count);
        preguntaActual = preguntasRestantes[index];
        preguntaTexto.text = preguntaActual.texto;

        // Reproducir el audio asociado, si existe
        if (audioSource != null && preguntaActual.audioClip != null)
        {
            audioSource.clip = preguntaActual.audioClip;
            audioSource.Play();
        }

        // Obtener la respuesta correcta
        Respuesta respuestaCorrecta = respuestas.Find(r => r.nombre == preguntaActual.respuestaAsociada);
        if (respuestaCorrecta == null)
        {
            Debug.LogError($"No se encontró una respuesta asociada con el nombre '{preguntaActual.respuestaAsociada}'");
            return;
        }

        // Crear una lista de respuestas aleatorias
        List<Respuesta> opciones = new List<Respuesta>(respuestas);
        opciones.Remove(respuestaCorrecta); // Quitar la respuesta correcta para evitar duplicados
        Shuffle(opciones);

        // Asignar la respuesta correcta a un botón aleatorio
        int botonCorrectoIndex = Random.Range(0, botones.Count);
        AsignarBoton(botonCorrectoIndex, respuestaCorrecta);

        // Asignar respuestas incorrectas a los demás botones
        int respuestaIndex = 0;
        for (int j = 0; j < botones.Count; j++)
        {
            if (j == botonCorrectoIndex) continue;
            AsignarBoton(j, opciones[respuestaIndex]);
            respuestaIndex++;
        }
    }


    void AsignarBoton(int botonIndex, Respuesta respuesta)
    {
        botones[botonIndex].GetComponent<Image>().sprite = respuesta.sprite;
        botones[botonIndex].onClick.RemoveAllListeners();
        botones[botonIndex].onClick.AddListener(() => ValidarRespuesta(respuesta.nombre));
    }

    void ValidarRespuesta(string respuestaSeleccionada)
    {
        if (respuestaSeleccionada == preguntaActual.respuestaAsociada)
        {
            preguntaTexto.text = "¡Correcto!";
            preguntasRestantes.Remove(preguntaActual); // Eliminar la pregunta actual de la lista
        }
        else
        {
            preguntaTexto.text = "Incorrecto. Intenta nuevamente.";
        }

        // Reiniciar los botones
        foreach (var boton in botones)
        {
            boton.onClick.RemoveAllListeners();
        }

        // Generar una nueva pregunta después de 2 segundos
        Invoke(nameof(GenerarPregunta), 2f);
    }

    void FinalizarQuiz()
    {
        panelpregunta.SetActive(false);
        panelrespuesta.SetActive(false);
        EmitSignal();
    }

    void Shuffle(List<Respuesta> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Respuesta temp = lista[i];
            lista[i] = lista[randomIndex];
            lista[randomIndex] = temp;
        }
    }

    public void EmitSignal()
    {
        if (receiverObject == null || signal == null)
        {
            Debug.LogWarning("Signal or receiver object is not set.");
            return;
        }

        SignalReceiver receiver = receiverObject.GetComponent<SignalReceiver>();
        if (receiver == null)
        {
            Debug.LogWarning("No SignalReceiver found on the receiver object.");
            return;
        }

        for (int i = 0; i < receiver.Count(); i++)
        {
            if (receiver.GetSignalAssetAtIndex(i) == signal)
            {
                UnityEvent reaction = receiver.GetReactionAtIndex(i);
                if (reaction != null)
                {
                    reaction.Invoke();
                }
            }
        }
    }
}
