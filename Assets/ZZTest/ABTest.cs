using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ABTest : MonoBehaviour
{
    public List<Image> list;
    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/StreamingAssets/assets/buildres/atlas/battleuisa.ab";
        var ab = AssetBundle.LoadFromFile(path);

        var sa = ab.LoadAsset<SpriteAtlas>("BattleUI");
        
        list[0].sprite = sa.GetSprite("img_battle_hp_04");
        list[1].sprite = sa.GetSprite("img_battle_attr_bar_bg");
        list[2].sprite = sa.GetSprite("img_battle_hp_06");

        // var s1 = ab.LoadAsset<Sprite>("img_battle_hp_04");
        // var s2 = ab.LoadAsset<Sprite>("img_battle_hp_05");
        // var s3 = ab.LoadAsset<Sprite>("img_battle_hp_06");
        //
        // list[0].sprite = s1;
        // list[1].sprite = s2;
        // list[2].sprite = s3;
    }

    // Update is called once per frame
    void Update()
    {
    }
}   