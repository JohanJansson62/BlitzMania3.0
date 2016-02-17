using UnityEngine;
using System.Collections;

public class CrownController : MonoBehaviour
{
    [SerializeField] private bool m_hasCrown = false;
    private bool m_isImmune = false;
    GameObject m_crown;

    
    IEnumerator CrownDelay()
    {
        yield return new WaitForSeconds(3);
        m_isImmune = false;
    }

    void Awake()
    {
        m_crown = GameObject.FindGameObjectWithTag("Crown"); 
    }

    	
	// Update is called once per frame
	public void CrownPickUp ()
    {
        m_hasCrown = true;
        m_isImmune = true;  
	}
}
