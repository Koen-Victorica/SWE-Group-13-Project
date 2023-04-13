using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rat : MonoBehaviour
{
	//Note(Francisco): Public values
	public float jump_height = 12.0f;
	public float max_speed = 20.0f;
	public float acceleration_time = 2.0f;
	public float decceleration_time = 2.0f;

	public Canvas ui;

	//Note(Francisco): These fields will be needed for acceleratoin
	private SpriteRenderer sprite_renderer;
	private Vector2 inputs = Vector2.zero;
	private float acceleration = 0f; 
	private float decceleration = 0f; 
	private Rigidbody2D body;

	//variables added by harrison
	private int score = 0;
	private BoxCollider2D bc;
	private bool canMove = true;

	//Note(Francisco): These fields are for ground-checks
	private bool is_grounded = false;
	private float ground_square_height = 0.3f;
	private Vector2 ground_square_check;
	private float ground_interval = 4.0f;
	public LayerMask ground;
	private Transform transform_position_local;
	private Vector2 ground_center;

	//Note(Francisco): Important init. for ground distance and a transform for ground checking
    void Start()
    {
		bc = GetComponent<BoxCollider2D>();

		// TODO(Francisco): Clamp acceleration and decceleration
		acceleration = 1 / ( ( (1/Time.fixedDeltaTime) * acceleration_time) / max_speed);
		decceleration = 1 / (( (1/Time.fixedDeltaTime) * decceleration_time) / max_speed);

		// Note(Francisco): Initialize ground stuff!
		ground_square_check = new Vector2(ground_interval*2, ground_square_height);
		transform_position_local = transform.GetChild(0);
		ground_center = transform_position_local.TransformPoint(transform_position_local.position);

		sprite_renderer = GetComponent<SpriteRenderer>();
		body = GetComponent<Rigidbody2D>();
		body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
		//Note(Harrison) Added bool to if statements to make sure rat is alive
		//Note(Francisco): Physics2D and Physics DO NOT INTERACT WITH EACH OTHER
		ground_center = transform_position_local.TransformPoint(transform_position_local.position);

		// Check if one of the three is hit
		if( Physics2D.OverlapBox(ground_center, ground_square_check, 0, ground))
			{ is_grounded = true; } else { is_grounded = false; }

		inputs = new Vector2(Input.GetAxis("Horizontal"), 0);
		if( (!sprite_renderer.flipX && (inputs.x < 0) ) || (sprite_renderer.flipX && (inputs.x > 0) ) && canMove)
		{ sprite_renderer.flipX = !sprite_renderer.flipX; }

		// Note(Francisco): jump if on gruond and pressed jump
		if(Input.GetButtonDown("Jump") && is_grounded && body.velocity.y == 0 && canMove) {
			body.AddForce(Vector2.up * 9.8f * jump_height, ForceMode2D.Force);
		}

		//Debug.Log(body.velocity.y);
    }

	void FixedUpdate()
	{
		//checks to make sure rat is alive before moving
		if (canMove)
		{
			acceleration = 1 / (((1 / Time.fixedDeltaTime) * acceleration_time) / max_speed);
			decceleration = 1 / (((1 / Time.fixedDeltaTime) * decceleration_time) / max_speed);

			// get the max speed
			float target_speed = inputs.x * max_speed;
			// get the difference in speed at the current step
			float speed_difference = target_speed - body.velocity.x;
			// determine if accelerating or deccelerating
			float acceleration_rate = (Mathf.Abs(target_speed) > 0.01f) ? acceleration : decceleration;

			float movement = speed_difference * acceleration_rate;
			body.AddForce(movement * Vector2.right, ForceMode2D.Force);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Spike")
		{
			canMove = false;
            ui.GetComponent<CanvasController>().lose();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cheese")
        {
            score++;
            ui.GetComponent<CanvasController>().UpdateScore(score);
            Destroy(collision.gameObject);
        }
		if(collision.gameObject.tag == "Flag")
		{
            ui.GetComponent<CanvasController>().win();
        }
    }
}
