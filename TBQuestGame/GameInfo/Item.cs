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

        public Item(int id, string name, string description, Tag tag) :
            base(id, name, description)
        {
            ItemTag = tag;
        }
    }
}
