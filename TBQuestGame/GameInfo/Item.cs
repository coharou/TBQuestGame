using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Item
    {
        #region ID
        public int ID { get; set; }
        #endregion

        #region TEXT DETAILS
        public string Name { get; set; }

        public string Description { get; set; }
        #endregion

        public Item(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}
