using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Button : MonoBehaviour
{
    [SerializeField] private int buttonNumber;
    [SerializeField] private ButtonPuzzleManager puzzleManager;
    [SerializeField] private Color pressedColor;
    [SerializeField] private Color normalColor;

    public void SendNum()
    {
       puzzleManager.GetNumber(buttonNumber);
    }
}
