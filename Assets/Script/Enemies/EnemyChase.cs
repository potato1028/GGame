using UnityEngine;

public class EnemyChase : MonoBehaviour, EnemySkill_Interface {
    [Header("Enemy State")]
    public float chaseRange;
    public float chaseSpeed;

    public bool isCanChase;

    [Header("Component")]
    public Rigidbody2D rb2D;

    void Awake() {
        rb2D = this.GetComponent<Rigidbody2D>();
    }

    void Start() {
        chaseRange = 6f;
        chaseSpeed = 2f;

        isCanChase = true;
    }

    public string GetName() {
        return "EnemyChase";
    }

    public void UseSkill(Vector2 directionToPlayer, float distanceToPlayer, Vector2[] detectionPoints) {
        if(chaseRange > distanceToPlayer) {
            rb2D.linearVelocity = new Vector2(directionToPlayer.normalized.x * chaseSpeed, rb2D.linearVelocity.y);
        }
    }

    public bool CanUseSkill() {
        return isCanChase;
    }
}