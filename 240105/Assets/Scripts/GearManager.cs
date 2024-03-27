using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GearManager : MonoBehaviour
{
    [System.Serializable]
    public class KeyLine
    {
        [SerializeField] KeyCode inputKey;
        [SerializeField] GameObject beam;
        [SerializeField] SpriteRenderer button;
        [SerializeField] TMP_Text keyText;

        public void Start()
        {
            keyText.text = inputKey.ToString();
        }
        public void Update()
        {
            bool isPress = Input.GetKey(inputKey);
            beam.SetActive(isPress);
            button.color = isPress ? new Color(1f, 1f, 1f, 0.8f) : new Color(0f, 0f, 0f, 0.8f);
        }
    }

    public Transform movePivot;       // 움직임 제어 오브젝트.
    public NoteObject notePrefab;     // 노트 프리팹.
    public Transform noteParent;      // 노트가 생성될 부모 오브젝트.
    public KeyLine[] keyLines;        // 키 라인 배열.

    // HideInspector : 인스펙터 창에서 해당 변수의 노출을 막는다.
    [HideInInspector] public MusicData currentMusic;        // 현재 음악 정보.
    [HideInInspector] public List<NoteObject> noteList;     // 생성된 노트 리스트.

    private void Start()
    {
        noteList = new List<NoteObject>();
        foreach (KeyLine keyLine in keyLines)
            keyLine.Start();
    }
    private void Update()
    {
        // 오브젝트가 있는데 S를 누르고 있으면 켜고 아니면 끈다.
        // GetKey는 버튼을 누르고 있으면 true고 아니면 false를 준다.
        // GameObject.SetActive(bool) : 오브젝트를 끄고 켠다.

        // GetKey : 누르고 있는 동안 true
        // GetKeyDown : 누르는 순간 true
        // GetKeyUp : 떼는 순간 true
        foreach (KeyLine keyLine in keyLines)
            keyLine.Update();

        if (MusicSheet.Instance.audioSource.isPlaying)
            movePivot.Translate(Vector3.down * MusicSheet.Instance.BEAT_HEIGHT * Time.deltaTime);
    }

    public void InitGearManager()
    {
        // 이전에 생성된 노트 삭제.
        foreach (NoteObject note in noteList)
            Destroy(note.gameObject);
        noteList.Clear();

        // 시트에서 데이터를 로그해 새로운 노트 그룹 생성.
        foreach (NoteData note in MusicSheet.Instance.musicData.notes)
            CreateNote(note);
    }
    public void CreateNote(NoteData data)
    {
        NoteObject newNote = Instantiate(notePrefab, noteParent);       // 노트 생성.
        newNote.data = data;
        newNote.UpdatePosition();           // 위치 갱신.
        noteList.Add(newNote);              // 리스트 대입.

        // 노트가 추가되면 리스트를 정렬 후 시트에 기입.
        NoteData[] noteDatas = noteList.Select(n => n.data).OrderBy(n => n.milliSec).ToArray();
        MusicSheet.Instance.musicData.notes = noteDatas;
    }
    public void RemoveNote(NoteObject target)
    {
        noteList.Remove(target);
        Destroy(target.gameObject);

        // 노트가 추가되면 리스트를 정렬 후 시트에 기입.
        NoteData[] noteDatas = noteList.Select(n => n.data).OrderBy(n => n.milliSec).ToArray();
        MusicSheet.Instance.musicData.notes = noteDatas;
    }
}

