using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace SettlementRename
{
    /**
     * Save store behaviour.
     */
    public class SaveDataStore : CampaignBehaviorBase
    {
        public static Dictionary<String, String> _customDataMap = new Dictionary<string, string>();

        public override void RegisterEvents()
        {
            // Update settlement names on load.
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, starter =>
            {
                if (_customDataMap != null)
                {
                    foreach (var currentSettlement in Campaign.Current.Settlements)
                    {
                        if (_customDataMap.ContainsKey(currentSettlement.Id.ToString()))
                        {
                            ToolsHelper.SetSettlementName(currentSettlement,
                                _customDataMap[currentSettlement.Id.ToString()], false);
                        }
                    }
                }
            });
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("settlementNames", ref _customDataMap);
        }

    }
   
    /**
     * Type definer.
     */
    public class SaveDefiner : SaveableTypeDefiner
    {
        public SaveDefiner() : base(991337077)
        {
        }

        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(SettlementNameInfo), 13370077);
        }

        protected override void DefineGenericClassDefinitions()
        {
            
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(List<SettlementNameInfo>));
        }
    }
}