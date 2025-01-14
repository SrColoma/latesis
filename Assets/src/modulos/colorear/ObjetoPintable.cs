using UnityEngine;

public class ObjetoPintable : MonoBehaviour
{
    private Renderer rendererObjeto;

    // Evento que se dispara cuando el objeto es pintado
    public event System.Action OnPintado;

    void Start()
    {
        // Obtener el Renderer para cambiar su color
        rendererObjeto = GetComponent<Renderer>();
    }

    // Método para aplicar color
    public void AplicarColor(Color color)
    {
        if (rendererObjeto != null)
        {
            rendererObjeto.material.color = color;
            OnPintado?.Invoke(); // Disparar evento
        }
    }
}
