using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public float patrolSpeed = 2f;                          // The nav mesh agent's speed when patrolling.
	public float chaseSpeed = 5f;                           // The nav mesh agent's speed when chasing.
	public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
	public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
	//public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.
	public float[] patrolWayPointsx;
	
	
	//private EnemySight enemySight;                          // Reference to the EnemySight script.
	//private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	private Transform player;                               // Reference to the player's transform.
	//private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
	//private LastPlayerSighting lastPlayerSighting;          // Reference to the last global sighting of the player.
	private float chaseTimer;                               // A timer for the chaseWaitTime.
	private float patrolTimer;                              // A timer for the patrolWaitTime.
	private int wayPointIndex;                              // A counter for the way point array.
	private float startPoint;
	private float patrolDistance;
	
	
	void Awake ()
	{
		// Setting up the references.
		//enemySight = GetComponent<EnemySight>();
		//nav = GetComponent<NavMeshAgent>();
		//player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		//playerHealth = player.GetComponent<PlayerHealth>();
		//lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
	}
	
	void Start() {
		startPoint = this.transform.position.x;
		BoxCollider2D enemyPlatCollider = GameObject.Find ("Plat_Enemy").GetComponent<BoxCollider2D>();
		patrolDistance = 0.5f * (float)enemyPlatCollider.size.x;
		// set two patrl points

	}
	
	void Update ()
	{/*
		// If the player is in sight and is alive...
		if(enemySight.playerInSight && playerHealth.health > 0f)
			// ... shoot.
			Shooting();
		
		// If the player has been sighted and isn't dead...
		else if(enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
			// ... chase.
			Chasing();
		
		// Otherwise...
		else
	*/		// ... patrol.
		Patrolling();
	}
	
	
	void Shooting ()
	{
		// Stop the enemy where it is.
		//nav.Stop();
	}
	
	
	void Chasing ()
	{/*
		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
		
		// If the the last personal sighting of the player is not close...
		if(sightingDeltaPos.sqrMagnitude > 4f)
			// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
			nav.destination = enemySight.personalLastSighting;
		
		// Set the appropriate speed for the NavMeshAgent.
		nav.speed = chaseSpeed;
		
		// If near the last personal sighting...
		if(nav.remainingDistance < nav.stoppingDistance)
		{
			// ... increment the timer.
			chaseTimer += Time.deltaTime;
			
			// If the timer exceeds the wait time...
			if(chaseTimer >= chaseWaitTime)
			{
				// ... reset last global sighting, the last personal sighting and the timer.
				lastPlayerSighting.position = lastPlayerSighting.resetPosition;
				enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
				chaseTimer = 0f;
			}
		}
		else
			// If not near the last sighting personal sighting of the player, reset the timer.
			chaseTimer = 0f;*/
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
