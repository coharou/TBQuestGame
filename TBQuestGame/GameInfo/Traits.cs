using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Traits
    {
        #region PROPERTIES
        public string Name { get; set; }

        public string Description { get; set; }
        #endregion

        #region CONSTRUCTOR
        public Traits(string name, string description)
        {
            Name = name;
            Description = description;
        }
        #endregion
    }
}
