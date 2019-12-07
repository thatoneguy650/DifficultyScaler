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
    private float AIWeaponDamageModifier = 7.0f;
    private float PlayerWeaponDamageModifier = 3.0f;
    private Keys EnableKey = Keys.F10;
    public bool DamageModifier { get; set; } = true;
 
    public Main()
    {
        KeyDown += OnKeyDown;
        Tick += OnTick;
        Interval = 500;
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

        if (!MyIni.KeyExists("PlayerWeaponDamageModifier"))
            MyIni.Write("PlayerWeaponDamageModifier", PlayerWeaponDamageModifier.ToString());
        else
            PlayerWeaponDamageModifier = float.Parse(MyIni.Read("PlayerWeaponDamageModifier"));

        if (!MyIni.KeyExists("EnableKey"))
            MyIni.Write("EnableKey", EnableKey.ToString());
        else
            EnableKey = (Keys)Enum.Parse(typeof(Keys), MyIni.Read("EnableKey"), true);  
    }

    private void OnTick(object sender, EventArgs e)
    {

    }
    private void OnKeyDown(object sender, KeyEventArgs e)//
    {
        if (e.KeyCode == EnableKey)
        {
            DamageModifier = !DamageModifier;
            ReadCreateSettings();
            if (DamageModifier)
                SetDamageModifier();
            else
                ResetDamageModifier();     
        }
    }
    private void SetDamageModifier()
    {
        Function.Call(Hash.SET_AI_WEAPON_DAMAGE_MODIFIER, AIWeaponDamageModifier);
        Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, PlayerWeaponDamageModifier);
        RDR2.UI.Screen.ShowSubtitle(string.Format("Damage Modifier AI: {0}, Player {1}", AIWeaponDamageModifier, PlayerWeaponDamageModifier));
    }
    private void ResetDamageModifier()
    {
        Function.Call(Hash.RESET_AI_WEAPON_DAMAGE_MODIFIER);
        Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, 1.0f);
        RDR2.UI.Screen.ShowSubtitle("Damage Modifier Disabled");
    }
}
class IniFile
{
    string Path;
    string EXE = Assembly.GetExecutingAssembly().GetName().Name;

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

    public IniFile(string IniPath = null)
    {
        Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
    }

    public string Read(string Key, string Section = null)
    {
        var RetVal = new StringBuilder(255);
        GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
        return RetVal.ToString();
    }

    public void Write(string Key, string Value, string Section = null)
    {
        WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
    }

    public void DeleteKey(string Key, string Section = null)
    {
        Write(Key, null, Section ?? EXE);
    }

    public void DeleteSection(string Section = null)
    {
        Write(null, null, Section ?? EXE);
    }

    public bool KeyExists(string Key, string Section = null)
    {
        return Read(Key, Section).Length > 0;
    }
}
