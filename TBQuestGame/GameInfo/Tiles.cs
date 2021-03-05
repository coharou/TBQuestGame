﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Tiles : Art
    {
        #region PROPERTIES
        public string Region { get; set; }
        #endregion

        #region CONSTRUCTORS
        public Tiles(int id, string name, string path, string region) :
            base(id, name, path)
        {
            Region = region;
        }

        public Tiles(int id) :
            base(id)
        {
            ID = id;
        }
        #endregion
    }
}
