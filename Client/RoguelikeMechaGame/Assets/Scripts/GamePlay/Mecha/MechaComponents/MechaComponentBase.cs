﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;

public abstract class MechaComponentBase : PoolObject, IDraggable
{
    internal Mecha ParentMecha = null;
    internal Draggable Draggable;

    internal MechaType MechaType => ParentMecha ? ParentMecha.MechaInfo.MechaType : MechaType.None;

    void Awake()
    {
        Draggable = GetComponent<Draggable>();
    }

    public override void PoolRecycle()
    {
        base.PoolRecycle();
        if (ParentMecha)
        {
            ParentMecha.RemoveMechaComponent(this);
        }

        ParentMecha = null;

        foreach (FX lighteningFX in lighteningFXs)
        {
            lighteningFX.PoolRecycle();
        }

        lighteningFXs.Clear();
        isReturningToBag = false;
    }

    public static MechaComponentBase BaseInitialize(MechaComponentInfo mechaComponentInfo, Mecha parentMecha)
    {
        GameObjectPoolManager.PrefabNames prefabName = (GameObjectPoolManager.PrefabNames) Enum.Parse(typeof(GameObjectPoolManager.PrefabNames), "MechaComponent_" + mechaComponentInfo.MechaComponentType);
        MechaComponentBase mcb = GameObjectPoolManager.Instance.PoolDict[prefabName].AllocateGameObject<MechaComponentBase>(parentMecha ? parentMecha.transform : null);
        mcb.Initialize(mechaComponentInfo, parentMecha);
        return mcb;
    }

    public MechaComponentInfo MechaComponentInfo;

    public virtual void Initialize(MechaComponentInfo mechaComponentInfo, Mecha parentMecha)
    {
        IsDead = false;
        MechaComponentInfo = mechaComponentInfo;
        RefreshOccupiedGridPositions();
        GridPos.ApplyGridPosToLocalTrans(mechaComponentInfo.GridPos, transform, GameManager.GridSize);
        ParentMecha = parentMecha;
        _totalLife = 50;
        _leftLife = 50;
    }

    public void RefreshOccupiedGridPositions()
    {
        MechaComponentInfo.OccupiedGridPositions = GridPos.TransformOccupiedPositions(MechaComponentInfo.GridPos, MechaComponentGrids.MechaComponentGridPositions);
    }

    public MechaComponentGrids MechaComponentGrids;
    public HitBoxRoot MechaHitBoxRoot;

    private void Rotate()
    {
        transform.Rotate(0, 90, 0);
    }

    #region Life

    internal bool IsDead = false;

    private int _leftLife;

    public int M_LeftLife
    {
        get { return _leftLife; }
    }

    private int _totalLife;
    private float _dragComponentDragMinDistance;
    private float _dragComponentDragMaxDistance;

    public int M_TotalLife
    {
        get { return _totalLife; }
    }

    public bool CheckAlive()
    {
        return M_LeftLife > 0;
    }

    public void AddLife(int addLifeValue)
    {
        _totalLife += addLifeValue;
        _leftLife += addLifeValue;
    }

    public void Heal(int healValue)
    {
    }

    private List<FX> lighteningFXs = new List<FX>();

    public void Damage(int damage)
    {
        if (_leftLife > M_TotalLife * 0.5f && _leftLife - damage <= M_TotalLife * 0.5f)
        {
            foreach (HitBox hb in MechaHitBoxRoot.HitBoxes)
            {
                FX lighteningFX = FXManager.Instance.PlayFX(FX_Type.FX_BlockDamagedLightening, hb.transform.position);
                lighteningFXs.Add(lighteningFX);
            }
        }

        _leftLife -= damage;
        FXManager.Instance.PlayFX(FX_Type.FX_BlockDamageHit, transform.position + Vector3.up * 0.5f);

        if (!IsDead && !CheckAlive())
        {
            IsDead = true;
            FXManager.Instance.PlayFX(FX_Type.FX_BlockExplode, transform.position);
            ParentMecha.RemoveMechaComponent(this);
            PoolRecycle(0.2f);
        }
    }

    public void HealAll()
    {
        _leftLife = M_TotalLife;
    }

    public void Change(int change)
    {
        _leftLife += change;
    }

    public void ChangeMaxLife(int change)
    {
        _totalLife += change;
    }

    #endregion

    #region IDraggable

    public void DragComponent_OnMouseDown()
    {
    }

    public void DragComponent_OnMousePressed(DragAreaTypes dragAreaTypes)
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Rotate();
        }

        switch (dragAreaTypes)
        {
            case DragAreaTypes.Bag:
            {
                ReturnToBag(true, true);
                break;
            }
        }
    }

    private bool isReturningToBag = false;

    private bool ReturnToBag(bool cancelDrag, bool dragTheItem)
    {
        bool suc = BagManager.Instance.AddMechaComponentToBag(MechaComponentInfo, out BagItem bagItem);
        if (suc)
        {
            if (cancelDrag)
            {
                isReturningToBag = true;
                DragManager.Instance.CancelCurrentDrag();
            }

            if (dragTheItem)
            {
                DragManager.Instance.CurrentDrag = bagItem.gameObject.GetComponent<Draggable>();
                DragManager.Instance.CurrentDrag.IsOnDrag = true;
            }

            PoolRecycle();
        }

        return suc;
    }

    public void DragComponent_OnMouseUp(DragAreaTypes dragAreaTypes)
    {
        switch (dragAreaTypes)
        {
            case DragAreaTypes.Bag:
            {
                if (!isReturningToBag)
                {
                    bool suc = ReturnToBag(false, false);
                    if (!suc)
                    {
                        DragManager.Instance.CurrentDrag.ReturnOriginalPositionRotation();
                    }
                }

                break;
            }
            case DragAreaTypes.MechaEditorArea:
            {
                MechaComponentInfo.GridPos = GridPos.GetGridPosByLocalTrans(transform, 1);
                MechaComponentInfo.OccupiedGridPositions = GridPos.TransformOccupiedPositions(MechaComponentInfo.GridPos, MechaComponentInfo.OccupiedGridPositions);
                Debug.Log(MechaComponentInfo.OccupiedGridPositions);
                break;
            }
            case DragAreaTypes.DiscardedArea:
            {
                break;
            }
            case DragAreaTypes.None:
            {
                bool suc = ReturnToBag(false, false);
                if (!suc)
                {
                    DragManager.Instance.CurrentDrag.ReturnOriginalPositionRotation();
                }

                break;
            }
        }
    }

    public void DragComponent_SetStates(ref bool canDrag, ref DragAreaTypes dragFrom)
    {
        canDrag = true;
        dragFrom = DragAreaTypes.MechaEditorArea;
    }

    float IDraggable.DragComponent_DragMinDistance => 0f;

    float IDraggable.DragComponent_DragMaxDistance => 9999f;

    public void DragComponent_DragOutEffects()
    {
    }

    #endregion
}