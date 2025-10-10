using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using EC;
using System;

public class UIInteractionTextController : MonoBehaviour
{
    // Single Responsibility: This script handles displaying interaction text on screen and navigating through each line
    // when an interactable is interacted with

    public static UIInteractionTextController Instance { get; private set; }
    private List<string> m_Lines;
    private int m_CurrentIndex;
    private bool m_TriggerNextLine = false;
    private Coroutine m_ShowLinesCoroutine;

    [Header("Components"), Space(5)]
    [Tooltip("A ref to the text object that displays interaction text.")]
    [SerializeField] private TextMeshProUGUI m_InteractionText;
    [Tooltip("A ref to the the parent GameObject of the pickup prompt and buttons.")]
    [SerializeField] private GameObject m_ItemPickupPromptPanel;
    [Tooltip("A ref to the text object that displays item pickup prompts.")]
    [SerializeField] private TextMeshProUGUI m_ItemPickupPromptText;
    [Tooltip("A ref to the yes button in the item pickup prompt menu.")]
    [SerializeField] private Button m_YesButton;
    [Tooltip("A ref to the no button in the item pickup prompt menu.")]
    [SerializeField] private Button m_NoButton;




    private void Awake()
    {
        // Check singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Cannot have more than one instance of UI Interaction Text Controller");
            Destroy(gameObject);
            return;
        }
    }

    // Changed from OnEnable for easy testing
    private void Start()
    {
        // Subscribe to event
        InputManager.Instance.OnUILeftMouseClick += InputManagerOnUILeftMouseClick;

        HideItemPickupPrompt();
        HideText();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        InputManager.Instance.OnUILeftMouseClick -= InputManagerOnUILeftMouseClick;
    }

    public void ShowLines(List<string> lines)
    {
        m_Lines = lines;
        m_CurrentIndex = 0;

        // Null check for lines
        if (m_Lines.Count == 0)
        {
            Debug.LogWarning($"Interaction Text list null! Update list in scriptable");
            return;
        }

        // Start the coroutine to display lines
            if (m_ShowLinesCoroutine != null)
            {
                StopCoroutine(m_ShowLinesCoroutine);
            }
        m_ShowLinesCoroutine = StartCoroutine(ShowLinesCoroutine());
    }

    private IEnumerator ShowLinesCoroutine()
    {
        while (m_CurrentIndex < m_Lines.Count)
        {
            // Display the current line
            DisplayLine(m_Lines[m_CurrentIndex]);
            m_TriggerNextLine = false;

            // Wait for the UI Left Mouse Click event before displaying the next line
            yield return new WaitUntil(() => m_TriggerNextLine);

            // Increment the index to show the next line
            m_CurrentIndex++;
        }

        // Optionally, hide the text or perform other actions after all lines are displayed
        HideText();

        // Tell Input Manager to switch Input Context
        InputManager.Instance.SwitchInputContext(InputContext.Gameplay);
    }

    private void DisplayLine(string line)
    {
        // Implement your logic to display the line on the UI
        ShowText();
        m_InteractionText.text = line;
    }

    private void HideText()
    {
        // Implement your logic to hide the text from the UI
        m_InteractionText.gameObject.SetActive(false);
    }

    private void ShowText()
    {
        m_InteractionText.gameObject.SetActive(true);
    }

    private void InputManagerOnUILeftMouseClick(object sender, EventArgs e)
    {
        // This method will be called when the UI Left Mouse Click event is triggered
        // The coroutine will resume and display the next line
        m_TriggerNextLine = true;
    }
   
    public void ShowItemPickupPrompt(BaseItemData itemData, ItemStorage storageFrom)
    {
        // Update pickup prompt text
        m_ItemPickupPromptText.text = $"Take {itemData.GetName()}?";
        // Toggle pickup prompt menu on
        m_ItemPickupPromptPanel.SetActive(true);

        // Clear button listeners to avoid stacking
        m_YesButton.onClick.RemoveAllListeners();
        m_NoButton.onClick.RemoveAllListeners();

        // Add new listener to yes button (Trying Lambda function/Anonymous function syntax here)
        m_YesButton.onClick.AddListener(() =>
        {
            // Yes Button is clicked
            // Try to pick up item from ItemStorage
            InventoryManager.Instance.TryTakeItemFromStorage(itemData, storageFrom);
            //Hide UI
            HideItemPickupPrompt();
            // Tell Input Manager to switch to Gameplay Input Context
            InputManager.Instance.SwitchInputContext(InputContext.Gameplay);
        });

        // Add new listner to no button
        m_NoButton.onClick.AddListener(() =>
        {
            // No Button is clicked
            //Hide UI
            HideItemPickupPrompt();
            // Tell Input Manager to switch to Gameplay Input Context
            InputManager.Instance.SwitchInputContext(InputContext.Gameplay);
        });
    }

    private void HideItemPickupPrompt()
    {
        m_ItemPickupPromptPanel.SetActive(false);
    }

}
