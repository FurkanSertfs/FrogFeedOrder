using UnityEngine;
using TMPro;

public class MoveView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moveText;

    public void UpdateMoveText(int move)
    {
        moveText.text = move == 0 ? "No moves left" : $"{move} MOVES";
    }
}
