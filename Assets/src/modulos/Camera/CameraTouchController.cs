using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTouchController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public LayerMask touchableLayer;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private Vector3 movementDirection;
    private bool isRotating = false;
    private InputAction touchAction;

    void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        touchAction = playerInput.actions["Touch"];
        //Debug.Log("CameraTouchController inicializado. PlayerInput encontrado: " + (playerInput != null));
    }

    void Update()
    {
        var touch = Touchscreen.current.primaryTouch;
        if (touch == null)
        {
            //Debug.LogWarning("No se detectó Touchscreen.current.primaryTouch");
            return;
        }

        if (touch.press.isPressed)
        {
            if (!isDragging)
            {
                lastTouchPosition = touch.position.ReadValue();
                isDragging = true;
                //Debug.Log($"Inicio de toque - Posición: {lastTouchPosition}");

                Ray ray = Camera.main.ScreenPointToRay(lastTouchPosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, touchableLayer))
                {
                    isRotating = true;
                    //Debug.Log($"Raycast exitoso - Objeto golpeado: {hit.collider.gameObject.name} en la posición {hit.point}");
                }
                else
                {
                    //Debug.Log("Raycast no golpeó ningún objeto en la capa especificada");
                }
            }
            else
            {
                Vector2 currentTouchPosition = touch.position.ReadValue();
                Vector2 deltaPosition = currentTouchPosition - lastTouchPosition;

                if (isRotating)
                {
                    float rotationX = -deltaPosition.y * rotationSpeed * Time.deltaTime;
                    float rotationY = deltaPosition.x * rotationSpeed * Time.deltaTime;
                    //Debug.Log($"Rotando - X: {rotationX:F2}° Y: {rotationY:F2}°");

                    transform.RotateAround(transform.position, transform.right, rotationX);
                    transform.RotateAround(transform.position, Vector3.up, rotationY);
                }
                else
                {
                    movementDirection = new Vector3(deltaPosition.x, 0, deltaPosition.y).normalized;
                   //Debug.Log($"Moviendo - Dirección: {movementDirection}, Delta: {deltaPosition}");
                    transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);
                }

                lastTouchPosition = currentTouchPosition;
            }
        }
        else if (isDragging)
        {
            //Debug.Log("Fin del toque - Reseteando estados");
            isDragging = false;
            isRotating = false;
            movementDirection = Vector3.zero;
        }
    }

    void OnEnable()
    {
        Debug.Log("CameraTouchController habilitado");
    }

    void OnDisable()
    {
        Debug.Log("CameraTouchController deshabilitado");
    }
}