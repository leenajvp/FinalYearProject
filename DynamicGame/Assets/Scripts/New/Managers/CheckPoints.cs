using UnityEngine;

public class CheckPoints : MonoBehaviour, ICheckPoint
{
    [SerializeField] private int cpNumber;
    public int checkPointID
    {
        get
        {
            return cpNumber;
        }
    }
}
