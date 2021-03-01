using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Art
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public Art(int id, string name, string path)
        {
            ID = id;
            Name = name;
            Path = path;
        }
    }
}
