using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : PooledObject
{
    public TargetMatchable CurrentTargetMatchable;
    public override void Initialize()
    {
    }
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
    }
    public override void OnObjectDeactive()
    {
        base.OnObjectDeactive();
    }
}
