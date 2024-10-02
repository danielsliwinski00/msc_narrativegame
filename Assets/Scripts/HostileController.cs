using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostileController : MonoBehaviour{

    [SerializeField] private GameObject dialogueIcon;

    bool inRange;
    Image image;
    public Sprite sprite;

    public IEnumerator StartBattle(){

        yield return new WaitForSeconds(1f);
    }

    private void Awake(){
        image = GetComponent<Image>();
        inRange = false;
        dialogueIcon.SetActive(false);
        GetComponent<Image>().sprite = sprite;
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
