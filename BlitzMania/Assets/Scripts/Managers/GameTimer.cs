using UnityEngine;
using System.Collections;



public class GameTimer : MonoBehaviour
{
    [SerializeField] private float m_timeCount = 60f; // Initialize the start time for each round in gamemode Crown. This serialized variable is mincapped @ 60 seconds.
    private float m_maxTimeCount;
    private bool m_playerIsReady = false;

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(3);
        m_playerIsReady = false;
    }

    void Awake()
    {
        StartDelay(); //Before game start, check if players are ready - tick down 3 seconds before game start.
    }

    void Start ()
    {
        m_maxTimeCount = m_timeCount;
	}
	
	
	void FixedUpdate ()
    {
        m_timeCount -= Time.deltaTime; //Counts down time per seconds per frame

        if(m_timeCount <= 0f)
        {
            Application.LoadLevel(Application.loadedLevel);
            
        }
	}
}
