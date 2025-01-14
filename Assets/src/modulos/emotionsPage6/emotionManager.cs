using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimationController : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator; // El Animator del objeto a animar.
    public Button[] buttons; // Array de botones (5 botones en este caso).

    [Header("Nombres de Animación")]
    public string[] animationNames; // Nombres de las animaciones a ejecutar.

    void Start()
    {
        // Validación inicial
        if (buttons.Length != animationNames.Length || buttons.Length == 0)
        {
            Debug.LogError("Asegúrate de que el número de botones y nombres de animaciones coincida.");
            return;
        }

        // Asignar funciones a cada botón
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Evitar cierre sobre variable local
            buttons[i].onClick.AddListener(() => PlayAnimation(index));
        }
    }

    // Función para reproducir una animación
    void PlayAnimation(int index)
    {
        if (animator != null && index >= 0 && index < animationNames.Length)
        {
            animator.Play(animationNames[index]);
        }
        else
        {
            Debug.LogError("Animator no asignado o índice fuera de rango.");
        }
    }
}
