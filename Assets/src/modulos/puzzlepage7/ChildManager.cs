using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables; // Necesario para controlar el Timeline.
using System.Collections;

public class ChildManager : MonoBehaviour
{
    private int currentChildIndex = 0; // Índice del hijo actualmente activo.
    public DialogController dialogoInstrucciones;

    // GameObject que contiene el Timeline.
    public GameObject transition;

    // Componente PlayableDirector del Timeline.
    private PlayableDirector timelineDirector;

    void Start()
    {
        if (transition != null)
        {
            timelineDirector = transition.GetComponent<PlayableDirector>();
            if (timelineDirector == null)
            {
                Debug.LogWarning("El GameObject de Timeline no tiene un componente PlayableDirector.");
            }
        }

        DeactivateAllExceptFirst();
    }

    public void DeactivateAllExceptFirst()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == 0);
        }
        currentChildIndex = 0;

        Transform nextChild = transform.GetChild(currentChildIndex);
        nextChild.gameObject.SetActive(true);
        dialogoInstrucciones.ShowPageInstructions(currentChildIndex);

        PageManager pageComponent = nextChild.GetComponent<PageManager>();
        if (pageComponent != null)
        {
            pageComponent.OnActivatePage();
        }
        else
        {
            Debug.LogWarning($"El hijo {nextChild.name} no tiene un componente PageManager.");
        }
    }

    public void ActivateNextChild()
    {
        if (transform.childCount == 0) return;

        StartCoroutine(ShowTimelineAndActivateNext());
    }

    private IEnumerator ShowTimelineAndActivateNext()
    {
        // Desactiva la página actual.
        DeactivatePageRoutine(currentChildIndex);

        // Muestra el Timeline si está configurado.
        if (transition != null && timelineDirector != null)
        {
            transition.SetActive(true);
            timelineDirector.Play();

            // Espera hasta que termine el Timeline.
            yield return new WaitForSeconds((float)timelineDirector.duration);

            // Espera 2 segundos adicionales.
            //yield return new WaitForSeconds(2f);

            // Desactiva el GameObject del Timeline.
            transition.SetActive(false);
        }

        // Calcula el siguiente índice de manera circular.
        currentChildIndex = (currentChildIndex + 1) % transform.childCount;

        // Activa la siguiente página.
        ActivatePageRoutine(currentChildIndex);
    }

    public void ActivatePageRoutine(int idx)
    {
        Transform nextChild = transform.GetChild(idx);
        nextChild.gameObject.SetActive(true);

        PageManager pageComponent = nextChild.GetComponent<PageManager>();
        if (pageComponent != null)
        {
            pageComponent.OnActivatePage();
            dialogoInstrucciones.ShowPageInstructions(currentChildIndex);
        }
        else
        {
            Debug.LogWarning($"El hijo {nextChild.name} no tiene un componente PageManager.");
        }
    }

    public void DeactivatePageRoutine(int idx)
    {
        if (idx < 0 || idx >= transform.childCount)
        {
            Debug.LogWarning("El índice proporcionado está fuera de rango.");
            return;
        }

        Transform child = transform.GetChild(idx);
        PageManager pageComponent = child.GetComponent<PageManager>();

        if (pageComponent != null)
        {
            pageComponent.OnDeactivatePage();
        }
        else
        {
            Debug.LogWarning($"El hijo {child.name} no tiene un componente PageManager.");
        }

        child.gameObject.SetActive(false);
    }
}
