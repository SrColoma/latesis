using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro; // Importa la biblioteca de TextMeshPro

public class DialogController : MonoBehaviour
{
    public GameObject dialogBox; // Cuadro de di�logo
    public Button openButton;    // Bot�n que abre el cuadro de di�logo
    public TMP_Text dialogText;  // Texto del cuadro de di�logo (TextMeshPro)
    public GameObject pages;     // Contenedor de p�ginas

    private PlayerInputActions playerInputActions; // Input Actions
    private bool isDialogOpen = false;

    void Awake()
    {
        // Instancia el asset de Input Actions
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        // Asigna el evento de la acci�n "Tap" a una funci�n
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
        // Aseg�rate de que el cuadro de di�logo est� oculto al inicio
        dialogBox.SetActive(false);

        // Asigna la funci�n para el bot�n de abrir
        openButton.onClick.AddListener(OpenDialog);
    }

    private void HandleTap(InputAction.CallbackContext context)
    {
        // Si el cuadro de di�logo est� abierto, cierra al tocar
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
    /// Cambia el texto del cuadro de di�logo seg�n el �ndice de la p�gina.
    /// </summary>
    /// <param name="pageIndex">�ndice de la p�gina en el contenedor de p�ginas.</param>
    public void ShowPageInstructions(int pageIndex)
    {
        // Verifica que el �ndice sea v�lido
        if (pageIndex < 0 || pageIndex >= pages.transform.childCount)
        {
            Debug.LogError("�ndice de p�gina fuera de rango.");
            return;
        }

        // Obtiene la p�gina por �ndice
        GameObject page = pages.transform.GetChild(pageIndex).gameObject;

        // Obtiene el componente PageManager
        PageManager pageManager = page.GetComponent<PageManager>();
        if (pageManager == null)
        {
            Debug.LogError("PageManager no encontrado en la p�gina.");
            return;
        }

        // Actualiza el texto del cuadro de di�logo con las instrucciones
        dialogText.text = pageManager.Instructions;
    }
}
