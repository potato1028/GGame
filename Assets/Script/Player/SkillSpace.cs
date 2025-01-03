using UnityEngine;
using System.Collections;

public class SkillSpace : MonoBehaviour {
    public PlayerStateData playerState;

    [Header("Object")]
    public Transform targetObject;

    [Header("Component")]
    public static SpriteRenderer skillSP;

    [Header("Condition")]
    public float maxDistance = 5f;
    public Vector3 worldPosition;

    void Start() {
        targetObject = this.transform.parent;
        skillSP = GetComponent<SpriteRenderer>();

        skillSpaceOnOff("Off");
    }

    void Update() {
        skillSpaceRange();
    }

    void skillSpaceRange() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 targetPosition = worldPosition;

        float distance = Vector2.Distance(targetObject.position, targetPosition);

        if (distance > maxDistance) {
            Vector2 direction = (targetPosition - targetObject.position).normalized;
            targetPosition = targetObject.position + (Vector3)(direction * maxDistance);
        }

        this.transform.position = targetPosition;
        
        #if UNITY_EDITOR
        Debug.DrawRay(this.transform.position, 
                  (targetObject.transform.position - this.transform.position).normalized * Vector2.Distance(this.transform.position,
                  targetObject.transform.position), 
                  Color.green);
        #endif
        
        playerState.blinkLock = Physics2D.Raycast(this.transform.position, 
                    (targetObject.transform.position - this.transform.position).normalized, 
                    Vector2.Distance(this.transform.position, 
                    targetObject.transform.position), Layer.groundLayer);
    }

    public static void skillSpaceOnOff(string state) {
        Color color = skillSP.color;

        switch (state) {
            case "On" :
                color.a = 0.3f;
                skillSP.color = color;
                break;
            
            case "Off" :
                color.a = 0f;
                skillSP.color = color;
                break;
        }
    }

    void OnTriggerStay2D(Collider2D collision2D) {
        if(collision2D.gameObject.layer == (int)LayerNum.Ground) {
            playerState.blinkLock = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision2D) {
        if(collision2D.gameObject.layer == (int)LayerNum.Ground) {
            playerState.blinkLock = false;
        }
    }
}