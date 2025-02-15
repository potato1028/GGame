using UnityEngine;

public interface EnemySkill_Interface {
    string GetName();
    void UseSkill(Vector2 directionToPlayer = default, float distanceToPlayer = 0, Vector2[] detectionPoints = null);
    bool CanUseSkill();
}