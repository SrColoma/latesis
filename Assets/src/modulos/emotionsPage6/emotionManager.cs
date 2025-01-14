using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimationController : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator; // El Animator del objeto a animar.
    public Button[] buttons; // Array de botones (5 botones en este caso).

    [Header("Nombres de Animaci�n")]
    public string[] animationNames; // Nombres de las animaciones a ejecutar.

    void Start()
    {
        // Validaci�n inicial
        if (buttons.Length != animationNames.Length || buttons.Length == 0)
        {
            Debug.LogError("Aseg�rate de que el n�mero de botones y nombres de animaciones coincida.");
            return;
        }

        // Asignar funciones a cada bot�n
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Evitar cierre sobre variable local
            buttons[i].onClick.AddListener(() => PlayAnimation(index));
        }
    }

    // Funci�n para reproducir una animaci�n
    void PlayAnimation(int index)
    {
        if (animator != null && index >= 0 && index < animationNames.Length)
        {
            animator.Play(animationNames[index]);
        }
        else
        {
            Debug.LogError("Animator no asignado o �ndice fuera de rango.");
        }
    }
}
