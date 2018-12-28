using System;
using UnityEngine;

public class SunshineNativeGalleryHandler : MonoBehaviour
{

    private const string PACKAGE_NAME = "com.SmileSoft.unityplugin";

    //Gallery

    private const string GALLERY_CLASS_NAME = ".GalleryFrag";
    private const string GALLERY_METHOD_TAKE = "OpenGallery";
    private const string GALLERY_METHOD_TAKE_CALLBACK = "OpenGalleryCallback";


    private const string FileProviderName = "com.SmileSoft.unityplugin.ShareProvider_test";

    public delegate void OnGalleryCallbackHandler(bool success, string[] paths);
    private OnGalleryCallbackHandler _callbackGallery;


    public void OpenGallery(OnGalleryCallbackHandler callback)
    {
#if UNITY_ANDROID
        using (AndroidJavaObject gallery = new AndroidJavaObject(PACKAGE_NAME + GALLERY_CLASS_NAME))
        {
            _callbackGallery = callback;
            gallery.Call(GALLERY_METHOD_TAKE, gameObject.name, GALLERY_METHOD_TAKE_CALLBACK);
        }
#endif
        Debug.Log("This plugin Only worked in android build");
    }


    public void OpenGalleryCallback(string result)
    {

#if UNITY_ANDROID
        Debug.Log("Take Picture Callback | " + "result: " + result);
        if (_callbackGallery != null)
        {
            _callbackGallery.Invoke(!string.IsNullOrEmpty(result), SpilitGalleryPaths(result));

            _callbackGallery = null;
        }
#endif

        Debug.Log("This plugin Only worked in android build");
    }


    string[] SpilitGalleryPaths(string bigString)
    {

        Debug.Log("Big String : " + bigString);
        string separator = "<Separate01234>";
        //bigString.Split();


        string[] splitString = bigString.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);



        return splitString;
    }
}