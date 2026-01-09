using BepInEx;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using DrifterTrashToTreasureController = On.RoR2.DrifterTrashToTreasureController;
using SteamUserManager = IL.RoR2.SteamUserManager;

namespace StrangeDrifter
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class StrangeDrifter : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "BismuthWin";
        public const string PluginName = "StrangeDrifter";
        public const string PluginVersion = "1.0.0";

        private ItemTier whiteTier = ItemTier.Tier1;
        private ItemTier greenTier = ItemTier.Tier2;
        private ItemTier redTier = ItemTier.Tier3;

        private void ApplyTrashStats(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!body.inventory)
                return;

            var strangeWhiteScrapCount = body.inventory.GetItemCountEffective(DLC1Content.Items.ScrapWhiteSuppressed);
            var statsFromScrapCount = body.inventory.GetItemCountEffective(DLC3Content.Items.StatsFromScrap);

            if (strangeWhiteScrapCount <= 0 || statsFromScrapCount <= 0)
                return;

            var bonus = 0.06f * strangeWhiteScrapCount * statsFromScrapCount;

            args.moveSpeedMultAdd += bonus;
        }


        public void Awake()
        {
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

            RecalculateStatsAPI.GetStatCoefficients += ApplyTrashStats;

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
}