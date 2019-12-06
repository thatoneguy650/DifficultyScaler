using RDR2;
using RDR2.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DifficultyScaler
{
    public class Main : Script
    {
        public bool DamageModifier { get; set; } = true;
        public Main()
        {
            KeyDown += OnKeyDown;
            Tick += OnTick;
            Interval = 1;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (DamageModifier)
            {
                Function.Call(Hash.SET_AI_WEAPON_DAMAGE_MODIFIER, 5.0f);
                Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, 3.0f);
            }
            else
            {
                Function.Call(Hash.RESET_AI_WEAPON_DAMAGE_MODIFIER);
                Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, 1.0f);
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)//
        {
            if (e.KeyCode == Keys.F10)
            {
                DamageModifier = !DamageModifier;
                RDR2.UI.Screen.ShowSubtitle(string.Format("Damage Modifier Running: {0}", DamageModifier));
            }
        }
    }
}
