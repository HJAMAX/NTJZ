using UnityEngine;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class UIGalleryManager : MonoBehaviour
{
#if UNITY_ANDROID
    private AndroidJavaObject jo;
#elif UNITY_IOS
    [DllImport("__Internal")]
    private static extern void __GetPhoto(string gameOjectName);
#endif
    private UIGalleryItem galleryItem;

    void Start()
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

    public void OnOpenPhotoGallery(UIGalleryItem item)
    {
        galleryItem = item;
#if UNITY_ANDROID
        jo.Call("chooseNativeImage", gameObject.name, "OnOpenPhotoGalleryCallback");
#elif UNITY_IOS
        __GetPhoto(gameObject.name);
#endif
    }

    public void OnOpenPhotoGalleryCallback(string path)
    {
        if(galleryItem != null)
        {
            galleryItem.InitializeTexture(path);
        }
    }

    public void GetbackImagePathWithIOS(string imagePath)
    {
        if (galleryItem != null)
        {
            galleryItem.InitializeTexture(imagePath);
        }
    }

    /*
    public void OpenPhotoGallery(UIGalleryItem item)
    {
        OpenGallery((bool success, string[] paths) =>
        {
            if (success && paths != null && paths.Length > 0)
            {
                item.InitializeTexture(paths[0]);
            }
        });
    }*/
}
