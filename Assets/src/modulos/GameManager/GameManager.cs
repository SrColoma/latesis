using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public event Action OnMainMenu;
    public event Action OnItemsMenu;
    public event Action OnARPosition;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
    }

    public void MainMenu()
    {
        OnMainMenu?.Invoke();
    }

    public void ItemsMenu()
    {
        OnItemsMenu?.Invoke();
    }

    public void ARPosition()
    {
        OnARPosition?.Invoke();
    }

    public void CloseApp()
    {
        Application.Quit(); 
    }
}
