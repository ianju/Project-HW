using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ManagerType
{
    ��������,
    �߼�����,
    ����
}

public enum BoostType
{
    �����ٶ�,
    װ���ٶ�
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
