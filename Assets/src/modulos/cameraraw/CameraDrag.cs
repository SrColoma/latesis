using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 0.1f; // Velocidad de arrastre de la cámara
    private Vector3 dragOrigin;

    void Update()
    {
        // Comprobar si hay un toque en la pantalla
        if (Input.touchCount == 1) // Solo un toque
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Registrar el punto de inicio del arrastre
                dragOrigin = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // Calcular la distancia del movimiento
                Vector3 delta = touch.deltaPosition * dragSpeed;

                // Invertir los ejes si es necesario para que se mueva en el mundo
                Vector3 movement = new Vector3(-delta.x, 0, -delta.y);

                // Mover la cámara en el espacio del mundo
                transform.Translate(movement, Space.World);
            }
        }
    }
}
