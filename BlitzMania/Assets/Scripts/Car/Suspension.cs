using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    // this script is specific to the car supplied in the the assets
    // it controls the suspension hub to make it move with the wheel are it goes over bumps
    public class Suspension : MonoBehaviour
    {
        public GameObject wheel; // The wheel that the script needs to referencing to get the postion for the suspension


        private Vector3 m_targetOriginalPosition;
        private Vector3 m_origin;


        private void Start()
        {
            m_targetOriginalPosition = wheel.transform.localPosition;
            m_origin = transform.localPosition;
        }


        private void Update()
        {
            transform.localPosition = m_origin + (wheel.transform.localPosition - m_targetOriginalPosition);
        }
    }
}
