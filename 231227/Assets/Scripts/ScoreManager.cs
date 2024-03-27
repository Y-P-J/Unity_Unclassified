using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }   //�̱���

    List<int> scoreList;                                        //�������

    int lastScore;                                              //�����ֱٱ��
    bool isNewRecord;                                           //��ϰ��ſ���

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
    /// ���� ��� ����
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
        }//��������� 10�������� ����

        PlayerPrefs.SetString("Scores", string.Join(',', scoreList));
        isNewRecord = lastScore > bestScore;
        return isNewRecord;
    }

    /// <summary>
    /// �÷��� ���� �ֱٱ�� ����
    /// </summary>
    /// <param name="score"></param>
    public void SetCurrentScore(int score)
    {
        lastScore = score;
    }
    
}