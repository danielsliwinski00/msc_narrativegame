using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("DialogueIcon")]
    [SerializeField] public GameObject dialogueIcon;

    private bool inRange;

    private void Awake(){
        inRange = false;
        dialogueIcon.SetActive(false);
    }

    private void Update(){

        if(inRange){
            dialogueIcon.SetActive(true);
        }
        else{
            dialogueIcon.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Player")){
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Player")){
            inRange = false;
        }
    }
}
