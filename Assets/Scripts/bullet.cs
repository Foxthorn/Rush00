using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	public Vector3 target;

	float speed = 10;

	void FireBullet()
	{
		var rb = GetComponent<Rigidbody2D>();
		rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
	}

	public void setTarget(Vector3 position)
	{
		Vector2 direction = new Vector2(position.x - transform.position.x, position.y - transform.position.y);
		transform.right = direction;
		FireBullet();
	}

	public void AimAtMouse()
	{
		Vector3 mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

		Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
		transform.right = direction;
		FireBullet();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.transform.tag == "Wall")
			Remove();
		if (other.gameObject.transform.tag == "Door")
			Remove();
	}

	public void Remove()
	{
		Destroy(gameObject);
	}
}
