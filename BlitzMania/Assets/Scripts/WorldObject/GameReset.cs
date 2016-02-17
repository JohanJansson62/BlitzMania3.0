using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameReset : MonoBehaviour
{
    [SerializeField] private float m_timeCount = 90f; // Sets the overall gametime for gamemode Crown, presented in seconds.
    private float m_maxTimeCount; // Variable to control so that the m_timeCount does not get past allowed gametime.


    //private bool m_playerStartReady = false;

    //IEnumerator StartDelay() // Sets a time delay of X seconds so that all players are ready before
    //{
    //    yield return new WaitForSeconds(5);
    //    m_playerStartReady = false;
    //}
	
	void Start ()
    {
        m_maxTimeCount = m_timeCount;

	}
	
	
	void Update ()
    {
        m_timeCount -= Time.deltaTime; // Counts down the time in seconds per frame.

        if(m_timeCount <= 0f)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}
}
