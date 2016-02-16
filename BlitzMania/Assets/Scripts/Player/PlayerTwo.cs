using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CarController))]

public class PlayerTwo : MonoBehaviour
{
    private CarController m_car;
    // Use this for initialization
    
    void Awake()
    {
        m_car = GetComponent<CarController>();
    }

    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float steering = CrossPlatformInputManager.GetAxis("PlayerTwoH");
        float acceleration = CrossPlatformInputManager.GetAxis("PlayerTwoV");
        m_car.Move(steering, acceleration, acceleration, 0.0f);
	}
}
