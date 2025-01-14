using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour

{
    public int pageNumber; // Número de página asignado.
    public string Instructions;

    // Método llamado cuando se activa la página.
    public virtual void OnActivatePage()
    {
        Debug.Log($"Página {pageNumber} activada.");
    }

    // Método opcional para manejar la desactivación.
    public virtual void OnDeactivatePage()
    {
        Debug.Log($"Página {pageNumber} desactivada.");
    }
}
