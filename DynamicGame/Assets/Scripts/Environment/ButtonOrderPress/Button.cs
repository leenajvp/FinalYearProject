using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private int buttonNumber;
    [SerializeField] private ButtonPuzzleManager puzzleManager;

    public void SendNum()
    {
        puzzleManager.GetNumber(buttonNumber);
    }
}
