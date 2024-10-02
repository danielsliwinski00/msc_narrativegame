using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialogue, Transition, Cutscene }
public class GameController : MonoBehaviour{

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] CutsceneManager cutsceneManager;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] Camera worldCamera;
    [SerializeField] public AudioSource[] worldMusic;
    [SerializeField] public CutsceneComponents cutscene;
    [SerializeField] public TextAsset introINK;
    //[SerializeField] public CutsceneComponents goodEnding;
    [SerializeField] public TextAsset goodINK;
    //[SerializeField] public CutsceneComponents neutralEnding;
    [SerializeField] public TextAsset neutralINK;
    //[SerializeField] public CutsceneComponents badEnding;
    [SerializeField] public TextAsset badINK;

    [SerializeField] public GameObject transition;

    string currentCutScene;
    public int lastPlayed = 9;
    LayerMask InteractableLayer;
    public GameState state;
    static GameController instance;

    GameState stateBeforeTransition;

    private IEnumerator Start(){
        worldMusic[lastPlayed].Play();
        playerController.OnBattle += StartBattle;

        dialogueManager.OnBattle += StartBattle;
        dialogueManager.OnDialogue += StartDialogue;
        dialogueManager.OnFreeRoam += StartFreeRoam;
        dialogueManager.OnCutscene += StartCutscene;

        battleSystem.OnBattleOver += EndBattle;

        cutscene.OnCutsceneOver += EndCutscene;
        
        instance = this;

        StartFreeRoam();
        transition.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        transition.SetActive(false);
        //StartFreeRoam();
        StartCutscene("intro");
    }

    public static GameController GetInstance(){
        return instance;
    }

    public void StartBattle(){
        playerController.controlsMenu.SetActive(false);
        playerController.helpTextMenu.SetActive(false);
        EndDialogue();
        StartCoroutine(Transition());
        worldMusic[lastPlayed].Pause();
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerUnit = playerController.martialArtist;
        var enemyUnit = playerController.enemyMartialArtist;

        battleSystem.StartBattle(playerUnit, enemyUnit);
    }

    void StartFreeRoam(){
        //playerController.controlsMenu.SetActive(true);
        playerController.helpTextMenu.SetActive(true);
        playerController.martialArtist.Init();
        StartCoroutine(Transition());
        cutsceneManager.gameObject.SetActive(false);
        if(!worldMusic[lastPlayed].isPlaying){
            worldMusic[lastPlayed].Play();
        }
        state = GameState.FreeRoam;
    }
    
    void StartDialogue(){
        playerController.helpTextMenu.SetActive(false);
        playerController.controlsMenu.SetActive(false);
        state = GameState.Dialogue;
        dialogueManager.gameObject.SetActive(true);
    }

    void EndDialogue(){
        dialogueManager.gameObject.SetActive(false);
    }

    public IEnumerator Transition(){
        //transition.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        //transition.SetActive(false);
    }

    void EndBattle(bool won){
        worldMusic[lastPlayed].Play();
        state = GameState.FreeRoam;
        StartCoroutine(Transition());
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        
        if(won){
            if(playerController.interactedWith.inkJSON != null){
                state = GameState.Dialogue;
                playerController.martialArtist.Level += 1;
                StartCoroutine(dialogueManager.EnterDialogueMode(playerController.interactedWith.inkJSON, playerController.interactedWith.speaker, playerController.interactedWith.sprite, 1));
            }
            else{
                playerController.martialArtist.Level += 1;
                StartCoroutine(playerController.interactedWith.Leave());
            }
        }
        else{
            if(playerController.interactedWith.inkJSON != null){
                //state = GameState.Dialogue;
                StartCoroutine(dialogueManager.EnterDialogueMode(playerController.interactedWith.inkJSON, playerController.interactedWith.speaker, playerController.interactedWith.sprite, 2));
            }
            else{
                Debug.Log("here");
                playerController.enemyMartialArtist.HP = playerController.enemyMartialArtist.MaxHP;
            }
        }
    }

    public void EndCutscene(){
        worldMusic[9].Pause();
        if(currentCutScene == "intro"){
            cutscene.gameObject.SetActive(false);
            cutsceneManager.gameObject.SetActive(false);
            worldCamera.gameObject.SetActive(true);
            lastPlayed = 7;
            state = GameState.FreeRoam;
        }
        else{
            Application.Quit();
        }
    }

    public void StartCutscene(string cutsceneName){
        playerController.helpTextMenu.SetActive(false);
        playerController.controlsMenu.SetActive(false);
        worldMusic[lastPlayed].Pause();
        worldMusic[9].Play();
        currentCutScene = cutsceneName;
        worldCamera.gameObject.SetActive(false);
        cutsceneManager.gameObject.SetActive(true);

        state = GameState.Cutscene;
        switch(cutsceneName){
            case "intro":
                cutscene.gameObject.SetActive(true);
                StartCoroutine(cutscene.EnterDialogueMode(introINK));
                break;
            case "goodEnding":
                cutscene.gameObject.SetActive(true);
                StartCoroutine(cutscene.EnterDialogueMode(goodINK));
                break;
            case "neutralEnding":
                cutscene.gameObject.SetActive(true);
                StartCoroutine(cutscene.EnterDialogueMode(neutralINK));
                break;
            case "badEnding":
                cutscene.gameObject.SetActive(true);
                StartCoroutine(cutscene.EnterDialogueMode(badINK));
                break;
        }
    }

    private void Update(){
        if (state == GameState.FreeRoam){
            //DialogueTrigger(true);
            playerController.HandleUpdate();
        }
        else if(state == GameState.Battle){
            //DialogueTrigger(false);
            battleSystem.HandleUpdate();        
        }
        else if(state == GameState.Dialogue){
            dialogueManager.HandleUpdate();        
        }
        else if(state == GameState.Cutscene){
            cutscene.HandleUpdate();  
        }
    }
}
