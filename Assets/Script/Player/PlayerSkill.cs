using UnityEngine;
using System.Collections;

public class PlayerSkill : MonoBehaviour {
    public PlayerStateData playerState;

    [Header("Object")]
    public GameObject skillSpaceCircle;

    void Start() {
        skillSpaceCircle = this.gameObject.transform.Find("TestSpace").gameObject;
    }

    void Update() {
        if(Input.GetMouseButton(1)) {
            SkillSpace.skillSpaceOnOff("On");
        }

        if(Input.GetMouseButtonUp(1)) {
            StartCoroutine(playerBlink(skillSpaceCircle.transform.position));
        }
    }

    IEnumerator playerBlink(Vector2 telPosition) {
        SkillSpace.skillSpaceOnOff("Off");

        if(!playerState.blinkLock) {
            PlayerControl.playerRb.linearVelocity = Vector2.zero;
            PlayerControl.playerRb.gravityScale = 0f;

            playerState.moveLock = true;

            yield return new WaitForSeconds(0.2f);

            this.transform.position = telPosition;

            yield return new WaitForSeconds(0.2f);

            PlayerControl.playerRb.gravityScale = 2f;
            playerState.moveLock = false;

            StartCoroutine(blinkDelay());
        }
    }

    IEnumerator blinkDelay() {
        playerState.blinkLock = true;

        yield return new WaitForSeconds(1f);

        playerState.blinkLock = false;
    }
}