using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public string mainMenu;
    public static SceneHandler sceneHandler;
    public KeybindSet keybindSet;
    public void ActivateWarrior()
    {
        keybindSet.playerClass = PlayerClass.WARRIOR;
    }
    public void ActivateWizard()
    {
        keybindSet.playerClass = PlayerClass.WIZARD;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToScene(mainMenu);
        }
    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
