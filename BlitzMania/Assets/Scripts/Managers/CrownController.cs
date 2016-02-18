using UnityEngine;
using System.Collections;


public class CrownController : MonoBehaviour
{
    [SerializeField] public bool m_hasCrown = false;
    [Range(1, 4)]
    [SerializeField] private int m_playerNr;
    private bool m_isImmune = false;
    
    
    //GameObject m_centerCrown;

    [SerializeField]
    GameObject m_thisCrown;
    [SerializeField]
    Score score;

    
    IEnumerator CrownDelay()
    {
        yield return new WaitForSeconds(3);
        m_isImmune = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if(m_hasCrown && !m_isImmune)
        {
            CrownController cController = other.gameObject.GetComponent<CrownController>();
            if (cController != null)
            {
                cController.CrownPickUp();
                RemoveCrown();
            }
        }
    }


    void Awake()
    {
        //m_centerCrown = GameObject.FindGameObjectWithTag("Crown"); 
    }
	
	// Update is called once per frame
	public void CrownPickUp ()
    {
        m_hasCrown = true;
        m_isImmune = true;
        StartCoroutine(CrownDelay());
        m_thisCrown.SetActive(true);
        score.NewPlayerWithCrown(m_playerNr); 
	}

    public void RemoveCrown()
    {
        m_hasCrown = false;
        m_thisCrown.SetActive(false);
        score.NewPlayerWithCrown(0);
    }
}
