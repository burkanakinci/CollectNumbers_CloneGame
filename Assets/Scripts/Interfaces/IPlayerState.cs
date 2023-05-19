using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPlayerState : IState
{
    Action OnEnterEvent { get; set; }
    Action OnExitEvent { get; set; }
}
