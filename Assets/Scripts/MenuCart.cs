using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCart : MonoBehaviour
{
    public MenuManager menuManager;

    public void OnAnimFinished(int whichOne)
    {
        switch (whichOne)
        {
            case 0:
                menuManager.ShowButtons();
                break;
            case 1:
                menuManager.StartGame();
                break;
            case 2:
                menuManager.Settings();
                break;
            case 3:
                menuManager.ExitGame();
                break;
        }
    }
}
