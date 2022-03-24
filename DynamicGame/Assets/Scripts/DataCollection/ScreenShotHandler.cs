using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotHandler : MonoBehaviour
{
    //private static ScreenShotHandler instance;
    //[SerializeField] private Camera mapCamera;
    //private bool takeScreenshotOnNextFrame;

    //private void Awake()
    //{
    //    instance = this;
    //}

    //private void OnPostRender()
    //{
    //    if (takeScreenshotOnNextFrame)
    //        takeScreenshotOnNextFrame = false;

    //    RenderTexture rTexture = mapCamera.targetTexture;
    //    Texture2D rResult = new Texture2D(rTexture.width, rTexture.height, TextureFormat.ARGB32, false);
    //    Rect rect = new Rect(0, 0, rTexture.width, rTexture.height);
    //    rResult.ReadPixels(rect, 0, 0);

    //    byte[] data = rResult.EncodeToPNG();
    //    System.IO.File.WriteAllBytes(Application.dataPath + "DynamicGame/CameraScreenshot.png", data);
    //    realFolder = System.IO.Directory.GetCurrentDirectory() + "\\" + realFolder;

    //    RenderTexture.ReleaseTemporary(rTexture);
    //    mapCamera.targetTexture = rTexture;
    //}

    //private void TakeScreenshot(int width, int height)
    //{
    //    mapCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
    //    takeScreenshotOnNextFrame = true;

    //}

    //public void GetScreenShot()
    //{
    //    instance.TakeScreenshot(500, 500);
    //    Debug.Log("Screenshot taken");
    //}

    //string folder = "ScreenShots";
    //private string realFolder = "";

    //private void Start()
    //{
    //    realFolder = folder;
    //    int count = 1;
    //    while (System.IO.Directory.Exists(realFolder))
    //    {
    //        realFolder = folder + count;
    //        count++;
    //    }

    //    System.IO.Directory.CreateDirectory(realFolder); // Create the folder
    //    realFolder = System.IO.Directory.GetCurrentDirectory() + "\\" + realFolder;
    //}

    //public void SaveScreenShot()
    //{
    //    var name = string.Format("\\ shot.png", realFolder);
    //    ScreenCapture.CaptureScreenshot(name);
    //}

}

