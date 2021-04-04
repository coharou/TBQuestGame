using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class PassiveNPC : Character, IShop
    {
        public PassiveNPC(int id, string name, int locationId, int tilePosition, Art icon, Character.Role role): 
            base(id, name, locationId, tilePosition, icon, role)
        {
            ItemPerShop = 3;
            ShopRNG = GetShopRNG();

            if (role == Role.Merchant)
            {
                MerchShop = new List<Item>();
                MerchShop = SetupShop();
            }
        }

        public double CostModifier { get; set; }

        private List<Item> _merchShop;

        public List<Item> MerchShop
        {
            get { return _merchShop; }
            set 
            { 
                _merchShop = value;
                OnPropertyChanged(nameof(MerchShop));
            }
        }

        public int ItemPerShop { get; set; }

        public Random ShopRNG { get; set; }

        public void ApplyCostModifier(int mod)
        {
            double dMod = mod / 100;

            foreach (var i in MerchShop)
            {
                int iMod = (int)Math.Floor(i.Cost * dMod);
                i.Cost += iMod;
            }
        }

        public Random GetShopRNG()
        {
            long tick = DateTime.Now.Ticks;
            int iTick = (int)tick;
            iTick = Math.Abs(iTick);
            Random _random = new Random(iTick);
            return _random;
        }

        public Item MatchItemByID(int id)
        {
            List<Item> iList = Data.GameData.InitItems();
            Item i = iList.Find(x => x.ID == id);
            return i;
        }

        public List<Item> SetupShop()
        {
            List<Item> items = new List<Item>();

            for (int i = 0; i < ItemPerShop; i++)
            {
                Item item = MatchItemByID(1);

                int chance = ShopRNG.Next(0, 100);

                if (chance >= 0 && chance < 80)
                {
                    item = MatchItemByID(1);
                }
                else
                {
                    item = MatchItemByID(2);
                }

                items.Add(item);
            }

            return items;
        }
    }
}
