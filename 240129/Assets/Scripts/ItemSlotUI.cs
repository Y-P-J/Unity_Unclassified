using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image itemIcon;
    [SerializeField] Image hoverImage;
    [SerializeField] TMP_Text countText;

    public Item item { get; private set; }

    public void ChangeHover(bool isOn) => hoverImage.enabled = isOn;

    void Start()
    {
        hoverImage.enabled = false;
    }

    public void UpdateItem(Item item)
    {
        this.item = item;
        if (item == null)
        {
            itemIcon.enabled = false;
            countText.enabled = false;
        }
        else
        {
            itemIcon.enabled = true;
            itemIcon.sprite = item.sprite;
            countText.enabled = item.count > 1;
            countText.text = item.count.ToString();
        }
    }

    public void ResetParentEnable()
    {
        if (this.item == null)
        {
            itemIcon.enabled = false;
            countText.enabled = false;
        }
        else
        {
            itemIcon.enabled = true;
            countText.enabled = item.count > 1;
        }

        hoverImage.enabled = false;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        hoverImage.enabled = true;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        hoverImage.enabled = false;
    }
}
