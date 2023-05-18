using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matchable : CustomBehaviour
{
     public MatchableVisual MatchableVisual;
    public override void Initialize()
    {
        MatchableVisual.Initialize(this);
    }
}
