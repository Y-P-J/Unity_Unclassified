using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Setting
{
    // ������ ���� ��. (����Ű, �ӵ�, �ػ� ���..)
    public static float noteSpeed;
    public static int snapCount;

    // static ������ : ���α׷� ���� �� ȣ�� �ȴ�.
    static Setting()
    {
        noteSpeed = 1f;
        snapCount = 1;
    }
}
