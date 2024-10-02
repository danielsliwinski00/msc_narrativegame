using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogueBox : MonoBehaviour{
    [SerializeField] float typingSpeed = 0.02f;
    [SerializeField] Color highlightedColor;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] GameObject actions;
    [SerializeField] GameObject moves;
    [SerializeField] GameObject movesDetails;

    [SerializeField] List<TextMeshProUGUI> actionsTexts;
    [SerializeField] List<TextMeshProUGUI> movesTexts;
    [SerializeField] TextMeshProUGUI usesText;
    [SerializeField] TextMeshProUGUI typeText;

    private Coroutine typeDialogueCoroutine;

    public void SetDialogue(string dialogue){
        dialogueText.text = dialogue;
    }
    public IEnumerator TypeDialogue(string dialogue){

        dialogueText.text = "";

        foreach(var letter in dialogue.ToCharArray()){

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(1f);
    }

    public void EnableDialogueText(bool enabled){
        dialogueText.enabled = enabled;
    }

    public void EnableActions(bool enabled){
        actions.SetActive(enabled);
    }

    public void EnableMoves(bool enabled){
        moves.SetActive(enabled);
        movesDetails.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction){
        for(int i = 0; i < actionsTexts.Count; i++){
            if(i == selectedAction){
                actionsTexts[i].color = highlightedColor;
            }
            else{
                actionsTexts[i].color = Color.black;
            }
        }
    }

    public void SetMoveNames(List<Move> moves){
        for(int i = 0; i < movesTexts.Count; i++){
            if(i< moves.Count){
                movesTexts[i].text = moves[i].Base.Name;
            }
            else{
                movesTexts[i].text = "-";
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move){
        for(int i = 0; i < movesTexts.Count; i++){
            if(i == selectedMove){
                movesTexts[i].color = highlightedColor;
            }
            else{
                movesTexts[i].color = Color.black;
            }
        }
        usesText.text = $"{move.Uses}/{move.Base.Uses}";
        typeText.text = move.Base.Type.ToString();
    }
}
