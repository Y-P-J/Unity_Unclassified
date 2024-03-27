using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [SerializeField] NumberUI scoreUI;          // ���� UI.
    [SerializeField] Transform pipePrefab;      // ������ ������.
    [SerializeField] float startDistance;       // ���� ���� �Ÿ�.
    [SerializeField] float pipeDistance;        // ������ �� ����.
    [SerializeField] float moveSpeed;           // �̵� �ӵ�.
    [SerializeField] float maxHeight;           // �ְ� ����.
    [SerializeField] float minHeight;           // ���� ����.
    [SerializeField] float playerX;             // �÷��̾��� X�� ��ġ.

    bool isMove;            // �̵����ΰ�?
    int currentPipeIndex;   // ���� �������� �ε���.

    // ������Ʈ Ǯ��
    // => ����,������ ����� ������Ʈ���� ����ϴ� ������ ����.
    //    �̸� ���� ����� ��Ȱ���Ѵ�.
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
            // Instantiate : Ư�� ������Ʈ�� �����Ѵ� (=Ŭ�δ�)
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

        // x�� ��ġ ���� �������� ���� ������ �ε����� �����Ѵ�.
        // ���� �����ؾ��� �ε����� ���������� �ε����� �����Ѵ�.
        int nextPipeIndex = (int)(Mathf.Abs(transform.position.x) / pipeDistance);
        if (currentPipeIndex < nextPipeIndex)
        {
            // ��ġ�� ���� ���� ������ġ + (���� * �ε���)
            Transform newPipe = GetPipe();
            newPipe.localPosition = Vector3.right * (startDistance + (pipeDistance * nextPipeIndex));
            newPipe.localPosition += Vector3.up * Random.Range(minHeight, maxHeight);
            pipeQueue.Enqueue(newPipe);

            currentPipeIndex = nextPipeIndex;
        }

        // ���� ���� ������� �������� �����Ǿ���ϴ°�?
        if (pipeQueue.Peek().transform.position.x < -startDistance)
        {
            Transform removePipe = pipeQueue.Dequeue();
            storage.Push(removePipe);
        }
    }

    void UpdateScore()
    {
        // ���ھ.
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
