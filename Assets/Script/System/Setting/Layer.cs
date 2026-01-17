using UnityEngine;

public enum LayerNum {
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Player = 3,
    Water = 4,
    UI = 5,
    Ground = 6,
    Wall = 7,
    Enemy = 8
}

public static class Layer {
    public static LayerMask groundLayer = 1 << (int)LayerNum.Ground;
    public static LayerMask wallLayer = 1 << (int)LayerNum.Wall;
    public static LayerMask playerLayer = 1 << (int)LayerNum.Player;

    public static LayerMask terrainLayer =  (1 << (int)LayerNum.Wall) | (1 << (int)LayerNum.Ground);
    public static LayerMask passengerLayer = (1 << (int)LayerNum.Player) | (1 << (int)LayerNum.Enemy);
}