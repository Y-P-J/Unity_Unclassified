using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MK1.Item;
using UnityEditor.MPE;

namespace MK1.Item
{
    public enum ITEMTYPE
    {
        EQUIPMENT,      // 장비.
        USEABLE,        // 소비.
        MATERIAL,       // 재료.
    }
    public enum EQUIP_TYPE
    {
        CLOTH,
        PANTS,
        SHOES,
        WEAPON,
    }
}

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public string context;
    public Sprite sprite;
    public int number;
    public int price;

    public int count { get; protected set; }
    public bool isOverlap => maxCount > 1;

    public abstract int maxCount { get; }
    protected abstract ITEMTYPE itemType { get; }
    public abstract string itemCode { get; }
    public abstract Item Copy(int count);

    public bool CompareID(string id)
    {
        return itemCode == id;
    }
    public bool CompareID(Item target)
    {
        return itemCode == target.itemCode;
    }
    public override string ToString()
    {
        return itemName;
    }

    /// <summary>
    /// 리턴 값은 중첩을 완벽하게 했느냐?
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Overlap(Item item)
    {
        int remaining = maxCount - count;
        if(item.count <= remaining)
        {
            count += item.count;
            item.count = 0;
            return true;
        }
        else
        {            
            count = maxCount;
            item.count = Mathf.Clamp(item.count - remaining, 0, item.maxCount);
            return false;
        }
    }

    public static string CreateID(ITEMTYPE type, EQUIP_TYPE equipType, int number)
    {
        string itemCode = type.ToString().Substring(0, 2);
        string typeCode = equipType.ToString().Substring(0, 2);
        string numbering = number.ToString("0000");
        return string.Concat(itemCode, typeCode, numbering);
    }
    public static string CreateID(ITEMTYPE type, int number)
    {
        if (type == ITEMTYPE.EQUIPMENT)
        {
            Debug.Log("EquipItem의 ID는 해당 함수를 이용해 만들 수 없습니다.");
            return string.Empty;
        }

        string itemCode = type.ToString().Substring(0, 2);
        string typeCode = type switch
        {
            ITEMTYPE.USEABLE => "US",
            ITEMTYPE.MATERIAL => "MA",
            _ => "NN",
        };
        string numbering = number.ToString("0000");
        return string.Concat(itemCode, typeCode, numbering);
    }
}




