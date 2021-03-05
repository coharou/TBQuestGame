using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Art
    {
        #region ID
        public int ID { get; set; }
        #endregion

        #region TEXT DETAILS
        public string Name { get; set; }

        public string Path { get; set; }
        #endregion

        #region CONSTRUCTORS
        public Art(int id, string name, string path)
        {
            ID = id;
            Name = name;
            Path = path;
        }

        public Art(int id)
        {
            ID = id;
        }
        #endregion
    }
}
