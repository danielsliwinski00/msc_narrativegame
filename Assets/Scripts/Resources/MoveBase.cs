using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move", menuName ="MartialArtist/Create new move")]
public class MoveBase : ScriptableObject{

    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] MartialArtistType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int uses;

    public string Name {
        get { return name; }
    }
    public string Description {
        get { return description; }
    }
    public MartialArtistType Type {
        get { return type; }
    }
    public int Power {
        get { return power; }
    }
    public int Accuracy {
        get { return accuracy; }
    }
    public int Uses {
        get { return uses; }
    }
}
