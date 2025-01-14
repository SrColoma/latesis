using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour

{
    public int pageNumber; // N�mero de p�gina asignado.
    public string Instructions;

    // M�todo llamado cuando se activa la p�gina.
    public virtual void OnActivatePage()
    {
        Debug.Log($"P�gina {pageNumber} activada.");
    }

    // M�todo opcional para manejar la desactivaci�n.
    public virtual void OnDeactivatePage()
    {
        Debug.Log($"P�gina {pageNumber} desactivada.");
    }
}
