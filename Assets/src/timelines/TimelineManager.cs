using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector; // Referencia al Playable Director
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
        }
    }

    // Método para cambiar el Timeline por nombre
    public void PlayTimelineByName(string timelineName)
    {
        if (timelineDictionary.ContainsKey(timelineName))
        {
            playableDirector.playableAsset = timelineDictionary[timelineName];
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
            playableDirector.playableAsset = timeline;
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
            // Avanzar al siguiente timeline, con bucle si llegamos al final
            currentTimelineIndex = (currentTimelineIndex + 1) % timelines.Count;
        }

        // Reproducir el timeline actual basado en el índice
        playableDirector.playableAsset = timelines[currentTimelineIndex];
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
