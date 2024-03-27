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
    // �̱����� �̿��� �ܺο��� ���� ������ �����ϵ��� ����.
    public static FileBrowser Instance { get; private set; }
    private MusicBundle bundle;
        
    private void Awake()
    {
        Instance = this;
    }
         
    // ���� ������ �����ϴ� �Լ�.
    public void GetMusicBundle(Action<MusicBundle> callback)
    {
        string root = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Music");           // root ���.
        string[] paths = StandaloneFileBrowser.OpenFolderPanel("Select Music", root, false);        // �ܺ� Library ���� ������ ����.
        if(paths.Length > 0)                                                                        // ������ ������ �ִٸ�.
            StartCoroutine(IEGetMusicBundle(paths[0], callback));                                   // �ڷ�ƾ ����.
    }

    // ���� ���� �ڷ�ƾ.
    private IEnumerator IEGetMusicBundle(string directory, Action<MusicBundle> callback)
    {
        bundle = new MusicBundle();                                     // ���� ����.
        bundle.id = Path.GetFileName(directory);                        // ���� �̸��� ID�� ����.
        string[] paths = Directory.GetFiles(directory);                 // Directory���� ���� ��� �˻�.
        foreach (string url in paths)
        {
            // ������ Ȯ���ڸ� ���� Request �ڷ�ƾ ����.
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
                
        callback?.Invoke(bundle);       // �˻��� �����ٸ� �̺�Ʈ�� ���� �ܺη� ����.
        bundle = null;                  // ���� ���� ����.
    }
    private IEnumerator DownloadSprite(string url)
    {
        // UnityWebRequest�� ���� ����(=�� ��ǻ��)�� ������ �о�´�.
        // UnityWebRequestTexture�� �̹��� ������ ���� �� �ִ�.
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        
        yield return request.SendWebRequest();                                      // �ε尡 ���� ������ ���.
        Texture2D texture2D = DownloadHandlerTexture.GetContent(request);           // Handler�� ���� ���̳ʸ� �����͸� Texture�� ��ȯ.

        Rect rect = new Rect(0f, 0f, texture2D.width, texture2D.height);            // Texture�� Rect ����.
        Sprite sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));    // Texture�� Sprite�� ����Ʈ.
        bundle.sprite = sprite;                                                     // ���� ���ο� ����.
    }
    private IEnumerator DownloadAudio(string url)
    {
        // url�� ���Ե� ������ Ȯ���ڸ��� ���� ����� ���� �Ǻ�.
        AudioType audioType = Path.GetExtension(url) switch
        {
            ".mp3" => AudioType.MPEG,
            ".wav" => AudioType.WAV,
            ".ogg" => AudioType.OGGVORBIS,
            _ => 0,
        };

        // UnityWebRequestMultimedia�� �����, ���� ������ ������ ���� �� �ִ�.
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, audioType);

        yield return request.SendWebRequest();                              // �ε尡 ���� ������ ���.
        AudioClip clip = DownloadHandlerAudioClip.GetContent(request);      // Handler�� ���� ���̳ʸ� �����͸� Audio�������� ��ȯ.
        bundle.audioClip = clip;                                            // ���� ���ο� ����.
        
    }
    private IEnumerator DownloadText(string url)
    {
        // �ؽ�Ʈ ���� �б�.
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();
        bundle.json = request.downloadHandler.text;
    }
}
