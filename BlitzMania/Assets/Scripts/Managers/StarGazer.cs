using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StarGazer : MonoBehaviour {
    private string m_levelToPlay;
    //these functions set both the local value(used for the toggles) and the
    void Start()
    {
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
                print(PlayerPrefs.GetString("levelToPlay"));
            }
        }
    }

}