using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public enum ID  //�� ������ enum
    {
        Title,
        Main,
        Game,
        Credit,
    }

    public static ID current { get; private set; }  //���� ���������� Ȯ�ο�

    static SceneHandler()
    {
        Scene scene = SceneManager.GetActiveScene();//���� �� ����
        ID id = 0;//�ӽ� enum ����

        System.Enum.TryParse<ID>(scene.name, out id);//�ش���� � enum����

        current = id;//����� ����
    }

    /// <summary>
    /// �� �ҷ�����
    /// </summary>
    /// <param name="id"></param>
    public static void LoadScene(ID id)
    {
        current = id;//����� ����
        SceneManager.LoadScene(id.ToString());//�� �ҷ�����
    }
}
