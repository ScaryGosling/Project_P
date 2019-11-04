using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public string mainMenu;
    public static SceneHandler sceneHandler;
    public KeybindSet keybindSet;

    public static SceneHandler instance;

    public void ActivateWarrior()
    {
        keybindSet.playerClass = PlayerClass.WARRIOR;
    }
    public void ActivateWizard()
    {
        keybindSet.playerClass = PlayerClass.WIZARD;
    }

    public void Awake()
    {
        instance = this;
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
        StartCoroutine(DelaySceneChange(scene));
    }

    public IEnumerator DelaySceneChange(string scene)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

    public IEnumerator DelayQuit()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void QuitGame()
    {
        StartCoroutine(DelayQuit());
    }


}
