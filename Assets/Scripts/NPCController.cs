using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using System.Numerics;

public class NPCController : MonoBehaviour, Interactable{

    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;
    [SerializeField] public string speaker;
    [SerializeField] public Sprite sprite;

    [Header("MartialArtist")]
    [SerializeField] public MartialArtist martialArtist;
    [SerializeField] public bool isHostile;
    [SerializeField] public bool isHostileBeforeDialogue;
    [SerializeField] public bool dialogueRepeat;

    [Header("DialogueTrigger")]
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private GameObject dialogueIcon;

    int dialogueRepeated = 0;
    UnityEngine.Vector3 originalPosition;

    [SerializeField] Animator animator;

    public IEnumerator Leave(){
        //transform.localPosition = new UnityEngine.Vector3(500f, originalPosition.y);
        transform.DOLocalMoveY(200f, 7f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    public IEnumerator LeaveX(){
        //transform.localPosition = new UnityEngine.Vector3(500f, originalPosition.y);
        dialogueTrigger.gameObject.SetActive(false);
        transform.DOLocalMoveX(200f, 7f);
        dialogueIcon.SetActive(false); 
        yield return new WaitForSeconds(1f);     
        gameObject.SetActive(false);

    }

    void Start(){
        originalPosition = transform.localPosition;
        if(isHostile){
            martialArtist.Init();
        }
    }

    void Update(){
    }

    public void Interact(){
                    
        if(!dialogueRepeat && dialogueRepeated > 0){
            return;
        }
        
        if(isHostileBeforeDialogue){
            StartCoroutine(DialogueManager.GetInstance().BattleSceneStart());
        }
        else{           
            if(!dialogueRepeat && dialogueRepeated > 0){
                return;
            }
            if(!dialogueRepeat){
                dialogueTrigger.dialogueIcon.SetActive(false);
                dialogueTrigger.gameObject.SetActive(false);
                dialogueRepeated = 1;

                if(DialogueManager.GetInstance().DialogueIsPlaying){
                    return;
                }
                else if(inkJSON != null){
                    StartCoroutine(DialogueManager.GetInstance().EnterDialogueMode(inkJSON, speaker, sprite, 0));
                }
            }
            else{
                if(DialogueManager.GetInstance().DialogueIsPlaying){
                    return;
                }
                else if(inkJSON != null){
                    StartCoroutine(DialogueManager.GetInstance().EnterDialogueMode(inkJSON, speaker, sprite, 0));
                }
            }
        }
    }

    public MartialArtist MartialArtist(){
        if(isHostile){
            return martialArtist;
        }
        else{
            return null;
        }
    }
}
