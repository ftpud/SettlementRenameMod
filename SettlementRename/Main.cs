using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SettlementRename
{
    /**
     * Entry point.
     */
    public class Main : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            // Adding save DataStore.
            Campaign campaign = game.GameType as Campaign;
            if (campaign == null) return;
            CampaignGameStarter gameInitializer = (CampaignGameStarter)gameStarterObject;
            gameInitializer.AddBehavior(new baseBehavior());
        }

        public override void OnGameEnd(Game game)
        {
            // Remove all data from static DataStore.
            baseBehavior._customDataMap = null;
        }
        
    }
}