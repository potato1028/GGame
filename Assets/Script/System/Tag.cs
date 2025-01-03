using UnityEngine;

public enum TagName {
    Player
}

public static class Tag {
    public static string Player => TagName.Player.ToString();
}