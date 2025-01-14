using UnityEngine;

public class CuboColor : MonoBehaviour
{
    public Color color; // Color a asignar
    public Shader shader; // Shader que se usar� para crear el material
    public GameObject targetObject; // Objeto cuyo segundo material ser� sobrescrito

    void Start()
    {
        // Verificar que el objeto objetivo est� asignado
        if (targetObject == null)
        {
            Debug.LogError("No se ha asignado el objeto objetivo en el Inspector.");
            return;
        }

        // Aplicar el color al objeto objetivo
        ApplyColorToTarget();
    }

    void ApplyColorToTarget()
    {
        // Obtener el MeshRenderer del objeto objetivo
        MeshRenderer meshRenderer = targetObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("El objeto objetivo no tiene un MeshRenderer.");
            return;
        }

        // Configurar el shader si no est� asignado
        if (shader == null)
        {
            shader = Shader.Find("Standard"); // Usar el shader est�ndar si no se asign�
            if (shader == null)
            {
                Debug.LogError("No se encontr� el shader Standard.");
                return;
            }
        }

        // Crear un nuevo material con el shader y color especificados
        Material newMaterial = new Material(shader);
        newMaterial.color = color;

        // Verificar que el objeto tenga al menos dos materiales
        if (meshRenderer.materials.Length < 2)
        {
            Debug.LogError("El objeto objetivo no tiene suficientes materiales. Debe tener al menos dos materiales asignados.");
            return;
        }

        // Sobrescribir el segundo material
        Material[] materials = meshRenderer.materials;
        materials[1] = newMaterial;
        meshRenderer.materials = materials;

        Debug.Log("Se ha sobrescrito el segundo material del objeto con el nuevo color.");
    }
}
