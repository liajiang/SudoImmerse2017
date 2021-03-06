﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirectionChangedEventArgs : EventArgs {
    public float Direction { get; set; }
    public DirectionChangedEventArgs(float direction)
    {
        Direction = direction;
    }
}
