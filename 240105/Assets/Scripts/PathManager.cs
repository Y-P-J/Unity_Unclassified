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
        // ���ȣ���� �̿��� Directory ������ ��� ������ �˻��� ����Ѵ�.
        string search = SearchDirectory(path, 0);
        Debug.Log(search);

        SampleData data = new SampleData("Sample", 300, 1321313132, 120.4f, "����", "����", "���");
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

        // ���� ���丮���� ����Ѵ�.
        string space = new string(Enumerable.Repeat(' ', depth * 2).ToArray());
        builder.AppendLine($"{space}��{Path.GetFileName(path)}");

        // ������ ��� ���丮�� �˻��� �ش� ��θ� ���� '���ȣ��' �Ѵ�.
        string[] directories = Directory.GetDirectories(path);
        foreach (string dir in directories)
            SearchDirectory(dir, depth + 1);

        // ������ ��� ������ �˻��� ����Ѵ�.
        string[] files = Directory.GetFiles(path);
        space = new string(Enumerable.Repeat(' ', (depth + 1) * 2).ToArray());
        foreach (string file in files)
            builder.AppendLine($"{space}��{Path.GetFileName(file)}");

        // ����� �����Ѵ�.
        if (depth == 0)
            return builder.ToString();
        else
            return string.Empty;
    }
    private long GetSize(string path)
    {
        // ���丮 ������ �̿��� ��� ���丮�� ������ �˻��Ѵ�.
        long size = 0;
        DirectoryInfo info = new DirectoryInfo(path);
        foreach(FileInfo fileInfo in info.GetFiles("*", SearchOption.AllDirectories))
            size += fileInfo.Length;                

        return size;
    }
    private string ByteToString(long size)
    {
        // �Ѱܹ��� byte���� size�� 1024�̸��� �ɶ����� �����ش�.
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
        // ��� ���
        // Path : ��� ���, ��ġ��
        // Directory : ����
        // File : ����
        // Application : ���α׷� ����.

        // Directory.Exists(path) : �ش� ��ο� ���丮(����)�� �����ϴ°�?
        // File.Exists(path) : ������ �ִ°�?
        // Application.dataPath : Asset������ ����� ��� (Build)
        string folder = "Dummy";
        string path = Path.Combine(Application.dataPath, folder);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // �ش� ����� ������ �����϶�
        // �Ϲ������� ������ ������� ������ ������ �� ������ �Ű������� ���� ������ �� �ִ�.
        Directory.Delete(path, true);

        // #if�� ��ó����, ������ ȯ���� ���
#if UNITY_EDITOR
        string metaPath = Path.Combine(Application.dataPath, string.Concat(folder, ".meta"));
        if (File.Exists(metaPath))
            File.Delete(metaPath);
        Debug.Log("full path : " + Path.GetFullPath(metaPath));                 // ���.
        Debug.Log("extention : " + Path.GetExtension(metaPath));                // Ȯ����.
        Debug.Log("file with ex : " + Path.GetFileName(metaPath));              // ���� �̸� (Ȯ���� ����)
        Debug.Log("file : " + Path.GetFileNameWithoutExtension(metaPath));      // ���� �̸� (Ȯ���� ������)
        Debug.Log("root : " + Path.GetPathRoot(metaPath));                      // ��Ʈ ���
        Debug.Log("directory : " + Path.GetDirectoryName(metaPath));            // ���丮 ���
#endif
    }
}
