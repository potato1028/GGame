using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerControl))]
public class Player : MonoBehaviour {

    float jumpHeight = 4f;
    float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6f;
    
    //jumpHeight = gravity * timeToJumpApex ^ 2 / 2 == gravity = 2 * jumpHeight / timeToJumpApex ^ 2
    //jumpVelocity = gravity * timeToJumpApex
    
    public float jumpVelocity; 
    public float gravity;
    Vector3 velocity;
    float velocityXSmoothing;

    PlayerControl playerControl;

    void Start() {
        playerControl = GetComponent<PlayerControl>();

        gravity = -(jumpHeight * 2) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update() {
        if(playerControl.collisions.above || playerControl.collisions.below) {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && playerControl.collisions.below) {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (playerControl.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += (gravity) * Time.deltaTime;
        playerControl.Move(velocity * Time.deltaTime);
    }
}