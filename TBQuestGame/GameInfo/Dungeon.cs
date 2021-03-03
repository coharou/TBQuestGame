﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Dungeon : Location
    {
        public Dungeon(int id, string name, string description, Random randObj):
            base(id, name, description, randObj)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}