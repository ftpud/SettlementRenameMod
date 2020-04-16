using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace SettlementRename
{
    /**
     * Base behaviour class.
     */
    public class baseBehavior : CampaignBehaviorBase
    {
        public static Dictionary<String, String> _customDataMap = new Dictionary<string, string>();

        public override void RegisterEvents()
        {
            // Update settlement names on load.
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, LoadSavedData);
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, AddRenameMenu);
            
        }

        private void AddRenameMenu(CampaignGameStarter obj)
        {
            RegisterMenu("town", obj);
            RegisterMenu("village", obj);
            RegisterMenu("castle", obj);
        }

        private void RegisterMenu(string menuId, CampaignGameStarter obj)
        {
            obj.AddGameMenuOption(
                menuId,
                "town_enter_entr_option",
                "Rename settlement",
                (MenuCallbackArgs args) =>
                {
                    args.optionLeaveType = GameMenuOption.LeaveType.Manage;
                    return Clan.PlayerClan.Settlements.Contains(Hero.MainHero.CurrentSettlement);
                }, (MenuCallbackArgs args) => SettlementRename(),
                false,
                4);
        }

        /**
         * Load data.
         */
        private void LoadSavedData(CampaignGameStarter obj)
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
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("settlementNames", ref _customDataMap);
        }
        
        private void SettlementRename()
        {
            var curSettlement = Hero.MainHero.CurrentSettlement;
            var nameDialog = new TextInquiryData($"Rename settlement {curSettlement.Name}",
                "Enter Name:",
                true,
                true,
                "Rename",
                "Cancel",
                (string s) => { ToolsHelper.SetSettlementName(curSettlement, s); },
                (Action) (() => { }),
                false,
                (Func<string, bool>) (s => s.Length > 0 && s.All(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x))),
                (string) null);

            InformationManager.ShowTextInquiry(nameDialog, true);
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