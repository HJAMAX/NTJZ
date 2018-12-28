using UnityEngine;
using System.IO;

public class UIGalleryItem : MonoBehaviour
{
    public byte[] fileData;

    private UITexture uiTex;

    void Start()
    {
        uiTex = GetComponent<UITexture>();
    }

    public void InitializeTexture(string url)
    {
        Texture2D tex2D = LoadPNG(url);
        uiTex.mainTexture = tex2D;
    }

    public Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
