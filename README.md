
# Welcome to DracoArts

![Logo](https://dracoarts-logo.s3.eu-north-1.amazonaws.com/DracoArts.png)




#   Saving and Loading Images in Unity Example
 ## The Native Gallery plugin
  enables Unity applications to interact with the device's photo gallery on Android and iOS, allowing developers to:

- Save images (screenshots, textures) directly to the device's gallery

- Load images from the gallery into the app

- Handle permissions automatically

- Support modern storage systems (including Android's Scoped Storage)

### This is particularly useful for:

- Screenshot-sharing features

- Profile picture selection

- Image-based customization in apps

- Photo-saving functionality


## Saving Images
### Process Flow:

- Convert the image (Texture2D or screenshot) to byte format (PNG/JPG)

- Specify a destination album/folder name

- Define a filename (auto-generate or user-provided)

- Save using NativeGallery's API, which handles:

- File system operations

 - Media database updates (so images appear in gallery apps)

- Permission requests (if needed)

## Key Considerations:

## Android: 
Images are saved to Pictures/[albumName]

## iOS: 
Images appear in the Photos app's "Recents" album

## File Formats: 
Supports PNG (lossless) and JPG (compressed)

Threading: Operations are asynchronous to prevent UI freezing

# Loading Images
## Process Flow:
- Request permission (if not already granted)

- Open the device's image picker interface

- Select an image (user interaction)

- Load the image with optional:

- Downscaling (to prevent memory issues)

- Format conversion

- Texture settings (mipmaps, read/write)
## Key Considerations:

 - Performance: Large images are automatically resized

- Permissions: Read access must be granted

## Platform Differences:

Android uses content URIs

iOS uses PHAsset system

 # Permission Handling
## Android Requirements:

- READ_EXTERNAL_STORAGE (for loading)

- WRITE_EXTERNAL_STORAGE (for saving)

- CAMERA (if using camera capture)

## iOS Requirements:

- NSPhotoLibraryUsageDescription (must be in Info.plist)

# Platform-Specific Behavior
## Android
- Scoped Storage (Android 10+):

- Uses MediaStore API

- No direct file path access

- Special directories for app-specific files

## Legacy Support:

- Falls back to traditional file operations

- Requires MANAGE_EXTERNAL_STORAGE for broad access

## iOS
- Uses Photos framework

- Limited to user-selected images (privacy restrictions)

- No direct filesystem access

- Automatic thumbnail generation
# Setup
## Installation
- Download from the
  [ Unity Asset Store]("https://assetstore.unity.com/packages/tools/integration/native-gallery-for-android-ios-112630")

- Import into your project (Assets > Import Package > Custom Package)

  # Required Permissions
## Android (Add to AndroidManifest.xml)
    <uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
    <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
    <uses-permission android:name="android.permission.CAMERA" />

## iOS (Add to Info.plist)
    <key>NSPhotoLibraryUsageDescription</key>
    <string>This app needs access to your photos to save and load images.</string>


## Usage/Examples

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

## Images
## Save

![](https://raw.githubusercontent.com/AzharKhemta/DemoClient/refs/heads/main/Load%20and%20SaveImage%201.gif)
## Load
![](https://raw.githubusercontent.com/AzharKhemta/DemoClient/refs/heads/main/Save%20and%20load%20image%20comp%20part%202.gif)

## Authors

- [@MirHamzaHasan](https://github.com/MirHamzaHasan)
- [@WebSite](https://mirhamzahasan.com)


## 🔗 Links

[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/company/mir-hamza-hasan/posts/?feedView=all/)
## Documentation

[UnityNativeGallery](https://github.com/yasirkula/UnityNativeGallery)


## Tech Stack
**Client:** Unity,C#

**Plugin:** Unity Native Gallery



