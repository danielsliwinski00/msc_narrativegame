using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomMove : MonoBehaviour{
    public Vector2 cameraChange;
    public Vector3 playerChange;
    private CameraMovement cameraMovement;
    public bool needText;
    public string placeName;
    public GameObject placeNameTextBox;
    public TextMeshProUGUI placeNameTMP;
    [SerializeField] AudioSource[] audioSources;
    [SerializeField] int currentMusic;
    [SerializeField] int destinationMusic;
    void Awake(){
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
    }

    void Update(){
        
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other){

        if(other.CompareTag("Player")){

            cameraMovement.smoothing = 1f;
            yield return new WaitForSeconds(0.2f);

            cameraMovement.minPosition += cameraChange;
            cameraMovement.maxPosition += cameraChange;
            other.transform.position += playerChange;

            placeNameTMP.text = "";
            GameController.GetInstance().lastPlayed = destinationMusic;
            audioSources[currentMusic].Pause();
            audioSources[destinationMusic].Play();
            yield return new WaitForSeconds(0.2f);
            cameraMovement.smoothing = 0.05f;

            StartCoroutine(PlaceNameCo());
        }
    }

    private IEnumerator PlaceNameCo(){

        placeNameTextBox.SetActive(true);
        placeNameTMP.text = placeName;
        yield return new WaitForSeconds(2f);

        placeNameTextBox.SetActive(false);
    }
}
