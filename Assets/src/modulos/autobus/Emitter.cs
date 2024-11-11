using UnityEngine;

public class Emitter : MonoBehaviour
{
    [SerializeField] private string eventID;  // Identificador del evento
    [SerializeField] private object[] arguments; // Arreglo genérico de argumentos

    private void Start()
    {
        // Registramos el evento en el MediatorController
        MediatorController.Instance.RegisterEvent(eventID);
    }

    // Método para emitir el evento con arreglo de argumentos
    public void Emit()
    {
        Debug.Log("Emitiendo evento con argumentos: " + string.Join(", ", arguments));
        MediatorController.Instance.EmitEvent(eventID, arguments);
    }
}
