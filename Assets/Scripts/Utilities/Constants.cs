using UnityEngine;
using System;

public class PlayerData
{
    public int PlayerLevel;
    public int PlayerScore;
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
        "#0000FF",
        "#FF8000",
        "#8000FF"
        };
}
public enum MatchableType
{
    Red = 0,
    Green = 1,
    Blue = 2,
    Orange=3,
    Purple=4,
    Random,
}
public enum PooledObjectType
{
    Matchable=0,
}
public enum PlayerStates
{
    IdleState = 0,
    RunState = 1,
    SuccessState = 2,
    FailState = 3,
    GeneralState
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
public enum FinishAreaType
{
    RampArea = 0,
    SuccessArea = 1,
    FailArea = 2,
}
public enum ActiveParents
{
    MatchableActiveParent = 0,
}
public enum DeactiveParents
{
    MatchableDeactiveParent=0,
}
public enum ListOperations
{
    Adding,
    Substraction,
}
