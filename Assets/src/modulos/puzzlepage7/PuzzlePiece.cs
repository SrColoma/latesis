using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Transform correctPosition; // Posici�n correcta de la pieza
    [HideInInspector] public Transform originalParent; // Contenedor original de la pieza
}
