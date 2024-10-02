using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour{

    public float speed;
    public float speedSprint;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    public CharacterAnimator animator;

    public bool IsMoving { get; set; }

    private void Awake(){
        animator = GetComponent<CharacterAnimator>();
    }

    public void HandleUpdate(){
        animator.IsMoving = IsMoving;
    }

    public IEnumerator Move(Vector2 moveVector){
        animator.IsMoving = IsMoving;

        animator.MoveX = moveVector.x;
        animator.MoveY = moveVector.y;

        var targetPosition = transform.position;
        targetPosition += (Vector3)moveVector * 0.5f;

        if(!IsWalkable(targetPosition)){
            yield break;
        }

        IsMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon){

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;

        IsMoving = false;
    }

    private bool IsWalkable(Vector3 targetPosition){

        if(Physics2D.OverlapCircle(targetPosition, 0.3f, solidObjectsLayer | interactableLayer) != null){
            return false;
        }
            return true;
    }
}