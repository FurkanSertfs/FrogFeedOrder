using System;
using UnityEngine;

public class MovePresenter : MonoSingleton<MovePresenter>
{
    [SerializeField]
    private MoveView moveView;

    [SerializeField]
    private Move move;

    public static Action OnMoveUsed;
    public static Action OnNoMovesLeft;

    private void OnEnable()
    {
        move.OnMoveUsed += UpdateMoveView;
        LevelController.OnSetMove += SetMove;
    }

    private void OnDestroy()
    {
        move.OnMoveUsed -= UpdateMoveView;
        LevelController.OnSetMove -= SetMove;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<IClickable>(out var clickable))
                {
                    clickable.OnClick();
                }
            }
        }
    }

    private void UpdateMoveView()
    {
        moveView.UpdateMoveText(move.CurrentMove);
    }

    public bool UseMove()
    {
        if (move.CurrentMove > 0)
        {
            move.UseMove();
            OnMoveUsed?.Invoke();
            move.AddActiveMove(+1);
            return true;
        }
        return false;
    }

    public void ControlMove()
    {
        move.AddActiveMove(-1);

        if (move.CurrentMove > 0)
        {
            return;
        }

        if (move.ActiveMove == 0)
        {
            OnNoMovesLeft?.Invoke();
        }
    }

    private void SetMove(int moveValue)
    {
        move.SetMove(moveValue);
        UpdateMoveView();
    }
}
