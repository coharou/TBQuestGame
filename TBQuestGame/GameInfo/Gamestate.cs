﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Gamestate : ObservableObject
    {
        #region SESSION PAUSE / PLAY
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
        #endregion

        #region ACTION STATUS
        private bool _canPlayerAct;

        public bool CanPlayerAct
        {
            get { return _canPlayerAct; }
            set { _canPlayerAct = value; }
        }
        #endregion

        #region OBJECT FOR Random CLASS

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

        #region CONSTRUCTOR
        public Gamestate(bool canPlayerAct)
        {
            PausedByOptions = false;
            PausedByTraits = false;
            RandObj = ObtainRandObj();
            CanPlayerAct = canPlayerAct;
        }
        #endregion
    }
}
