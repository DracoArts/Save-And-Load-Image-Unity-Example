using UnityEngine;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ImageManager : MonoBehaviour
{
    public Image image;

    public void PickImage()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");
        if (!string.IsNullOrEmpty(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData))
            {
                TextureToSprite(texture);
            }
        }
#elif UNITY_ANDROID || UNITY_IOS

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize: 2048, markNonReadable: false);
                if (texture != null)
                {
                    TextureToSprite(texture);
                }
            }

        }, "Select an image", "image/*");
#else
     
        Debug.LogWarning("Image picking not implemented for this platform");
#endif
    }

    private void TextureToSprite(Texture2D texture)
    {
        if (texture == null || image == null) return;

    
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        image.sprite = sprite;

        SaveTexture(texture, "uploaded_image.png");
    }

    private void SaveTexture(Texture2D texture, string filename)
    {
        if (texture == null) return;

        try
        {
            byte[] bytes = texture.EncodeToPNG();
            string savePath = Path.Combine(Application.persistentDataPath, filename);
            File.WriteAllBytes(savePath, bytes);
            Debug.Log("Saved to: " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save texture: " + e.Message);
        }
    }
    public void LoadImage()
    {

        string filePath = Path.Combine(Application.persistentDataPath, "uploaded_image.png");
        if (File.Exists(filePath))
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D loadedTexture = new Texture2D(2, 2);
            loadedTexture.LoadImage(bytes);
           TextureToSprite(loadedTexture);
        }
    }
}
