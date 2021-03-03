using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.GameInfo;

namespace TBQuestGame.Data
{
    public class GameData
    {
        public static Player InitPlayer() 
        {
            int id = 1;
            string name = "player_default";
            int locationId = 0;
            Character.Role role = Character.Role.Soldier;
            Player.SoldierRole soldierRole = Player.SoldierRole.Knight;
            Player player = new Player(id, name, locationId, role, soldierRole);

            // Remove when player customization is added
            Moves standard = new Moves("move_default", "default", Moves.DamageType.Melee, 1, 100, Moves.StatusType.None, 0, false, Moves.Ammunition.None);
            player.MoveOne = standard;
            player.MoveTwo = standard;
            player.MoveThree = standard;
            player.MoveFour = standard;
            player.MoveFive = standard;
            player.MoveSix = standard;

            // Remove when player customization is added
            Traits trait_standard = new Traits("DEFAULT TRAIT", "Default text for the default trait, applied before opening the window.");
            player.TraitChosenFirst = trait_standard;
            player.TraitChosenSecond = trait_standard;
            player.TraitRandomNeg = trait_standard;
            player.TraitRandomPos = trait_standard;

            return player;
        }

        /*
        public static List<Moves> InitMoves()
        {
            List<Moves> moves = new List<Moves>();
            Moves standard = new Moves("move_default", "default", Moves.DamageType.Melee, 1, 100, Moves.StatusType.None, 0, false, Moves.Ammunition.None);
            moves.Add(standard);
            return moves;
        }
        */
    }
}
