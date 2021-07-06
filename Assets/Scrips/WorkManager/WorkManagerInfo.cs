using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ManagerType
{
    初级经理,
    高级经理,
    主管
}

public enum BoostType
{
    步行速度,
    装载速度
}

[CreateAssetMenu]
public class WorkManagerInfo : ScriptableObject
{
    [Header("Manager Info")]
    public ManagerType managerType;
    public Color levelColor;

    public BoostType BoostType;
    public Sprite BoostIcon;
    public float BoostDuration;
    public string BoostDescription;
    public float BoostValue;
}
