using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] SpriteRenderer flash;      //��������Ʈ ����
    [SerializeField] float flashTime;           //��½�̴� �ð�
    [SerializeField] NumberUI currentScore;     //���� ���
    [SerializeField] NumberUI bestScore;        //�ְ� ���

    void Start()
    {
        StartCoroutine(IEFlash());
    }

    void Update()
    {
        
    }

    /// <summary>
    /// ���ӿ��� �� ª�� ��½�̴� IEnumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator IEFlash()
    {
        flash.gameObject.SetActive(true);

        float time = flashTime;

        while (time > 0.0f)
        {
            flash.color = new Color(1.0f, 1.0f, 1.0f, time / flashTime);
            time = Mathf.Clamp(time - Time.deltaTime, 0.0f, flashTime);
            yield return null;
        }

        currentScore.SetNumber(ScoreManager.instance.LastScore);
        bestScore.SetNumber(ScoreManager.instance.BestScore);

        flash.gameObject.SetActive(false);
    }

    public void OnReturnMenu()
    {
        SceneManager.LoadScene("Main");
    }
}
