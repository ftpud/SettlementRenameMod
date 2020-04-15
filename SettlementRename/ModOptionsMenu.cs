using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.GauntletUI.Map;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace SettlementRename
{
    /**
     * Escape menu view override to add 'Tools' buttin.
     */
    [OverrideView(typeof(MapEscapeMenu))]
    public class ModOptionsMenu : GauntletMapEscapeMenu
    {
        public ModOptionsMenu(List<EscapeMenuItemVM> items) : base(items)
        {
            items.Add(
                new EscapeMenuItemVM(
                    new TextObject("Tools", null),
                    delegate(object o) { ShowModOptions(); },
                    null,
                    false,
                    false));
        }

        private void ShowModOptions()
        {
            ScreenManager.PushScreen(new RenameMenuScreen());
        }
    }

    /**
     * Tools menu view definition.
     */
    public class RenameMenuScreen : ScreenBase
    {
        private RenameModViewModel _dataSource;
        private GauntletLayer _gauntletLayer;
        private GauntletMovie _movie;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _dataSource = new RenameModViewModel();
            _gauntletLayer = new GauntletLayer(100)
            {
                IsFocusLayer = true
            };
            AddLayer(_gauntletLayer);
            _gauntletLayer.InputRestrictions.SetInputRestrictions();
            _movie = _gauntletLayer.LoadMovie("SettlementRenameMenu", _dataSource);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ScreenManager.TrySetFocus(_gauntletLayer);
            _dataSource.RefreshValues();
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            _gauntletLayer.IsFocusLayer = false;
            ScreenManager.TryLoseFocus(_gauntletLayer);
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            RemoveLayer(_gauntletLayer);
            _dataSource = null;
            _gauntletLayer = null;
        }
    }

    /**
     * Tools menu ViewModel.
     */
    public class RenameModViewModel : ViewModel
    {
        public RenameModViewModel()
        {
            RefreshProperties();
        }

        [DataSourceProperty]
        public bool SettlementRenameDisabled => Hero.MainHero.CurrentSettlement == null ||
                                                !Clan.PlayerClan.Settlements.Contains(Hero.MainHero.CurrentSettlement);

        private void ExitOptionsMenu()
        {
            ScreenManager.PopScreen();
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

        private void RefreshProperties()
        {
            OnPropertyChanged("SettlementRenameDisabled");
        }

        public sealed override void RefreshValues()
        {
            base.RefreshValues();
            RefreshProperties();
        }
    }
}