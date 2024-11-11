using UnityEngine;

public class PincelPunta : MonoBehaviour
{
    private Renderer rendererPincel; // Renderer del pincel para cambiar su color
    private Color colorPincel;       // Color actual del pincel

    void Start()
    {
        // Obtén el Renderer del pincel y su color inicial
        rendererPincel = GetComponent<Renderer>();
        colorPincel = rendererPincel.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto tiene el script CuboColor
        CuboColor cuboColor = other.GetComponent<CuboColor>();
        if (cuboColor != null)
        {
            // Cambia el color del pincel al color del cubo
            colorPincel = cuboColor.color;
            rendererPincel.material.color = colorPincel;
            //Debug.Log("Pincel cambió de color a: " + colorPincel);
            return; // Salimos para no pintar si es un CuboColor
        }

        // Verifica si el objeto tiene el script ObjetoPintable
        ObjetoPintable objetoPintable = other.GetComponent<ObjetoPintable>();
        if (objetoPintable != null)
        {
            // Aplica el color actual del pincel al objeto pintable
            objetoPintable.AplicarColor(colorPincel);
            //Debug.Log("Objeto pintado con color: " + colorPincel);
        }
    }
}
