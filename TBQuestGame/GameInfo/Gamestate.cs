using System;
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


        public Gamestate()
        {
            PausedByOptions = false;
            PausedByTraits = false;
        }
    }
}
