using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Fader : MonoBehaviour
{
    public static Fader Instance { get; private set; }

    [System.Serializable]
    struct Scene
    {
        public string name;
        public GameObject obj;
    }

    [SerializeField] float transition;                  // ��ȯ �ð�.
    [SerializeField] string firstSceneName;             // ù �� �̸�.
    [SerializeField] Scene[] scenes;                    // �� ���� �迭.

    SpriteRenderer spriteRenderer;     // ������.
    Collider2D collider2D;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();

        ChangeScene(firstSceneName);
        StartCoroutine(FadeIn());

        // GameObject.SetActive(bool) : ���� ������Ʈ ������Ʈ���� ������Ʈ�� ����/�Ҵ�.
        // Component.enabled = bool;  : �ش��ϴ� ������Ʈ�� '��Ȱ��ȭ' �Ѵ�.
    }

    private void ChangeScene(string sceneName)
    {
        // ��� �� ������Ʈ�� ���ؼ� �Ű������� ���� �͸� �Ҵ�.
        foreach (Scene scene in scenes)
            scene.obj.SetActive(scene.name == sceneName);
    }

    // ���� �ѱ�ǵ� Ʈ������ �ð����� ȭ���� ��ȯ�ϰ� A�� ���鼭 B�� ����.
    public void NextScene(string sceneName)
    {
        // �����ڸ� ����Ƽ �̺�Ʈ�� ��Ͻ�Ų�� => �ڷ�ƾ
        // �� �����Ӹ��� �����ڸ� ȣ���ϰ� �� �̻� ������ �� ���ٸ� �ڵ����� �����Ѵ�.
        StartCoroutine(IENextScene(sceneName));
    }
    
    // �ڷ�ƾ : �����带 ���� �ð� ��� �� �纸(=�ݳ�)�� �� �ִ� ���
    IEnumerator IENextScene(string sceneName)
    {
        // �ڷ�ƾ ������ ����
        // => �ش� �ڷ�ƾ�� ���������� �Ͻ������Ѵ�.
        yield return StartCoroutine(FadeOut());     // ���̵� �ƿ�.
        ChangeScene(sceneName);                     // �� ��ȯ.
        yield return StartCoroutine(FadeIn());      // ����Ʈ ��.
    }
    IEnumerator FadeOut()
    {
        collider2D.enabled = true;      // �ݶ��̴� Ȱ��ȭ.

        // Fade Out.
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            spriteRenderer.color = new Color(0, 0, 0, time / transition);
            if (time >= transition)
                break;

            yield return null;
        }
        spriteRenderer.color = Color.white;
    }
    IEnumerator FadeIn()
    {
        collider2D.enabled = true;      // �ݶ��̴� Ȱ��ȭ.

        // Fade In.
        float time = transition;
        while (true)
        {
            time -= Time.deltaTime;
            spriteRenderer.color = new Color(0, 0, 0, time / transition);
            if (time <= 0.0f)
                break;

            yield return null;
        }

        collider2D.enabled = false;      // ��Ȱ��ȭ.
        spriteRenderer.color = new Color(0, 0, 0, 0);
    }
}
