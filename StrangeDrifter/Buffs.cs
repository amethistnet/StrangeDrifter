using UnityEngine;

namespace StrangeDrifter;

public class Buffs
{
    public override string buffIconBundle => "strangebuff";
    public void CreateBuff()
    {
        StrangeWhiteBuff = ScriptableObject.CreateInstance<RoR2.BuffDef>();
        StrangeWhiteBuff.name = "Strange White Buff";
        StrangeWhiteBuff.buffColor = Color.white;
        StrangeWhiteBuff.canStack = true;
        StrangeWhiteBuff.isDebuff = false;
        StrangeWhiteBuff.iconSprite = AssetBundle.
    }
}