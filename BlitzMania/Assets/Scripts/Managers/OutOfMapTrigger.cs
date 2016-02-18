using UnityEngine;
using System.Collections;

public class OutOfMapTrigger : MonoBehaviour {
    
	void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<CarReset>().CarReseter();
    }
}
