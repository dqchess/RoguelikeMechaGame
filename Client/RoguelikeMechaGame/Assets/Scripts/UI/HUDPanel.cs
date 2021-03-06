﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUDPanel : BaseUIForm
{
    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: true,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.Normal,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.Penetrable);
    }

    [SerializeField] private MechaInfoHUD PlayerMechaInfoHUD;
    [SerializeField] private MechaInfoHUD EnemyMechaInfoHUD;

    public void Initialize()
    {
        PlayerMechaInfoHUD.Initialize(BattleManager.Instance.PlayerMecha);
    }

    public void LoadEnemyMech(Mecha enemyMecha)
    {
        EnemyMechaInfoHUD.Initialize(enemyMecha);
    }
}