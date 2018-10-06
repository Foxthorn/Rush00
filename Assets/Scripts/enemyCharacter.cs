using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCharacter : MonoBehaviour {

	public GameObject bullet;
	
	public float viewRadius;
	public float viewMinRadius;
	[Range(0,360)]
	public float viewAngle;
	public LayerMask targetMask;
	public LayerMask obstacleMask;
	public Transform player;
	public float speed;
	public List<GameObject> patrolPoints = new List<GameObject>();

	bool patrol;
	bool playerVisible;
	Vector3 playersLastPosition;
	float timeDelay;
	bool CR_Running = false;
	int currentPatrolPoint = 0;
	void Start () {
		if (patrolPoints.Count > 0)
			patrol = true;
	}

	void Update () {
		playerVisible = isPlayerVisible();
		if (playerVisible)
		{
			playersLastPosition = player.position;
			FacePlayer();
			Move();
			if (!CR_Running)
				StartCoroutine(ShootAtPlayer(2f));
		}
		else if (patrol)
		{
			Patrol();
			StopAllCoroutines();
		}
		timeDelay += Time.deltaTime;
	}

	void Patrol()
	{
		if (currentPatrolPoint > patrolPoints.Count - 1)
			currentPatrolPoint = 0;
		FacePatrolPoint(currentPatrolPoint);
		transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPatrolPoint].transform.position, speed * Time.deltaTime);
		if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].transform.position) < 0.1)
			currentPatrolPoint += 1;
	}

	void FacePatrolPoint(int i)
	{
		Vector2 direction = new Vector2(patrolPoints[i].transform.position.x - transform.position.x, patrolPoints[i].transform.position.y - transform.position.y);
		transform.up = -direction;
	}

	IEnumerator ShootAtPlayer(float delay)
	{
		for(;;)
		{
			CR_Running = true;
			var obj = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
			obj.GetComponent<bullet>().setTarget(player.position);
			yield return new WaitForSeconds(delay);
		}
		CR_Running = false;
	}

	void FacePlayer()
	{
		Vector2 direction = new Vector2(playersLastPosition.x - transform.position.x, playersLastPosition.y - transform.position.y);
		transform.up = -direction;
	}

	void Move()
	{
		transform.position = Vector3.MoveTowards(transform.position, playersLastPosition, speed * Time.deltaTime);		
	}

	bool isPlayerVisible()
	{
		if (player == null)
			return false;
		if (Vector3.Angle(-transform.up, player.position - transform.position) < viewAngle / 2)
		{
			var layerMask = ~(1 << 8);
			RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, viewRadius, layerMask);
			if (hit.collider != null)
			{
				if (hit.collider.transform.tag == "Player")
				{
					return true;
				}
			}
		}
		else
		{
			var layerMask = ~(1 << 8);
			RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, viewMinRadius, layerMask);
			if (hit.collider != null)
			{
				if (hit.collider.transform.tag == "Player")
				{
					return true;
				}
			}
		}
		return false;
	}

	public Vector3 DirectionFromAngle(float angle, bool global)
	{
		if (!global)
			angle += transform.eulerAngles.y;
		return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad), 0);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.transform.tag == "Player_Bullet")
		{
			var script = col.gameObject.GetComponent<bullet>();
			script.Remove();
			Destroy(gameObject);
		}
	}
}
