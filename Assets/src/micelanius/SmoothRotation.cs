using UnityEngine;

public class SmoothPivotRotation : MonoBehaviour
{
    public enum RotationMode
    {
        ANTICIPATION,
        DELAY
    }

    [SerializeField]
    private RotationMode rotationMode = RotationMode.DELAY;

    [SerializeField]
    private float rotationSpeed = 5.0f;

    private Quaternion targetRotation;
    private Transform cameraTransform;

    private void Start()
    {
        // Configuramos el pivote como hijo de la c�mara
        cameraTransform = transform.parent; // La c�mara es el padre del pivote

        if (cameraTransform != null)
            targetRotation = cameraTransform.rotation;
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        // Obtener la rotaci�n actual de la c�mara
        Quaternion currentRotation = cameraTransform.rotation;

        // Calcular la rotaci�n relativa entre el cuadro anterior y el actual
        Quaternion rotationDiff = Quaternion.Inverse(targetRotation) * currentRotation;

        // Interpolar suavemente hacia la nueva rotaci�n de la c�mara
        targetRotation = Quaternion.Slerp(targetRotation, currentRotation, Time.deltaTime * rotationSpeed);

        // Aplicar la rotaci�n en el pivote seg�n el modo seleccionado
        if (rotationMode == RotationMode.ANTICIPATION)
        {
            transform.rotation = rotationDiff * targetRotation;
        }
        else // RotationMode.DELAY
        {
            transform.rotation = Quaternion.Inverse(rotationDiff) * targetRotation;
        }
    }
}
