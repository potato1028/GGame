using UnityEngine;

public interface ISkill {
    string GetName();
    void UseSkill();
    void OnUpdate();
}