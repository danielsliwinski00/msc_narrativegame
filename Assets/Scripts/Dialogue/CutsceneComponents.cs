using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneComponents : MonoBehaviour{
    
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerName;
    [SerializeField] private TextMeshProUGUI dialogueTextMain;
    [SerializeField] private GameObject continueText;
    [SerializeField] private GameObject[] scenes;
    [SerializeField] private GameObject[] leaders;

    [Header("Audio")]
    [SerializeField] AudioSource typingText;
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f;
    [SerializeField] private bool stopAudio;
    
    [Header("Other")]
    private Story currentStory;
    public bool DialogueIsPlaying {get; private set;}
    public bool MakingChoice {get; private set;}
    private static CutsceneComponents instance;
    public event Action OnCutsceneOver;
    int lettersPerSecond = 40;
    int lettersPerSecondFast = 120;
    [SerializeField] float typingSpeed = 1f/80;
    private Coroutine typeDialogueCoroutine;
    private bool canContinueToNextLine = false;
    bool isTyping = false;

    private void Awake(){

        if(instance != null){
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        instance = this;
    }

    public static CutsceneComponents GetInstance(){
        return instance;
    }

    private void Start(){

        DialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        dialogueTextMain.text = "";
        MakingChoice = false;
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
    }

    void PlayDialogueSound(int characters){
        if(lettersPerSecond == 120){
            if(characters % 9 == 0){
                if(stopAudio){
                    typingText.Stop();
                }
                typingText.pitch = 1;
                typingText.Play();
            }
        }

        if(characters % 3 == 0){
            if(stopAudio){
                typingText.Stop();
            }
            typingText.pitch = 1;
            typingText.Play();
        }
    }

    public IEnumerator TypeDialogue(string dialogue){

        isTyping = true;
        yield return new WaitForSeconds(0.01f);
        dialogueTextMain.text = "";

        continueText.SetActive(false);
        canContinueToNextLine = false;

        foreach(var letter in dialogue.ToCharArray()){

            PlayDialogueSound(dialogueTextMain.text.ToCharArray().Length);
            dialogueTextMain.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }

        isTyping = false;

        continueText.SetActive(true);
        canContinueToNextLine = true;

        yield return new WaitForSeconds(0.5f);

    }

    public IEnumerator EnterDialogueMode(TextAsset inkJSON){
        dialogueTextMain.text = "";
        currentStory = new Story(inkJSON.text);
        MakingChoice = false;
        DialogueIsPlaying = true;
        dialogueBox.SetActive(true);

        //Debug.Log(currentStory);

        currentStory.BindExternalFunction("setScene", (int scene) => {
            ShowScene(scene);   
        });
        currentStory.BindExternalFunction("setSceneLeader", (int scene) => {
            ShowLeader(scene);   
        });

        yield return new WaitForSeconds(0.1f);
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode(){

        MakingChoice = false;
        DialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        continueText.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        OnCutsceneOver();
    }

    private void ShowScene(int scene){

        foreach(GameObject i in scenes){
            i.SetActive(false);
        }

        scenes[scene].SetActive(true);
    }

    private void ShowLeader(int leader){

        leaders[leader].SetActive(true);
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
}
