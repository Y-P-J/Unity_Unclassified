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

    [SerializeField] float transition;                  // 전환 시간.
    [SerializeField] string firstSceneName;             // 첫 씬 이름.
    [SerializeField] Scene[] scenes;                    // 씬 정보 배열.

    SpriteRenderer spriteRenderer;     // 렌더러.
    Collider2D collider2D;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();

        ChangeScene(firstSceneName);
        StartCoroutine(FadeIn());

        // GameObject.SetActive(bool) : 게임 오브젝트 컴포넌트에게 오브젝트를 끄고/켠다.
        // Component.enabled = bool;  : 해당하는 컴포넌트를 '비활성화' 한다.
    }

    private void ChangeScene(string sceneName)
    {
        // 모든 씬 오브젝트에 대해서 매개변수와 같은 것만 켠다.
        foreach (Scene scene in scenes)
            scene.obj.SetActive(scene.name == sceneName);
    }

    // 씬을 넘길건데 트랜지션 시간동안 화면을 전환하고 A를 끄면서 B를 켜줘.
    public void NextScene(string sceneName)
    {
        // 열거자를 유니티 이벤트에 등록시킨다 => 코루틴
        // 매 프레임마다 열거자를 호출하고 더 이상 실행할 수 없다면 자동으로 해제한다.
        StartCoroutine(IENextScene(sceneName));
    }
    
    // 코루틴 : 쓰레드를 일정 시간 사용 후 양보(=반납)할 수 있는 기능
    IEnumerator IENextScene(string sceneName)
    {
        // 코루틴 리턴의 정의
        // => 해당 코루틴이 끝날때까지 일시정지한다.
        yield return StartCoroutine(FadeOut());     // 페이드 아웃.
        ChangeScene(sceneName);                     // 씬 전환.
        yield return StartCoroutine(FadeIn());      // 페이트 인.
    }
    IEnumerator FadeOut()
    {
        collider2D.enabled = true;      // 콜라이더 활성화.

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
        collider2D.enabled = true;      // 콜라이더 활성화.

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

        collider2D.enabled = false;      // 비활성화.
        spriteRenderer.color = new Color(0, 0, 0, 0);
    }
}
