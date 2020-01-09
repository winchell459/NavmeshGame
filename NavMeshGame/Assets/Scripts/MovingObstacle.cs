using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    bool movingForward = true; //-x
    public float Speed = 2;
    public float xMin = -2;
    public float xMax = 2;
    private float xStart;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        xStart = transform.position.x;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(movingForward){
            rb.velocity = new Vector3(-Speed, 0, 0);
            if (transform.position.x < xMin + xStart) movingForward = false;
        }else{
            rb.velocity = new Vector3(Speed, 0, 0);
            if (transform.position.x > xMax + xStart) movingForward = true;
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
        if(collision.gameObject.CompareTag("Player")){
            FindObjectOfType<PlayerController>().KillPlayer();
        }
	}
}
