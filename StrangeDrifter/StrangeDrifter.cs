using BepInEx;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using DrifterTrashToTreasureController = On.RoR2.DrifterTrashToTreasureController;

namespace StrangeDrifter;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class StrangeDrifter : BaseUnityPlugin
{
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "BismuthWin";
    public const string PluginName = "StrangeDrifter";
    public const string PluginVersion = "1.0.0";

    private const ItemTier whiteTier = ItemTier.Tier1;
    private const ItemTier greenTier = ItemTier.Tier2;
    private const ItemTier redTier = ItemTier.Tier3;




    public void Awake()
    {
        // Replace suppressed scrap tiers to match normal scrap tiers
        var strangeWhiteScrap = LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + nameof(DLC1Content.Items.ScrapWhiteSuppressed));
        var strangeGreenScrap = LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + nameof(DLC1Content.Items.ScrapGreenSuppressed));
        var strangeRedScrap = LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + nameof(DLC1Content.Items.ScrapRedSuppressed));
        if (strangeWhiteScrap)
        {
            strangeWhiteScrap.tier = whiteTier;
            strangeWhiteScrap.deprecatedTier = whiteTier;
        }

        if (strangeGreenScrap)
        {
            strangeGreenScrap.tier = greenTier;
            strangeGreenScrap.deprecatedTier = greenTier;
        }

        if (strangeRedScrap)
        {
            strangeRedScrap.tier = redTier;
            strangeRedScrap.deprecatedTier = redTier;
        }

        // COMMENTED OUT BECAUSE WE HAVENT DONE CUSTOM BUFFS FOR STRANGE SCRAP YET (WILL BE ADDED AS CONFIG LATER)
        //LanguageAPI.Add("DRIFTER_RECYCLE_TOOLTIP",
            //$"poopen fartern shitten<style=cKeywordName>Recycle</style><style=cSub>Gain unique stats from each Item Scrap tier:\r\nCommon - <style=cIsUtility>+6% move speed</style> <style=cStack>(per stack)</style>.\r\nUncommon - <style=cIsHealing>+3 hp/s base health regeneration</style> <style=cStack>(per stack)</style>.\r\nLegendary - <style=cIsDamage>+30% attack speed</style> <style=cStack>(per stack)</style>.\r\nBoss - <style=cIsUtility>-15% skill cooldowns</style> <style=cStack>(per stack)</style>.</style>");

        RecalculateStatsAPI.GetStatCoefficients += Stats.ApplyStrangeScrapStats;
        DrifterTrashToTreasureController.UpdateBuffCounts += (orig, self) =>
        {
            orig(self);

            if (!NetworkServer.active)
                return;

            var body = self.body;
            if (!body || !body.inventory)
                return;
            //white scrap buff
            var whiteCount =
                body.inventory.GetItemCountEffective(RoR2Content.Items.ScrapWhite) +
                body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapWhiteSuppressed);

            body.SetBuffCount(DLC3Content.Buffs.TrashToTreasureWhite.buffIndex, whiteCount);
            //green scrap buff
            var greenCount =
                body.inventory.GetItemCountEffective(RoR2Content.Items.ScrapGreen) +
                body.inventory.GetItemCountEffective(DLC1Content.Items.RegeneratingScrap) +
                body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapGreenSuppressed);
            
            body.SetBuffCount(DLC3Content.Buffs.TrashToTreasureGreen.buffIndex, greenCount);
            //red scrap buff
            var redCount =
                body.inventory.GetItemCountEffective(RoR2Content.Items.ScrapRed) +
                body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapRedSuppressed);
            
            body.SetBuffCount(DLC3Content.Buffs.TrashToTreasureRed.buffIndex, redCount);
            
            body.RecalculateStats();
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(DLC1Content.Items.ScrapRedSuppressed.itemIndex), transform.position, transform.up * 10f);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapWhite.itemIndex), transform.position, transform.up * 10f);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapRed.itemIndex), transform.position, transform.up * 10f);
        }
    }
}