using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    class Turn
    {
        public Character character;
        public float remaining;

        public void Action(Turn actor)
        {
            remaining -= actor.remaining;
        }

        public void StartTurn()
        {
            remaining = 0;
        }

        public void Reset()
        {
            remaining = 10000 / character.Speed;
        }
    }

    Character[] characters;
    List<Turn> turnQueue;
    Character currentCharacter;

    void Start()
    {
        characters = FindObjectsOfType<Character>();

        characters.OrderByDescending(x => x.Speed).ToArray();
        turnQueue = characters.Select(x => new Turn { character = x }).ToList();
        turnQueue.ForEach(x => x.Reset());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextTurn();
            ShowActionQueue();
        }
    }

    /// <summary>
    /// remeaning을 초기화하고, 이번 턴에 사용할 캐릭터를 반환한다.
    /// </summary>
    void NextTurn()
    {
        Turn next = turnQueue[0];
        turnQueue.RemoveAt(0);
        turnQueue.ForEach(x => x.Action(next));
        next.Reset();
        turnQueue.Add(next);
        turnQueue.OrderBy(x => x.remaining);

        currentCharacter = next.character;
    }

    public void ShowActionQueue()
    {
        Debug.Log(string.Join("->", turnQueue.Select(x => x.character.CharaName)));
    }
}
