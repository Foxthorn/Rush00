using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {

	public float speed;
	
	Rigidbody2D rb2D;
	float horizontal;
	float vertical;
	float moveLimiter = 0.7f;
	bool holdingWeapon = false;
	GameObject weapon;
	weapons script;
	AudioSource sound;

	void Start () {
		rb2D = gameObject.GetComponent<Rigidbody2D>();
		sound = GetComponent<AudioSource>();
	}
	
	void Update () {
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");
		FaceMouse();
		if (Input.GetKeyDown("e"))
		{
			PickUpWeapon();
		}
		if (Input.GetKeyDown(KeyCode.Mouse0) && holdingWeapon)
		{
			script.Shoot(transform.position);
			if (script.ammo >= 0)
				sound.PlayOneShot(script.clips[0], 0.7f);
			else
				sound.PlayOneShot(script.clips[1], 0.7f);
		}
		if (Input.GetKeyDown(KeyCode.Mouse1) && holdingWeapon)
			ThrowWeapon();
	}

	void ThrowWeapon()
	{
		sound.PlayOneShot(script.clips[2], 0.7f);
		script.pickedUp = false;
		script.SetUpThrow(transform.position, -transform.up);
	}

	void FaceMouse()
	{
		Vector3 mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

		Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
		transform.up = -direction;
	}

	void FixedUpdate()
	{
		if (horizontal != 0 && vertical != 0)
			rb2D.velocity = new Vector2((horizontal * speed) * moveLimiter, (vertical * speed) * moveLimiter);
		else
			rb2D.velocity = new Vector2(horizontal * speed, vertical * speed);
	}

	void PickUpWeapon()
	{
		var layerMask = ~(1 << 9);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward, Mathf.Infinity, layerMask);
		if (hit.collider != null)
		{
			Debug.Log(hit.collider.transform.tag);
			if (hit.collider.transform.tag == "Weapon")
			{
				weapon = hit.collider.gameObject;
				holdingWeapon = true;
				script = weapon.GetComponent<weapons>();
				sound.Play(0);
				script.pickedUp = true;
			}
		}
		else
			holdingWeapon = false;
	}
}
