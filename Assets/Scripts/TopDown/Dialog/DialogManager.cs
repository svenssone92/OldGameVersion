using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System;
using UnityEditor.Rendering;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class DialogManager : MonoBehaviour
{
    [Header("Dialog UI")]
    [SerializeField] private GameObject dialogPanel;

    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [SerializeField] private Animator portraitAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;

    public bool dialogIsPlaying { get; private set; }

    private static DialogManager instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialoge Manager in the scene");
        }
        instance = this;
    }

    public static DialogManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogIsPlaying = false;
        dialogPanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CountinueStory();
        }
    }

    public void EnterDialogMode(TextAsset inkJson)
    {
        currentStory = new Ink.Runtime.Story(inkJson.text);
        dialogIsPlaying = true;
        dialogPanel.SetActive(true);
        CountinueStory();
    }

    private void CountinueStory()
    {
        if (currentStory.canContinue)
        {
            //set text for the current dialog line
            dialogText.text = currentStory.Continue();
            //display choices, if anyy, for this dialog line
            DisplayChoices();
            //Handle tags
            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogMode());
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            //parse the tag
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2 )
            {
                Debug.LogError("Tag could not be appropriately parsed:" + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handle the tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but it is notcurrentlyy being handled: " + tag);
                    break;
            }
        }
    }

    private IEnumerator ExitDialogMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogIsPlaying = false;
        dialogPanel.SetActive(false);
        dialogText.text = "";

        NPCController.isInteractedWith = false;
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
            + currentChoices.Count);
        }

        int index = 0;

        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(selectFirstChoice());
    }

    private IEnumerator selectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void makeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }
}
