using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public enum ID  //씬 관리용 enum
    {
        Title,
        Main,
        Game,
        Credit,
    }

    public static ID current { get; private set; }  //현재 무슨씬인지 확인용

    static SceneHandler()
    {
        Scene scene = SceneManager.GetActiveScene();//지금 씬 참조
        ID id = 0;//임시 enum 변수

        System.Enum.TryParse<ID>(scene.name, out id);//해당씬이 어떤 enum인지

        current = id;//현재씬 갱신
    }

    /// <summary>
    /// 씬 불러오기
    /// </summary>
    /// <param name="id"></param>
    public static void LoadScene(ID id)
    {
        current = id;//현재씬 갱신
        SceneManager.LoadScene(id.ToString());//씬 불러오기
    }
}
