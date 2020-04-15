using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SandBox.GauntletUI.Map;
using SandBox.ViewModelCollection.Nameplate;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SettlementRename
{
    public static class ToolsHelper
    {
        /**
         * Helper method that renames settlement.
         */
        public static void SetSettlementName(Settlement sourceSettlement, string newName, bool updateGlobalMap = true)
        {
            if (updateGlobalMap)
            {
                var globalMapScene = SandBox.View.Map.MapScreen.Instance.GetMapView<GauntletMapSettlementNameplate>();
                SettlementNameplatesVM plates = (SettlementNameplatesVM) globalMapScene.GetType()
                        .GetField("_dataSource", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.GetValue(globalMapScene);

                foreach (var curPlate in plates.Nameplates.Where(plate => plate.Settlement == sourceSettlement))
                {
                    curPlate.Name = newName;
                    InformationManager.DisplayMessage(new InformationMessage($"Renamed"));
                }
                plates.Update();
            }

            sourceSettlement.Name = new TextObject(newName);
            if (SaveDataStore._customDataMap == null)
            {
                SaveDataStore._customDataMap = new Dictionary<string, string>();
            }

            var storage = SaveDataStore._customDataMap;
            if (storage.ContainsKey(sourceSettlement.Id.ToString()))
            {
                storage.Remove(sourceSettlement.Id.ToString());
            }

            SaveDataStore._customDataMap.Add(sourceSettlement.Id.ToString(), newName);
            Campaign.Current.EncyclopediaManager.CreateEncyclopediaPages();
        }
    }
}