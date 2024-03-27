using SFB;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] GearManager gear;          // 기어 관리자.
    [SerializeField] MusicInfoPanel infoPanel;  // 정보 패널.
    [SerializeField] GameObject gridObject;     // 격자 오브젝트.

    [Header("Note")]
    [SerializeField] Transform previewNote;     // 미리보기 노트.
    [SerializeField] Transform movePivot;       // 에디터용 movePivot 위치.

    [Header("Grid")]
    [SerializeField] Transform gridParent;      // Grid의 부모 오브젝트.
    [SerializeField] GridObject gridPrefab;     // Grid 프리팹.

    GridObject[] grids;                         // 생성된 Grid 오브젝트 배열.
    Vector3 mouseWorldPos;                      // 마우스의 월드 좌표계 위치값.
    bool isMouseInsideGear;                     // 마우스가 기어 안에 있는가?
    bool isSetGrid;                             // 세팅이 끝났는가?

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

        // 마우스의 위치를 월드 좌표계로 변환한 뒤 기어 내부에 있는지 확인한다.
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isMouseInsideGear = mouseWorldPos.x >= -2f && mouseWorldPos.x <= 2f;
        
        HotKey();
        NoteGenerate();
    }
    private void HotKey()
    {
        // 1.마우스 휠을 이용해 Grid를 위,아래로 움직일 수 있다.
        // 2.Ctrl과 같이 누르면 Snap count를 조절할 수 있다. (최대 1/8)
        // 3.Space bar를 통해 음악을 재생, 일시정지, 중지 할 수 있다.

        // Input.mousePosition : 스크린 좌표계 상 마우스의 위치 값.
        // ScreenToWorldPoint : 스크린 좌표계 위치 값을 월드 좌표계 위치 값으로 변환.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float wheel = Input.GetAxis("Mouse ScrollWheel") * 10f;             // -1, 0, 1을 반환한다.

        // Ctrl키를 누르고 있는 동안 true.
        if (Input.GetKey(KeyCode.LeftControl) && wheel != 0)
        {
            int amount = (wheel > 0) ? 1 : -1;
            Setting.snapCount = Mathf.Clamp(Setting.snapCount + amount, 1, 8);
            UpdateGridObject();
        }
        else if (mousePos.x > -2f && mousePos.x < 2f && wheel != 0f)
        {
            float movement = MusicSheet.Instance.BeatHeight / 4;            // 이동량 계산.
            gear.movePivot.Translate(Vector3.down * movement * wheel);          // 오브젝트 이동.

            // 오브젝트는 내려가기 때문에 가장 최소 값은 0이 되어야 한다.
            if (gear.movePivot.localPosition.y > 0f)
                gear.movePivot.localPosition = Vector3.zero;

            // 두 오브젝트는 위치가 동일해야한다.
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
        // 노트를 생성하는 함수.
        Vector3 previewWorldPos = mouseWorldPos;
        previewWorldPos.z = 0f;
        previewNote.position = previewWorldPos;        
        previewNote.gameObject.SetActive(isMouseInsideGear);
        if (!isMouseInsideGear)
            return;

        // 메인카메라를 통해 마우스 위치에 Ray를 생성한다.
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector3.forward);
        if (!hit.collider || hit.collider.tag != "Snap")
            return;

        // 현재 마우스가 Snap위에 존재한다.
        previewWorldPos.x = Mathf.Floor(mouseWorldPos.x) + 0.5f;
        previewWorldPos.y = hit.collider.transform.position.y;
        previewNote.position = previewWorldPos;

        // 월드 좌표인 previewWorldPos의 값을 noteParent의 로컬 좌표계로 변환한다.
        Vector3 noteLocalPos = gear.noteParent.InverseTransformPoint(previewWorldPos);
        int noteMilliSec = MusicSheet.HeightToMilliSec(noteLocalPos.y);
        int noteLine = Mathf.FloorToInt(mouseWorldPos.x) + 2;

        // 마우스 포인터 위에 노트가 설치되어있다.
        NoteObject hoverNote = gear.noteList.Find(note => note.data.milliSec == noteMilliSec && note.data.line == noteLine);
        if (hoverNote != null)
        {
            previewNote.gameObject.SetActive(false);

            // 마우스 오른쪽 클릭 시 설치된 노트 삭제.
            if(Input.GetMouseButtonDown(1))
                gear.RemoveNote(hoverNote);

            return;
        }

        // 노트를 설치하겠다.
        if (Input.GetMouseButtonDown(0))
        {
            NoteData data = new NoteData
            {
                milliSec = noteMilliSec,        // y축(=높이)에 따른 시간 값 계산.
                line = noteLine,                // 라인 입력.
                type = 0                        // 타입 입력.
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


    // 새로운 번들을 로드
    private void LoadNewBundle(MusicBundle bundle)
    {
        MusicSheet.Instance.audioSource.clip = bundle.audioClip;      // Bundle에 있는 이미지와 오디오 세팅.

        if (string.IsNullOrEmpty(bundle.json))                  // 새로운 폴더를 선택했다
        {
            string id = bundle.id;
            MusicSheet.Instance.musicData = new MusicData(id);  // ID를 기반으로 한 새로운 음악 정보 객체 생성.
            infoPanel.ShowInfoPanel(Generate);                  // 정보 패널을 이용해 새로운 정보 기입.
        }
        else
        {
            // Json을 이용해서 데이터 복호화. 시트에 전달.        
            MusicData data = JsonUtility.FromJson<MusicData>(bundle.json);
            MusicSheet.Instance.musicData = data;
            Generate();                                 // 이전에 등록한 폴더를 선택했다.
        }
    }
    private void Generate()
    {
        // 현재 입력된 값을 이용해 수치 값 계산.
        MusicSheet.Instance.Calculate();

        // 이전 그리드 삭제.
        foreach (GridObject grid in grids)
            Destroy(grid.gameObject);

        // 전체 길이 / 1박자 길이 => 올림
        int gridCount = Mathf.CeilToInt(MusicSheet.Instance.totalMS / (float)MusicSheet.Instance.beatPerMS);
        grids = new GridObject[gridCount];
        for (int i = 0; i < grids.Length; i++)
            grids[i] = Instantiate(gridPrefab, gridParent);

        UpdateGridObject();                 // 그리드 오브젝트 위치 업데이트.
        gridObject.SetActive(true);         // 그리드 오브젝트 활성화.

        gear.gameObject.SetActive(true);    // 기어 매니저 활성화.
        gear.InitGearManager();             // 기어 매니어 초기화.

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
