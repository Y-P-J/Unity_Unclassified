using System;
using System.IO;
using UnityEngine;

// 직렬화 : 유니티 컴포넌트로 연결할 수 있도록 처리를 한다.
[System.Serializable]
public struct NoteData
{
    public int milliSec;    // 시간 값.
    public int line;        // 라인 번호.
    public int type;        // 노트 타입.
}
[Serializable]
public class MusicData
{
    public string id;           // 파일명
    public string title;        // 음악명
    public string artist;       // 가수명
    public int bpm;             // 분당 박자 수.
    public int signature;       // 시그니처 (3/4, 4/4)
    public NoteData[] notes;    // 노트 데이터

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

    public readonly float BEAT_HEIGHT = 8;       // 1박자의 기본 높이.
    public float BeatHeight => BEAT_HEIGHT * (60f / musicData.bpm) * Setting.noteSpeed;

    public AudioSource audioSource;
    public MusicData musicData;

    public int totalMS;         // 음악의 총 길이.
    public int beatPerMS;       // 1박자당 밀리초.

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
        // 높이 / 기준 높이 / 속도 * 1000 = 시간 값
        return Mathf.RoundToInt(height / Instance.BEAT_HEIGHT / Setting.noteSpeed * 1000);
    }
    public static float MilliSecToHeight(int milliSec)
    {
        // 시간 * 기준 높이 * 속도 / 1000 = 높이 값
        return milliSec * Instance.BEAT_HEIGHT * Setting.noteSpeed / 1000f;
    }
}

