using UnityEngine;
using System.Collections;

public class CrownScript : MonoBehaviour
{
	void OnTriggerEnter(Collider Other)
    {
        Debug.Log("Collide");
        Other.GetComponent<CrownController>().CrownPickUp();

        gameObject.SetActive(false);
    }
}
