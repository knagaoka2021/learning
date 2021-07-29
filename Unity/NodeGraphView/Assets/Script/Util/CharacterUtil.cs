using System;
using System.Collections.Generic;
using CharacterDesign;
using UnityEngine;

namespace CUtil {
    public enum CHARASTS //CHARACTER_STATUS
    {
        NONE,
        HP,
        SP,
        STR,
        VIT,
        DEX,
        SPD,
        TOTALHP
    }

    public static class CharacterUtil {
        public static Dictionary<CHARASTS, Func<Character,int>> CharaStsDictionary
        = new Dictionary<CHARASTS, Func<Character, int>>(){
            {CHARASTS.HP,(Character chara) => {return chara.Hp;}},
            {CHARASTS.SP,(Character chara) => {return chara.Sp;}},
            {CHARASTS.STR,(Character chara) => {return chara.Str;}},
            {CHARASTS.VIT,(Character chara) => {return chara.Vit;}},
            {CHARASTS.DEX,(Character chara) => {return chara.Dex;}},
            {CHARASTS.SPD,(Character chara) => {return chara.Spd;}},
            {CHARASTS.TOTALHP,(Character chara) => {return chara.TotalHp;}}
        };
    }

}