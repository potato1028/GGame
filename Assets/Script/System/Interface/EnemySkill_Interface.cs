using UnityEngine;

public interface EnemySkill_Interface {
    string GetName();
    void UseSkill();
    bool CanUseSkill();
}