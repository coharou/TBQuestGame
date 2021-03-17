using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Item : GameObject
    {
        public Item(int id, string name, string description) :
            base(id, name, description)
        {
            
        }
    }
}
