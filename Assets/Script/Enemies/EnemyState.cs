using UnityEngine;
using System.Collections;

public class EnemyState : MonoBehaviour {
    [Header("Object")]
    public GameObject player;
    public CapsuleCollider2D playerCap2D;

    [Header("Parameter")]
    public Vector2 direction;
    public float distance;

    [Header("State")]
    public float wanderSpeed;

    public float detectionRange;
    public float detectionAngle;

    public float chaseRange;
    public float chaseSpeed;

    public float attackRange;
    public float attackReadyTime;
    public float attackCoolTime;

    public bool isPlayerDetected;

    public RaycastHit2D obstacleHit;

    public enum State {
        Wandering,
        Chasing,
        Attacking
    }

    public State currentState;

    void Awake() {
        currentState = State.Wandering;

        wanderSpeed = 1f;
        detectionRange = 3f;
        detectionAngle = 45f;

        chaseRange = 6f;
        chaseSpeed = 2f;

        attackRange = 2f;
        attackReadyTime = 0.5f;
        attackCoolTime = 1.0f;

        isPlayerDetected = false;
    }

    public void PlayerDistance() {
        Collider2D detectedObject = Physics2D.OverlapCircle(this.transform.position, chaseRange, Layer.playerLayer);

        if(detectedObject == null) {
            nonePlayer();
            return;
        }

        playerCap2D = detectedObject.GetComponent<CapsuleCollider2D>();
        player = playerCap2D.gameObject;

        direction = (Vector2)player.transform.position - (Vector2)this.transform.position;
        this.distance = direction.magnitude;

        #if UNITY_EDITOR    
        Debug.DrawRay(this.transform.position, direction * distance, Color.red);
        #endif

        obstacleHit = Physics2D.Raycast(this.transform.position, direction, distance, Layer.wallLayer);
        if (obstacleHit.collider != null) {
            nonePlayer();
        }

        if(currentState == State.Wandering) {
            if(isPlayerDetected) {
                if(distance <= attackRange) {
                    currentState = State.Attacking;
                }
                else if(distance <= chaseRange) {
                    currentState = State.Chasing;
                }
            }
        }

        if(currentState == State.Chasing) {
            if(distance <= attackRange) {
                currentState = State.Attacking;
            }
            else if(distance > chaseRange) {
                nonePlayer();
            }
        }

        if(currentState == State.Attacking && distance > attackRange) {
            if(distance > chaseRange) {
                nonePlayer();
            }
            else if(distance <= chaseRange) {
                currentState = State.Chasing;
            }
        }
    }

    private void nonePlayer() {
        playerCap2D = null;
        player = null;
        currentState = State.Wandering;
        isPlayerDetected = false;
    }
}