using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Tooltip("캐릭터 이름")]
    [SerializeField] protected string charaName;
    [Tooltip("캐릭터 이미지")]
    [SerializeField] protected Sprite sprite;
    [Tooltip("캐릭터 스탯")]
    [SerializeField] protected Status status;
    protected float hp;

    #region 람다식 프로퍼티
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
