using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public int PlayerLevel;
}
[Serializable]
public class MatchableOnCell
{
    public MatchableType MatchableTypeOnCell;
    public int MatchableXIndis;
    public int MatchableYIndis;
}
public struct Constant
{
    public const string PLAYER_DATA = "PlayerSavedData";
}
public struct ObjectTags
{
}
public struct PooledObjectTags
{
    public const string MATCHABLE = "Matchable";
}

[System.Serializable]
public struct TargetMatchable
{
    public MatchableType TargetMatchableType;
    public int TargetMatchableCount;
}
public class Colors
{
    public static readonly string[] ColorArray = {
        "#FF0000",
        "#00FF00" ,
        "#0000FF"
        };
}
public enum MatchableType
{
    Red = 0,
    Green = 1,
    Blue = 2,
    Random,
}
public enum ObjectsLayer
{
    Default = 0,
    Matchable = 6,
}
public enum UIPanelType
{
    MainMenuPanel = 0,
    HudPanel = 1,
    FinishPanel = 2,
}

public enum ActiveParents
{
    MatchableActiveParent = 0,
}

public enum ListOperations
{
    Adding,
    Substraction,
}
