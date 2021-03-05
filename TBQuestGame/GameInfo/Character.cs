using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Character : ObservableObject
    {
        #region TEXT DETAILS
        public string Name { get; set; }

        public string Descriptor { get; set; }
        #endregion

        #region IDs
        public int ID { get; set; }

        public int LocationID { get; set; }
        #endregion

        #region CHARACTER POSITION
        private int _tilePosition;

        public int TilePosition
        {
            get { return _tilePosition; }
            set
            {
                _tilePosition = value;
                OnPropertyChanged(nameof(TilePosition));
            }
        }
        #endregion

        #region CHARACTER ART ASSETS
        private Art _icon;

        public Art Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        #endregion

        #region ROLE
        public Role RoleDescriptor { get; set; }

        public enum Role
        {
            Merchant,
            Admiral,
            Peasant,
            Soldier,
            General
        }

        public virtual string FullTitle()
        {
            string title = $"{Name} the {Descriptor}";
            return title;
        }
        #endregion

        #region CONSTRUCTOR
        public Character(int id, string name, int locationId, int tilePosition, Art icon, Role role)
        {
            ID = id;
            Name = name;
            Descriptor = nameof(RoleDescriptor);
            RoleDescriptor = role;
            LocationID = locationId;
            TilePosition = tilePosition;
            Icon = icon;
        }
        #endregion
    }
}