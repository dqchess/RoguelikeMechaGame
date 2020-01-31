﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// The class control the display and the background of a grid in a bag
/// </summary>
public class BagGrid : PoolObject
{
    [SerializeField] private Image Image;
    [SerializeField] private Color AvailableColor;
    [SerializeField] private Color UnavailableColor;

    private bool _available;

    public bool Available
    {
        get { return _available && !_locked; }
        set
        {
            _available = value;
            Image.color = value ? AvailableColor : UnavailableColor;
        }
    }

    private bool _locked;

    public bool Locked
    {
        get { return _locked; }
        set
        {
            _locked = value;
            Image.enabled = !value;
        }
    }
}