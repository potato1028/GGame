using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour, EnemySkill_Interface {
    [Header("Enemy State")]
    public float attackReadyTime;
    public float attackCoolTime;
    
    public bool isCanAttack;

    [Header("Component")]
    private EnemyState enemyState;

    void Start() {
        enemyState = this.GetComponent<EnemyState>();
        
        attackReadyTime = enemyState.attackReadyTime;
        attackCoolTime = enemyState.attackCoolTime;

        isCanAttack = true;
    }

    public string GetName() {
        return "EnemyAttack";
    }

    public void UseSkill() {
        StartCoroutine(Attack());
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