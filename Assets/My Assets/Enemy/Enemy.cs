using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float patrolSpeed = 2f;                          // The nav mesh agent's speed when patrolling.
	public int life = 3;
	//private EnemySight enemySight;                          // Reference to the EnemySight script.
	//private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	private bool isStomp;
	private bool fall;
	private float headSize;
	//private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
	private float patrolTimer;                              // A timer for the patrolWaitTime.
	private float startPoint;
	private float patrolDistance;
	public float playerpos;


	void Start() 
	{
		isStomp = false;
		fall = true;
		headSize = GetComponent<BoxCollider2D>().size.x;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		// if collision on top -> dead
		// if collision on left or right -> cause player dead
		// if collision on bottom -> update standing platform
		if(gameObject!=null&&other!=null){
			if (this.transform.position.y > other.transform.position.y) {
						startPoint = other.transform.position.x;
						patrolDistance = 0.5f * other.gameObject.GetComponent<BoxCollider2D> ().size.x;
			} else if (other.gameObject.tag == "Player" &&
						this.transform.position.x + headSize > other.transform.position.x &&
						this.transform.position.x - headSize < other.transform.position.x &&
						this.transform.position.y < (other.transform.position.y - 0.61)) {
						gameObject.SetActive (false);
						//isStomp = true;
			} else  if(other.gameObject.name.IndexOf("BulletTrail")>=0){
				this.life-=1;
				if(this.life==0)
					gameObject.SetActive(false);
			}
		}
	}

	void FixedUpdate ()
	{
		if (isStomp) {
			isStomp = false;
			fall = true;
			Vector3 scale = this.transform.localScale;
			scale.y /= 2;
			this.transform.localScale = scale;
			GetComponent<BoxCollider2D>().enabled = false;
		}
		else
			Patrolling();

		if (fall) {
			this.transform.Translate(0f, -0.5f * Time.deltaTime, 0f);
		}

		if (this.transform.position.y < -30)
			gameObject.SetActive (false);
	}
	
	void Patrolling ()
	{
		Transform boo = GetComponent<Transform>();
		
		if (boo.localPosition.x > startPoint - patrolDistance &&
		    boo.localPosition.x < startPoint + patrolDistance)
		{
			// move toward patrol position
			boo.Translate(patrolSpeed * Time.deltaTime, 0f, 0f);
		}
		else {
			patrolSpeed = -patrolSpeed;
			// NEED TO UPDATE MIRROR SPRITE
			boo.Translate(patrolSpeed * Time.deltaTime, 0f, 0f);
		}
	}
}
