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

        // Replace Drifter Recycle Tooltip
        // TODO: YOUR TURN BUDDY!

        RecalculateStatsAPI.GetStatCoefficients += Stats.ApplyTrashStats;
        DrifterTrashToTreasureController.UpdateBuffCounts += (orig, self) =>
        {
            orig(self);

            if (!NetworkServer.active)
                return;

            var body = self.body;
            if (!body || !body.inventory)
                return;

            var count =
                body.inventory.GetItemCountEffective(RoR2Content.Items.ScrapWhite) +
                body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapWhiteSuppressed);

            body.SetBuffCount(DLC3Content.Buffs.TrashToTreasureWhite.buffIndex, count);

            body.RecalculateStats();
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(DLC1Content.Items.ScrapWhiteSuppressed.itemIndex), transform.position, transform.up * 10f);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapWhite.itemIndex), transform.position, transform.up * 10f);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
            PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapGreen.itemIndex), transform.position, transform.up * 10f);
        }
    }
}