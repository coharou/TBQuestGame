using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Item : GameObject
    {
        public Tag ItemTag { get; set; }

        public enum Tag
        {
            Health,
            Teleport,
            None
        }

        private int _cost;

        public int Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }


        public Item(int id, string name, string description, Tag tag, int cost) :
            base(id, name, description)
        {
            ItemTag = tag;
            Cost = cost;
        }
    }
}
