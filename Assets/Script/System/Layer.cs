using UnityEngine;

public enum LayerNum {
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Player = 3,
    Water = 4,
    UI = 5,
    Ground = 6
}

public static class Layer {
    public static LayerMask groundLayer = 1 << (int)LayerNum.Ground;
}