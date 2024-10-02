using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour{

    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;

    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool IsSprinting { get; set; }


    SpriteAnimator walkDownAnimation;
    SpriteAnimator walkUpAnimation;
    SpriteAnimator walkLeftAnimation;
    SpriteAnimator walkRightAnimation;

    SpriteAnimator sprintDownAnimation;
    SpriteAnimator sprintUpAnimation;
    SpriteAnimator sprintLeftAnimation;
    SpriteAnimator sprintRightAnimation;

    SpriteAnimator currentAnimation;
    bool wasPreviouslyMoving;
    bool wasPreviouslySprinting;

    SpriteRenderer spriteRenderer;

    private void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnimation = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnimation = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkLeftAnimation = new SpriteAnimator(walkLeftSprites, spriteRenderer);
        walkRightAnimation = new SpriteAnimator(walkRightSprites, spriteRenderer);

        sprintDownAnimation = new SpriteAnimator(walkDownSprites, spriteRenderer, 0.08f);
        sprintUpAnimation = new SpriteAnimator(walkUpSprites, spriteRenderer, 0.08f);
        sprintLeftAnimation = new SpriteAnimator(walkLeftSprites, spriteRenderer, 0.08f);
        sprintRightAnimation = new SpriteAnimator(walkRightSprites, spriteRenderer, 0.08f);

        currentAnimation = walkDownAnimation;
    }

    private void Update(){

        var previousAnimation = currentAnimation;

        if(IsSprinting){
            if(MoveX >= 1){
                currentAnimation = sprintRightAnimation;
            }
            else if(MoveX <= -1){
                currentAnimation = sprintLeftAnimation;
            }
            else if(MoveY >= 1){
                currentAnimation = sprintUpAnimation;
            }
            else if(MoveY <= -1){
                currentAnimation = sprintDownAnimation;
            }
        }
        else{
            if(MoveX >= 1){
                currentAnimation = walkRightAnimation;
            }
            else if(MoveX <= -1){
                currentAnimation = walkLeftAnimation;
            }
            else if(MoveY >= 1){
                currentAnimation = walkUpAnimation;
            }
            else if(MoveY <= -1){
                currentAnimation = walkDownAnimation;
            }  
        }

        if(currentAnimation != previousAnimation || IsMoving != wasPreviouslyMoving || IsSprinting != wasPreviouslySprinting){
            currentAnimation.Start();
        }

        if(IsMoving){
            currentAnimation.HandleUpdate();
        }
        else{
            spriteRenderer.sprite = currentAnimation.Frames[0];
        }

        wasPreviouslyMoving = IsMoving;
        wasPreviouslySprinting = IsSprinting;
    }
}
