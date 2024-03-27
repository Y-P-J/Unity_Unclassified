using SFB;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] GearManager gear;          // ��� ������.
    [SerializeField] MusicInfoPanel infoPanel;  // ���� �г�.
    [SerializeField] GameObject gridObject;     // ���� ������Ʈ.

    [Header("Note")]
    [SerializeField] Transform previewNote;     // �̸����� ��Ʈ.
    [SerializeField] Transform movePivot;       // �����Ϳ� movePivot ��ġ.

    [Header("Grid")]
    [SerializeField] Transform gridParent;      // Grid�� �θ� ������Ʈ.
    [SerializeField] GridObject gridPrefab;     // Grid ������.

    GridObject[] grids;                         // ������ Grid ������Ʈ �迭.
    Vector3 mouseWorldPos;                      // ���콺�� ���� ��ǥ�� ��ġ��.
    bool isMouseInsideGear;                     // ���콺�� ��� �ȿ� �ִ°�?
    bool isSetGrid;                             // ������ �����°�?

    private void Start()
    {
        grids = new GridObject[0];
        gridObject.SetActive(false);
        infoPanel.gameObject.SetActive(false);

        FileBrowser.Instance.GetMusicBundle(LoadNewBundle);
    }
    private void Update()
    {
        if (!isSetGrid)
            return;

        // ���콺�� ��ġ�� ���� ��ǥ��� ��ȯ�� �� ��� ���ο� �ִ��� Ȯ���Ѵ�.
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isMouseInsideGear = mouseWorldPos.x >= -2f && mouseWorldPos.x <= 2f;
        
        HotKey();
        NoteGenerate();
    }
    private void HotKey()
    {
        // 1.���콺 ���� �̿��� Grid�� ��,�Ʒ��� ������ �� �ִ�.
        // 2.Ctrl�� ���� ������ Snap count�� ������ �� �ִ�. (�ִ� 1/8)
        // 3.Space bar�� ���� ������ ���, �Ͻ�����, ���� �� �� �ִ�.

        // Input.mousePosition : ��ũ�� ��ǥ�� �� ���콺�� ��ġ ��.
        // ScreenToWorldPoint : ��ũ�� ��ǥ�� ��ġ ���� ���� ��ǥ�� ��ġ ������ ��ȯ.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float wheel = Input.GetAxis("Mouse ScrollWheel") * 10f;             // -1, 0, 1�� ��ȯ�Ѵ�.

        // CtrlŰ�� ������ �ִ� ���� true.
        if (Input.GetKey(KeyCode.LeftControl) && wheel != 0)
        {
            int amount = (wheel > 0) ? 1 : -1;
            Setting.snapCount = Mathf.Clamp(Setting.snapCount + amount, 1, 8);
            UpdateGridObject();
        }
        else if (mousePos.x > -2f && mousePos.x < 2f && wheel != 0f)
        {
            float movement = MusicSheet.Instance.BeatHeight / 4;            // �̵��� ���.
            gear.movePivot.Translate(Vector3.down * movement * wheel);          // ������Ʈ �̵�.

            // ������Ʈ�� �������� ������ ���� �ּ� ���� 0�� �Ǿ�� �Ѵ�.
            if (gear.movePivot.localPosition.y > 0f)
                gear.movePivot.localPosition = Vector3.zero;

            // �� ������Ʈ�� ��ġ�� �����ؾ��Ѵ�.
            movePivot.transform.position = gear.movePivot.position;
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            if (MusicSheet.Instance.audioSource.isPlaying)
                MusicSheet.Instance.audioSource.Pause();
            else
                MusicSheet.Instance.audioSource.Play();
        }
    }
    private void NoteGenerate()
    {
        // ��Ʈ�� �����ϴ� �Լ�.
        Vector3 previewWorldPos = mouseWorldPos;
        previewWorldPos.z = 0f;
        previewNote.position = previewWorldPos;        
        previewNote.gameObject.SetActive(isMouseInsideGear);
        if (!isMouseInsideGear)
            return;

        // ����ī�޶� ���� ���콺 ��ġ�� Ray�� �����Ѵ�.
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector3.forward);
        if (!hit.collider || hit.collider.tag != "Snap")
            return;

        // ���� ���콺�� Snap���� �����Ѵ�.
        previewWorldPos.x = Mathf.Floor(mouseWorldPos.x) + 0.5f;
        previewWorldPos.y = hit.collider.transform.position.y;
        previewNote.position = previewWorldPos;

        // ���� ��ǥ�� previewWorldPos�� ���� noteParent�� ���� ��ǥ��� ��ȯ�Ѵ�.
        Vector3 noteLocalPos = gear.noteParent.InverseTransformPoint(previewWorldPos);
        int noteMilliSec = MusicSheet.HeightToMilliSec(noteLocalPos.y);
        int noteLine = Mathf.FloorToInt(mouseWorldPos.x) + 2;

        // ���콺 ������ ���� ��Ʈ�� ��ġ�Ǿ��ִ�.
        NoteObject hoverNote = gear.noteList.Find(note => note.data.milliSec == noteMilliSec && note.data.line == noteLine);
        if (hoverNote != null)
        {
            previewNote.gameObject.SetActive(false);

            // ���콺 ������ Ŭ�� �� ��ġ�� ��Ʈ ����.
            if(Input.GetMouseButtonDown(1))
                gear.RemoveNote(hoverNote);

            return;
        }

        // ��Ʈ�� ��ġ�ϰڴ�.
        if (Input.GetMouseButtonDown(0))
        {
            NoteData data = new NoteData
            {
                milliSec = noteMilliSec,        // y��(=����)�� ���� �ð� �� ���.
                line = noteLine,                // ���� �Է�.
                type = 0                        // Ÿ�� �Է�.
            };
            gear.CreateNote(data);
        }
    }
  
    private void UpdateGridObject()
    {
        int index = 0;
        foreach (GridObject grid in grids)
        {
            grid.Setup();
            grid.transform.localPosition = Vector3.up * MusicSheet.Instance.BeatHeight * index;
            index++;
        }
    }


    // ���ο� ������ �ε�
    private void LoadNewBundle(MusicBundle bundle)
    {
        MusicSheet.Instance.audioSource.clip = bundle.audioClip;      // Bundle�� �ִ� �̹����� ����� ����.

        if (string.IsNullOrEmpty(bundle.json))                  // ���ο� ������ �����ߴ�
        {
            string id = bundle.id;
            MusicSheet.Instance.musicData = new MusicData(id);  // ID�� ������� �� ���ο� ���� ���� ��ü ����.
            infoPanel.ShowInfoPanel(Generate);                  // ���� �г��� �̿��� ���ο� ���� ����.
        }
        else
        {
            // Json�� �̿��ؼ� ������ ��ȣȭ. ��Ʈ�� ����.        
            MusicData data = JsonUtility.FromJson<MusicData>(bundle.json);
            MusicSheet.Instance.musicData = data;
            Generate();                                 // ������ ����� ������ �����ߴ�.
        }
    }
    private void Generate()
    {
        // ���� �Էµ� ���� �̿��� ��ġ �� ���.
        MusicSheet.Instance.Calculate();

        // ���� �׸��� ����.
        foreach (GridObject grid in grids)
            Destroy(grid.gameObject);

        // ��ü ���� / 1���� ���� => �ø�
        int gridCount = Mathf.CeilToInt(MusicSheet.Instance.totalMS / (float)MusicSheet.Instance.beatPerMS);
        grids = new GridObject[gridCount];
        for (int i = 0; i < grids.Length; i++)
            grids[i] = Instantiate(gridPrefab, gridParent);

        UpdateGridObject();                 // �׸��� ������Ʈ ��ġ ������Ʈ.
        gridObject.SetActive(true);         // �׸��� ������Ʈ Ȱ��ȭ.

        gear.gameObject.SetActive(true);    // ��� �Ŵ��� Ȱ��ȭ.
        gear.InitGearManager();             // ��� �ŴϾ� �ʱ�ȭ.

        isSetGrid = true;
    }
    
    public void SaveSheet()
    {
        MusicSheet.Instance.Save();
    }
    public void LoadSheet()
    {
        Generate();
    }

}
