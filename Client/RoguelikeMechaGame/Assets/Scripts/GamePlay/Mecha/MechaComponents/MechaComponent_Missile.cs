﻿using UnityEngine;
using System.Collections;

public class MechaComponent_Missile : MechaComponent_Controllable_Base, IBuff_PowerUp
{
    public Shooter Shooter;

    void Start()
    {
        Shooter.Initialize(new ShooterInfo(MechaType.Self, 0.1f, 50f, new ProjectileInfo(MechaType.Self, ProjectileType.Projectile_Butter, ConfigManager.Instance.MissileSpeed, ConfigManager.Instance.MissileDamage)));
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void ControlPerFrame()
    {
        if (Input.GetButton("Fire2"))
        {
            Shooter?.ContinuousShoot();
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shooter?.Shoot();
            }
        }
    }

    public void AddModifier(Modifier modifier)
    {
        Shooter.ShooterInfo.ProjectileInfo.Modifiers_Damage.Add(modifier);
    }

    public void RemoveModifier(Modifier modifier)
    {
        Shooter.ShooterInfo.ProjectileInfo.Modifiers_Damage.Remove(modifier);
    }
}