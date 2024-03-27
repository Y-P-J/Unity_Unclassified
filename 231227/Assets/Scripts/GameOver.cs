using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] SpriteRenderer flash;      //스프라이트 지정
    [SerializeField] float flashTime;           //번쩍이는 시간
    [SerializeField] NumberUI currentScore;     //현재 기록
    [SerializeField] NumberUI bestScore;        //최고 기록

    void Start()
    {
        StartCoroutine(IEFlash());
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 게임오버 시 짧게 번쩍이는 IEnumerator
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
