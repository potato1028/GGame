using UnityEngine;
using System.Collections;

public class EnemySkillManager : MonoBehaviour {
    [Header("Component")]
    private EnemySkill_Interface[] enemySkills;
    private EnemyState enemyState;

    void Start() {
        enemyState = this.GetComponent<EnemyState>();
        enemySkills = GetComponents<EnemySkill_Interface>();
    }

    void Update() {
        enemyState.PlayerDistance();

        foreach (var skill in enemySkills) {
            if(skill.CanUseSkill()) {
                if(enemyState.currentState == EnemyState.State.Wandering && skill.GetName() == "EnemyWander") {
                    skill.UseSkill();
                }
                else if(enemyState.currentState == EnemyState.State.Chasing && skill.GetName() == "EnemyChase" && enemyState.player != null) {
                    skill.UseSkill();
                }
                else if(enemyState.currentState == EnemyState.State.Attacking && skill.GetName() == "EnemyAttack") {
                    skill.UseSkill();
                }
            }
        }
    }

    public void ActivateSkill(string skillName) { //특정 스킬 즉시 사용을 위해 다른 곳에서 언제나 호출 가능하게 만듦
        foreach(var skill in enemySkills) {
            if(skill.GetName() == skillName) {
                skill.UseSkill();
                return;
            }
        }

        Debug.LogWarning($"Skill '{skillName}' not found!");
    }
}