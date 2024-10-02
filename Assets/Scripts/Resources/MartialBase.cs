using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MartialArtist", menuName = "MartialArtist/Create new MartialArtist")]
public class MartialBase : ScriptableObject{
    [SerializeField] string name;
    [SerializeField] Sprite frontSprite;
    [SerializeField] int level;

    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    [SerializeField] MartialArtistType type1;

    [SerializeField] List<LearnableMoves> learnableMoves;

    public string Name {
        get { return name; }
    }
    public int Level {
        get { return level; }
    }
    public Sprite FrontSprite {
        get { return frontSprite; }
    }
    public int MaxHP {
        get { return maxHP; }
    }
    public int Attack {
        get { return attack; }
    }
    public int Defense {
        get { return defense; }
    }
    public int SpAttack {
        get { return spAttack; }
    }
    public int SpDefense {
        get { return spDefense; }
    }
    public int Speed {
        get { return speed; }
    }
    public MartialArtistType Type1 {
        get { return type1; }
    }
    public List<LearnableMoves> LearnableMoves {
        get { return learnableMoves; }
    }
}

[System.Serializable]
public class LearnableMoves{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base {
        get { return moveBase; }
    }
    public int Level {
        get { return level; }
    }
}

public enum MartialArtistType{
    None,
    Heal,
    Normal,
    Fire,
    Water,
    Grass
}

public class TypeChart{
    static float[][] chart = {
        //                      NORMAL, FIRE, WATER, GRASS
        /*NORMAL*/  new float[] {1f,    1f,   1f,    1f},
        /*FIRE*/    new float[] {1f,    0.5f, 0.5f,  2f},
        /*WATER*/   new float[] {1f,    2f,   0.5f,  0.5f},
        /*GRASS*/   new float[] {1f,    0.5f, 2f,    0.5f},
    };

    public static float GetEffectiveness(MartialArtistType attackType, MartialArtistType defenseType){

        if(attackType == MartialArtistType.None || defenseType == MartialArtistType.None){
            return 1;
        }

        int row = (int)attackType - 2;
        int col = (int)defenseType - 2;
        return chart[row][col];
    }
}
