using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PageCincoPintar : PageManager
{
    public List<GameObject> objectsToToggle; // Lista de objetos a activar/desactivar
    public List<ObjetoPintable> pintables; // Lista de objetos pintables
    private int pintadosCount = 0; // Contador de objetos pintados


    public  GameObject butonNext;
    private PlayableDirector playableDirector;

    [Header("Señales y Eventos")]
    public SignalAsset signal; // Referencia al Signal Asset
    public GameObject receiverObject; // Objeto con el SignalReceiver

    private void Awake()
    {
        // Obtiene el PlayableDirector del mismo GameObject
        playableDirector = GetComponent<PlayableDirector>();

        // Registra el evento de cada objeto pintable
        foreach (var pintable in pintables)
        {
            pintable.OnPintado += IncrementarContador; // Suscribir evento
        }
    }

    public override void OnActivatePage()
    {
        if (playableDirector != null)
        {
            playableDirector.time = 0; // Reinicia al principio
            playableDirector.Play();  // Reproduce la Timeline
        }

        butonNext.SetActive(false);
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

    private void IncrementarContador()
    {
        pintadosCount++;
        if (pintadosCount >= pintables.Count)
        {
            //EmitSignal(); // Emite la señal cuando todos los objetos hayan sido pintados
            butonNext.SetActive(true);
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
