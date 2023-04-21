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

	//public Canvas ui; NOTE(Charles): do not need

	//Note(Francisco): These fields will be needed for acceleratoin
	private SpriteRenderer sprite_renderer;
	private Vector2 inputs = Vector2.zero;
	private float acceleration = 0f; 
	private float decceleration = 0f; 
	private Rigidbody2D body;

	//variables added by harrison
	//private int score = 0; NOTE(Charles) do not need
	private BoxCollider2D bc;

	//Note(Francisco): These fields are for ground-checks
	private bool is_grounded = false;
	private float ground_square_height = 0.08f;
	private Vector2 ground_square_check;
	private float ground_interval = 0.55f;
	public LayerMask ground;
	private Transform transform_position_local;
	private Vector2 ground_center;

	//Note(Charles): variables for UI to work
	public GameObject deathScreen;
	public GameObject WinScreen;
	public GameObject HighScoreInput;

    //Note(Charles): added awake method to ensure death/win screens are set inactive
    private void Awake()
    {
		deathScreen.SetActive(false);
		WinScreen.SetActive(false);
		HighScoreInput.SetActive(false);
    }

    //Note(Francisco): Important init. for ground distance and a transform for ground checking
    void Start()
    {
		bc = GetComponent<BoxCollider2D>();

		// TODO(Francisco): Clamp acceleration and decceleration
		acceleration = 1 / ( ( (1/Time.fixedDeltaTime) * acceleration_time) / max_speed);
		decceleration = 1 / (( (1/Time.fixedDeltaTime) * decceleration_time) / max_speed);

		// Note(Francisco): Initialize ground stuff!
		ground_square_check = new Vector2(ground_interval, ground_square_height);
		transform_position_local = transform.GetChild(0);
		ground_center = transform_position_local.position;

		sprite_renderer = GetComponent<SpriteRenderer>();
		body = GetComponent<Rigidbody2D>();
		body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
		//Note(Harrison) Added bool to if statements to make sure rat is alive
		//Note(Francisco): Physics2D and Physics DO NOT INTERACT WITH EACH OTHER
		ground_center = transform_position_local.position;

		// Check if one of the three is hit
		if( Physics2D.OverlapBox(ground_center, ground_square_check, 0, ground))
			{ is_grounded = true; } else { is_grounded = false; }

		inputs = new Vector2(Input.GetAxis("Horizontal"), 0);
		if( (!sprite_renderer.flipX && (inputs.x < 0) ) || (sprite_renderer.flipX && (inputs.x > 0) ) )
		{ sprite_renderer.flipX = !sprite_renderer.flipX; }

		// Note(Francisco): jump if on gruond and pressed jump
		if(Input.GetButtonDown("Jump") && is_grounded && (body.velocity.y >= -0.01f && body.velocity.y <= 0.01f) ) {
			body.AddForce(Vector2.up * 9.8f * jump_height, ForceMode2D.Force);
		}

		//check if rat has fallen off the stage
		if(transform.position.y < -5)
		{
			body.gameObject.SetActive(false);

			
			deathScreen.SetActive(true);
			HighScoreInput.SetActive(true);
		}
		//Debug.Log(body.velocity.y);
    }

	void FixedUpdate()
	{
        //checks to make sure rat is alive before moving
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

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Spike")
		{
			body.gameObject.SetActive(false); //NOTE(Charles) not necessary, can use "body.gameObject.SetActive(False);", this is up to ya'll.
			//ui.GetComponent<CanvasController>().lose();  NOTE(Charles) do not need anymore

			//NOTE(charles): the death screen and score input will now display for the user input their score
			deathScreen.SetActive(true);
			HighScoreInput.SetActive(true);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cheese")
        {
			//score++; NOTE(Charles) do not need
			//ui.GetComponent<CanvasController>().UpdateScore(score); NOTE(Charles) do not need
			Destroy(collision.gameObject);

			//NOTE(Charles) scoring system
			ScoreBoss.instance.AddPoint();

		}
		if (collision.gameObject.tag == "Flag")
		{
            body.gameObject.SetActive(false);
			//ui.GetComponent<CanvasController>().win(); NOTe(Charles) do not need anymore

			//NOTE(charles): the win screen and score input will now display for the user input their score
			WinScreen.SetActive(true);
			HighScoreInput.SetActive(true);
		}
    }
}
