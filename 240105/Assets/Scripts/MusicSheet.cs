using System;
using System.IO;
using UnityEngine;

// ����ȭ : ����Ƽ ������Ʈ�� ������ �� �ֵ��� ó���� �Ѵ�.
[System.Serializable]
public struct NoteData
{
    public int milliSec;    // �ð� ��.
    public int line;        // ���� ��ȣ.
    public int type;        // ��Ʈ Ÿ��.
}
[Serializable]
public class MusicData
{
    public string id;           // ���ϸ�
    public string title;        // ���Ǹ�
    public string artist;       // ������
    public int bpm;             // �д� ���� ��.
    public int signature;       // �ñ״�ó (3/4, 4/4)
    public NoteData[] notes;    // ��Ʈ ������

    public MusicData(string id)
    {
        this.id = id;
        title = string.Empty;
        artist = string.Empty;
        bpm = 60;
        signature = 4;
        notes = new NoteData[0];
    }
}


public class MusicSheet : MonoBehaviour
{
    public static MusicSheet Instance { get; private set; }

    public readonly float BEAT_HEIGHT = 8;       // 1������ �⺻ ����.
    public float BeatHeight => BEAT_HEIGHT * (60f / musicData.bpm) * Setting.noteSpeed;

    public AudioSource audioSource;
    public MusicData musicData;

    public int totalMS;         // ������ �� ����.
    public int beatPerMS;       // 1���ڴ� �и���.

    private void Awake()    
    {
        Instance = this;
    }

    public void Save()
    {
        string root = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Music");
        string path = Path.Combine(root, musicData.id, $"{musicData.id}.txt");
        using (StreamWriter sw = new StreamWriter(path))
        {
            string json = JsonUtility.ToJson(musicData);
            sw.Write(json);
        }
        Debug.Log("Saved file : " + musicData.id);
    }
    

    public void Calculate()
    {
        totalMS = Mathf.RoundToInt(audioSource.clip.length * 1000);
        beatPerMS = Mathf.RoundToInt((60f / musicData.bpm) * 1000);
    }

    public static int HeightToMilliSec(float height)
    {
        // ���� / ���� ���� / �ӵ� * 1000 = �ð� ��
        return Mathf.RoundToInt(height / Instance.BEAT_HEIGHT / Setting.noteSpeed * 1000);
    }
    public static float MilliSecToHeight(int milliSec)
    {
        // �ð� * ���� ���� * �ӵ� / 1000 = ���� ��
        return milliSec * Instance.BEAT_HEIGHT * Setting.noteSpeed / 1000f;
    }
}

