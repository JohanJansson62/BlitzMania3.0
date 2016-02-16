using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CarController))]
public class PlayerOne : MonoBehaviour
{
    private CarController m_car;

	// Use this for initialization
	void Awake ()
    {
        m_car = GetComponent<CarController>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float steering = CrossPlatformInputManager.GetAxis("PlayerOneH");
        float acceleration = CrossPlatformInputManager.GetAxis("PlayerOneV");

        m_car.Move(steering, acceleration, acceleration, 0.0f);
        
	}
}
