using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    [SerializeField] private GameObject tracker;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dataHolder;
    [SerializeField] private GameObject pDead;
    [SerializeField] private DDA.DDAManager ddaManager;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject UITokeep;
    [SerializeField] private List<Camera> camera = new List<Camera>();
    [SerializeField] private Camera dataCam;
    [SerializeField] private EmailData emailData;
    


    public ScreenShotHandler screenShotHandler;

    public void GetDataScreen()
    {
        // screenShotHandler.GetScreenShot(500,500);
        UI.SetActive(false);
        UITokeep.SetActive(true);
        camera.ForEach(c => c.gameObject.SetActive(false));
        dataCam.gameObject.SetActive(true);

        //string folderPath = "DynamicGame_Data/"; // the path of your project folder

        //if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
        //    System.IO.Directory.CreateDirectory(folderPath);  // it will get created

        //var screenshotName =
        //                        "screenshot.png";
        //                       // System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + // puts the current time right into the screenshot name
        //                      // ".png"; // put youre favorite data format here
        //ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 2); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
        //Debug.Log(folderPath + screenshotName); // You get instant feedback in the console
        //StartCoroutine(wait());

    }

    private void Start()
    {
    }
    private void LateUpdate()
    {
        Instantiate(tracker, player.transform.position, player.transform.rotation, dataHolder.transform);
    }

    private void Update()
    {
        if (ddaManager.playerDead)
        {
            Instantiate(pDead, player.transform.position, player.transform.rotation, dataHolder.transform);
        }
    }

    private void IstatianteTracker()
    {
        Instantiate(tracker, player.transform.position, player.transform.rotation, dataHolder.transform);
    }
}
