using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector; // Referencia al Playable Director
    //[SerializeField] private SceneController sceneController; // Referencia al Playable Director
    [SerializeField] private List<TimelineAsset> timelines;     // Lista de timelines para gestionar

    private Dictionary<string, TimelineAsset> timelineDictionary; // Diccionario para fácil acceso por nombre
    private int currentTimelineIndex = 0; // Índice actual del Timeline

    private void Awake()
    {
        // Inicializar el diccionario de timelines
        timelineDictionary = new Dictionary<string, TimelineAsset>();
        foreach (TimelineAsset timeline in timelines)
        {
            if (timeline != null)
            {
                timelineDictionary[timeline.name] = timeline;
            }
        }

        // Asegurarnos de que la lista de timelines tenga al menos uno
        if (timelines.Count > 0)
        {
            currentTimelineIndex = 0; // Inicializar al primer timeline
            //sceneController.SetPage(currentTimelineIndex);
        }
    }

    // Método para sincronizar al final de la animación actual
    private void SyncToEnd()
    {
        if (playableDirector.playableAsset != null)
        {
            // Avanzar al tiempo de duración total del timeline
            playableDirector.time = playableDirector.duration;
            playableDirector.Evaluate(); // Evaluar el estado final
        }
    }

    // Método para cambiar el Timeline por nombre
    public void PlayTimelineByName(string timelineName)
    {
        if (timelineDictionary.ContainsKey(timelineName))
        {
            SyncToEnd(); // Sincronizar al final de la animación actual
            playableDirector.playableAsset = timelineDictionary[timelineName];
            playableDirector.time = 0; // Iniciar desde el principio
            playableDirector.Play();
        }
        else
        {
            Debug.LogWarning("Timeline no encontrado: " + timelineName);
        }
    }

    // Método para cambiar el Timeline por referencia directa
    public void PlayTimeline(TimelineAsset timeline)
    {
        if (timeline != null && timelines.Contains(timeline))
        {
            SyncToEnd(); // Sincronizar al final de la animación actual
            playableDirector.playableAsset = timeline;
            playableDirector.time = 0; // Iniciar desde el principio
            playableDirector.Play();
        }
        else
        {
            Debug.LogWarning("Timeline no válido.");
        }
    }

    // Método para reproducir el siguiente Timeline en la lista
    public void PlayTimelineNext()
    {
        if (timelines.Count == 0)
        {
            Debug.LogWarning("No hay timelines en la lista.");
            return;
        }

        // Si el PlayableDirector aún no tiene un timeline asignado, reproducir el primero
        if (playableDirector.playableAsset == null)
        {
            currentTimelineIndex = 0; // Iniciar desde el primer timeline
        }
        else
        {
            SyncToEnd(); // Sincronizar al final de la animación actual
            // Avanzar al siguiente timeline, con bucle si llegamos al final
            currentTimelineIndex = (currentTimelineIndex + 1) % timelines.Count;
        }
        //sceneController.SetPage(currentTimelineIndex);
        // Reproducir el timeline actual basado en el índice
        playableDirector.playableAsset = timelines[currentTimelineIndex];
        playableDirector.time = 0; // Iniciar desde el principio
        playableDirector.Play();
    }

    // Método para pausar el Timeline actual
    public void PauseTimeline()
    {
        playableDirector.Pause();
    }

    // Método para reanudar el Timeline actual
    public void ResumeTimeline()
    {
        playableDirector.Play();
    }

    // Método para detener el Timeline actual
    public void StopTimeline()
    {
        playableDirector.Stop();
    }
}
