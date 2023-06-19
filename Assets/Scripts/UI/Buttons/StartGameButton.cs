using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : UIBaseButton<MainMenuPanel>
{
    public override void Initialize(MainMenuPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
    }
    protected override void OnClickAction()
    {
        CachedComponent.StartMainMenuDisolve();
    }
}
