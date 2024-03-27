using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if(player)
        {
            player.gameObject.SetActive(false);//플레이어 비활성
            Invoke(nameof(ReloadScene), 1.5f);//1.5초뒤에 ReloadScene 실행됨
        }
    }

    /// <summary>
    /// 씬 재실행
    /// </summary>
    void ReloadScene()
    {
        SceneHandler.LoadScene(SceneHandler.ID.Game);
    }
}
