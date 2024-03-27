using SFB;
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class MusicBundle
{
    public string id;
    public AudioClip audioClip;
    public Sprite sprite;
    public string json;
}

public class FileBrowser : MonoBehaviour
{
    // 싱글톤을 이용해 외부에서 쉽게 접근이 가능하도록 설정.
    public static FileBrowser Instance { get; private set; }
    private MusicBundle bundle;
        
    private void Awake()
    {
        Instance = this;
    }
         
    // 음악 번들을 전달하는 함수.
    public void GetMusicBundle(Action<MusicBundle> callback)
    {
        string root = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Music");           // root 경로.
        string[] paths = StandaloneFileBrowser.OpenFolderPanel("Select Music", root, false);        // 외부 Library 파일 브라우저 실행.
        if(paths.Length > 0)                                                                        // 선택한 폴더가 있다면.
            StartCoroutine(IEGetMusicBundle(paths[0], callback));                                   // 코루틴 실행.
    }

    // 통합 관리 코루틴.
    private IEnumerator IEGetMusicBundle(string directory, Action<MusicBundle> callback)
    {
        bundle = new MusicBundle();                                     // 번들 생성.
        bundle.id = Path.GetFileName(directory);                        // 폴더 이름을 ID로 대입.
        string[] paths = Directory.GetFiles(directory);                 // Directory내부 파일 경로 검색.
        foreach (string url in paths)
        {
            // 파일의 확장자를 통해 Request 코루틴 실행.
            switch (Path.GetExtension(url))
            {
                case ".ogg":
                case ".wav":
                case ".mp3":
                    yield return StartCoroutine(DownloadAudio(url));
                    break;
                case ".png":
                case ".jpg":
                case ".jpeg":
                    yield return StartCoroutine(DownloadSprite(url));
                    break;
                case ".txt":
                    yield return StartCoroutine(DownloadText(url));
                    break;
            }
        }
                
        callback?.Invoke(bundle);       // 검색이 끝났다면 이벤트를 통해 외부로 전달.
        bundle = null;                  // 번들 참조 해제.
    }
    private IEnumerator DownloadSprite(string url)
    {
        // UnityWebRequest를 통해 로컬(=내 컴퓨터)의 파일을 읽어온다.
        // UnityWebRequestTexture는 이미지 파일을 읽을 수 있다.
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        
        yield return request.SendWebRequest();                                      // 로드가 끝날 때까지 대기.
        Texture2D texture2D = DownloadHandlerTexture.GetContent(request);           // Handler를 통해 바이너리 데이터를 Texture로 변환.

        Rect rect = new Rect(0f, 0f, texture2D.width, texture2D.height);            // Texture의 Rect 정보.
        Sprite sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));    // Texture를 Sprite로 컨버트.
        bundle.sprite = sprite;                                                     // 번들 내부에 대입.
    }
    private IEnumerator DownloadAudio(string url)
    {
        // url에 포함된 음원의 확장자명을 통해 오디오 형식 판별.
        AudioType audioType = Path.GetExtension(url) switch
        {
            ".mp3" => AudioType.MPEG,
            ".wav" => AudioType.WAV,
            ".ogg" => AudioType.OGGVORBIS,
            _ => 0,
        };

        // UnityWebRequestMultimedia는 오디오, 비디오 형식의 파일을 읽을 수 있다.
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, audioType);

        yield return request.SendWebRequest();                              // 로드가 끝날 때까지 대기.
        AudioClip clip = DownloadHandlerAudioClip.GetContent(request);      // Handler를 통해 바이너리 데이터를 Audio형식으로 변환.
        bundle.audioClip = clip;                                            // 번들 내부에 대입.
        
    }
    private IEnumerator DownloadText(string url)
    {
        // 텍스트 파일 읽기.
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();
        bundle.json = request.downloadHandler.text;
    }
}
