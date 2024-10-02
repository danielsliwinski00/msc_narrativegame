using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.PlayerLoop;

[System.Serializable]
public class MartialArtist{
    [SerializeField] MartialBase uBase;
    [SerializeField] public int level;

    public MartialBase Base {
        get{
            return uBase;
        }
    }
    public int Level {
        get{
            return level;
        }
        set{
            level = value;
        }
    }
    
    public int HP { get; set; }
    public List<Move> Moves { get; set; }

    public void Init(){
        HP = MaxHP;

        Moves = new List<Move>();
        foreach(var move in Base.LearnableMoves){

            if(move.Level <= Level){
                Moves.Add(new Move(move.Base));
            }

            if(Moves.Count >= 4){
                break;
            }
        }
    }

    public int Attack{
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5;}
    }
    public int Defense{
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5;}
    }
    public int SpAttack{
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5;}
    }
    public int SpDefense{
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5;}
    }
    public int Speed{
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5;}
    }
    public int MaxHP{
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10;}
    }

    public void HealSelf(Move move, MartialArtist user){

        if(move.Base.Type == MartialArtistType.Heal){
            float modifiers = UnityEngine.Random.Range(0.85f, 1f);
            float a = (2 * user.Level + 10) / 250f;
            float d = a * move.Base.Power * ((float)user.Attack / Defense) + 2;
            int heal = Mathf.FloorToInt(d * modifiers);

            if(user.HP+heal > MaxHP){
                user.HP = MaxHP;
            }
            else{
                user.HP += heal;
            } 
        }
    }

    public DamageDetails TakeDamage(Move move, MartialArtist attacker){

        float critical = 1f;
        if(UnityEngine.Random.value * 100f <= 6.25f){
            critical = 2f;
        }

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1);

        var damageDetails = new DamageDetails(){
            TypeEffectiveness = type,
            Critical = critical,
            Defeated = false
        };

        float modifiers = UnityEngine.Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;

        if(HP <= 0){
            HP = 0;
            damageDetails.Defeated = true;
        }

        return damageDetails;
    }

    public Move GetRandomMove(){
        int r = UnityEngine.Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails{
    public bool Defeated { get; set;}
    public float TypeEffectiveness { get; set;}
    public float Critical { get; set;}
}
