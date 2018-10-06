using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapons : MonoBehaviour {

	public GameObject ammunition;
	public int ammo;
	public bool pickedUp = false;

	Rigidbody2D rb;
	bool thrown = false;
	Vector2 thrownDirection;
	float throwForce = 20f;
	float resistance = 0.1f;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (pickedUp)
			gameObject.SetActive(false);
		else
			gameObject.SetActive(true);
		if (thrown)
			Thrown();
	}

	void Thrown()
	{
		var rotation = (rb.velocity.magnitude/ 10f) * 10000f;
		transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
		rb.velocity = rb.velocity * 0.95f;
	}

	public void SetUpThrow(Vector3 position, Vector2 direction)
	{
		transform.position = position;
		thrownDirection = direction;
		gameObject.SetActive(true);
		thrown = true;
		rb.AddForce(thrownDirection * throwForce, ForceMode2D.Impulse);
	}

	public void Shoot(Vector3 position)
	{
		if (ammo > 0)
		{
			ammo -= 1;
			var bullet = (GameObject)Instantiate(ammunition, position, Quaternion.identity);
			bullet.GetComponent<bullet>().AimAtMouse();
		}
	}
}
