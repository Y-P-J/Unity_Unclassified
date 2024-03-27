using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Setting
{
    // 유저의 설정 값. (단축키, 속도, 해상도 등등..)
    public static float noteSpeed;
    public static int snapCount;

    // static 생성자 : 프로그램 시작 시 호출 된다.
    static Setting()
    {
        noteSpeed = 1f;
        snapCount = 1;
    }
}
