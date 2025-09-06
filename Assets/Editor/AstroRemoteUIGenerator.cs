using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AstroRemoteUIGenerator : EditorWindow
{
    private bool redMode = true;
    private Color redBackground = new Color(0.1f, 0.01f, 0.01f, 1f); // Dark red
    private Color redButton = new Color(0.3f, 0.05f, 0.05f, 1f); // Red button
    private Color redText = new Color(0.9f, 0.2f, 0.2f, 1f); // Red text
    
    private Color darkBackground = new Color(0.05f, 0.05f, 0.05f, 1f); // Dark gray
    private Color darkButton = new Color(0.15f, 0.15f, 0.15f, 1f); // Dark button
    private Color lightText = new Color(0.8f, 0.8f, 0.8f, 1f); // Light text
    
    [MenuItem("Tools/AstroRemote/Generate UI")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AstroRemoteUIGenerator), false, "AstroRemote UI Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("AstroRemote UI Generator", EditorStyles.boldLabel);
        
        redMode = EditorGUILayout.Toggle("Red Mode", redMode);
        
        if (GUILayout.Button("Generate UI"))
        {
            GenerateUI();
        }
    }
    
    private void GenerateUI()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("AstroRemote_Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Set up CanvasScaler for mobile
        CanvasScaler scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920); // Portrait mode
        scaler.matchWidthOrHeight = 0.5f; // Balance width/height
        
        // Create main background panel
        GameObject bgPanel = CreatePanel("BG_Panel", canvasObj, 0, 0, 1, 1);
        RectTransform bgRect = bgPanel.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        // Set background color based on mode
        Image bgImage = bgPanel.GetComponent<Image>();
        bgImage.color = redMode ? redBackground : darkBackground;
        
        // Create content container with padding
        GameObject contentContainer = CreatePanel("Content_Container", bgPanel, 0, 0, 1, 1);
        RectTransform contentRect = contentContainer.GetComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(40, 40); // Left, bottom padding
        contentRect.offsetMax = new Vector2(-40, -40); // Right, top padding
        
        // Make it transparent
        Image contentImage = contentContainer.GetComponent<Image>();
        contentImage.color = new Color(0, 0, 0, 0);
        
        // Add vertical layout group
        VerticalLayoutGroup vertLayout = contentContainer.AddComponent<VerticalLayoutGroup>();
        vertLayout.spacing = 20;
        vertLayout.childAlignment = TextAnchor.UpperCenter;
        vertLayout.childControlWidth = true;
        vertLayout.childControlHeight = false;
        vertLayout.childForceExpandWidth = true;
        vertLayout.childForceExpandHeight = false;
        
        // Create Header
        GameObject header = CreatePanel("Header", contentContainer, 0, 0, 1, 0);
        RectTransform headerRect = header.GetComponent<RectTransform>();
        headerRect.sizeDelta = new Vector2(0, 80);
        
        // Make header transparent
        Image headerImage = header.GetComponent<Image>();
        headerImage.color = new Color(0, 0, 0, 0);
        
        // Add horizontal layout group to header
        HorizontalLayoutGroup headerLayout = header.AddComponent<HorizontalLayoutGroup>();
        headerLayout.childAlignment = TextAnchor.MiddleLeft;
        headerLayout.childControlWidth = false;
        headerLayout.childForceExpandWidth = false;
        
        // Create title text
        GameObject titleObj = CreateText("Title", header, "AstroRemote", 36);
        titleObj.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 60);
        TextMeshProUGUI titleText = titleObj.GetComponent<TextMeshProUGUI>();
        titleText.color = redMode ? redText : lightText;
        titleText.fontWeight = FontWeight.Bold;
        
        // Create spacer
        GameObject spacer = CreatePanel("Spacer", header, 0, 0, 0, 0);
        RectTransform spacerRect = spacer.GetComponent<RectTransform>();
        spacerRect.sizeDelta = new Vector2(10, 60);
        LayoutElement spacerLayout = spacer.AddComponent<LayoutElement>();
        spacerLayout.flexibleWidth = 1;
        
        // Make spacer transparent
        Image spacerImage = spacer.GetComponent<Image>();
        spacerImage.color = new Color(0, 0, 0, 0);
        
        // Create red mode toggle
        GameObject redModeToggle = CreateButton("RedModeButton", header, "R", 20);
        redModeToggle.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
        Image redModeImage = redModeToggle.GetComponent<Image>();
        redModeImage.color = redMode ? redButton : darkButton;
        
        // Create battery text
        GameObject batteryObj = CreateText("Battery", header, "Battery: 85%", 24);
        batteryObj.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 60);
        TextMeshProUGUI batteryText = batteryObj.GetComponent<TextMeshProUGUI>();
        batteryText.color = redMode ? redText : lightText;
        
        // Create Controls Section
        GameObject controlsSection = CreatePanel("Controls_Section", contentContainer, 0, 0, 1, 0);
        RectTransform controlsRect = controlsSection.GetComponent<RectTransform>();
        controlsRect.sizeDelta = new Vector2(0, 500);
        
        // Make controls section transparent
        Image controlsImage = controlsSection.GetComponent<Image>();
        controlsImage.color = new Color(0, 0, 0, 0);
        
        // Add vertical layout to controls
        VerticalLayoutGroup controlsLayout = controlsSection.AddComponent<VerticalLayoutGroup>();
        controlsLayout.spacing = 20;
        controlsLayout.childAlignment = TextAnchor.UpperCenter;
        controlsLayout.childControlWidth = true;
        controlsLayout.childForceExpandWidth = true;
        
        // Create Number of Exposures Control
        CreateNumericControl(controlsSection, "Number of Exposures", "120", redMode);
        
        // Create Exposure Duration Control
        CreateNumericControl(controlsSection, "Exposure Duration (sec)", "20", redMode);
        
        // Create Interval Control
        CreateNumericControl(controlsSection, "Interval Between Shots (sec)", "30", redMode);
        
        // Create Toggle Section
        GameObject toggleSection = CreatePanel("Toggle_Section", contentContainer, 0, 0, 1, 0);
        RectTransform toggleRect = toggleSection.GetComponent<RectTransform>();
        toggleRect.sizeDelta = new Vector2(0, 80);
        
        // Make toggle section transparent
        Image toggleImage = toggleSection.GetComponent<Image>();
        toggleImage.color = new Color(0, 0, 0, 0);
        
        // Add horizontal layout to toggles
        HorizontalLayoutGroup toggleLayout = toggleSection.AddComponent<HorizontalLayoutGroup>();
        toggleLayout.childAlignment = TextAnchor.MiddleCenter;
        toggleLayout.spacing = 40;
        toggleLayout.childControlWidth = false;
        toggleLayout.childForceExpandWidth = false;
        
        // Create Bulb Mode Toggle
        CreateToggle(toggleSection, "Bulb Mode", false, redMode);
        
        // Create Sound Alerts Toggle
        CreateToggle(toggleSection, "Sound Alerts", true, redMode);
        
        // Create Progress Section
        GameObject progressSection = CreatePanel("Progress_Section", contentContainer, 0, 0, 1, 0);
        RectTransform progressRect = progressSection.GetComponent<RectTransform>();
        progressRect.sizeDelta = new Vector2(0, 120);
        
        // Make progress section transparent
        Image progressImage = progressSection.GetComponent<Image>();
        progressImage.color = new Color(0, 0, 0, 0.2f);
        
        // Add vertical layout to progress section
        VerticalLayoutGroup progressLayout = progressSection.AddComponent<VerticalLayoutGroup>();
        progressLayout.spacing = 10;
        progressLayout.childAlignment = TextAnchor.UpperCenter;
        progressLayout.childControlWidth = true;
        progressLayout.childForceExpandWidth = true;
        progressLayout.padding = new RectOffset(20, 20, 20, 20);
        
        // Create Progress Header
        GameObject progressHeader = CreatePanel("Progress_Header", progressSection, 0, 0, 1, 0);
        RectTransform progressHeaderRect = progressHeader.GetComponent<RectTransform>();
        progressHeaderRect.sizeDelta = new Vector2(0, 30);
        
        // Make progress header transparent
        Image progressHeaderImage = progressHeader.GetComponent<Image>();
        progressHeaderImage.color = new Color(0, 0, 0, 0);
        
        // Add horizontal layout to progress header
        HorizontalLayoutGroup progressHeaderLayout = progressHeader.AddComponent<HorizontalLayoutGroup>();
        progressHeaderLayout.childAlignment = TextAnchor.MiddleCenter;
        progressHeaderLayout.childControlWidth = false;
        progressHeaderLayout.childForceExpandWidth = false;
        
        // Create Progress Label
        GameObject progressLabelObj = CreateText("ProgressLabel", progressHeader, "Progress:", 24);
        progressLabelObj.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 30);
        TextMeshProUGUI progressLabelText = progressLabelObj.GetComponent<TextMeshProUGUI>();
        progressLabelText.color = redMode ? redText : lightText;
        progressLabelText.alignment = TextAlignmentOptions.MidlineLeft;
        
        // Create spacer for progress header
        GameObject progressSpacer = CreatePanel("Progress_Spacer", progressHeader, 0, 0, 0, 0);
        RectTransform progressSpacerRect = progressSpacer.GetComponent<RectTransform>();
        progressSpacerRect.sizeDelta = new Vector2(10, 30);
        LayoutElement progressSpacerLayout = progressSpacer.AddComponent<LayoutElement>();
        progressSpacerLayout.flexibleWidth = 1;
        
        // Make progress spacer transparent
        Image progressSpacerImage = progressSpacer.GetComponent<Image>();
        progressSpacerImage.color = new Color(0, 0, 0, 0);
        
        // Create Progress Counter
        GameObject progressCounterObj = CreateText("ProgressCounter", progressHeader, "0/120 exposures", 24);
        progressCounterObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 30);
        TextMeshProUGUI progressCounterText = progressCounterObj.GetComponent<TextMeshProUGUI>();
        progressCounterText.color = redMode ? redText : lightText;
        progressCounterText.alignment = TextAlignmentOptions.MidlineRight;
        
        // Create Progress Bar
        GameObject progressBarBg = CreatePanel("ProgressBarBg", progressSection, 0, 0, 1, 0);
        RectTransform progressBarBgRect = progressBarBg.GetComponent<RectTransform>();
        progressBarBgRect.sizeDelta = new Vector2(0, 30);
        Image progressBarBgImage = progressBarBg.GetComponent<Image>();
        progressBarBgImage.color = new Color(0.2f, 0.2f, 0.2f, 1);
        
        GameObject progressBarFill = CreatePanel("ProgressBarFill", progressBarBg, 0, 0, 0, 1);
        RectTransform progressBarFillRect = progressBarFill.GetComponent<RectTransform>();
        progressBarFillRect.anchorMin = new Vector2(0, 0);
        progressBarFillRect.anchorMax = new Vector2(0.3f, 1); // 30% full
        progressBarFillRect.offsetMin = Vector2.zero;
        progressBarFillRect.offsetMax = Vector2.zero;
        Image progressBarFillImage = progressBarFill.GetComponent<Image>();
        progressBarFillImage.color = new Color(0.2f, 0.7f, 0.2f, 1); // Green
        
        // Create Time Remaining section
        GameObject timeHeader = CreatePanel("Time_Header", progressSection, 0, 0, 1, 0);
        RectTransform timeHeaderRect = timeHeader.GetComponent<RectTransform>();
        timeHeaderRect.sizeDelta = new Vector2(0, 30);
        
        // Make time header transparent
        Image timeHeaderImage = timeHeader.GetComponent<Image>();
        timeHeaderImage.color = new Color(0, 0, 0, 0);
        
        // Add horizontal layout to time header
        HorizontalLayoutGroup timeHeaderLayout = timeHeader.AddComponent<HorizontalLayoutGroup>();
        timeHeaderLayout.childAlignment = TextAnchor.MiddleCenter;
        timeHeaderLayout.childControlWidth = false;
        timeHeaderLayout.childForceExpandWidth = false;
        
        // Create Time Remaining Label
        GameObject timeLabelObj = CreateText("TimeLabel", timeHeader, "Est. remaining:", 24);
        timeLabelObj.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 30);
        TextMeshProUGUI timeLabelText = timeLabelObj.GetComponent<TextMeshProUGUI>();
        timeLabelText.color = redMode ? redText : lightText;
        timeLabelText.alignment = TextAlignmentOptions.MidlineLeft;
        
        // Create spacer for time header
        GameObject timeSpacer = CreatePanel("Time_Spacer", timeHeader, 0, 0, 0, 0);
        RectTransform timeSpacerRect = timeSpacer.GetComponent<RectTransform>();
        timeSpacerRect.sizeDelta = new Vector2(10, 30);
        LayoutElement timeSpacerLayout = timeSpacer.AddComponent<LayoutElement>();
        timeSpacerLayout.flexibleWidth = 1;
        
        // Make time spacer transparent
        Image timeSpacerImage = timeSpacer.GetComponent<Image>();
        timeSpacerImage.color = new Color(0, 0, 0, 0);
        
        // Create Time Counter
        GameObject timeCounterObj = CreateText("TimeCounter", timeHeader, "00:45:00", 24);
        timeCounterObj.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 30);
        TextMeshProUGUI timeCounterText = timeCounterObj.GetComponent<TextMeshProUGUI>();
        timeCounterText.color = redMode ? redText : lightText;
        timeCounterText.alignment = TextAlignmentOptions.MidlineRight;
        
        // Create flexible spacer before bottom buttons
        GameObject bottomSpacer = CreatePanel("Bottom_Spacer", contentContainer, 0, 0, 1, 0);
        RectTransform bottomSpacerRect = bottomSpacer.GetComponent<RectTransform>();
        bottomSpacerRect.sizeDelta = new Vector2(0, 10);
        LayoutElement bottomSpacerLayout = bottomSpacer.AddComponent<LayoutElement>();
        bottomSpacerLayout.flexibleHeight = 1;
        
        // Make bottom spacer transparent
        Image bottomSpacerImage = bottomSpacer.GetComponent<Image>();
        bottomSpacerImage.color = new Color(0, 0, 0, 0);
        
        // Create Start Button
        GameObject startButton = CreateButton("StartButton", contentContainer, "START SEQUENCE", 32);
        RectTransform startButtonRect = startButton.GetComponent<RectTransform>();
        startButtonRect.sizeDelta = new Vector2(0, 80);
        Image startButtonImage = startButton.GetComponent<Image>();
        startButtonImage.color = new Color(0.2f, 0.7f, 0.2f, 1); // Green
        
        // Create Settings Button
        GameObject settingsButton = CreateButton("SettingsButton", contentContainer, "Settings", 24);
        RectTransform settingsButtonRect = settingsButton.GetComponent<RectTransform>();
        settingsButtonRect.sizeDelta = new Vector2(0, 60);
        Image settingsButtonImage = settingsButton.GetComponent<Image>();
        settingsButtonImage.color = redMode ? redButton : darkButton;
        
        // Add basic RuntimeController script to the canvas
        canvasObj.AddComponent<AstroRemoteController>();
        
        Debug.Log("AstroRemote UI successfully generated!");
        
        // Select the canvas in the hierarchy
        Selection.activeGameObject = canvasObj;
    }
    
    private GameObject CreatePanel(string name, GameObject parent, float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent.transform, false);
        
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(anchorMinX, anchorMinY);
        rect.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        Image image = panel.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0.2f);
        
        return panel;
    }
    
    private GameObject CreateText(string name, GameObject parent, string content, int fontSize)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent.transform, false);
        
        RectTransform rect = textObject.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.color = redMode ? redText : lightText;
        text.alignment = TextAlignmentOptions.Midline;
        
        return textObject;
    }
    
    private GameObject CreateButton(string name, GameObject parent, string content, int fontSize)
    {
        GameObject buttonObject = new GameObject(name);
        buttonObject.transform.SetParent(parent.transform, false);
        
        RectTransform rect = buttonObject.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        Image image = buttonObject.AddComponent<Image>();
        image.color = redMode ? redButton : darkButton;
        
        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;
        ColorBlock colors = button.colors;
        colors.normalColor = redMode ? redButton : darkButton;
        colors.highlightedColor = new Color(
            colors.normalColor.r * 1.2f,
            colors.normalColor.g * 1.2f,
            colors.normalColor.b * 1.2f,
            colors.normalColor.a
        );
        colors.pressedColor = new Color(
            colors.normalColor.r * 0.8f,
            colors.normalColor.g * 0.8f,
            colors.normalColor.b * 0.8f,
            colors.normalColor.a
        );
        button.colors = colors;
        
        GameObject textObj = CreateText(name + "_Text", buttonObject, content, fontSize);
        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.color = Color.white;
        text.fontWeight = FontWeight.Bold;
        
        return buttonObject;
    }
    
    private void CreateNumericControl(GameObject parent, string label, string defaultValue, bool redMode)
    {
        // Create container
        GameObject container = CreatePanel(label.Replace(" ", "_"), parent, 0, 0, 1, 0);
        RectTransform containerRect = container.GetComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(0, 120);
        
        // Make container transparent
        Image containerImage = container.GetComponent<Image>();
        containerImage.color = new Color(0, 0, 0, 0);
        
        // Add vertical layout
        VerticalLayoutGroup containerLayout = container.AddComponent<VerticalLayoutGroup>();
        containerLayout.spacing = 10;
        containerLayout.childAlignment = TextAnchor.UpperCenter;
        containerLayout.childControlWidth = true;
        containerLayout.childForceExpandWidth = true;
        
        // Create label
        GameObject labelObj = CreateText(label + "_Label", container, label, 28);
        RectTransform labelRect = labelObj.GetComponent<RectTransform>();
        labelRect.sizeDelta = new Vector2(0, 40);
        TextMeshProUGUI labelText = labelObj.GetComponent<TextMeshProUGUI>();
        labelText.color = redMode ? redText : lightText;
        labelText.alignment = TextAlignmentOptions.MidlineLeft;
        
        // Create control row
        GameObject controlRow = CreatePanel(label + "_Control", container, 0, 0, 1, 0);
        RectTransform controlRowRect = controlRow.GetComponent<RectTransform>();
        controlRowRect.sizeDelta = new Vector2(0, 70);
        
        // Make control row transparent
        Image controlRowImage = controlRow.GetComponent<Image>();
        controlRowImage.color = new Color(0, 0, 0, 0);
        
        // Add horizontal layout
        HorizontalLayoutGroup controlRowLayout = controlRow.AddComponent<HorizontalLayoutGroup>();
        controlRowLayout.childAlignment = TextAnchor.MiddleCenter;
        controlRowLayout.childControlWidth = false;
        controlRowLayout.childForceExpandWidth = false;
        
        // Create minus button
        GameObject minusButton = CreateButton(label + "_Minus", controlRow, "-", 36);
        RectTransform minusButtonRect = minusButton.GetComponent<RectTransform>();
        minusButtonRect.sizeDelta = new Vector2(70, 70);
        
        // Create value display
        GameObject valueDisplay = CreatePanel(label + "_Value", controlRow, 0, 0, 1, 1);
        RectTransform valueDisplayRect = valueDisplay.GetComponent<RectTransform>();
        valueDisplayRect.sizeDelta = new Vector2(0, 70);
        LayoutElement valueDisplayLayout = valueDisplay.AddComponent<LayoutElement>();
        valueDisplayLayout.flexibleWidth = 1;
        
        // Set value display color
        Image valueDisplayImage = valueDisplay.GetComponent<Image>();
        valueDisplayImage.color = redMode ? redButton : darkButton;
        
        // Create value text
        GameObject valueText = CreateText(label + "_Value_Text", valueDisplay, defaultValue, 36);
        TextMeshProUGUI valueTextTMP = valueText.GetComponent<TextMeshProUGUI>();
        valueTextTMP.fontWeight = FontWeight.Bold;
        
        // Create plus button
        GameObject plusButton = CreateButton(label + "_Plus", controlRow, "+", 36);
        RectTransform plusButtonRect = plusButton.GetComponent<RectTransform>();
        plusButtonRect.sizeDelta = new Vector2(70, 70);
    }
    
    private void CreateToggle(GameObject parent, string label, bool defaultValue, bool redMode)
    {
        // Create container
        GameObject container = CreatePanel(label.Replace(" ", "_"), parent, 0, 0, 0, 1);
        RectTransform containerRect = container.GetComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(200, 60);
        
        // Make container transparent
        Image containerImage = container.GetComponent<Image>();
        containerImage.color = new Color(0, 0, 0, 0);
        
        // Add horizontal layout
        HorizontalLayoutGroup containerLayout = container.AddComponent<HorizontalLayoutGroup>();
        containerLayout.spacing = 10;
        containerLayout.childAlignment = TextAnchor.MiddleLeft;
        containerLayout.childControlWidth = false;
        containerLayout.childForceExpandWidth = false;
        
        // Create toggle
        GameObject toggleObj = new GameObject(label + "_Toggle");
        toggleObj.transform.SetParent(container.transform, false);
        
        RectTransform toggleRect = toggleObj.AddComponent<RectTransform>();
        toggleRect.sizeDelta = new Vector2(40, 40);
        
        Toggle toggle = toggleObj.AddComponent<Toggle>();
        toggle.isOn = defaultValue;
        
        // Create background image
        GameObject background = CreatePanel(label + "_Bg", toggleObj, 0, 0, 1, 1);
        Image bgImage = background.GetComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 1);
        
        // Create checkmark
        GameObject checkmark = CreatePanel(label + "_Checkmark", background, 0.15f, 0.15f, 0.85f, 0.85f);
        Image checkmarkImage = checkmark.GetComponent<Image>();
        checkmarkImage.color = new Color(0.8f, 0.8f, 0.8f, 1);
        
        toggle.graphic = checkmarkImage;
        toggle.targetGraphic = bgImage;
        
        // Create label
        GameObject labelObj = CreateText(label + "_Label", container, label, 24);
        RectTransform labelRect = labelObj.GetComponent<RectTransform>();
        labelRect.sizeDelta = new Vector2(150, 40);
        TextMeshProUGUI labelText = labelObj.GetComponent<TextMeshProUGUI>();
        labelText.color = redMode ? redText : lightText;
        labelText.alignment = TextAlignmentOptions.MidlineLeft;
    }
}

// Runtime controller script for basic functionality
public class AstroRemoteController : MonoBehaviour
{
    // State variables
    private int exposures = 120;
    private int exposureDuration = 20;
    private int interval = 30;
    private bool isRunning = false;
    private int progress = 0;
    private bool bulbMode = false;
    private bool redMode = true;
    private bool soundAlerts = true;
    
    // UI references - these would be set up in Start() in a real implementation
    private TextMeshProUGUI exposuresText;
    private TextMeshProUGUI durationText;
    private TextMeshProUGUI intervalText;
    private TextMeshProUGUI progressText;
    private TextMeshProUGUI timeRemainingText;
    private RectTransform progressBarFill;
    private Button startButton;
    private TextMeshProUGUI startButtonText;
    private Image startButtonImage;
    private Toggle bulbModeToggle;
    private Toggle soundAlertsToggle;
    
    private void Start()
    {
        // In a real implementation, we would find all references here
        // This is just a template to be expanded upon
        
        // Example of finding UI elements:
        // exposuresText = transform.Find("Content_Container/Controls_Section/Number_of_Exposures/Number_of_Exposures_Control/Number_of_Exposures_Value/Number_of_Exposures_Value_Text").GetComponent<TextMeshProUGUI>();
        
        // Hook up events:
        // startButton.onClick.AddListener(ToggleSequence);
    }
    
    private void ToggleSequence()
    {
        isRunning = !isRunning;
        
        if (isRunning)
        {
            // Start sequence
            startButtonText.text = "STOP SEQUENCE";
            startButtonImage.color = new Color(0.7f, 0.2f, 0.2f, 1); // Red
        }
        else
        {
            // Stop sequence
            startButtonText.text = "START SEQUENCE";
            startButtonImage.color = new Color(0.2f, 0.7f, 0.2f, 1); // Green
            progress = 0;
            UpdateProgress();
        }
    }
    
    private void UpdateProgress()
    {
        // Update progress text
        progressText.text = progress + "/" + exposures + " exposures";
        
        // Update progress bar
        float progressPercentage = (float)progress / exposures;
        progressBarFill.anchorMax = new Vector2(progressPercentage, 1);
        
        // Update time remaining
        int totalSeconds = (exposures - progress) * (interval + exposureDuration);
        timeRemainingText.text = FormatTime(totalSeconds);
    }
    
    private string FormatTime(int seconds)
    {
        int hours = seconds / 3600;
        int minutes = (seconds % 3600) / 60;
        int secs = seconds % 60;
        
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, secs);
    }
    
    private void SetExposures(int value)
    {
        exposures = Mathf.Max(1, value);
        exposuresText.text = exposures.ToString();
        UpdateProgress();
    }
    
    private void SetExposureDuration(int value)
    {
        exposureDuration = Mathf.Max(1, value);
        durationText.text = exposureDuration.ToString();
        UpdateProgress();
    }
    
    private void SetInterval(int value)
    {
        interval = Mathf.Max(1, value);
        intervalText.text = interval.ToString();
        UpdateProgress();
    }
    
    // These methods would be hooked up to the appropriate buttons
    public void IncrementExposures() { SetExposures(exposures + 10); }
    public void DecrementExposures() { SetExposures(exposures - 10); }
    public void IncrementDuration() { SetExposureDuration(exposureDuration + 5); }
    public void DecrementDuration() { SetExposureDuration(exposureDuration - 5); }
    public void IncrementInterval() { SetInterval(interval + 5); }
    public void DecrementInterval() { SetInterval(interval - 5); }
}