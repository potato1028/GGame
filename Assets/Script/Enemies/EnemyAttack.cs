using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour, EnemySkill_Interface {
    [Header("Enemy State")]
    public float attackRange;
    public float attackReadyTime;
    public float attackCoolTime;
    
    public bool isCanAttack;

    //[Header("Component")]

    void Start() {
        attackRange = 2f;
        attackReadyTime = 0.5f;
        attackCoolTime = 1.0f;

        isCanAttack = true;
    }

    public string GetName() {
        return "EnemyAttack";
    }

    public void UseSkill(Vector2 directionToPlayer, float distanceToPlayer, Vector2[] detectionPoints) {
        if(attackRange > distanceToPlayer) {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() {
        isCanAttack = false;

        Debug.Log("Ready");

        yield return new WaitForSeconds(attackReadyTime);

        Debug.Log("Attack!");

        yield return new WaitForSeconds(attackCoolTime);

        isCanAttack = true;
    }

    public bool CanUseSkill() {
        return isCanAttack;
    }
}