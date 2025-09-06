using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AstroRemoteController : MonoBehaviour
{
    [Header("UI Elements")]
    // Main controls
    public TextMeshProUGUI exposuresText;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI intervalText;
    
    // Progress elements
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI timeRemainingText;
    public RectTransform progressBarFill;
    
    // Buttons
    public Button startButton;
    public TextMeshProUGUI startButtonText;
    public Image startButtonImage;
    public Button singlePhotoButton;
    public TextMeshProUGUI singlePhotoText;
    public Image singlePhotoImage;
    
    // Increment/decrement buttons
    public Button exposuresPlusButton;
    public Button exposuresMinusButton;
    public Button durationPlusButton;
    public Button durationMinusButton;
    public Button intervalPlusButton;
    public Button intervalMinusButton;
    
    // Toggles
    public Toggle bulbModeToggle;
    public Toggle soundAlertsToggle;
    
    // Red mode
    public Button redModeButton;

    // Status
    public TextMeshProUGUI statusText;

    
    [Header("Controller Settings")]
    // State variables
    private int exposures = 20;
    private int exposureDuration = 1; // in seconds
    private int interval = 1; // in seconds
    private bool isRunning = false;
    private int progress = 0;
    private bool bulbMode = false;
    private bool redMode = true;
    private bool soundAlerts = true;
    
    // Runtime values
    private Coroutine sequenceCoroutine;
    private Color redBackground = new Color(0.1f, 0.01f, 0.01f, 1f);
    private Color darkBackground = new Color(0.05f, 0.05f, 0.05f, 1f);
    private Color greenButtonColor = new Color(0.2f, 0.7f, 0.2f, 1f);
    private Color redButtonColor = new Color(0.7f, 0.2f, 0.2f, 1f);
    
    void Start()
    {
        // Initialize UI with default values
        UpdateDisplayValues();
        
        // Set up button click listeners and UI elements first
        SetupUIElements();

        // Check IR blaster with delay to ensure manager is initialized
        StartCoroutine(CheckIRBlasterWithDelay());
    }

    private IEnumerator CheckIRBlasterWithDelay()
    {
        // Wait slightly longer than the manager's initialization
        yield return new WaitForSeconds(6.0f);
        
        // Now check if IR blaster is available
        bool hasIrEmitter = IRBlasterManager.Instance.HasIrEmitter();
        if (!hasIrEmitter)
        {
            Debug.LogWarning("No IR emitter detected on this device");
            // Could display a warning in the UI
        }
    }

    private void SetupUIElements()
    {
                // Set up button click listeners
        if (startButton != null)
            startButton.onClick.AddListener(ToggleSequence);
            
        if (exposuresPlusButton != null)
            exposuresPlusButton.onClick.AddListener(IncrementExposures);
            
        if (exposuresMinusButton != null)
            exposuresMinusButton.onClick.AddListener(DecrementExposures);
            
        if (durationPlusButton != null)
            durationPlusButton.onClick.AddListener(IncrementDuration);
            
        if (durationMinusButton != null)
            durationMinusButton.onClick.AddListener(DecrementDuration);
            
        if (intervalPlusButton != null)
            intervalPlusButton.onClick.AddListener(IncrementInterval);
            
        if (intervalMinusButton != null)
            intervalMinusButton.onClick.AddListener(DecrementInterval);
            
        if (redModeButton != null)
            redModeButton.onClick.AddListener(ToggleRedMode);
            
        // Set up toggle event listeners
        if (bulbModeToggle != null)
            bulbModeToggle.onValueChanged.AddListener(OnBulbModeChanged);
            
        if (soundAlertsToggle != null)
            soundAlertsToggle.onValueChanged.AddListener(OnSoundAlertsChanged);
    }
    
    // Value modification methods
    public void IncrementExposures()
    {
        if (exposures < 10)
            exposures += 1;
        else
            exposures += 10;
        UpdateDisplayValues();
    }
    
    public void DecrementExposures()
    {
        if (exposures <= 10)
            exposures = Mathf.Max(1, exposures - 1);
        else
            exposures = Mathf.Max(1, exposures - 10);
        UpdateDisplayValues();
    }
    
    public void IncrementDuration()
    {
        if (exposureDuration < 10)
            exposureDuration += 1;
        else
            exposureDuration += 5;
        UpdateDisplayValues();
    }
    
    public void DecrementDuration()
    {
        if (exposureDuration < 10)
            exposureDuration = Mathf.Max(1, exposureDuration - 1);
        else
            exposureDuration = Mathf.Max(1, exposureDuration - 5);
        UpdateDisplayValues();
    }
    
    public void IncrementInterval()
    {
        if (interval < 10)
            interval += 1;
        else
            interval += 5;
        UpdateDisplayValues();
    }
    
    public void DecrementInterval()
    {
        if (interval < 10)
            interval = Mathf.Max(1, interval - 1);
        else
            interval = Mathf.Max(1, interval - 5);
        UpdateDisplayValues();
    }
    
    // Toggle callbacks
    public void OnBulbModeChanged(bool value)
    {
        bulbMode = value;
    }
    
    public void OnSoundAlertsChanged(bool value)
    {
        soundAlerts = value;
    }
    
    public void ToggleRedMode()
    {
        redMode = !redMode;
        UpdateTheme();
    }
    
    // UI update methods
    private void UpdateDisplayValues()
    {
        if (exposuresText != null)
            exposuresText.text = exposures.ToString();
            
        if (durationText != null)
            durationText.text = exposureDuration.ToString();
            
        if (intervalText != null)
            intervalText.text = interval.ToString();
            
        UpdateProgress();
    }
    
    private void UpdateProgress()
    {
        if (progressText != null)
            progressText.text = progress + "/" + exposures + " exposures";
            
        if (progressBarFill != null)
        {
            float progressPercentage = (float)progress / exposures;
            progressBarFill.anchorMax = new Vector2(progressPercentage, 1);
        }
        
        if (timeRemainingText != null)
        {
            int totalSeconds = (exposures - progress) * (interval + exposureDuration);
            timeRemainingText.text = FormatTime(totalSeconds);
        }
    }
    
    private string FormatTime(int seconds)
    {
        int hours = seconds / 3600;
        int minutes = (seconds % 3600) / 60;
        int secs = seconds % 60;
        
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, secs);
    }
    
    private void UpdateTheme()
    {
        // This would be implemented to change the UI theme based on red mode
        // You'll need to adjust this based on how you want to handle theme changes
        Debug.Log("Red mode: " + (redMode ? "ON" : "OFF"));
        
        // This is just a placeholder - you would add your own theme update logic here
        // For example, find and update the background panel color, etc.
    }
    
    // Sequence control methods
    public void ToggleSequence()
    {
        isRunning = !isRunning;
        
        if (isRunning)
        {
            // Start the sequence
            if (startButtonText != null)
                startButtonText.text = "STOP SEQUENCE";
                
            if (startButtonImage != null)
                startButtonImage.color = redButtonColor;
                
            sequenceCoroutine = StartCoroutine(RunSequence());
        }
        else
        {
            // Stop the sequence
            if (sequenceCoroutine != null)
                StopCoroutine(sequenceCoroutine);
                
            if (startButtonText != null)
                startButtonText.text = "START SEQUENCE";
                
            if (startButtonImage != null)
                startButtonImage.color = greenButtonColor;
                
            progress = 0;
            UpdateProgress();
        }
    }
    
    private IEnumerator RunSequence()
    {
        // Reset progress
        progress = 0;
        UpdateProgress();
        
        // Initial delay with countdown display
        if (statusText != null)
            statusText.text = "Starting sequence in:";
        
        for (int i = 5; i > 0; i--)
        {
            if (statusText != null)
                statusText.text = $"Starting sequence in: {i}";
            yield return new WaitForSeconds(1);
        }
        
        if (statusText != null)
            statusText.text = "Sequence running...";
        
        // Loop through the number of exposures
        for (progress = 0; progress < exposures; progress++)
        {
            // Abort if sequence was stopped
            if (!isRunning)
                break;
            
            // Update UI
            UpdateProgress();
            
            // Take a picture
            bool success = IRBlasterManager.Instance.SendNikonTrigger();
            
            if (!success)
            {
                Debug.LogError("Failed to trigger camera");
                break;
            }
            
            // Play sound if enabled
            if (soundAlerts)
            {
                // Play sound code would go here
                // AudioSource.PlayClipAtPoint(...);
            }
            
            // Wait for the exposure duration
            yield return new WaitForSeconds(exposureDuration);
            
            // Wait for the interval between shots (if not the last exposure)
            if (progress < exposures - 1)
            {
                yield return new WaitForSeconds(interval);
            }
        }
        
        // Ensure progress is updated one last time
        UpdateProgress();
        
        // Reset running state
        isRunning = false;
        if (startButtonText != null)
            startButtonText.text = "START SEQUENCE";
            
        if (startButtonImage != null)
            startButtonImage.color = greenButtonColor;
            
        // Update status
        if (statusText != null)
            statusText.text = "Sequence complete!";

        // Play completion sound if enabled
        if (soundAlerts)
        {
            // Play completion sound code would go here
            // AudioSource.PlayClipAtPoint(...);
        }
    }
    
    // For taking a single photo (can be connected to a separate button)
    public void TakeSinglePhoto()
    {
        StartCoroutine(SinglePhoto());
    }

    // This coroutine would handle the single photo loop
    private IEnumerator SinglePhoto()
    {
        if (singlePhotoImage != null)
            singlePhotoImage.color = redButtonColor;
                
        // Initial delay with countdown display
        if (statusText != null)
            statusText.text = "Taking photo in:";
        
        for (int i = 5; i > 0; i--)
        {
            if (statusText != null)
                statusText.text = $"Taking photo in: {i}";
            yield return new WaitForSeconds(1);
        }
        
        if (statusText != null)
            statusText.text = "Triggering camera...";

        IRBlasterManager.Instance.SendNikonTrigger();

        if (statusText != null)
            statusText.text = "Photo taken!";
            
        if (singlePhotoImage != null)
            singlePhotoImage.color = greenButtonColor;
    }
}