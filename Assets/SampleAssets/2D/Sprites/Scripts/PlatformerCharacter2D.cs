using UnityEngine;
using System;

namespace UnitySampleAssets._2D
{

    public class PlatformerCharacter2D : MonoBehaviour
    {
        private bool facingRight = true; // For determining which way the player is currently facing.

        [SerializeField] private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
        [SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.	

        [Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;
                                                     // Amount of maxSpeed applied to crouching movement. 1 = 100%

        [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character

        private Transform groundCheck; // A position marking where to check if the player is grounded.
        private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool grounded = false; // Whether or not the player is grounded.
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator anim; // Reference to the player's animator component.
		private float GameOverY = -10;
		public Transform playerGraphics;
		private Vector3 groundPosition;
		public Sprite []spriteArray = new Sprite[2];
		public static int mode = TypeMode.NORMALSPONGE; //this mode is to indicate the character type
		private bool DoubleJump = true;
		private int HealthPoint = 20;
		private bool whoisyourdaddy = false;
		private long startime = 0;
		private long oldTime  = 0;
		private GameObject blood;
		private void Start(){
			GameManager.GameStart += GameStart;
			GameManager.GameOver += GameOver;
			blood = GameObject.Find ("Blood");
			blood.SetActive (false);
		}
		private void GameStart(){
			anim.enabled = true;;
			this.enabled = true;
			this.renderer.enabled = true;
			this.rigidbody2D.isKinematic = false;
			HealthPoint = 100;
			setBlood (HealthPoint);
			whoisyourdaddy = false;
			openBlood ();

		}
		private void GameOver(){
			anim.enabled = false;
			this.enabled = false;
			this.renderer.enabled = false;
			this.rigidbody2D.isKinematic = true;
			rigidbody2D.MovePosition(new Vector2(0f,groundPosition.y+10f));
			disableBlood ();
		}
		private void Awake()
        {
//DontDestry
			DontDestroyOnLoad (this.transform.gameObject);

            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
            anim = GetComponent<Animator>();
			playerGraphics = transform.FindChild ("Graphics");	

			if(playerGraphics == null){
				Debug.LogError ("Let's freak out! There is no 'Graphics' object as a child of the player");
			}
        }

		void OnCollisionEnter2D(Collision2D collisionInfo){
			string collisionObject = collisionInfo.gameObject.ToString ().ToLower(); 
			if (collisionObject.IndexOf("fruit")>=0) { //this is fruit that can change player
				playerGraphics = transform.FindChild ("Graphics");
				SpriteRenderer q =(SpriteRenderer) playerGraphics.GetComponent("SpriteRenderer");
				if(collisionObject.IndexOf("big")>=0){
					q.sprite =spriteArray[TypeMode.MUSCLESPONGE];
					mode = TypeMode.MUSCLESPONGE;
				}else if(collisionObject.IndexOf("small")>=0){
					q.sprite = spriteArray[TypeMode.NORMALSPONGE];
					mode = TypeMode.NORMALSPONGE;
				}else if(collisionObject.IndexOf("gun")>=0){
					q.sprite = spriteArray[TypeMode.GUNSPONGE];
					mode = TypeMode.GUNSPONGE;
				}
				collisionInfo.gameObject.SetActive(false); //make the fruit disable
			}else if(collisionObject.StartsWith("enemy")){
				float collisionUpperBound = collisionInfo.transform.position.x-collisionInfo.transform.localScale.x/2;
				float collisionDownBound  = collisionInfo.transform.position.x+collisionInfo.transform.localScale.x/2;
				float collisionSkyBound   = collisionInfo.transform.position.y-0.61f;
				if(!(this.transform.position.x>collisionUpperBound&&this.transform.position.x<collisionDownBound&&this.transform.position.y>collisionSkyBound)){
					HealthPoint-=TypeMode.OCTOPUS;
					this.rigidbody2D.isKinematic = true;
					this.collider2D.enabled = false;
					if(HealthPoint<=0){
						GameManager.TriggerGameOver();	
					}
					whoisyourdaddy = true; //I can be beaten
					startime  =long.Parse(GetTimeStamp(false));
					oldTime   = startime;
				}
			}
		}
		void blink(){
			playerGraphics = transform.FindChild ("Graphics");
			SpriteRenderer q =(SpriteRenderer) playerGraphics.GetComponent("SpriteRenderer");
			q.enabled = !q.enabled;
		}
        private void FixedUpdate()
        {
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

            anim.SetBool("Ground", grounded);
			if (grounded) {
				groundPosition = transform.localPosition;
				DoubleJump = true;
			} else if (!grounded) {
				this.collider2D.enabled = true;
				this.rigidbody2D.isKinematic = false;
				whoisyourdaddy = false;
				playerGraphics = transform.FindChild ("Graphics");
				SpriteRenderer q =(SpriteRenderer) playerGraphics.GetComponent("SpriteRenderer");
				q.enabled = true;
			}
            // Set the vertical animation
            anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
			float y =  anim.transform.localPosition.y;
        	if (y < GameOverY) {
				GameManager.TriggerGameOver ();
			}
			if (whoisyourdaddy) {
				if(long.Parse(GetTimeStamp(false))-startime>100){
					blink();
					startime = long.Parse(GetTimeStamp(false));
				}
				if(long.Parse(GetTimeStamp(false))-oldTime>=3000){
					whoisyourdaddy = false;
					playerGraphics = transform.FindChild ("Graphics");
					SpriteRenderer q =(SpriteRenderer) playerGraphics.GetComponent("SpriteRenderer");
					q.enabled =true;
					this.collider2D.enabled = true;
					this.rigidbody2D.isKinematic = false;
				}
			}
			setBlood(HealthPoint);
			anim.SetInteger("Mode", mode);
		
		}
        public void Move(float move, bool crouch, bool jump)
        {


            // If crouching, check to see if the character can stand up
            if (!crouch && anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                    crouch = true;
            }

            // Set whether or not the character is crouching in the animator
            anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (grounded || airControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*crouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                rigidbody2D.velocity = new Vector2(move*maxSpeed, rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !facingRight)
                    // ... flip the player.
                    Flip();
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && facingRight)
                    // ... flip the player.
                    Flip();
            }
            // If the player should jump...
            if (grounded && jump && anim.GetBool ("Ground")) {
				// Add a vertical force to the player.
				grounded = false;
				anim.SetBool ("Ground", false);
				rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
			} else if (DoubleJump && jump&&mode==TypeMode.MUSCLESPONGE) {
				DoubleJump = false;	
				rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
			}

        }
        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = playerGraphics.localScale;
            theScale.x *= -1;
            playerGraphics.localScale = theScale;
        }
		public static string GetTimeStamp(bool bflag = true)  
		{  
			TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);  
			string ret = string.Empty;  
			if (bflag)  
				ret = Convert.ToInt64(ts.TotalSeconds).ToString();  
			else  
				ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();  
			return ret;  
		}
		private void setBlood(int b){
			float startPos = 0f-(b)/2;
			if(blood!=null)
				blood.guiTexture.pixelInset = new Rect(startPos,0,b,5f);
		}
		private void openBlood(){
			blood.SetActive (true);
		}
		private void disableBlood(){
			blood.SetActive (false);
		}
    }
}