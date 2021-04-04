using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public interface IShop
    {
        double CostModifier { get; set; }

        List<Item> MerchShop { get; set; }

        int ItemPerShop { get; set; }

        void ApplyCostModifier(int mod);

        List<Item> SetupShop();

        Item MatchItemByID(int id);

        Random ShopRNG { get; set; }

        Random GetShopRNG();
    }
}
