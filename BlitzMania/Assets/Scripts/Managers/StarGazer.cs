using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class StarGazer : MonoBehaviour {
    private string m_levelToPlay;
    //these functions set both the local value(used for the toggles) and the
    void Start()
    {
        m_levelToPlay = "Level_1"; ;//avoids errors by having a default value
    }
    public void SetTimeToPlay(Toggle sender)
    {
        if (sender.isOn)
        {//an ugly but working soloution, naming the objects for their value as you cannot use multiple arguments
            //with the 
            PlayerPrefs.SetFloat("timeToPlay", float.Parse(sender.name));
            print(PlayerPrefs.GetFloat("timeToPlay"));
        }
    }
    public void SetNumberOfPlayers(Toggle sender)
    {//same as the other function
        if (sender.isOn)
        {
            PlayerPrefs.SetInt("numberOfPlayers", int.Parse(sender.name));
            print(PlayerPrefs.GetInt("numberOfPlayers"));
        }
    }
    public void SetLevelToPlay(Toggle sender)
    {
        {//same as the other function
            if (sender.isOn)
            {
                m_levelToPlay = sender.name;
                PlayerPrefs.SetString("levelToPlay",m_levelToPlay);
                print(PlayerPrefs.GetString("levelToPlay"));//this is saved for next time
            }
        }
    }
    //public void GoToNextLevel(Toggle sender)
    //{
    //    if (sender.isOn)
    //    {
    //        SceneManager.LoadScene(m_levelToPlay);
    //    }
    //}

}