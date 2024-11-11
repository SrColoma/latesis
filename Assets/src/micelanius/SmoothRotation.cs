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
        // Configuramos el pivote como hijo de la cámara
        cameraTransform = transform.parent; // La cámara es el padre del pivote

        if (cameraTransform != null)
            targetRotation = cameraTransform.rotation;
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        // Obtener la rotación actual de la cámara
        Quaternion currentRotation = cameraTransform.rotation;

        // Calcular la rotación relativa entre el cuadro anterior y el actual
        Quaternion rotationDiff = Quaternion.Inverse(targetRotation) * currentRotation;

        // Interpolar suavemente hacia la nueva rotación de la cámara
        targetRotation = Quaternion.Slerp(targetRotation, currentRotation, Time.deltaTime * rotationSpeed);

        // Aplicar la rotación en el pivote según el modo seleccionado
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
