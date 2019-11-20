using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public string mainMenu;
    public static SceneHandler sceneHandler;
    public Settings settings;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private GameObject mainMenuPrompt;
    [SerializeField] private GameObject[] promptToggles;

    public static SceneHandler instance;

    public void SetupPromptToggles()
    {
        promptToggles[0].SetActive(settings.UseWarnings);
        promptToggles[1].SetActive(settings.UseInfo);
        promptToggles[2].SetActive(settings.UseBonus);
    }

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        if(promptToggles != null && promptToggles.Length > 0)
        {
            SetupPromptToggles();
        }
    }

    public void ToggleExtraShopTime(bool toggle) { settings.UseExtraShopTime = !settings.UseExtraShopTime; }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopWindow && shopWindow.activeSelf)
            { 
                shopWindow.SetActive(false);
            }
            else if(mainMenuPrompt != null)
            {
                Time.timeScale = 0;
                mainMenuPrompt.SetActive(true);
            }
                
        }
    }

    public void RestoreTimeScale()
    {
        Time.timeScale = 1;
    }

    public void Toggle(GameObject go) {

        go.SetActive(!go.activeSelf);
    }

    public void ToggleAllPrompts(bool toggle)
    {
        settings.UseWarnings = toggle;
        settings.UseBonus = toggle;
        settings.UseInfo = toggle;
    }
    public void ToggleWarnings() {
        settings.UseWarnings = !settings.UseWarnings;
    }
    public void ToggleInfo()
    {
        settings.UseInfo = !settings.UseInfo;
    }
    public void ToggleBonus()
    {
        settings.UseBonus = !settings.UseBonus;
    }
    public void ToggleAutoRefill()
    {
        settings.UseAutoRefill = !settings.UseAutoRefill;
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
