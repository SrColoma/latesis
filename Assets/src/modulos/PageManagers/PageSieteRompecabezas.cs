using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PageSieteRompecabezas : PageManager
{

    public VirtualHandController pincel;
    public List<GameObject> objectsToToggle; // Lista de objetos a activar/desactivar


    private PlayableDirector playableDirector;

    private void Awake()
    {
        // Obtiene el PlayableDirector del mismo GameObject
        playableDirector = GetComponent<PlayableDirector>();
    }

    public override void OnActivatePage()
    {
        if (playableDirector != null)
        {
            playableDirector.time = 0; // Reinicia al principio
            playableDirector.Play();  // Reproduce la Timeline
        }
        //pincel.ActivateRandomPuzzle();
        pincel.ActivatePuzzle(0);
        //Debug.Log($"Página {pageNumber} activada.");

        // Activar todos los objetos de la lista
        foreach (GameObject obj in objectsToToggle)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Uno de los objetos en la lista es nulo.");
            }
        }
    }

    public override void OnDeactivatePage()
    {
        if (playableDirector != null)
        {
            playableDirector.Stop(); // Detiene la Timeline
        }

        Debug.Log($"Página {pageNumber} desactivada.");

        // Desactivar todos los objetos de la lista
        foreach (GameObject obj in objectsToToggle)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Uno de los objetos en la lista es nulo.");
            }
        }
    }
}

