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
            player.gameObject.SetActive(false);//�÷��̾� ��Ȱ��
            Invoke(nameof(ReloadScene), 1.5f);//1.5�ʵڿ� ReloadScene �����
        }
    }

    /// <summary>
    /// �� �����
    /// </summary>
    void ReloadScene()
    {
        SceneHandler.LoadScene(SceneHandler.ID.Game);
    }
}
