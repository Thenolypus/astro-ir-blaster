using UnityEngine;
using System.Collections;
using UnityEngine.Android;

public class IRBlasterManager : MonoBehaviour
{
    // Singleton instance
    private static IRBlasterManager _instance;
    public static IRBlasterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("IRBlasterManager");
                _instance = go.AddComponent<IRBlasterManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private AndroidJavaObject _plugin;
    private bool _initialized = false;
    private bool _permissionRequested = false;

    private void Awake()
    {
        // Ensure initialization happens at the right time in the Unity lifecycle
        StartCoroutine(InitializeWithDelay());
    }

    private IEnumerator InitializeWithDelay()
    {
        // Wait for the Unity Android environment to be fully initialized
        yield return new WaitForSeconds(5.0f);

        // Request permissions first
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!Permission.HasUserAuthorizedPermission("android.permission.TRANSMIT_IR"))
        {
            _permissionRequested = true;
            Permission.RequestUserPermission("android.permission.TRANSMIT_IR");
            
            // Wait for permission dialog to be answered
            yield return new WaitForSeconds(0.5f);
        }
#endif
        
        // Now initialize the plugin (outside any yield statements)
        InitializePlugin();
    }

    // Initialize the plugin
    private void InitializePlugin()
    {
        if (_initialized)
            return;

#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.astroremote.irblaster.IRBlasterPlugin"))
            {
                bool result = pluginClass.CallStatic<bool>("init");
                _initialized = result;
                Debug.Log("IR Blaster initialization: " + (result ? "Success" : "Failed"));
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error initializing IR Blaster: " + e.Message);
            _initialized = false;
        }
#else
        Debug.Log("IR Blaster is only available on Android devices");
        _initialized = false;
#endif
    }

    // Check if the device has an IR blaster
    public bool HasIrEmitter()
    {
        if (!_initialized)
            InitializePlugin();

#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.astroremote.irblaster.IRBlasterPlugin"))
            {
                bool result = pluginClass.CallStatic<bool>("hasIrEmitter");
                return result;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error checking IR Emitter: " + e.Message);
            return false;
        }
#else
        Debug.Log("IR Emitter check is only available on Android devices");
        return false;
#endif
    }

    // Send the Nikon trigger signal
    public bool SendNikonTrigger()
    {
        if (!_initialized)
            InitializePlugin();

#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.astroremote.irblaster.IRBlasterPlugin"))
            {
                bool result = pluginClass.CallStatic<bool>("sendNikonTrigger");
                return result;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error sending Nikon trigger: " + e.Message);
            return false;
        }
#else
        Debug.Log("IR transmission is only available on Android devices");
        return false;
#endif
    }

    // Take multiple shots with delay
    public bool TakeMultipleShots(int numShots, int delayBetweenShotsMs)
    {
        if (!_initialized)
            InitializePlugin();

#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.astroremote.irblaster.IRBlasterPlugin"))
            {
                bool result = pluginClass.CallStatic<bool>("takeMultipleShots", numShots, delayBetweenShotsMs);
                return result;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error taking multiple shots: " + e.Message);
            return false;
        }
#else
        Debug.Log("IR transmission is only available on Android devices");
        return true; // Return true for testing in editor
#endif
    }
}