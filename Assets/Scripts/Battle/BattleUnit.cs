using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class BattleUnit : MonoBehaviour{
    [SerializeField] bool isPlayerUnit;

    public MartialArtist MartialArtist { get; set; }

    Image image;
    Vector3 originalPosition;
    Color originalColor;

    private void Awake(){

        image = GetComponent<Image>();
        originalPosition = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(MartialArtist martialArtist){

        MartialArtist = martialArtist;
        image.sprite = MartialArtist.Base.FrontSprite;
        image.color = originalColor;

        PlayEnterAnimation();
    }

    public void PlayEnterAnimation(){

        if(isPlayerUnit){
            image.transform.localPosition = new Vector3(-500f, originalPosition.y);
        }
        else{
            image.transform.localPosition = new Vector3(500f, originalPosition.y);
        }

        image.transform.DOLocalMoveX(originalPosition.x, 1f);
    }

    public void PlayAttackAnimation(){
        var sequence = DOTween.Sequence();
        if(isPlayerUnit){
            sequence.Append(image.transform.DOLocalMoveY(originalPosition.y + 30f, 0.25f));
        }
        else{
             sequence.Append(image.transform.DOLocalMoveY(originalPosition.y - 30f, 0.25f));
        }

        sequence.Append(image.transform.DOLocalMoveY(originalPosition.y, 0.25f));
    }

    public void PlayHitAnimation(){
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayHealAnimation(){
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.green, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayDefeatedAnimation(){
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveX(originalPosition.x + 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}
