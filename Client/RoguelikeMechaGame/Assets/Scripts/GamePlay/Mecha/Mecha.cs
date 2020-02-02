﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mecha : PoolObject
{
    public MechaInfo MechaInfo;

    private List<MechaComponentBase> mechaComponents = new List<MechaComponentBase>();

    [SerializeField] private Transform MechaComponentContainer;
    public MechaEditArea MechaEditArea;

    public void Initialize(MechaInfo mechaInfo)
    {
        MechaInfo = mechaInfo;
        foreach (MechaComponentInfo mci in mechaInfo.MechaComponentInfos)
        {
            MechaComponentBase mcb = MechaComponentBase.BaseInitialize(mci, MechaComponentContainer, this);
            mechaComponents.Add(mcb);
        }

        MechaEditArea.Hide();
        GridShown = false;
        SlotLightsShown = false;
    }

    public void AddMechaComponent(MechaComponentBase mcb)
    {
        mechaComponents.Add(mcb);
        mcb.MechaComponentGrids.SetGridShown(GridShown);
        mcb.MechaComponentGrids.SetSlotLightsShown(SlotLightsShown);
    }

    public float Speed = 3f;

    void Update()
    {
        if (MechaInfo.MechaType == MechaType.Self)
        {
            if (GameManager.Instance.GetState() == GameState.Fighting)
            {
                float movement = 0.7f * Time.deltaTime * Speed;
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(-movement, 0, -movement, Space.World);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(movement, 0, movement, Space.World);
                }

                if (Input.GetKey(KeyCode.W))
                {
                    transform.Translate(-movement, 0, movement, Space.World);
                }

                if (Input.GetKey(KeyCode.S))
                {
                    transform.Translate(movement, 0, -movement, Space.World);
                }

                RotateToMouseDirection();
            }

            if (Input.GetKeyUp(KeyCode.G))
            {
                SlotLightsShown = !SlotLightsShown;
                GridShown = !GridShown;
            }
        }
    }

    private void RotateToMouseDirection()
    {
        Ray ray = GameManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 intersect = ClientUtils.GetIntersectWithLineAndPlane(ray.origin, ray.direction, Vector3.up, transform.position);
        Quaternion rotation = Quaternion.LookRotation(intersect - transform.position);
        transform.localRotation = Quaternion.Lerp(transform.rotation, rotation, 1);
    }

    private bool _slotLightsShown = true;

    public bool SlotLightsShown
    {
        get { return _slotLightsShown; }
        set
        {
            if (_slotLightsShown != value)
            {
                foreach (MechaComponentBase mcb in mechaComponents)
                {
                    mcb.MechaComponentGrids.SetSlotLightsShown(value);
                }
            }

            _slotLightsShown = value;
        }
    }

    private bool _gridShown = true;

    public bool GridShown
    {
        get { return _gridShown; }
        set
        {
            if (_gridShown != value)
            {
                foreach (MechaComponentBase mcb in mechaComponents)
                {
                    mcb.MechaComponentGrids.SetGridShown(value);
                }
            }

            _gridShown = value;
        }
    }
}