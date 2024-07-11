using System;
using UnityEngine;

public class Move : MonoBehaviour
{
    public event Action OnMoveUsed;

    private int minMove = 0;
    private int currentMove = 0;
    private int activeMove = 0;

    public int CurrentMove
    {
        get => currentMove;
        set => currentMove = Mathf.Max(value, minMove);
    }

    public int ActiveMove
    {
        get => activeMove;
        set => activeMove = value;
    }

    public int MinMove
    {
        get => minMove;
        set => minMove = value;
    }

    public void AddActiveMove(int move)
    {
        ActiveMove += move;
    }

    public void UseMove()
    {
        if (currentMove > minMove)
        {
            currentMove--;
            OnMoveUsed?.Invoke();
        }
    }

    public void SetMove(int move)
    {
        CurrentMove = move;
        activeMove = 0;
    }
}
