using UnityEngine;
using System.Collections;

public class JumpSkill : MonoBehaviour, PlayerSkill_Interface{
    public PlayerStateData playerState;

    private Rigidbody2D playerRb;
    

    void Start() {
        if(playerState == null) playerState = Resources.Load<PlayerStateData>("State");
        
        playerRb = PlayerControl.playerRb;
    }

    public string GetName() {
        return "Jump";
    }

    public void UseSkill() {
        if ((playerState.isGround || playerState.lastOnGroundTime > 0f) && !playerState.jumpLock) PerformJump();
        else if(!playerState.isGround && !playerState.jumpLock) StartCoroutine(JumpBufferTime());
    }

    private void PerformJump() {
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerState.jumpForce);

        StartCoroutine(PlayerGravity());
        StartCoroutine(JumpCoolDown());
    }

    IEnumerator JumpCoolDown() {
        playerState.jumpLock = true;

        yield return new WaitForSeconds(0.25f);

        playerState.jumpLock = false;
    }

    IEnumerator JumpBufferTime() {
        yield return new WaitForSeconds(playerState.jumpBufferTime);
        
        if ((playerState.isGround || playerState.lastOnGroundTime > 0f) && !playerState.jumpLock) PerformJump();
    }

    IEnumerator PlayerGravity() {
        if(playerRb.linearVelocity.y > -0.1f && playerRb.linearVelocity.y < 0.1f) {
            playerRb.gravityScale = playerState.apexGravity;

            yield return new WaitForSeconds(playerState.apexTime);
        }

        if(playerRb.linearVelocity.y < 0 && !playerState.isGround) playerRb.gravityScale = playerState.fallingGravity;

        if(playerState.isGround) playerRb.gravityScale = playerState.defaultGravity;
    }

    public void OnUpdate() {
        if (Input.GetKeyDown(KeyCode.Space)) UseSkill();
    }
}