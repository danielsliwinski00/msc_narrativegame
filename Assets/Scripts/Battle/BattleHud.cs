using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleHud : MonoBehaviour{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HPBar hpBar;

    MartialArtist _martialArtist;

    public void SetData(MartialArtist martialArtist){
        _martialArtist = martialArtist;
        nameText.text = martialArtist.Base.Name;
        levelText.text = "Lvl "+martialArtist.Level;
        hpBar.SetHP((float) martialArtist.HP / martialArtist.MaxHP);
    }

    public IEnumerator UpdateHP(){
        yield return hpBar.SetHPSmooth((float) _martialArtist.HP / _martialArtist.MaxHP);
    }
}
