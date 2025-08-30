using System.IO;
using UnityEngine;

public class Photograp : MonoBehaviour
{
    public Camera cam; // assign your camera in the Inspector
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // press P to take a photo
        {
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        RenderTexture rt = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        cam.targetTexture = rt;

        Texture2D screenShot = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);

        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        screenShot.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // save to file
        byte[] bytes = screenShot.EncodeToPNG();
        string filePath = Path.Combine(Application.persistentDataPath, "CameraPhoto.png");
        File.WriteAllBytes(filePath, bytes);

        Debug.Log("Photo saved at: " + filePath);
    }
}
