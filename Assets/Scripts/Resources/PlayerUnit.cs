using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour{
    [SerializeField] MartialArtist martialArtist;

    private void Start(){
        martialArtist.Init();
    }

    public MartialArtist GetMartialArtist(){
        return martialArtist;
    }
}
