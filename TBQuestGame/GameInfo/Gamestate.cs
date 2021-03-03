﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Gamestate : ObservableObject
    {
        private bool _pausedByOptions;

        public bool PausedByOptions
        {
            get { return _pausedByOptions; }
            set 
            { 
                _pausedByOptions = value;
                OnPropertyChanged(nameof(PausedByOptions));
            }
        }

        private bool _pausedByTraits;

        public bool PausedByTraits
        {
            get { return _pausedByTraits; }
            set
            {
                _pausedByTraits = value;
                OnPropertyChanged(nameof(PausedByTraits));
            }
        }
        #region Random Object
        // Taken from the Simple PvE game
        // Updated to fit the inclusion of constructors
        public Random RandObj;

        private static Random ObtainRandObj()
        {
            long tick = DateTime.Now.Ticks;
            int iTick = (int)tick;
            iTick = Math.Abs(iTick);
            Random _random = new Random(iTick);
            return _random;
        }
        #endregion


        public Gamestate()
        {
            PausedByOptions = false;
            PausedByTraits = false;
            RandObj = ObtainRandObj();
        }
    }
}