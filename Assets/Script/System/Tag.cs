using UnityEngine;

public enum TagName {
    Player,
    SkillSpace
}

public static class Tag {
    public static string Player => TagName.Player.ToString();
    public static string SkillSpace => TagName.SkillSpace.ToString();
}