using UnityEngine;

public class ObjetoPintable : MonoBehaviour
{
    private Renderer rendererObjeto;

    void Start()
    {
        // Obtener el Renderer para cambiar su color
        rendererObjeto = GetComponent<Renderer>();
    }

    // M�todo para aplicar color
    public void AplicarColor(Color color)
    {
        if (rendererObjeto != null)
        {
            rendererObjeto.material.color = color;
        }
    }
}