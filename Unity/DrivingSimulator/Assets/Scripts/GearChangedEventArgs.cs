using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GearChangedEventArgs : EventArgs
{
    public bool IsReverse { get; set; }

    public GearChangedEventArgs(bool isReverse)
    {
        IsReverse = isReverse;
    }

}
