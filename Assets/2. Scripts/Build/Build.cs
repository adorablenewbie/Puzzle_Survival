using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BuildRequirement
{
    public ItemData item;
    public int amount;
}

[Serializable]
public class Build
{
    public string buildName;
    public GameObject installPrefab;
    public GameObject previewPrefab;

    public BuildRequirement[] requirements;
}
