using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    [Range(1,4)]
    [SerializeField] private int m_playerNumber;
    [SerializeField] private Text m_diplayScore;
    private float m_timer;
    private int m_numbPlayers;
    private int m_playerWithCrown = 0;//gives the number of the player
    private System.Collections.Generic.List<float> m_playerScoreList;
    // Use this for initialization
    void Start () {
        m_diplayScore = GetComponent<Text>();
        m_playerScoreList = new System.Collections.Generic.List<float>();

        if (PlayerPrefs.HasKey("timeToPlay"))//checks if the value exists
        {
            m_timer = PlayerPrefs.GetFloat("timeToPlay");//then loads it
        }
        else
        {
            m_timer = 300;//or is set to a default value
        }
        
        if (PlayerPrefs.HasKey("numberOfPlayers"))//same as previous
        {
            m_numbPlayers = PlayerPrefs.GetInt("numberOfPlayers");//gets the number of players
        }
        else
        {
            m_numbPlayers = 4;//or is set to a default value
        }
        for (int i = 0; i < m_numbPlayers; i++)//runs the script once for each player
        {
        
            m_playerScoreList.Add(0.0f);//all of them start at zero score
            
        }//the players score is placed at index playerNumber-1 (because there is no player 0)
    }
    public void NewPlayerWithCrown(int playerNr)
    {
        //when a player gets the crown it sends its number here
        m_playerWithCrown = playerNr;
    }
	// Update is called once per frame
	void Update () {
        m_timer -= Time.deltaTime; ;//removes the time since last update
        if (m_playerWithCrown != 0)
        {
            for (int i = 0; i < m_playerScoreList.Count; i++)
            {
                if (i == (m_playerWithCrown - 1))//decrease by one as there is still no player 0
                {
                    m_playerScoreList[i] += Time.deltaTime * 10; ;
                }
            }
        }
        if (m_timer > 0)//has to make check after or the last score wont be added
        {
            m_diplayScore.text = "";//resets the text so it can be updated
            //for (int i = 0; i < m_playerScoreList.Count; i++)
            switch(m_playerNumber)
            {
                case 1:
                    m_diplayScore.text += "RED: " + (int)m_playerScoreList[0];
                    break;

                case 2:
                    m_diplayScore.text += "BLU: " + (int)m_playerScoreList[1];
                    break;

                case 3:
                    m_diplayScore.text += "GRN: " + (int)m_playerScoreList[2];
                    break;

                case 4:
                    m_diplayScore.text += "PNK: " + (int)m_playerScoreList[3];
                    break;
            }
            //{
            //    m_diplayScore.text += "Player " + (i + 1) + " score equals " + (int)m_playerScoreList[i] + "\n \n";
            //}//converts to int in order to show only no decimals
        }
        else
        {
            m_timer = 300;
        }
    }
}
