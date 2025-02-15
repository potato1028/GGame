using UnityEngine;
using System.Collections;

public class EnemySkillManager : MonoBehaviour {
    [Header("Object")]
    public GameObject player;

    [Header("State")]
    Vector2 direction;
    float distance;
    Vector2[] detectionPoints;

    [Header("Component")]
    private EnemySkill_Interface[] enemySkills;
    private EnemyState enemyState;
    public BoxCollider2D playerBox2D;

    void Awake() {
        player = GameObject.FindWithTag("Player");

        enemyState = this.GetComponent<EnemyState>();
        playerBox2D = player.GetComponent<BoxCollider2D>();
        enemySkills = GetComponents<EnemySkill_Interface>();
    }

    void Update() {
        direction = (Vector2)player.transform.position - (Vector2)this.transform.position;
        distance = direction.magnitude;

        detectionPoints = new Vector2[] {
            (Vector2)playerBox2D.bounds.center,
            (Vector2)playerBox2D.bounds.center - new Vector2(0, playerBox2D.bounds.extents.y),
            (Vector2)playerBox2D.bounds.center + new Vector2(0, playerBox2D.bounds.extents.y)
        };

        foreach (var skill in enemySkills) {
            if(skill.CanUseSkill()) {
                if(enemyState.currentState == EnemyState.State.Wandering && skill.GetName() == "EnemyWander") skill.UseSkill(direction, distance, detectionPoints);
                else if(enemyState.currentState == EnemyState.State.Chasing && skill.GetName() == "EnemyChase") skill.UseSkill(direction, distance, detectionPoints);
                else if(enemyState.currentState == EnemyState.State.Attacking && skill.GetName() == "EnemyAttack") skill.UseSkill(direction, distance, detectionPoints);
            }
        }
    }

    public void ActivateSkill(string skillName) { //특정 스킬 즉시 사용을 위해 다른 곳에서 언제나 호출 가능하게 만듦
        foreach(var skill in enemySkills) {
            if(skill.GetName() == skillName) {
                skill.UseSkill(direction, distance, detectionPoints);
                return;
            }
        }

        Debug.LogWarning($"Skill '{skillName}' not found!");
    }
}