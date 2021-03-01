using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.GameInfo
{
    public class Character
    {
        #region TEXT DETAILS
        public string Name { get; set; }

        public string Descriptor { get; set; }
        #endregion

        #region IDs
        public int ID { get; set; }

        public int LocationID { get; set; }
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

        // A virtual method

        public virtual string FullTitle()
        {
            string title = $"{Name} the {Descriptor}";
            return title;
        }
        #endregion

        #region CONSTRUCTOR
        public Character(int id, string name, int locationId, Role role)
        {
            ID = id;
            Name = name;
            Descriptor = nameof(RoleDescriptor);
            RoleDescriptor = role;
            LocationID = locationId;
        }
        #endregion
    }
}