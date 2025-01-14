using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Transform correctPosition; // Posición correcta de la pieza
    [HideInInspector] public Transform originalParent; // Contenedor original de la pieza
}
