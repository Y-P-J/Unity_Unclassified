using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Tooltip("ĳ���� �̸�")]
    [SerializeField] protected string charaName;
    [Tooltip("ĳ���� �̹���")]
    [SerializeField] protected Sprite sprite;
    [Tooltip("ĳ���� ����")]
    [SerializeField] protected Status status;
    protected float hp;

    #region ���ٽ� ������Ƽ
    public string CharaName => charaName;
    public float Hp => hp;
    public float Attack => status.attack;
    public float Defense => status.defense;
    public float Speed => status.speed;
    #endregion

    public abstract bool SkillQ();
    public abstract bool SkillE();
    public abstract bool SkillR();
}
