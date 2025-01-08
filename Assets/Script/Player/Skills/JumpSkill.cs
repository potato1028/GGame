using UnityEngine;
using System.Collections;

public class JumpSkill : MonoBehaviour, ISkill {
    public PlayerStateData playerState;

    private Rigidbody2D playerRb;
    private bool jumpBufferActive;

    void Start() {
        playerRb = PlayerControl.playerRb;
    }

    public string GetName() {
        return "Jump";
    }

    public void UseSkill() {
        if (playerState.isGround && !playerState.jumpLock) {
            PerformJump();
        } else if (!playerState.isGround && !jumpBufferActive) {
            jumpBufferActive = true;
            Invoke(nameof(BufferedJump), playerState.jumpBufferTime);
        }
    }

    private void PerformJump() {
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerState.jumpForce);

        StartCoroutine(JumpCoolDown());
    }

    private void BufferedJump() {
        if (playerState.isGround) {
            PerformJump();
        }
        jumpBufferActive = false;
    }

    IEnumerator JumpCoolDown() {
        playerState.jumpLock = true;

        yield return new WaitForSeconds(0.25f);

        playerState.jumpLock = false;
    }

    public void OnUpdate() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UseSkill();
        }
    }
}