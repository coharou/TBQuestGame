using System;
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

        private bool _pausedByQuests;

        public bool PausedByQuests
        {
            get { return _pausedByQuests; }
            set 
            { 
                _pausedByQuests = value;
                OnPropertyChanged(nameof(PausedByQuests));
            }
        }

        private bool _pausedByHelp;

        public bool PausedByHelp
        {
            get { return _pausedByHelp; }
            set 
            { 
                _pausedByHelp = value;
                OnPropertyChanged(nameof(PausedByHelp));
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

        private bool _pausedByMerchant;

        public bool PausedByMerchant
        {
            get { return _pausedByMerchant; }
            set 
            { 
                _pausedByMerchant = value;
                OnPropertyChanged(nameof(PausedByMerchant));
            }
        }


        private bool _pausedByInventory;

        public bool PausedByInventory
        {
            get { return _pausedByInventory; }
            set 
            { 
                _pausedByInventory = value;
                OnPropertyChanged(nameof(PausedByInventory));
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

        private int _turnCount;

        public int TurnCount
        {
            get { return _turnCount; }
            set 
            { 
                _turnCount = value;
                OnPropertyChanged(nameof(TurnCount));
            }
        }

        private int _layerCount;

        public int LayerCount
        {
            get { return _layerCount; }
            set { _layerCount = value; }
        }


        private int _locationCount;

        public int LocationCount
        {
            get { return _locationCount; }
            set 
            { 
                _locationCount = value;
                OnPropertyChanged(nameof(LocationCount));
            }
        }

        private string _location;

        public string Location
        {
            get { return _location; }
            set 
            { 
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private int _locID;

        public int LocID
        {
            get { return _locID; }
            set { _locID = value; }
        }

        private int _timeSinceMerchantSpawn;

        public int TimeSinceMerchantSpawn
        {
            get { return _timeSinceMerchantSpawn; }
            set { _timeSinceMerchantSpawn = value; }
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
            PausedByInventory = false;
            PausedByMerchant = false;
            PausedByQuests = false;
            PausedByHelp = false;

            RandObj = ObtainRandObj();

            CanPlayerAct = canPlayerAct;

            LayerCount = 1;
            TurnCount = 1;

            TimeSinceMerchantSpawn = 0;

            LocationCount = 1;
            Location = "The Beginning";
            LocID = 0;
        }
        #endregion
    }
}
