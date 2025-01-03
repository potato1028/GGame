using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "ScriptableObject/PlayerStateData", order = 1)]
public class PlayerStateData : ScriptableObject {
    public string playerName;

    [Header("Player Move")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Player States")]
    public bool isGround;
    public bool jumpLock;
    public bool moveLock;
    public bool blinkLock;

    [Header("Player System Stage")]
    public float jumpBufferTime;
    public float coyoteTime;
}