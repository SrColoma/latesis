using UnityEngine;
using UnityEngine.Events;

public class Receptor : MonoBehaviour
{
    [SerializeField] private string eventID; // Identificador del evento
    [SerializeField] private UnityEvent<object[]> onEventTriggered; // Evento configurable desde el inspector

    private void Start()
    {
        // Suscribimos el UnityEvent al evento correspondiente en el MediatorController
        MediatorController.Instance.SubscribeEvent(eventID, OnEventTriggered);
    }

    // Método que será llamado cuando el evento sea emitido
    private void OnEventTriggered(object[] arguments)
    {
        Debug.Log("Evento recibido: " + eventID + " con argumentos: " + string.Join(", ", arguments));

        // Invocamos el UnityEvent con los argumentos recibidos
        onEventTriggered?.Invoke(arguments);
    }
}
