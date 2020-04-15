using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace SettlementRename
{
    /**
     * DataStore structure definition.
     */
    public class SettlementNameInfo
    {
        public SettlementNameInfo(Settlement settlement)
        {
            _settlementId = settlement.Id.ToString();
            _settlementName = settlement.Name.ToString();
        }

        public string Id => _settlementId;
        public string Name => _settlementName;
        
        [SaveableField(1)] private string _settlementId;
        [SaveableField(2)] private string _settlementName;
    }
}