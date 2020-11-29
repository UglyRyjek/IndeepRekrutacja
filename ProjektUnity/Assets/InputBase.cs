using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class InputBase : MonoBehaviour
{
    public abstract void UpdateMe(Transform playerPivot, Action onClickAction);
}