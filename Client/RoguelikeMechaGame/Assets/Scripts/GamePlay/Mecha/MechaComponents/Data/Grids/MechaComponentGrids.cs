﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MechaComponentGrids : MonoBehaviour
{
    private List<MechaComponentGrid> mechaComponentGrids = new List<MechaComponentGrid>();

    internal List<GridPos> MechaComponentGridPositions = new List<GridPos>();

    void Awake()
    {
        mechaComponentGrids = GetComponentsInChildren<MechaComponentGrid>().ToList();
        foreach (MechaComponentGrid mcg in mechaComponentGrids)
        {
            MechaComponentGridPositions.Add(mcg.GetGridPos());
        }
    }

    public void SetSlotLightsShown(bool shown)
    {
        foreach (MechaComponentGrid mcg in mechaComponentGrids)
        {
            mcg.SetSlotLightsShown(shown);
        }
    }

    public void SetGridShown(bool shown)
    {
        foreach (MechaComponentGrid mcg in mechaComponentGrids)
        {
            mcg.SetGridShown(shown);
        }
    }

    public void TurnOffAllForbidIndicator()
    {
        foreach (MechaComponentGrid grid in mechaComponentGrids)
        {
            grid.SetForbidIndicatorShown(false);
        }
    }

    public void SetForbidIndicatorShown(bool shown, GridPos gridPos)
    {
        foreach (MechaComponentGrid grid in mechaComponentGrids)
        {
            GridPos gp = grid.GetGridPos();
            if (gp.x == gridPos.x && gp.z == gridPos.z)
            {
                grid.SetForbidIndicatorShown(shown);
            }
        }
    }

    public void SetIsolatedIndicatorShown(bool shown)
    {
        foreach (MechaComponentGrid grid in mechaComponentGrids)
        {
            grid.SetIsolatedIndicatorShown(shown);
        }
    }
}