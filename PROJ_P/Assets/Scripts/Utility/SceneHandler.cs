using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public string optionsScene;
    private string lastScene;
    public static SceneHandler sceneHandler;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().name == optionsScene)
            {
                GoToScene(lastScene);
            }
            else
            {
                GoToScene(optionsScene);
            }
        }
    }

    public void GoToScene(string scene)
    {
        lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }

    public void Awake()
    {
        if(sceneHandler == null)
            sceneHandler = this;

        if (sceneHandler != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
    }

}
