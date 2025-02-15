using UnityEngine;

public class EnemyDetect : MonoBehaviour, EnemySkill_Interface {
    [Header("Enemy State")]
    public float detectionRange;
    public float detectionAngle;

    public bool isCanDetect;
    public bool isPlayerDetect;

    void Start() {
        detectionRange = 3f;
        detectionAngle = 45f;

        isCanDetect = true;
        isPlayerDetect = false;
    }

    public string GetName() {
        return "EnemyDetect";
    }
    
    public void UseSkill(Vector2 directionToPlayer, float distanceToPlayer, Vector2[] detectionPoints) {
        bool detected = false;

        foreach(Vector2 target in detectionPoints) {
            Vector2 direction = target - (Vector2)this.transform.position;
            float distance = direction.magnitude;

            if(distance > detectionRange) continue;

            Vector2 enemyForward = transform.right;
            float angleToPlayer = Vector2.Angle(enemyForward, direction.normalized);

            if(angleToPlayer <= detectionAngle * 0.5f) {
                detected = true;
                break;
            }
        }

        OnDraw();
        isPlayerDetect = detected;
    }

    public bool CanUseSkill() {
        return isCanDetect;
    }

    void OnDraw() { //시각적 표현
        Vector2 enemyForward = transform.right;

        // 시야각의 왼쪽 경계
        Vector2 leftBoundary = Quaternion.Euler(0, 0, detectionAngle * 0.5f) * enemyForward;

        // 시야각의 오른쪽 경계
        Vector2 rightBoundary = Quaternion.Euler(0, 0, -detectionAngle * 0.5f) * enemyForward;
        
        #if UNITY_EDITOR
        // 실제 게임 화면에서 디버그 선을 그림
        Debug.DrawLine(transform.position, transform.position + (Vector3)(leftBoundary * detectionRange), Color.green);
        Debug.DrawLine(transform.position, transform.position + (Vector3)(rightBoundary * detectionRange), Color.green);
        #endif
    }
}