using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPuzzleManager : ElectricLock
{
    [Tooltip("Text to match player input")]
    [SerializeField] private Text playerInputText;
    [Space(20)] [SerializeField] private List<int> correctSequence = new List<int>();

    private List<int> enteredNums = new List<int>();
    private string inputTxt = "";

    protected override void Start()
    {
        base.Start();
        playerInputText.text = inputTxt;
    }

    private void Update()
    {
        CheckSequence();
    }

    private void CheckSequence()
    {
        if (enteredNums.Count == correctSequence.Count)
        {
            for (int i = 0; i < correctSequence.Count; i++)
            {
                bool isEqual = Enumerable.SequenceEqual(correctSequence, enteredNums);

                if (isEqual && available)
                {
                    puzzle.SetActive(false);
                    playerController.interacting = false;
                    playerController.DisablePlayer();
                    door.active = true;
                    available = false;
                }

                else
                {
                    enteredNums.Clear();
                    inputTxt = "";
                }
            }
        }
    }

    public void GetNumber(int num)
    {
        enteredNums.Add(num);
        inputTxt += num;
        playerInputText.text = inputTxt;
    }
}
