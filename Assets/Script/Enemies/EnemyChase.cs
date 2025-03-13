using UnityEngine;

public class EnemyChase : MonoBehaviour, EnemySkill_Interface {
    [Header("Enemy State")]
    public float chaseSpeed;

    public bool isCanChase;

    [Header("Component")]
    public Rigidbody2D rb2D;
    private EnemyState enemyState;

    void Start() {
        rb2D = this.GetComponent<Rigidbody2D>();
        enemyState = this.GetComponent<EnemyState>();

        chaseSpeed = enemyState.chaseSpeed;

        isCanChase = true;
    }

    public string GetName() {
        return "EnemyChase";
    }

    public void UseSkill() {
        rb2D.linearVelocity = new Vector2(enemyState.direction.normalized.x * chaseSpeed, rb2D.linearVelocity.y);
    }

    public bool CanUseSkill() {
        return isCanChase;
    }
}