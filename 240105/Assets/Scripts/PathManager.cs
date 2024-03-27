using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [System.Serializable]
    struct SampleData
    {
        public string userName;
        public int level;
        public long money;
        public float hp;
        public string[] inven;

        public SampleData(string userName, int level, int money, float hp, params string[] inven)
        {
            this.userName = userName;
            this.level = level;
            this.money = money;
            this.hp = hp;
            this.inven = inven;
        }
    }

    [SerializeField] string path;

    public string RootDirectory
    {
        get
        {
#if UNITY_EDITOR
            string root = Path.GetDirectoryName(Application.dataPath);
            root = Path.Combine(root, "Music");
#else
            string root = Path.Combine(Application.dataPath, "Music");
#endif
            return root;
        }
    }

    void Start()
    {
        // 재귀호출을 이용해 Directory 내부의 모드 파일을 검색해 출력한다.
        string search = SearchDirectory(path, 0);
        Debug.Log(search);

        SampleData data = new SampleData("Sample", 300, 1321313132, 120.4f, "포션", "모자", "장비");
        string smaplePath = Path.Combine(RootDirectory, "Sample.txt");
        using (StreamWriter wr = new StreamWriter(smaplePath))
        {
            string json = JsonUtility.ToJson(data, true);
            wr.Write(json);
        }
    }

    StringBuilder builder;
    private string SearchDirectory(string path, int depth = 0)
    {
        if (depth == 0)
            builder = new StringBuilder();

        // 현재 디렉토리명을 기록한다.
        string space = new string(Enumerable.Repeat(' ', depth * 2).ToArray());
        builder.AppendLine($"{space}▶{Path.GetFileName(path)}");

        // 내부의 모든 디렉토리를 검색해 해당 경로를 통해 '재귀호출' 한다.
        string[] directories = Directory.GetDirectories(path);
        foreach (string dir in directories)
            SearchDirectory(dir, depth + 1);

        // 내부의 모든 파일을 검색해 기록한다.
        string[] files = Directory.GetFiles(path);
        space = new string(Enumerable.Repeat(' ', (depth + 1) * 2).ToArray());
        foreach (string file in files)
            builder.AppendLine($"{space}ㄴ{Path.GetFileName(file)}");

        // 결과를 리턴한다.
        if (depth == 0)
            return builder.ToString();
        else
            return string.Empty;
    }
    private long GetSize(string path)
    {
        // 디렉토리 정보를 이용해 모든 디렉토리의 파일을 검색한다.
        long size = 0;
        DirectoryInfo info = new DirectoryInfo(path);
        foreach(FileInfo fileInfo in info.GetFiles("*", SearchOption.AllDirectories))
            size += fileInfo.Length;                

        return size;
    }
    private string ByteToString(long size)
    {
        // 넘겨받은 byte단위 size를 1024미만이 될때까지 나눠준다.
        string[] exten = { "Byte", "KB", "MB", "GB", "TB" };
        foreach(string ex in exten)
        {
            if (size < 1024)
                return $"{size}{ex}";
            else
                size /= 1024;
        }
        return "Out of range";
    }

    private void Sample1()
    {
        string root = RootDirectory;
        if (!Directory.Exists(root))
            Directory.CreateDirectory(root);

        string MUSIC = "PredawnFarewell";
        for (int i = 0; i < 10; i++)
        {
            string musicName = string.Concat(MUSIC, i);
            string folder = Path.Combine(root, musicName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string file = Path.Combine(folder, "sheet.txt");
            File.Create(file);
        }
    }
    private void Sample()
    {
        // 경로 계산
        // Path : 경로 계산, 합치기
        // Directory : 폴더
        // File : 파일
        // Application : 프로그램 관련.

        // Directory.Exists(path) : 해당 경로에 디렉토리(폴더)가 존재하는가?
        // File.Exists(path) : 파일이 있는가?
        // Application.dataPath : Asset폴더의 상대적 경로 (Build)
        string folder = "Dummy";
        string path = Path.Combine(Application.dataPath, folder);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // 해당 경로의 폴더를 삭제하라
        // 일반적으로 폴더가 비어있지 않으면 삭제할 수 없지만 매개변수를 통해 무시할 수 있다.
        Directory.Delete(path, true);

        // #if는 전처리문, 에디터 환경일 경우
#if UNITY_EDITOR
        string metaPath = Path.Combine(Application.dataPath, string.Concat(folder, ".meta"));
        if (File.Exists(metaPath))
            File.Delete(metaPath);
        Debug.Log("full path : " + Path.GetFullPath(metaPath));                 // 경로.
        Debug.Log("extention : " + Path.GetExtension(metaPath));                // 확장자.
        Debug.Log("file with ex : " + Path.GetFileName(metaPath));              // 파일 이름 (확장자 포함)
        Debug.Log("file : " + Path.GetFileNameWithoutExtension(metaPath));      // 파일 이름 (확장자 미포함)
        Debug.Log("root : " + Path.GetPathRoot(metaPath));                      // 루트 경로
        Debug.Log("directory : " + Path.GetDirectoryName(metaPath));            // 디렉토리 경로
#endif
    }
}
