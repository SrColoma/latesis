using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro; // Importa la biblioteca de TextMeshPro

public class DialogController : MonoBehaviour
{
    public GameObject dialogBox; // Cuadro de diálogo
    public Button openButton;    // Botón que abre el cuadro de diálogo
    public TMP_Text dialogText;  // Texto del cuadro de diálogo (TextMeshPro)
    public GameObject pages;     // Contenedor de páginas

    private PlayerInputActions playerInputActions; // Input Actions
    private bool isDialogOpen = false;

    void Awake()
    {
        // Instancia el asset de Input Actions
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        // Asigna el evento de la acción "Tap" a una función
        playerInputActions.Gameplay.Tap.performed += HandleTap;
        playerInputActions.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Gameplay.Tap.performed -= HandleTap;
        playerInputActions.Disable();
    }

    void Start()
    {
        // Asegúrate de que el cuadro de diálogo esté oculto al inicio
        dialogBox.SetActive(false);

        // Asigna la función para el botón de abrir
        openButton.onClick.AddListener(OpenDialog);
    }

    private void HandleTap(InputAction.CallbackContext context)
    {
        // Si el cuadro de diálogo está abierto, cierra al tocar
        if (isDialogOpen)
        {
            // Ignorar si el toque ocurre sobre UI
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            CloseDialog();
        }
    }

    void OpenDialog()
    {
        dialogBox.SetActive(true);
        isDialogOpen = true;
    }

    void CloseDialog()
    {
        dialogBox.SetActive(false);
        isDialogOpen = false;
    }

    /// <summary>
    /// Cambia el texto del cuadro de diálogo según el índice de la página.
    /// </summary>
    /// <param name="pageIndex">Índice de la página en el contenedor de páginas.</param>
    public void ShowPageInstructions(int pageIndex)
    {
        // Verifica que el índice sea válido
        if (pageIndex < 0 || pageIndex >= pages.transform.childCount)
        {
            Debug.LogError("Índice de página fuera de rango.");
            return;
        }

        // Obtiene la página por índice
        GameObject page = pages.transform.GetChild(pageIndex).gameObject;

        // Obtiene el componente PageManager
        PageManager pageManager = page.GetComponent<PageManager>();
        if (pageManager == null)
        {
            Debug.LogError("PageManager no encontrado en la página.");
            return;
        }

        // Actualiza el texto del cuadro de diálogo con las instrucciones
        dialogText.text = pageManager.Instructions;
    }
}
