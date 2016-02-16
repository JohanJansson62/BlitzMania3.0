using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CarController))]

public class PlayerThree : MonoBehaviour
{
    private CarController m_car;

    void Awake()
    {
        m_car = GetComponent<CarController>();
    }    
	
	
	void FixedUpdate ()
    {
        float steering = CrossPlatformInputManager.GetAxis("PlayerThreeH");
        float acceleration = CrossPlatformInputManager.GetAxis("PlayerThreeV");
        m_car.Move(steering, acceleration, acceleration, 0.0f);
	}
}
