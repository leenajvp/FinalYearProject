using Player;
using UnityEngine;
using UnityEngine.UI;

public class BulletsHUD : MonoBehaviour
{
    private Text bulletTxt;
    private PlayerInventory currentBullets;

    // Start is called before the first frame update
    void Start()
    {
        currentBullets = FindObjectOfType<PlayerInventory>();
        bulletTxt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletTxt.text = currentBullets.bullets.ToString();
    }
}
