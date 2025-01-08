using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {
    private ISkill[] skills;

    void Start() {
        skills = GetComponents<ISkill>();
    }

    public void ActivateSkill(string skillName) {
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