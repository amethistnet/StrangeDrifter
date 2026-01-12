using System.Reflection;
using RoR2;
using R2API;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace StrangeDrifter;

public static class Buffs
{
    public static BuffDef strangeWhiteScrapBuff;
    public static void StrangeBuffs()
    {
        strangeWhiteScrapBuff = ScriptableObject.CreateInstance<BuffDef>();
        strangeWhiteScrapBuff.name = "Strange White Buff";
        strangeWhiteScrapBuff.buffColor = new Color32(166, 20, 20, 255);
        strangeWhiteScrapBuff.canStack = true;
        strangeWhiteScrapBuff.isCooldown = false;
        strangeWhiteScrapBuff.isDebuff = false;
        strangeWhiteScrapBuff.iconSprite = StrangeDrifter.mainBundle.LoadAsset<Sprite>("BuffIconTemplate");
        
        ContentAddition.AddBuffDef(strangeWhiteScrapBuff);
    }
}