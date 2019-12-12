using RDR2;
using RDR2.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DifficultyScaler
{
    class Testing
    {
        public void Test()
        {

            int StaminaAttributeRank = Function.Call<int>(Hash.GET_ATTRIBUTE_RANK, Game.Player.Character, 1);
            int DeadEyeAttributeRank = Function.Call<int>(Hash.GET_ATTRIBUTE_RANK, Game.Player.Character, 2);

            int HealthAttributeCore = Function.Call<int>((Hash)0x36731AC041289BB1, Game.Player.Character, 0); //How much of the core do you have left

            //RDR2.UI.Screen.ShowSubtitle(string.Format("HealthAttributeRank: {0} StaminaAttributeRank: {1} DeadEyeAttributeRank: {2} HealthAttributeCore: {3}", HealthAttributeRank, StaminaAttributeRank, DeadEyeAttributeRank, HealthAttributeCore));


            Function.Call<int>(Hash.SET_ATTRIBUTE_BASE_RANK, Game.Player.Character, 0, 4); //set level of heatlh core? Works


            int HealthAttributeRank = Function.Call<int>(Hash.GET_ATTRIBUTE_RANK, Game.Player.Character, 0);//Rank of the item, always one less than the game shows
            //int StaminaAttributeRank = Function.Call<int>(Hash.GET_ATTRIBUTE_RANK, Game.Player.Character, 1);
            //int DeadEyeAttributeRank = Function.Call<int>(Hash.GET_ATTRIBUTE_RANK, Game.Player.Character, 2);


            RDR2.UI.Screen.ShowSubtitle(string.Format("HealthAttributeRank: {0} StaminaAttributeRank: {1} DeadEyeAttributeRank: {2}", HealthAttributeRank, StaminaAttributeRank, DeadEyeAttributeRank));
        }
    }
}
