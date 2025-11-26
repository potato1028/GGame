using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "ScriptableObject/PlayerStateData", order = 1)]
public class PlayerStateData : ScriptableObject {
    public string playerName = "Vod";

    [Header("Player Move")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float lastOnGroundTime = 0f;

    [Header("Player States")]
    public bool isGround;
    public bool jumpLock = false;
    public bool moveLock = false;
    public bool blinkLock = false;

    [Header("Player System Stage")]
    public float jumpBufferTime = 0.05f;
    public float coyoteTime = 0.25f;
    public float apexTime = 0.1f;
    
    [Header("System Setting")]
    public float defaultGravity = 2f;
    public float fallingGravity = 3f;
    public float apexGravity;
}