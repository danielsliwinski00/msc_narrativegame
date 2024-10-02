using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using System.Threading;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerName;
    [SerializeField] private TextMeshProUGUI dialogueTextMain, obtainedItemText;
    [SerializeField] private GameObject continueText;
    [SerializeField] private Image portraitSprite;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] GameObject obtainedItem;
    private TextMeshProUGUI[] choicesText;

    [Header("Audio")]
    [SerializeField] AudioSource buttonSwitch;
    [SerializeField] AudioSource questComplete;
    [SerializeField] AudioSource typingText;
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f;
    [SerializeField] private bool stopAudio;


    [Header("Other")]
    private Story currentStory;
    public bool DialogueIsPlaying {get; private set;}
    public bool MakingChoice {get; private set;}
    private static DialogueManager instance;
    public float time = 5;
    [SerializeField] PlayerController playerController;
    [SerializeField] private GameObject[] leaders;

    public event Action OnBattle;
    public event Action OnDialogue;
    public event Action OnFreeRoam;
    public event Action<string> OnCutscene;

    [SerializeField] int lettersPerSecond = 40;
    [SerializeField] int lettersPerSecondFast = 120;
    //[SerializeField] float typingSpeed = 1f/lettersPerSecond;
    [SerializeField] Color highlightedColor;
    int currentChoice;

    private Coroutine typeDialogueCoroutine;
    private bool canContinueToNextLine = false;
    bool isTyping = false;

    GameController gameController;

    private void Awake(){

        if(instance != null){
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance(){
        return instance;
    }

    private void Start(){

        DialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        dialogueTextMain.text = "";
        MakingChoice = false;

        choicesText = new TextMeshProUGUI[choices.Length];

        int i = 0;
        foreach(GameObject choice in choices){

            choicesText[i] = choice.GetComponentInChildren<TextMeshProUGUI>();
            i++;
        }
    }

    public void HandleUpdate(){

        if(Input.GetKey(KeyCode.Space)){
            lettersPerSecond = lettersPerSecondFast;
        }
        else{
            lettersPerSecond = 40;
        }

        if(!isTyping && canContinueToNextLine && currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.Space)){
            ContinueStory();
        }  

        if(MakingChoice){
            HandleChoiceSelection();
        }
    }

    void PlayDialogueSound(int characters){
        if(lettersPerSecond != 40){
            if(characters % 9 == 0){
                if(stopAudio){
                    typingText.Stop();
                }
                typingText.pitch = 1;
                typingText.Play();
            }
        }

        if(characters % 2 == 0){
            if(stopAudio){
                typingText.Stop();
            }
            typingText.pitch = 1;
            typingText.Play();
        }
    }

    public IEnumerator TypeDialogue(string dialogue){
        isTyping = true;
        dialogueTextMain.text = "";

        continueText.SetActive(false);
        HideChoices();
        canContinueToNextLine = false;

        foreach(var letter in dialogue.ToCharArray()){

            /*if(isTyping && Input.GetKeyDown(KeyCode.Space)){
                dialogueTextMain.text = dialogue;
                isTyping = false;
                break;
            }*/

            PlayDialogueSound(dialogueTextMain.text.ToCharArray().Length);
            dialogueTextMain.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }

        isTyping = false;
        continueText.SetActive(false);
        DisplayChoices();
        canContinueToNextLine = true;

        yield return new WaitForSeconds(0.5f);

    }

    private IEnumerator ItemReceived(string itemName){
        obtainedItem.SetActive(true);
        obtainedItemText.text = "You have obtained '"+itemName+"'";
        yield return new WaitForSeconds(2f);
        obtainedItem.SetActive(false);
    }

    private IEnumerator ItemLost(string itemName){
        obtainedItem.SetActive(true);
        obtainedItemText.text = "You have lost '"+itemName+"'";
        yield return new WaitForSeconds(2f);
        obtainedItem.SetActive(false);
    }

    public IEnumerator EnterDialogueMode(TextAsset inkJSON, string speaker, Sprite sprite, int battleStatus){
        OnDialogue();
        dialogueTextMain.text = "";
        currentStory = new Story(inkJSON.text);
        MakingChoice = false;
        DialogueIsPlaying = true;
        dialogueBox.SetActive(true);
        continueText.SetActive(true);
        dialogueSpeakerName.text = speaker;
        portraitSprite.sprite = sprite;

        if(battleStatus == 1){
            currentStory.variablesState["questStatus"] = "complete";
        }
        else if(battleStatus == 2){
            currentStory.variablesState["questStatus"] = "lost";
        }

        currentStory.BindExternalFunction("battleStart", () => {

            StartCoroutine(BattleSceneStart());
        });
        currentStory.BindExternalFunction("debug", (int value) => {
            Debug.Log(value);
        });

        currentStory.BindExternalFunction("giveQuestItem", (string itemName) => {
            playerController.Inventory.Add(itemName);
            StartCoroutine(ExitDialogueMode()); 
            StartCoroutine(ItemReceived(itemName));
        });

        currentStory.BindExternalFunction("takeQuestItem", (string itemName) => {
            playerController.Inventory.Remove(itemName);
            StartCoroutine(ExitDialogueMode());
            StartCoroutine(ItemLost(itemName));
        });

        currentStory.BindExternalFunction("npcAction", (string name) => {
            StartCoroutine(ExitDialogueMode());
            NPCAction(name);
        });

        currentStory.BindExternalFunction("setQuestStatus", (string questName, string questStatus) => {

            foreach(Quest quest in playerController.Quests)
            {
                if(quest.questName == questName)
                {
                    quest.questState = questStatus;
                }
            }
        });

        currentStory.BindExternalFunction("getQuestStatus", (string questName) => {

            foreach(Quest quest in playerController.Quests)
            {
                if(quest.questName == questName)
                {
                    switch(quest.questState){
                        case "unobtained":
                            currentStory.variablesState["questStatus"] = "unobtained";
                            break;
                        case "incomplete":
                            currentStory.variablesState["questStatus"] = "incomplete";
                            break;
                        case "complete":
                            currentStory.variablesState["questStatus"] = "complete";
                            break;
                        case "completeW":
                            currentStory.variablesState["questStatus"] = "completeW";
                            break;
                        case "completeF":
                            currentStory.variablesState["questStatus"] = "completeF";
                            break;
                        case "success":
                            currentStory.variablesState["questStatus"] = "success";
                            break;
                        case "fail":
                            currentStory.variablesState["questStatus"] = "fail";
                            break;
                        case "lost":
                            currentStory.variablesState["questStatus"] = "lost";
                            break;
                        default:
                            currentStory.variablesState["questStatus"] = "unobtained";
                            break;
                    }
                }
            }
        });

        currentStory.BindExternalFunction("questItem", (string itemName) => {
            if(playerController.Inventory.Contains(itemName)){
                currentStory.variablesState["questItem"] = 0;
            }
            else{
                currentStory.variablesState["questItem"] = 1;
            }
        });

        currentStory.BindExternalFunction("getMainQuestStatus", () => {

            var successQ = 0;
            var total = 0;

            foreach(Quest quest in playerController.Quests)
            {
                if(quest.questState == "success"){
                    successQ += 1;
                    total += 1;
                }
                else if(quest.questState == "fail"){
                    total += 1;
                }
            }

            if(total == 5){
                currentStory.variablesState["mainQuestStatus"] = "complete";
                currentStory.variablesState["mainQuestAmount"] = 5;
            }else if(total == 3){
                currentStory.variablesState["mainQuestStatus"] = "incomplete";
                currentStory.variablesState["   "] = 3;
            }
            else if(total == 1){
                currentStory.variablesState["mainQuestStatus"] = "incomplete";
                currentStory.variablesState["mainQuestAmount"] = 1;
            }
            else{
                currentStory.variablesState["mainQuestStatus"] = "incomplete";
                currentStory.variablesState["mainQuestAmount"] = 0;
            }
        });

        currentStory.BindExternalFunction("setEnding", () => {

            var successQ = 0;

            foreach(Quest quest in playerController.Quests){
                if(quest.questState == "success"){
                    successQ += 1;
                }
            }

            switch(successQ){
                case >=4:
                    playerController.Quests[6].questState = "goodEnding";
                    break;
                case 3:
                    playerController.Quests[6].questState = "neutralEnding";
                    break;
                case <=2:
                    playerController.Quests[6].questState = "badEnding";
                    break;
                }
                
            foreach(Quest quest in playerController.Quests){
                if(quest.questState == "success"){
                    switch(quest.questName){
                        //0.shaolin 1.Namgung 2.MountHua 3.Wudang 4.Tang
                        case "questShaolin":
                            leaders[0].SetActive(true);
                            break;
                        case "questNamgung":
                            leaders[1].SetActive(true);
                            break;
                        case "questMountHua":
                            leaders[2].SetActive(true);
                            break;
                        case "questWudang":
                            leaders[3].SetActive(true);
                            break;
                        case "questTang":
                            leaders[4].SetActive(true);
                            break;
                    }
                }
            }
        });

        currentStory.BindExternalFunction("getEnding", () => {
            StartCoroutine(CutsceneStart(playerController.Quests[6].questState));
        });

        yield return new WaitForSeconds(0.1f);
        ContinueStory();
    }

    public IEnumerator CutsceneStart(string cutsceneName){
        MakingChoice = false;
        DialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        continueText.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        OnCutscene(cutsceneName);
    }

    public IEnumerator BattleSceneStart(){
        MakingChoice = false;
        DialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        continueText.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        OnBattle();
    }

    private IEnumerator ExitDialogueMode(){

        MakingChoice = false;
        DialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        continueText.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        OnFreeRoam();
    }

    private void ContinueStory(){

        if(currentStory.canContinue){

            if(typeDialogueCoroutine != null){
                StopCoroutine(typeDialogueCoroutine);
            }

            typeDialogueCoroutine = StartCoroutine(TypeDialogue(currentStory.Continue()));
            //dialogueTextMain.text = currentStory.Continue();
        }
        else{
            StartCoroutine(ExitDialogueMode());
        }
    }
    private void DisplayChoices(){
        MakingChoice = true;
        currentChoice = 0;

        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > 0)
        {
            continueText.SetActive(false);
        }
        else{
            continueText.SetActive(true);
        }

        if(currentChoices.Count > choices.Length){
            Debug.LogError("Too many choices. count: "+currentChoices.Count);
        }

        int i = 0;
        foreach(Choice choice in currentChoices){

            choices[i].gameObject.SetActive(true);
            choicesText[i].text = choice.text;
            i++;
        }

        for (int j=i; j < choices.Length; j++){
            choices[j].gameObject.SetActive(false);
        }
    }

    private void HideChoices(){
        foreach(GameObject choice in choices){
            choice.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex){
        if(canContinueToNextLine){
            MakingChoice = false;
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    void HandleChoiceSelection(){
        List<Choice> currentChoices = currentStory.currentChoices;

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            if(currentChoice < currentChoices.Count-1){
                currentChoice++;
                buttonSwitch.Play();
            }
        }
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            if(currentChoice > 0){
                currentChoice--;
                buttonSwitch.Play();
            }
        }

        UpdateChoiceSelection(currentChoice);

        if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Z)){
            MakeChoice(currentChoice);
        }
    }

    public void UpdateChoiceSelection(int selectedAction){

        for(int i = 0; i < choicesText.Length; i++){

            if(i == selectedAction){
                choicesText[i].color = highlightedColor;
            }
            else{
                choicesText[i].color = Color.black;
            }
        }
    }

    public void NPCAction(string name){
        switch(name){
            case "leave":
                StartCoroutine(playerController.interactedWith.Leave());
                break;
            case "leaveX":
                StartCoroutine(playerController.interactedWith.LeaveX());
                break;
            case "healPlayer":
                playerController.martialArtist.HP = playerController.martialArtist.MaxHP;
                break;
            case "questComplete":
                questComplete.Play();
                break;
        }
    }
}