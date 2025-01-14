using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class VirtualHandController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform grabPoint;
    [SerializeField] private LayerMask puzzleLayer;

    [Header("Configuración")]
    [SerializeField] private float snapDistance = 0.1f;
    [SerializeField] private float snapDuration = 0.3f;

    [Header("Señales y Eventos")]
    public SignalAsset signal; // Referencia al Signal Asset
    public GameObject receiverObject; // Objeto con el SignalReceiver

    private GameObject grabbedPiece;
    private int correctPieces;
    private SphereCollider handCollider;


    [SerializeField] private List<PuzzleSet> puzzleSets = new List<PuzzleSet>();
    private int currentPuzzleIndex;

    private void Start()
    {
        // Asegurarse de que tenemos un SphereCollider
        handCollider = GetComponent<SphereCollider>();
        if (handCollider == null)
        {
            handCollider = gameObject.AddComponent<SphereCollider>();
        }
        handCollider.isTrigger = true; // Importante: debe ser trigger
        handCollider.radius = 0.5f; // Ajusta este valor según necesites
    }

    private void Update()
    {
        if (grabbedPiece != null)
        {
            // Obtener el componente PuzzlePiece
            PuzzlePiece pieceScript = grabbedPiece.GetComponent<PuzzlePiece>();
            if (pieceScript != null && pieceScript.correctPosition != null)
            {
                // Verificar si estamos cerca de la posición correcta
                float distance = Vector3.Distance(
                    grabbedPiece.transform.position,
                    pieceScript.correctPosition.position
                );

                // Si estamos suficientemente cerca, hacer snap
                if (distance < snapDistance)
                {
                    ReleasePiece();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si no tenemos una pieza y tocamos una pieza de puzzle
        if (grabbedPiece == null && other.CompareTag("PuzzlePiece"))
        {
            GrabPiece(other.gameObject);
        }
    }


    private void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (grabbedPiece != null)
        {
            ReleasePiece();
        }
        else
        {
            TryGrabNearestPiece();
        }
    }

    private void TryGrabNearestPiece()
    {
        Collider[] nearbyPieces = Physics.OverlapSphere(grabPoint.position, 0.5f, puzzleLayer);

        if (nearbyPieces.Length == 0) return;

        // Encontrar la pieza más cercana
        GameObject nearestPiece = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider col in nearbyPieces)
        {
            if (!col.CompareTag("PuzzlePiece")) continue;

            float distance = Vector3.Distance(grabPoint.position, col.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPiece = col.gameObject;
            }
        }

        if (nearestPiece != null)
        {
            GrabPiece(nearestPiece);
        }
    }

    private void GrabPiece(GameObject piece)
    {
        if (piece == null) return;

        grabbedPiece = piece;

        // Desactivar física
        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Guardar el padre original
        PuzzlePiece pieceScript = piece.GetComponent<PuzzlePiece>();
        if (pieceScript != null && pieceScript.originalParent == null)
        {
            pieceScript.originalParent = piece.transform.parent;
        }

        // Hacer la pieza hija del punto de agarre
        piece.transform.SetParent(grabPoint);
        piece.transform.localPosition = Vector3.zero;
        piece.transform.localRotation = Quaternion.Euler(0, 90, 0);
    }

    private void ReleasePiece()
    {
        if (grabbedPiece == null) return;

        PuzzlePiece pieceScript = grabbedPiece.GetComponent<PuzzlePiece>();
        if (pieceScript == null) return;

        Transform targetTransform = pieceScript.correctPosition;
        if (Vector3.Distance(grabbedPiece.transform.position, targetTransform.position) < snapDistance)
        {
            StartCoroutine(SnapPieceToPosition(grabbedPiece, targetTransform));
            correctPieces++;
            CheckPuzzleCompletion();
        }
        else
        {
            // Si no estamos cerca de la posición correcta, devolver la física
            Rigidbody rb = grabbedPiece.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            grabbedPiece.transform.SetParent(pieceScript.originalParent);
        }

        grabbedPiece = null;
    }

    private void SetPiecePhysics(GameObject piece, bool isKinematic)
    {
        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = isKinematic;
            rb.useGravity = !isKinematic;
        }

        Collider col = piece.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = !isKinematic;
        }
    }

    private IEnumerator SnapPieceToPosition(GameObject piece, Transform target)
    {
        Vector3 startPos = piece.transform.position;
        Quaternion startRot = piece.transform.rotation;
        float elapsed = 0;

        while (elapsed < snapDuration)
        {
            float t = elapsed / snapDuration;
            t = Mathf.SmoothStep(0, 1, t); // Suavizar el movimiento

            piece.transform.position = Vector3.Lerp(startPos, target.position, t);
            piece.transform.rotation = Quaternion.Lerp(startRot, target.rotation, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Colocar la pieza en su posición final
        piece.transform.SetParent(target);
        piece.transform.localPosition = Vector3.zero;
        piece.transform.localRotation = Quaternion.identity;

        // Desactivar física y collider
        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider col = piece.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    private void CheckPuzzleCompletion()
    {
        PuzzleSet currentPuzzle = puzzleSets[currentPuzzleIndex];
        if (correctPieces >= currentPuzzle.totalPieces)
        {
            StartCoroutine(HandlePuzzleCompletion());
        }
    }

    private void DeactivatePuzzle(int index)
    {
        if (index < 0 || index >= puzzleSets.Count)
        {
            Debug.LogError("Índice de puzzle inválido para desactivar");
            return;
        }

        PuzzleSet puzzle = puzzleSets[index];

        // Recorrer todas las piezas y resetearlas
        foreach (Transform child in puzzle.container.transform)
        {
            if (child.CompareTag("PuzzlePiece"))
            {
                // Obtener componentes
                PuzzlePiece pieceScript = child.GetComponent<PuzzlePiece>();
                Rigidbody rb = child.GetComponent<Rigidbody>();
                Collider col = child.GetComponent<Collider>();

                // Resetear la física
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }

                // Reactivar el collider
                if (col != null)
                {
                    col.enabled = true;
                }

                // Devolver al padre original si existe
                if (pieceScript != null && pieceScript.originalParent != null)
                {
                    child.SetParent(pieceScript.originalParent);
                }

                // Resetear posición y rotación si hay una posición correcta guardada
                if (pieceScript != null && pieceScript.correctPosition != null)
                {
                    child.position = pieceScript.correctPosition.position;
                    child.rotation = pieceScript.correctPosition.rotation;
                }
            }
        }

        // Desactivar el contenedor
        puzzle.container.SetActive(false);
        correctPieces = 0;
    }

    private IEnumerator HandlePuzzleCompletion()
    {
        // Esperar un momento para que el jugador vea la última pieza colocada
        yield return new WaitForSeconds(1f);

        // Desactivar el puzzle actual antes de incrementar el índice
        DeactivatePuzzle(currentPuzzleIndex);

        currentPuzzleIndex++;
        if (currentPuzzleIndex < puzzleSets.Count)
        {
            ActivatePuzzle(currentPuzzleIndex);
        }
        else
        {
            OnAllPuzzlesCompleted();
        }
    }

    public void ActivatePuzzle(int index)
    {
        if (index < 0 || index >= puzzleSets.Count)
        {
            Debug.LogError("Índice de puzzle inválido");
            return;
        }

        // Desactivar puzzle actual si existe
        if (currentPuzzleIndex < puzzleSets.Count && currentPuzzleIndex != index)
        {
            DeactivatePuzzle(currentPuzzleIndex);
        }

        // Activar nuevo puzzle
        currentPuzzleIndex = index;
        PuzzleSet newPuzzle = puzzleSets[index];
        newPuzzle.container.SetActive(true);
        ScatterPuzzlePieces(newPuzzle.container);
        correctPieces = 0;
    }

    private void ScatterPuzzlePieces(GameObject container)
    {
        foreach (Transform piece in container.transform)
        {
            if (!piece.CompareTag("PuzzlePiece")) continue;

            // Guardar padre original
            PuzzlePiece pieceScript = piece.GetComponent<PuzzlePiece>();
            if (pieceScript != null && pieceScript.originalParent == null)
            {
                pieceScript.originalParent = piece.parent;
            }

            // Posición aleatoria
            Vector3 randomPosition = container.transform.position +
                                   Random.insideUnitSphere;
            randomPosition.y = container.transform.position.y; // Mantener misma altura

            // Rotación aleatoria
            Quaternion randomRotation = Quaternion.Euler(
                0,
                Random.Range(0f, 360f),
                0
            );

            piece.position = randomPosition;
            piece.rotation = randomRotation;
            SetPiecePhysics(piece.gameObject, false);
        }
    }

    private void OnAllPuzzlesCompleted()
    {
        Debug.Log("¡Felicidades! Has completado todos los puzzles!");
        // Aquí puedes agregar más lógica de victoria
        EmitSignal();
    }

    private void OnDrawGizmos()
    {
        if (handCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, handCollider.radius);
        }
    }

    public void EmitSignal()
    {
        if (receiverObject == null || signal == null)
        {
            Debug.LogWarning("Signal or receiver object is not set.");
            return;
        }

        SignalReceiver receiver = receiverObject.GetComponent<SignalReceiver>();
        if (receiver == null)
        {
            Debug.LogWarning("No SignalReceiver found on the receiver object.");
            return;
        }

        for (int i = 0; i < receiver.Count(); i++)
        {
            if (receiver.GetSignalAssetAtIndex(i) == signal)
            {
                UnityEvent reaction = receiver.GetReactionAtIndex(i);
                if (reaction != null)
                {
                    reaction.Invoke();
                }
            }
        }
    }
}

[System.Serializable]
public class PuzzleSet
{
    public GameObject container;
    public int totalPieces;
    public string puzzleName;
}


