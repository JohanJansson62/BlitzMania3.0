using UnityEngine;
using System.Collections;

public class CarStartPos : MonoBehaviour {
    public Vector3 m_startPos;
    public Quaternion m_startRotation;

	// Use this for initialization
	void Start ()
    {
        m_startPos = transform.position;
        m_startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
