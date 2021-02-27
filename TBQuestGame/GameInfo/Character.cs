using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Character
    {
        protected int ID { get; set; } 

        protected string Name { get; set; }

        protected string Descriptor { get; set; }

        protected int LocationID { get; set; }

        // Requires an enumerated prop

        // A virtual method

        // An abstract method

        public Character(int id, string name, int locationId)
        {
            ID = id;
            Name = name;
            LocationID = locationId;
        }
    }
}
