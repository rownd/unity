# Rownd SDK for Unity

The Rownd SDK for Unity enables user authentication and profile management in Unity applications. This package provides a seamless way to integrate Rownd's authentication services into your Unity projects.

## Features

- User authentication
- User profile management 
- Meta (Oculus) platform integration
- Secure data handling

## Installation

1. Clone this repository
2. Open your Unity project
3. Import the Rownd SDK package:
   - In the Unity Editor, right-click on the Assets folder
   - Select "Import Package" -> "Custom Package"
   - Navigate to and select `rownd-unity-plugin.unitypackage` from the cloned repository

## Quick Start

Initialize the SDK in your main scene. Here's a basic example:

```csharp
using Rownd;

public class RowndInitializer : MonoBehaviour
{
    async void Start()
    {
        try {
            await RowndInstance.Instance.InitializeAsync("YOUR_ROWND_APP_KEY");
            Debug.Log("Rownd SDK initialized successfully");
        }
        catch (System.Exception ex) {
            Debug.LogError($"Failed to initialize Rownd SDK: {ex.Message}");
        }
    }
}
```

### Meta (Oculus) Platform Integration

If you're developing for Meta Quest, here's how to integrate both platforms:

```csharp
using Rownd;
using Meta.Platform;

public class MetaAuthSignIn : MonoBehaviour
{
    async void Start()
    {
        await InitializePlatform();
    }

    private async Task InitializePlatform()
    {
        try
        {
            // Initialize Meta Platform
            string metaHorizonAppId = "YOUR_META_HORIZON_APP_ID";
            Core.Initialize(metaHorizonAppId);
            Debug.Log("Meta Platform initialized successfully");

            // Initialize Rownd SDK
            await RowndInstance.Instance.InitializeAsync("YOUR_ROWND_APP_KEY");
            Debug.Log("Rownd SDK initialized successfully");

            LoadStatusScene();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to initialize platforms: {ex.Message}");
            LoadStatusScene();
        }
    }

    private void LoadStatusScene()
    {
        SceneManager.LoadScene("AuthStatusScene");
    }
}
```

## Usage Examples

### Get User Data

```csharp
using UnityEngine;
using TMPro;
using Rownd;




public class AuthStatusDisplay : MonoBehaviour
{
    public TMP_Text emailText;
    public TMP_Text userIdText;

    void Start()
    {
        var user = RowndInstance.Instance.State.User;
        string email = user.Email;
        string userId = user.UserId;
        string appId = RowndInstance.Instance.State.AppConfig.App.Id;
        
        if (appId != null)
        {
            emailText.text = "user email: " + email;
            userIdText.text = "user id: " + userId;
        }
        else
        {
            emailText.text = "no app id";
            userIdText.text = "no app id";
        }
    }
}
```

## Support

If you encounter any issues or need assistance:
- Open an issue in this repository
- Contact support at support@rownd.io

