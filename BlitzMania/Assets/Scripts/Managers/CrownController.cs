using UnityEngine;
using System.Collections;

public class CrownController : MonoBehaviour
{
    [SerializeField] private bool m_hasCrown = false;
    private bool m_isImmune = false;
    
    GameObject m_centerCrown;

    [SerializeField]
    GameObject m_thisCrown;

    
    IEnumerator CrownDelay()
    {
        yield return new WaitForSeconds(3);
        m_isImmune = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if(m_hasCrown && !m_isImmune)
        {
            other.gameObject.GetComponent<CrownController>().CrownPickUp();
            RemoveCrown();
        }
    }


    void Awake()
    {
        m_centerCrown = GameObject.FindGameObjectWithTag("Crown"); 
    }
	
	// Update is called once per frame
	public void CrownPickUp ()
    {
        m_hasCrown = true;
        m_isImmune = true;
        StartCoroutine(CrownDelay());
        m_thisCrown.SetActive(true);
	}

    public void RemoveCrown()
    {
        m_hasCrown = false;
        m_thisCrown.SetActive(false);

    }
}
