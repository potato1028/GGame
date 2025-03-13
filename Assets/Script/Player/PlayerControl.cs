using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public PlayerStateData playerState;

    [Header("Player Component")]
    public static Rigidbody2D playerRb;

    [Header("Player States")]
    public bool isAttachedLeftWall;
    public bool isAttachedRightWall;

    void Awake() {
        if(playerState == null) playerState = Resources.Load<PlayerStateData>("State");

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

    void playerDetectGround() {
        bool[] isDownGround = new bool[9];

        Vector2 groundRayVec = new Vector2(transform.position.x - 0.4f, transform.position.y);

        for (int i = 0; i < 9; i++) {
            isDownGround[i] = Physics2D.Raycast(groundRayVec, Vector2.down, 1.1f, Layer.terrainLayer);

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