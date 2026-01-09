using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class PlayerControl : MonoBehaviour {
    public PlayerStateData playerState;

    [Header("Player Component")]
    public static Rigidbody2D playerRb;

    [Header("Player States")]
    public float moveSpeed;
    public bool isAttachedLeftWall;
    public bool isAttachedRightWall;

    void Awake() {
        if(playerState == null) playerState = Resources.Load<PlayerStateData>("State");

        playerRb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Start() {
        moveSpeed = playerState.moveSpeed;

        //////////////
        /////////////
        boxCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    void FixedUpdate() {
        PlayerDetectWall();
        PlayerDetectGround();
        PlayerMove();
    }

    void PlayerMove() {
        if (!playerState.moveLock) {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (!isAttachedLeftWall && horizontalInput < 0) {
                playerRb.linearVelocity = new Vector2(horizontalInput * moveSpeed, playerRb.linearVelocity.y);
            }

            if (!isAttachedRightWall && horizontalInput > 0) {
                playerRb.linearVelocity = new Vector2(horizontalInput * moveSpeed, playerRb.linearVelocity.y);
            }
        }
    }

    void PlayerDetectWall() {
        Vector2 wallRayVec = new Vector2(transform.position.x, transform.position.y + 1.0f);


        for (int i = 0; i < 9; i++) {
            if (Physics2D.Raycast(wallRayVec, Vector2.left, 0.51f, Layer.terrainLayer)) {
                isAttachedLeftWall = true;
                break;
            }

            if (Physics2D.Raycast(wallRayVec, Vector2.right, 0.51f, Layer.terrainLayer)) {
                isAttachedRightWall = true;
                break;
            }

            #if UNITY_EDITOR
            Debug.DrawRay(wallRayVec, Vector2.left * 0.51f, Color.green);
            Debug.DrawRay(wallRayVec, Vector2.right * 0.51f, Color.green);
            #endif

            isAttachedLeftWall = false;
            isAttachedRightWall = false;
            wallRayVec.y -= 0.25f;
        }
    }

    void PlayerDetectGround() {
        bool[] isDownGround = new bool[9];

        Vector2 groundRayVec = new Vector2(transform.position.x - 0.4f, transform.position.y);

        for (int i = 0; i < 9; i++) {
            isDownGround[i] = Physics2D.Raycast(groundRayVec, Vector2.down, 1.1f, Layer.terrainLayer);

            #if UNITY_EDITOR
            Debug.DrawRay(groundRayVec, Vector2.down * 1.1f, Color.green);
            #endif

            if (isDownGround[i]) {
                playerState.isGround = true;
                playerState.lastOnGroundTime = playerState.coyoteTime;
                break;
            }

            groundRayVec.x += 0.1f;
            playerState.isGround = false;
            playerState.lastOnGroundTime -= Time.deltaTime;
        }

        moveSpeed = playerState.isGround ? playerState.moveSpeed : (playerState.moveSpeed * 0.8f);
    }



    #region 강의 부분

    [Header("RayCast ReNew")]
    const float skinWidth = .015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float maxClimbAngle = 80f;

    float horizontalRaySpacing;
    float verticalRaySpacing;
    
    BoxCollider2D boxCollider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;


    public void Move(Vector3 velocity) {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0) {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0) {
            VerticalCollistions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for(int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight; //움직이는 방향에 따라 Ray를 바꿈 
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, Layer.terrainLayer);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if(hit) {

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if(i == 0 && slopeAngle <= maxClimbAngle) {
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisions.slopeAngleOld) {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if(!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;
                    
                    if(collisions.climbingSlope) velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollistions(ref Vector3 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for(int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft; //움직이는 방향에 따라 Ray를 바꿈 
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, Layer.terrainLayer);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if(hit) {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                
                if(collisions.climbingSlope) velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(velocity.y <= climbVelocityY) {
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = boxCollider.bounds; 
        bounds.Expand(skinWidth * -2); //여백만큼 BoxCollider2D의 범위를 줄이는 행위

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing() {
        Bounds bounds = boxCollider.bounds; 
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
     
    struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public float slopeAngle, slopeAngleOld;

        public void Reset() {
            above = below = false;
            left = right = false;
            climbingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    #endregion
}