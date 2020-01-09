using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickUp : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
        if(other.CompareTag("Player")){
            FindObjectOfType<PlayerController>().AddDiamond();
            Destroy(gameObject);
        }
	}
}
