using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void quit_game()
    {
        print("Jeu quitté");
        Application.Quit();
    }
}
