using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] UnityEvent onStartGame;
    [SerializeField] UnityEvent onGameOver;

    bool isStartGame;

    private void Awake()
    {
        Instance = this;
    }

    private void OnMouseUpAsButton()
    {
        if(!isStartGame)
        {
            isStartGame = true;
            onStartGame?.Invoke();
        }
    }

    public void OnGameOver()
    {
        onGameOver?.Invoke();
    }
}
