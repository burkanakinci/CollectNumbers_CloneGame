
using UnityEngine;

public static class Utilities
{
    public static void Open(this CanvasGroup canvas)
    {
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
    }

    public static void Close(this CanvasGroup canvas)
    {
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
    }

    public static Vector2 RandomPosInsideCircle( float _radius)
    {
        return (UnityEngine.Random.insideUnitCircle * _radius);
    }
}
