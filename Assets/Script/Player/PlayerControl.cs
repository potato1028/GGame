using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public PlayerStateData playerState;

    [Header("Player Component")]
    public static Rigidbody2D playerRb;

    [Header("Player States")]
    public bool isAttachedLeftWall;
    public bool isAttachedRightWall;

    void Awake() {
        playerRb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Start() {
        playerState.moveSpeed = 5f;
    }

    void Update() {
        playerMove();
    }

    void FixedUpdate() {
        playerDetectWall();
        playerDetectGround();
    }

    void playerMove() {
        if (!playerState.moveLock) {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (!isAttachedLeftWall && horizontalInput < 0) {
                playerRb.linearVelocity = new Vector2(horizontalInput * playerState.moveSpeed, playerRb.linearVelocity.y);
            }

            if (!isAttachedRightWall && horizontalInput > 0) {
                playerRb.linearVelocity = new Vector2(horizontalInput * playerState.moveSpeed, playerRb.linearVelocity.y);
            }
        }
    }

    void playerDetectWall() {
        bool[] isLeftWalls = new bool[5];
        bool[] isRightWalls = new bool[5];

        Vector2 wallRayVec = new Vector2(transform.position.x, transform.position.y + 0.9f);

        for (int i = 0; i < 5; i++) {
            isLeftWalls[i] = Physics2D.Raycast(wallRayVec, Vector2.left, 0.51f, Layer.groundLayer);
            isRightWalls[i] = Physics2D.Raycast(wallRayVec, Vector2.right, 0.51f, Layer.groundLayer);

            #if UNITY_EDITOR
            Debug.DrawRay(wallRayVec, Vector2.left * 0.51f, Color.green);
            Debug.DrawRay(wallRayVec, Vector2.right * 0.51f, Color.green);
            #endif

            isAttachedLeftWall = isLeftWalls[i];
            isAttachedRightWall = isRightWalls[i];

            if (isAttachedLeftWall || isAttachedRightWall) {
                break;
            }

            wallRayVec.y -= 0.45f;
        }
    }

    void playerDetectGround() {
        bool[] isDownGround = new bool[9];

        Vector2 groundRayVec = new Vector2(transform.position.x - 0.4f, transform.position.y);

        for (int i = 0; i < 9; i++) {
            isDownGround[i] = Physics2D.Raycast(groundRayVec, Vector2.down, 1.1f, Layer.groundLayer);

            #if UNITY_EDITOR
            Debug.DrawRay(groundRayVec, Vector2.down * 1.1f, Color.green);
            #endif

            if (isDownGround[i]) {
                CancelInvoke("endCoyote");
                playerState.isGround = true;
                break;
            }

            groundRayVec.x += 0.1f;
        }

        Invoke("endCoyote", playerState.coyoteTime);
    }

    void endCoyote() {
        playerState.isGround = false;
    }
}