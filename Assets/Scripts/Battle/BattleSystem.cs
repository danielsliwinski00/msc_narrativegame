using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BattleState { Start, PlayerTurn, PlayerMove, EnemyMove, Busy}

public enum BattleAction { Move, UseItem, Run}

public class BattleSystem : MonoBehaviour{

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogueBox dialogueBox;

    [SerializeField] AudioSource battleMusic;
    [SerializeField] AudioSource victoryMusic;
    [SerializeField] AudioSource defeatMusic;
    [SerializeField] AudioSource buttonSwitch;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;

    MartialArtist playerMartialArtist;
    MartialArtist enemyMartialArtist;

    public void StartBattle(MartialArtist playerMartialArtist, MartialArtist enemyMartialArtist){
        //Debug.Log(playerMartialArtist.Base.Name+", "+enemyMartialArtist.Base.Name);
        if(playerMartialArtist.HP <= 0){
            playerMartialArtist.HP += 1;
        }

        this.playerMartialArtist = playerMartialArtist;
        this.enemyMartialArtist = enemyMartialArtist;

        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle(){
        battleMusic.Play(0);
        //Debug.Log(playerMartialArtist.Base.Name + ", " + enemyMartialArtist.Base.Name);

        playerUnit.Setup(playerMartialArtist);
        enemyUnit.Setup(enemyMartialArtist);

        playerHud.SetData(playerUnit.MartialArtist);
        enemyHud.SetData(enemyUnit.MartialArtist);

        dialogueBox.SetMoveNames(playerUnit.MartialArtist.Moves);

        yield return dialogueBox.TypeDialogue($"{enemyUnit.MartialArtist.Base.Name} has initiated a duel!");

        PlayerTurn();
    }

    void PlayerTurn(){
        state = BattleState.PlayerTurn;
        StartCoroutine(dialogueBox.TypeDialogue("Choose your next move..."));
        dialogueBox.EnableActions(true);
    }

    void PlayerMove(){
        state = BattleState.PlayerMove;
        dialogueBox.EnableActions(false);
        dialogueBox.EnableDialogueText(false);
        dialogueBox.EnableMoves(true);
    }

    IEnumerator PerformPlayerMove(){
        state = BattleState.Busy;

        var move = playerUnit.MartialArtist.Moves[currentMove];
        move.Uses--;

        if(move.Base.Type == MartialArtistType.Heal){
            playerUnit.MartialArtist.HealSelf(move, playerUnit.MartialArtist);
            playerUnit.PlayHealAnimation();
            yield return playerHud.UpdateHP();
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyMove());
        }
        else{
            yield return dialogueBox.TypeDialogue($"You used {move.Base.Name}");
            playerUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            enemyUnit.PlayHitAnimation();

            var damageDetails = enemyUnit.MartialArtist.TakeDamage(move, playerUnit.MartialArtist);
            yield return enemyHud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);

            if(damageDetails.Defeated){
                yield return dialogueBox.TypeDialogue($"{enemyUnit.MartialArtist.Base.Name} has been defeated!");
                enemyUnit.PlayDefeatedAnimation();
                battleMusic.Pause();
                victoryMusic.Play(0);
                yield return new WaitForSeconds(5f);
                OnBattleOver(true);
            }
            else{
                StartCoroutine(EnemyMove());
            }
        }
    }

    IEnumerator EnemyMove(){
        state = BattleState.EnemyMove;
        var move = enemyUnit.MartialArtist.GetRandomMove();
        move.Uses--;

        yield return dialogueBox.TypeDialogue($"{enemyUnit.MartialArtist.Base.Name} used {move.Base.Name}");
        enemyUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        playerUnit.PlayHitAnimation();

        var damageDetails = playerUnit.MartialArtist.TakeDamage(move, enemyUnit.MartialArtist);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);


        if(damageDetails.Defeated){
            yield return dialogueBox.TypeDialogue($"You have been defeated!");
            playerUnit.PlayDefeatedAnimation();
            battleMusic.Pause();
            defeatMusic.Play(0);
            yield return new WaitForSeconds(3f);
            OnBattleOver(false);
        }
        else{
            PlayerTurn();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails){

        if(damageDetails.Critical > 1){
            yield return dialogueBox.TypeDialogue($"A critical hit!");
        }
        if(damageDetails.TypeEffectiveness > 1){
            yield return dialogueBox.TypeDialogue($"It's super effective!");
        }
        else if(damageDetails.TypeEffectiveness < 1){
            yield return dialogueBox.TypeDialogue($"It's not very effective...");
        }
    }

    public void HandleUpdate(){
        if(state == BattleState.PlayerTurn){
            StartCoroutine(HandleActionSelection());
        }
        else if(state == BattleState.PlayerMove){
            HandleMoveSelection();
        }
    }

    IEnumerator HandleActionSelection(){
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            if(currentAction < 1){
                currentAction++;
                buttonSwitch.Play();
            }
        }
        else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            if(currentAction > 0){
                currentAction--;
                buttonSwitch.Play();
            }
        }

        dialogueBox.UpdateActionSelection(currentAction);

        if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Z)){
            if(currentAction == 0){
                PlayerMove();
            }
            else if(currentAction == 1){
                var playerUnitSpeed = playerMartialArtist.Base.Speed;
                var enemyUnitSpeed = playerMartialArtist.Base.Speed;

                float odds = playerUnitSpeed / enemyUnitSpeed;
                float chance = 60f*odds;
                if(UnityEngine.Random.Range(0f, 100.0f) <= chance){
                    yield return dialogueBox.TypeDialogue("You ran away");
                    OnBattleOver(false);
                }
                else{
                    yield return dialogueBox.TypeDialogue("You failed to run away");
                    yield return EnemyMove();
                }
            }
        }
    }

    void HandleMoveSelection(){

        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            if(currentMove < playerUnit.MartialArtist.Moves.Count - 1){
                currentMove++;
                buttonSwitch.Play();
            }
        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            if(currentMove > 0){
                currentMove--;
                buttonSwitch.Play();
            }
        }
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            if(currentMove < playerUnit.MartialArtist.Moves.Count - 2){
                currentMove += 2;
                buttonSwitch.Play();
            }
        }
        else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            if(currentMove > 1){
                currentMove -= 2;
                buttonSwitch.Play();
            }
        }

        dialogueBox.UpdateMoveSelection(currentMove, playerUnit.MartialArtist.Moves[currentMove]);

        if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Z)){
            if(playerUnit.MartialArtist.Moves[currentMove].Uses < 1){
                HandleMoveSelection();
            }else{
                dialogueBox.EnableMoves(false);
                dialogueBox.EnableDialogueText(true);
                StartCoroutine(PerformPlayerMove());
            }
        }
    }
}
