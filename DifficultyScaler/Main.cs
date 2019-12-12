using RDR2;
using RDR2.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;



public class Main : Script
{
    private float AIWeaponDamageModifier = 6.0f;
    private float AIMeleeDamageModifer = 1.0f;
    private float PlayerWeaponDamageModifier = 7.0f;
    private float PlayerMeleeDamageModifier = 1.0f;
    private float PlayerHealthRechargeMultiplier = 0.75f;
    private int GameTimeLastSet;

    private Keys EnableKey = Keys.F10;
    public bool DamageModifier { get; set; } = true;

    public enum CoreIndex
    {
        Health = 0,
        Stamina = 1,
        DeadEye = 2,
    }
    public Main()
    {
        KeyDown += OnKeyDown;
        Tick += OnTick;
        Interval = 1;
        Initialize();
    }
    private void Initialize()
    {
        ReadCreateSettings();
    }
    private void ReadCreateSettings()
    {
        var MyIni = new IniFile("scripts\\DifficultyScaler.ini");
        if(!MyIni.KeyExists("AIWeaponDamageModifier"))
            MyIni.Write("AIWeaponDamageModifier", AIWeaponDamageModifier.ToString());
        else
            AIWeaponDamageModifier = float.Parse(MyIni.Read("AIWeaponDamageModifier"));

        if (!MyIni.KeyExists("AIMeleeDamageModifer"))
            MyIni.Write("AIMeleeDamageModifer", AIMeleeDamageModifer.ToString());
        else
            AIMeleeDamageModifer = float.Parse(MyIni.Read("AIMeleeDamageModifer"));

        if (!MyIni.KeyExists("PlayerWeaponDamageModifier"))
            MyIni.Write("PlayerWeaponDamageModifier", PlayerWeaponDamageModifier.ToString());
        else
            PlayerWeaponDamageModifier = float.Parse(MyIni.Read("PlayerWeaponDamageModifier"));

        if (!MyIni.KeyExists("PlayerMeleeDamageModifier"))
            MyIni.Write("PlayerMeleeDamageModifier", PlayerMeleeDamageModifier.ToString());
        else
            PlayerMeleeDamageModifier = float.Parse(MyIni.Read("PlayerMeleeDamageModifier"));

        if (!MyIni.KeyExists("PlayerHealthRechargeMultiplier"))
            MyIni.Write("PlayerHealthRechargeMultiplier", PlayerHealthRechargeMultiplier.ToString());
        else
            PlayerHealthRechargeMultiplier = float.Parse(MyIni.Read("PlayerHealthRechargeMultiplier"));

        if (!MyIni.KeyExists("EnableKey"))
            MyIni.Write("EnableKey", EnableKey.ToString());
        else
            EnableKey = (Keys)Enum.Parse(typeof(Keys), MyIni.Read("EnableKey"), true);

    }
    private void OnTick(object sender, EventArgs e)
    {
        if (Game.GameTime - GameTimeLastSet >= 5000)//Reset it every 5 seconds
        {
            if (DamageModifier)
                SetModifier(false);
            else
                ResetModifier(false);

            GameTimeLastSet = Game.GameTime;
        }
    }
    private void OnKeyDown(object sender, KeyEventArgs e)//
    {
        if (e.KeyCode == EnableKey)
        {
            DamageModifier = !DamageModifier;
            ReadCreateSettings();
            if (DamageModifier)
                SetModifier(true);
            else
                ResetModifier(true);     
        }
    }
    private void SetModifier(bool ShowNotification)
    {
        Function.Call(Hash.SET_AI_WEAPON_DAMAGE_MODIFIER, AIWeaponDamageModifier);
        Function.Call(Hash.SET_AI_MELEE_WEAPON_DAMAGE_MODIFIER, AIMeleeDamageModifer);
        Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, PlayerWeaponDamageModifier);
        Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER, Game.Player, PlayerMeleeDamageModifier);
        Function.Call(Hash.SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER, Game.Player, PlayerHealthRechargeMultiplier);    
        if (ShowNotification)
            RDR2.UI.Screen.ShowSubtitle(string.Format("AI Weapon Modifier: {0}; AI Melee Modifier {1}, Player Weapon Modifer {2}; Player Melee Modifier {3}; Player Health Recharge Rate {4}", AIWeaponDamageModifier, AIMeleeDamageModifer, PlayerWeaponDamageModifier, PlayerMeleeDamageModifier, PlayerHealthRechargeMultiplier));
    }
    private void ResetModifier(bool ShowNotification)
    {
        Function.Call(Hash.RESET_AI_WEAPON_DAMAGE_MODIFIER);
        Function.Call(Hash.SET_AI_MELEE_WEAPON_DAMAGE_MODIFIER, 1.0f);
        Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, 1.0f);
        Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER, Game.Player, 1.0f);
        Function.Call(Hash.SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER, Game.Player, 1.0f);     
        if (ShowNotification)
            RDR2.UI.Screen.ShowSubtitle("Modifiers Disabled");
    }
}
