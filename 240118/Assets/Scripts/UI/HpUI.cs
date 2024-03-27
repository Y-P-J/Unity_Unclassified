using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    public void SwitchToggle()
    {
        toggle.interactable = !toggle.interactable;   
    }
}
