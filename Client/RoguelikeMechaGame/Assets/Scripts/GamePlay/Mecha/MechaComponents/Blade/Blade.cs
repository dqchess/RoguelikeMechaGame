﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public BladeInfo BladeInfo;

    public void Initialize(BladeInfo bladeInfo)
    {
        BladeInfo = bladeInfo;
    }

    private float bladeAttackTick = 0;

    List<HitBox> HittingHitBoxes = new List<HitBox>();

    void Update()
    {
        if (GameManager.Instance.GetState() == GameState.Fighting)
        {
            bladeAttackTick += Time.deltaTime;
            if (bladeAttackTick > BladeInfo.FinalInterval)
            {
                bladeAttackTick = 0;
                foreach (HitBox hb in HittingHitBoxes)
                {
                    hb.ParentHitBoxRoot.MechaComponentBase.Damage(BladeInfo.FinalDamage);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (GameManager.Instance.GetState() == GameState.Fighting)
        {
            HitBox hb = c.gameObject.GetComponent<HitBox>();
            if (hb && hb.ParentHitBoxRoot.MechaComponentBase.MechaType != BladeInfo.MechaType)
            {
                HittingHitBoxes.Add(hb);
                return;
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (GameManager.Instance.GetState() == GameState.Fighting)
        {
            HitBox hb = c.gameObject.GetComponent<HitBox>();
            if (hb && hb.ParentHitBoxRoot.MechaComponentBase.MechaType != BladeInfo.MechaType)
            {
                HittingHitBoxes.Remove(hb);
                return;
            }
        }
    }
}