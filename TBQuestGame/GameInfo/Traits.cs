using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Traits
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Traits(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
