﻿using System.Collections.Generic;

public class MechaInfo
{
    public MechaType MechaType;
    public List<MechaComponentInfo> MechaComponentInfos;

    public MechaInfo(MechaType mechaType, List<MechaComponentInfo> mechaComponentInfos)
    {
        MechaType = mechaType;
        MechaComponentInfos = mechaComponentInfos;
    }
}

public enum MechaType
{
    None = 0,
    Self = 1,
    Enemy = 2,
}