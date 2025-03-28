using UnityEngine;

public class EnemyDetect : MonoBehaviour{
    [Header("State")]
    public Vector2[] detectionPoints;
    public float detectionRange;
    public float detectionAngle;
    public RaycastHit2D backHit;

    [Header("Component")]
    private EnemyState enemyState;

    void Start() {
        enemyState = this.GetComponent<EnemyState>();

        detectionRange = enemyState.detectionRange;
        detectionAngle = enemyState.detectionAngle;

        detectionPoints = new Vector2[3];
    }
    
    public void DetectOfPlayer(int enemyForward) {
        OnDraw(new Vector2(enemyForward, 0));

        if(enemyState.playerBox2D == null) return;

        detectionPoints[0] = (Vector2)enemyState.playerBox2D.bounds.center;
        detectionPoints[1] = detectionPoints[0] - new Vector2(0, enemyState.playerBox2D.bounds.extents.y);
        detectionPoints[2] = detectionPoints[0] + new Vector2(0, enemyState.playerBox2D.bounds.extents.y);
        
        foreach(Vector2 target in detectionPoints) {
            Vector2 direction = target - (Vector2)this.transform.position;
            float distance = direction.magnitude;

            if(distance > detectionRange) continue;

            float angleToPlayer = Vector2.Angle(new Vector2(enemyForward, 0), direction.normalized);

            if(angleToPlayer <= detectionAngle * 0.5f) {
                enemyState.isPlayerDetected = true;
                #if UNITY_EDITOR
                Debug.DrawLine(this.transform.position, target, Color.red); // 감지된 경로 (빨간색)
                #endif
                break;
            }
            else {
                enemyState.isPlayerDetected = false;
                #if UNITY_EDITOR
                Debug.DrawLine(this.transform.position, target, Color.blue); // 감지되지 않은 경로를 파란색으로 표시
                #endif
            }
        }
    }

    public void DetectBackOfPlayer(int enemyForward) {
        backHit = Physics2D.Raycast(this.transform.position, Vector2.right * enemyForward * -1f, 1f, Layer.playerLayer);

        if(backHit.collider != null) {
            enemyState.isPlayerDetected = true;
            return;
        }

        #if UNITY_EDITOR
        Debug.DrawRay(this.transform.position, Vector2.right * enemyForward * -1f, Color.yellow);
        #endif
    }

    void OnDraw(Vector2 enemyForward) { // 시각적 표현
        // 시야각의 왼쪽 경계
        Vector2 leftBoundary = Quaternion.Euler(0, 0, detectionAngle * 0.5f) * enemyForward;
        Vector2 rightBoundary = Quaternion.Euler(0, 0, -detectionAngle * 0.5f) * enemyForward;

        #if UNITY_EDITOR
        // 원형 감지 영역 시각화 (OverlapCircle)
        int circleSegments = 30;
        float angleStep = 360f / circleSegments;
        Vector3 prevPoint = transform.position + (Vector3)(Quaternion.Euler(0, 0, 0) * Vector2.right * enemyState.chaseRange); // 반지름 4f

        for (int i = 1; i <= circleSegments; i++) {
            float angle = angleStep * i;
            Vector3 newPoint = transform.position + (Vector3)(Quaternion.Euler(0, 0, angle) * Vector2.right * enemyState.chaseRange);
            Debug.DrawLine(prevPoint, newPoint, Color.yellow); // 원형 감지 범위 표시 (노란색)
            prevPoint = newPoint;
        }

        // 시야각 표시
        int segments = 20;
        float detectionAngleStep = detectionAngle / segments;
        Vector3 prevViewPoint = transform.position + (Vector3)(Quaternion.Euler(0, 0, -detectionAngle * 0.5f) * enemyForward * detectionRange);

        for (int i = 1; i <= segments; i++) {
            float angle = -detectionAngle * 0.5f + detectionAngleStep * i;
            Vector3 newViewPoint = transform.position + (Vector3)(Quaternion.Euler(0, 0, angle) * enemyForward * detectionRange);
            Debug.DrawLine(prevViewPoint, newViewPoint, Color.green);
            prevViewPoint = newViewPoint;
        }

        // 시야각 범위 경계선
        Debug.DrawLine(transform.position, transform.position + (Vector3)(leftBoundary * detectionRange), Color.green);
        Debug.DrawLine(transform.position, transform.position + (Vector3)(rightBoundary * detectionRange), Color.green);
        #endif
    }
}
