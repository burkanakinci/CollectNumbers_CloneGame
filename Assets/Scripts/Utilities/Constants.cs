using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct PlayerData
{
    public int PlayerLevel;
    public int PlayerScore;
}
[Serializable]
public class MatchableOnCell
{
    public MatchableColor MatchableColorOnCell;
    public int MatchableXIndis;
    public int MatchableYIndis;
}
[Serializable]
public struct MatchableType
{
    public MatchableColor MatchableColor;
    public Sprite MatchableSprite;
    public Sprite BackSprite;
    public int MatchableValue;
}
public struct Constant
{
    public const string PLAYER_DATA = "PlayerSavedData";
    public const string DISSOLVE_VALUE = "_DissolveValue";
}

[System.Serializable]
public struct TargetMatchable
{
    public MatchableColor TargetMatchableColor;
    public int TargetMatchableCount;
}
[System.Serializable]
public struct TargetAreaValue
{
    public float BGHeight;
    public Sprite AreaBG;
}
public class Colors
{
    public static readonly Vector4[] ColorArray = {
        new Vector4(1.0f,0.0f,0.0f,1.0f),
        new Vector4(0.0f,1.0f,0.0f,1.0f),
        new Vector4(0.0f,0.0f,1.0f,1.0f),
        new Vector4(1.0f,0.5f,0.0f,1.0f),
        new Vector4(0.5f,0.0f,1.0f,1.0f),
        new Vector4(1.0f,1.0f,1.0f,1.0f)
        };
    public static Color GetColor(MatchableColor _color)
    {
        return new Color(Colors.ColorArray[(int)_color].x, Colors.ColorArray[(int)_color].y, Colors.ColorArray[(int)_color].z, Colors.ColorArray[(int)_color].w);
    }
}
public enum MatchableColor
{
    Red = 0,
    Green = 1,
    Blue = 2,
    Orange = 3,
    Purple = 4,
    Random = 5,
}
public enum NeighbourType
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
}
public enum PooledObjectType
{
    Matchable = 0,
    Blast_VFX = 1,
    Star_VFX = 2,
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
    SuccessArea = 0,
    FailArea = 1,
}
public enum ActiveParents
{
    MatchableActiveParent = 0,
    VFXActiveParent = 1,
    UIObjects = 2
}
public enum DeactiveParents
{
    MatchableDeactiveParent = 0,
    VFXDeactiveParent = 1,
    UIObjects = 2
}
public enum ListOperations
{
    Adding,
    Substraction,
}

