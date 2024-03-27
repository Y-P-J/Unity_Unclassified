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

    public Transform movePivot;       // ������ ���� ������Ʈ.
    public NoteObject notePrefab;     // ��Ʈ ������.
    public Transform noteParent;      // ��Ʈ�� ������ �θ� ������Ʈ.
    public KeyLine[] keyLines;        // Ű ���� �迭.

    // HideInspector : �ν����� â���� �ش� ������ ������ ���´�.
    [HideInInspector] public MusicData currentMusic;        // ���� ���� ����.
    [HideInInspector] public List<NoteObject> noteList;     // ������ ��Ʈ ����Ʈ.

    private void Start()
    {
        noteList = new List<NoteObject>();
        foreach (KeyLine keyLine in keyLines)
            keyLine.Start();
    }
    private void Update()
    {
        // ������Ʈ�� �ִµ� S�� ������ ������ �Ѱ� �ƴϸ� ����.
        // GetKey�� ��ư�� ������ ������ true�� �ƴϸ� false�� �ش�.
        // GameObject.SetActive(bool) : ������Ʈ�� ���� �Ҵ�.

        // GetKey : ������ �ִ� ���� true
        // GetKeyDown : ������ ���� true
        // GetKeyUp : ���� ���� true
        foreach (KeyLine keyLine in keyLines)
            keyLine.Update();

        if (MusicSheet.Instance.audioSource.isPlaying)
            movePivot.Translate(Vector3.down * MusicSheet.Instance.BEAT_HEIGHT * Time.deltaTime);
    }

    public void InitGearManager()
    {
        // ������ ������ ��Ʈ ����.
        foreach (NoteObject note in noteList)
            Destroy(note.gameObject);
        noteList.Clear();

        // ��Ʈ���� �����͸� �α��� ���ο� ��Ʈ �׷� ����.
        foreach (NoteData note in MusicSheet.Instance.musicData.notes)
            CreateNote(note);
    }
    public void CreateNote(NoteData data)
    {
        NoteObject newNote = Instantiate(notePrefab, noteParent);       // ��Ʈ ����.
        newNote.data = data;
        newNote.UpdatePosition();           // ��ġ ����.
        noteList.Add(newNote);              // ����Ʈ ����.

        // ��Ʈ�� �߰��Ǹ� ����Ʈ�� ���� �� ��Ʈ�� ����.
        NoteData[] noteDatas = noteList.Select(n => n.data).OrderBy(n => n.milliSec).ToArray();
        MusicSheet.Instance.musicData.notes = noteDatas;
    }
    public void RemoveNote(NoteObject target)
    {
        noteList.Remove(target);
        Destroy(target.gameObject);

        // ��Ʈ�� �߰��Ǹ� ����Ʈ�� ���� �� ��Ʈ�� ����.
        NoteData[] noteDatas = noteList.Select(n => n.data).OrderBy(n => n.milliSec).ToArray();
        MusicSheet.Instance.musicData.notes = noteDatas;
    }
}

