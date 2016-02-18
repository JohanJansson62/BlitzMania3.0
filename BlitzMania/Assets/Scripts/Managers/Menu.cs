using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public void GoToPage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitProgram()
    {
        Application.Quit();
    }
     void Start()
    {

    }

    
    void Update()
    {

    }
}
