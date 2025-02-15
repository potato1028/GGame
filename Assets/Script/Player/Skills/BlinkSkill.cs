using UnityEngine;
using System.Collections;

public class BlinkSkill : MonoBehaviour, PlayerSkill_Interface {
    public PlayerStateData playerState;
    public GameObject skillSpaceCircle;

    private Rigidbody2D playerRb;
    private bool isRightMousePressed;

    void Start() {
        if(playerState == null) playerState = Resources.Load<PlayerStateData>("State");
        
        playerRb = PlayerControl.playerRb;
    }

    public string GetName() {
        return "Blink";
    }

    public void UseSkill() {
        if(Input.GetMouseButton(1)) {
            if(!isRightMousePressed) {
                isRightMousePressed = true;
                SkillSpace.skillSpaceOnOff("On");
            }
        }

        if(Input.GetMouseButtonUp(1) && isRightMousePressed) {
            StartCoroutine(BlinkToPosition(skillSpaceCircle.transform.position));
            isRightMousePressed = false;
        }
    }

    IEnumerator BlinkToPosition(Vector2 targetPosition) {
        SkillSpace.skillSpaceOnOff("Off");

        if(!playerState.blinkLock) {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.gravityScale = 0f;

            playerState.moveLock = true;

            yield return new WaitForSeconds(0.2f);

            transform.parent.position = targetPosition;

            yield return new WaitForSeconds(0.2f);

            PlayerControl.playerRb.gravityScale = 2f;
            playerState.moveLock = false;

            StartCoroutine(BlinkCoolDown());
        }
    }

    IEnumerator BlinkCoolDown() {
        playerState.blinkLock = true;

        yield return new WaitForSeconds(1f);

        playerState.blinkLock = false;
    }

    public void OnUpdate() {
        UseSkill();
    }
}