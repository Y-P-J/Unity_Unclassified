using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [SerializeField] NumberUI scoreUI;          // 점수 UI.
    [SerializeField] Transform pipePrefab;      // 파이프 프리팹.
    [SerializeField] float startDistance;       // 최초 시작 거리.
    [SerializeField] float pipeDistance;        // 파이프 간 간격.
    [SerializeField] float moveSpeed;           // 이동 속도.
    [SerializeField] float maxHeight;           // 최고 높이.
    [SerializeField] float minHeight;           // 최저 높이.
    [SerializeField] float playerX;             // 플레이어의 X축 위치.

    bool isMove;            // 이동중인가?
    int currentPipeIndex;   // 현재 파이프의 인덱스.

    // 오브젝트 풀링
    // => 생성,삭제가 빈번한 오브젝트에게 사용하는 디자인 패턴.
    //    미리 많이 만들고 재활용한다.
    Stack<Transform> storage;
    Queue<Transform> pipeQueue;

    private void Start()
    {
        isMove = false;
        currentPipeIndex = -1;

        storage = new Stack<Transform>();
        pipeQueue = new Queue<Transform>();

        CreatePipe(5);
    }

    private void CreatePipe(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            // Instantiate : 특정 오브젝트를 복사한다 (=클로닝)
            Transform newPipe = Instantiate(pipePrefab, Vector3.zero, Quaternion.identity, transform);
            newPipe.gameObject.SetActive(false);
            storage.Push(newPipe);
        }
    }
    private Transform GetPipe()
    {
        if (storage.Count <= 0)
            CreatePipe();

        Transform pipe = storage.Pop();
        pipe.gameObject.SetActive(true);
        return pipe;
    }

    private void Update()
    {
        if (!isMove)
            return;

        UpdatePipe();

        UpdateScore();
    }

    void UpdatePipe()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // x축 위치 값을 기준으로 현재 파이프 인덱스를 도출한다.
        // 다음 생성해야할 인덱스와 동일해지면 인덱스를 갱신한다.
        int nextPipeIndex = (int)(Mathf.Abs(transform.position.x) / pipeDistance);
        if (currentPipeIndex < nextPipeIndex)
        {
            // 위치는 로컬 기준 시작위치 + (간격 * 인덱스)
            Transform newPipe = GetPipe();
            newPipe.localPosition = Vector3.right * (startDistance + (pipeDistance * nextPipeIndex));
            newPipe.localPosition += Vector3.up * Random.Range(minHeight, maxHeight);
            pipeQueue.Enqueue(newPipe);

            currentPipeIndex = nextPipeIndex;
        }

        // 가장 먼저 만들어진 파이프가 삭제되어야하는가?
        if (pipeQueue.Peek().transform.position.x < -startDistance)
        {
            Transform removePipe = pipeQueue.Dequeue();
            storage.Push(removePipe);
        }
    }

    void UpdateScore()
    {
        // 스코어링.
        int score = 0;
        float x = (transform.position.x + startDistance - playerX) * -1f;
        if (x >= 0)
            score = Mathf.Clamp((int)(x / pipeDistance), -1, 9999) + 1;

        scoreUI.SetNumber(score);
        ScoreManager.instance.SetCurrentScore(score);
    }

    public void Activate(bool isActive)
    {
        isMove = isActive;
    }
}
