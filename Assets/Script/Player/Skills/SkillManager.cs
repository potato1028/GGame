using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {
    private PlayerSkill_Interface[] skills;

    void Start() {
        skills = GetComponents<PlayerSkill_Interface>();
    }

    public void ActivateSkill(string skillName) { //특정 스킬 즉시 사용을 위해 다른 곳에서 언제나 호출 가능하게 만듦
        foreach(var skill in skills) {
            if(skill.GetName() == skillName) {
                skill.UseSkill();
                return;
            }
        }

        Debug.LogWarning($"Skill '{skillName}' not found!");
    }

    void Update() {
        foreach (var skill in skills) {
            skill.OnUpdate();
        }
    }
}