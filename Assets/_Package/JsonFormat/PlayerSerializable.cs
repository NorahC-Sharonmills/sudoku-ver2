using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSerializable
{
    public string Id = "hihidochoo";
    public bool IsAds = true;
    public int Gold;

    public PlayerSerializable()
    {
        Id = SystemInfo.deviceUniqueIdentifier;
        IsAds = true;
        Gold = 0;
    }
}

