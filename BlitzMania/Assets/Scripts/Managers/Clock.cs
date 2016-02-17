using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Clock : MonoBehaviour {
    private float m_timer;
    private Text m_displayTime;
	// Use this for initialization
	void Start () {//this object starts disabled, the objects are initialized but the values should be changed
        m_timer = 500;
        m_displayTime = GetComponent<Text>();
        if (PlayerPrefs.HasKey("timeToPlay"))//checks if the value exists
        {
            m_timer = PlayerPrefs.GetFloat("timeToPlay");//then loads it
        }
        else
        {
            m_timer = 300;//or is set to a default value
        }

    }
    // Update is called once per frame
    void Update () {

        m_timer -= Time.deltaTime; ;//removes the time since last update
        m_displayTime.text = "Time Left = " + m_timer.ToString() + "\n \n";//the \n means it's a new line
    }
}
