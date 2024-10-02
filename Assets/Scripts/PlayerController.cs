using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 input;
    private CharacterAnimator animator;
    public LayerMask combatLayer, interactableLayer, solidObjectsLayer;
    public List<string> Inventory;
    [SerializeField] public Quest[] Quests;
    [SerializeField] public MartialArtist martialArtist;
    [SerializeField] public MartialArtist enemyMartialArtist;
    public NPCController interactedWith;
    public float speed, speedSprint;
    bool isMoving;
    bool helpShown;
    [SerializeField] public GameObject controlsMenu, helpTextMenu;
    [SerializeField] public TextMeshProUGUI helpText;

    public float OffsetY { get; private set; } = 0.3f;

    public bool IsSwitchingRoom { get; set; } = false;

    private Rigidbody2D playerRigidBody;

    public event Action OnBattle;

    void Awake(){
        animator = GetComponent<CharacterAnimator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        martialArtist.Init();
        SetPosition(transform.position);
        helpShown = false;
    }

    public void HandleUpdate(){
        //input = Vector3.zero;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if(!helpShown && Input.GetKeyDown(KeyCode.F1)){
            helpShown = true;
            helpText.text = "Press F1 to Hide";
            controlsMenu.SetActive(true);
        }
        else if(helpShown && Input.GetKeyDown(KeyCode.F1)){
            helpShown = false;
            helpText.text = "Press F1 for help";
            controlsMenu.SetActive(false);
        }

        if(Input.GetKey(KeyCode.LeftShift) && input != Vector3.zero || Input.GetKey(KeyCode.RightShift) && input != Vector3.zero){
            
            animator.MoveX = input.x;
            animator.MoveY = input.y; 
            animator.IsMoving = true;

            var targetPosition = transform.position;
            targetPosition += (Vector3)input * 0.5f;

            if(IsWalkable(targetPosition)){
                SprintC();
            }
        }
        else if(input != Vector3.zero){
            
            animator.MoveX = input.x;
            animator.MoveY = input.y; 
            animator.IsMoving = true;

            var targetPosition = transform.position;
            targetPosition += (Vector3)input * 0.5f;
            
            if(IsWalkable(targetPosition)){
                MoveC();
            }
        }
        else{
            animator.IsMoving = false;
        }

        if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Z)){
            Interact();
        }

       
    }

    public void SetPosition(Vector2 position){
        position.x = Mathf.Floor(position.x) + 0.5f;
        position.y = Mathf.Floor(position.y) + 0.5f + OffsetY;
    }

    void MoveC(){
        playerRigidBody.MovePosition(
            transform.position + speed * Time.fixedDeltaTime * input.normalized
        );
    }
    void SprintC(){
        playerRigidBody.MovePosition(
            transform.position + speedSprint * Time.fixedDeltaTime * input.normalized
        );
    }

    void Interact(){
        var faceDirection = new Vector3(animator.MoveX, animator.MoveY);
        var interactPosition = transform.position + faceDirection;

        var collider = Physics2D.OverlapCircle(interactPosition, 0.2f, interactableLayer);
        if(collider != null){
            enemyMartialArtist = collider.GetComponent<Interactable>()?.MartialArtist();
            interactedWith = collider.GetComponent<NPCController>();
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    private bool IsWalkable(Vector3 targetPosition){

        if(Physics2D.OverlapCircle(targetPosition, 0.1f, solidObjectsLayer | interactableLayer) != null){
            return false;
        }
            return true;
    }
}

[Serializable]
public class Quest{
    [SerializeField] public string questName;
    [SerializeField] public string questState;
}
/*


*/

