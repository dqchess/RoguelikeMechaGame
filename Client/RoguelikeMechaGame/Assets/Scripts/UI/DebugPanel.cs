﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugPanel : BaseUIForm
{
    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: false,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.Fixed,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.Penetrable);
    }

    public Text fpsText;
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }

    public void AddSword()
    {
        BagManager.Instance.AddMechaComponentToBag(new MechaComponentInfo(MechaComponentType.Sword, new GridPos(-2, 3, GridPos.Orientation.Right)), out BagItem bagItem);
    }

    public void AddEngine()
    {
        BagManager.Instance.AddMechaComponentToBag(new MechaComponentInfo(MechaComponentType.Engine, new GridPos(-2, 3, GridPos.Orientation.Right)), out BagItem bagItem);
    }
}