using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }   //싱글톤

    List<int> scoreList;                                        //상위기록

    int lastScore;                                              //가장최근기록
    bool isNewRecord;                                           //기록갱신여부

    public int BestScore => scoreList.Max();
    public int LastScore => lastScore;
    public bool IsNewRecord => isNewRecord;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreList = new List<int>();
        lastScore = 0;

        string scoreData = PlayerPrefs.GetString("Scores", string.Join(',', Enumerable.Repeat(0, 10)));
        scoreList.AddRange(scoreData.Split(',').Select(x => int.Parse(x)));
    }

    void Update()
    {

    }

    /// <summary>
    /// 상위 기록 갱신
    /// </summary>
    /// <returns></returns>
    public bool RecordScore()
    {
        int bestScore = scoreList.Max();

        scoreList.Add(lastScore);
        scoreList = scoreList.OrderByDescending(x => x).ToList();

        if(scoreList.Count > 10)
        {
            scoreList.RemoveAt(10);
        }//상위기록은 10개까지만 유지

        PlayerPrefs.SetString("Scores", string.Join(',', scoreList));
        isNewRecord = lastScore > bestScore;
        return isNewRecord;
    }

    /// <summary>
    /// 플레이 중인 최근기록 갱신
    /// </summary>
    /// <param name="score"></param>
    public void SetCurrentScore(int score)
    {
        lastScore = score;
    }
    
}