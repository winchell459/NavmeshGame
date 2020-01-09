using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : MonoBehaviour
{
	private void OnTriggerExit(Collider other)
	{
        if(other.CompareTag("Player")){
            GetComponent<Animator>().SetBool("Following", false);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Animator>().SetBool("Following", true);
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
        if(collision.gameObject.CompareTag("Player")){
            FindObjectOfType<PlayerController>().KillPlayer();
            GetComponent<Animator>().SetBool("Following", false);
        }
	}
}
