using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        [SerializeField] private int m_playerInput;
        private CarController m_Car; // the car controller we want to use


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!

            if (m_playerInput == 1)
            {
                float h = CrossPlatformInputManager.GetAxis("PlayerOneH");
                float v = CrossPlatformInputManager.GetAxis("PlayerOneV");

                float handbrake = CrossPlatformInputManager.GetAxis("PlayerOneBrake");
                m_Car.Move(h, v, v, handbrake);
            }

            if(m_playerInput == 2)
            {
                float h = CrossPlatformInputManager.GetAxis("PlayerTwoH");
                float v = CrossPlatformInputManager.GetAxis("PlayerTwoV");

                float handbrake = CrossPlatformInputManager.GetAxis("PlayerTwoBrake");
                m_Car.Move(h, v, v, handbrake);
            }

            if(m_playerInput == 3)
            {
                float h = CrossPlatformInputManager.GetAxis("PlayerThreeH");
                float v = CrossPlatformInputManager.GetAxis("PlayerThreeV");

                float handbrake = CrossPlatformInputManager.GetAxis("PlayerThreeBrake");
                m_Car.Move(h, v, v, handbrake);
            }

            if(m_playerInput == 4)
            {
                float h = CrossPlatformInputManager.GetAxis("PlayerFourH");
                float v = CrossPlatformInputManager.GetAxis("PlayerFourV");

                float handbrake = CrossPlatformInputManager.GetAxis("PlayerFourBrake");
                m_Car.Move(h, v, v, handbrake);
            }

        }
    }

