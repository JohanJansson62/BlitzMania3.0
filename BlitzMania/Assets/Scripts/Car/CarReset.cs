using System;
using UnityEngine;


public class CarReset : MonoBehaviour
{
    // Automatically put the car the right way up, if it has come to rest upside-down.
    [SerializeField]
    private Score[] m_score = new Score[4];
    [SerializeField]
    GameObject m_crown_PickUp;
    [SerializeField]
    private float m_WaitTime = 3f;           // time to wait before self righting
    [SerializeField]
    private float m_VelocityThreshold = 1f;  // the velocity below which the car is considered stationary for self-righting
    CrownController m_crownController;
  
    private CarStartPos m_startPos;


    private float m_LastOkTime; // the last time that the car was in an OK state
    private Rigidbody m_Rigidbody;


    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_startPos = GetComponent<CarStartPos>();
        m_crownController = GetComponent<CrownController>();
    }


    private void Update()
    {
        // is the car is the right way up
        if (transform.up.y > 0f || m_Rigidbody.velocity.magnitude > m_VelocityThreshold)
        {
            m_LastOkTime = Time.time;
        }

        if (Time.time > m_LastOkTime + m_WaitTime)
        {
            CarReseter();
        }
    }


    // put the car back the right way up:
    public void CarReseter()
    {
        // set the correct orientation for the car, and lift it off the ground a little
        //transform.position += Vector3.up;
        //transform.rotation = Quaternion.LookRotation(transform.forward);
        transform.position = m_startPos.m_startPos;
        transform.rotation = m_startPos.m_startRotation;
        if (m_crownController.m_hasCrown)
        {
            m_crownController.RemoveCrown();
            m_crown_PickUp.SetActive(true);
            for (int i = 0; i<4; i++)
            {
                m_score[i].NewPlayerWithCrown(0);
            }

        }
    }
}
