using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(CarController))]

public class PlayerFour : MonoBehaviour
{

    private CarController m_car;


    void Awake()
    {
        m_car = GetComponent<CarController>();
    }

	void Start ()
    {
	
	}
	
	
	void FixedUpdate ()
    {
        float steering = CrossPlatformInputManager.GetAxis("PlayerFourH");
        float acceleration = CrossPlatformInputManager.GetAxis("PlayerFourV");
        m_car.Move(steering, acceleration, acceleration, 0.0f);
	}
}
