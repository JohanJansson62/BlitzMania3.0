using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public void GoToPage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitProgram()
    {
        Application.Quit();
    }
	// Use this for initializaton
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
