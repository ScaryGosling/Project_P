using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public string mainMenu;
    public static SceneHandler sceneHandler;
    public KeybindSet keybindSet;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private GameObject[] promptToggles;

    public static SceneHandler instance;

    public void SetupPromptToggles()
    {
        promptToggles[0].SetActive(keybindSet.useWarnings);
        promptToggles[1].SetActive(keybindSet.useInfo);
        promptToggles[2].SetActive(keybindSet.useBonus);
    }

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        if(promptToggles.Length > 0)
        {
            SetupPromptToggles();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopWindow && shopWindow.activeSelf)
            { 
                shopWindow.SetActive(false);
            }
            else
                GoToScene(mainMenu);
        }
    }

    public void Toggle(GameObject go) {

        go.SetActive(!go.activeSelf);
    }

    public void ToggleWarnings() {
        keybindSet.useWarnings = !keybindSet.useWarnings;
    }
    public void ToggleInfo()
    {
        keybindSet.useInfo = !keybindSet.useInfo;
    }
    public void ToggleBonus()
    {
        keybindSet.useBonus = !keybindSet.useBonus;
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
