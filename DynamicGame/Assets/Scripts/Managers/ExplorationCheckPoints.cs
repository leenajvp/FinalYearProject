using Player;
using UnityEngine;

public class ExplorationCheckPoints : MonoBehaviour
{
    private GameObject player => FindObjectOfType<PlayerController>().gameObject;
    private SceneMngr sceneMngr => FindObjectOfType<SceneMngr>();
    private DDA.DDAManager ddaManager => FindObjectOfType<DDA.DDAManager>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            sceneMngr.startpos = gameObject.transform;
            ddaManager.events += "\n Player have saved on exploration path, survivor path progression is " + PlayerPrefs.GetInt("Progression").ToString() + "  " + Time.time.ToString();
        }
    }
}
