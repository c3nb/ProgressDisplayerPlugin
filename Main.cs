using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;
using HarmonyLib;
using ProgressDisplayer;
using System.Reflection;

namespace ProgressDisplayerPlugin
{
    public class Main
    {
        public static UnityModManager.ModEntry mod;
        public static UnityModManager.ModEntry pdmod;
        public static Harmony harmony;
        public static PACSForm form;
        public static Type pdMain;
        public static bool IsEnabled = false;
        public static int[] VariableTable
        {
            get
            {
                return (int[])GetMain().GetField("VariableTable", AccessTools.all).GetValue(null);
            }
        }
        public static object thisGUIComponent
        {
            get
            {
                return GetMain().GetField("thisGUIComponent", AccessTools.all).GetValue(null);
            }
            set
            {
                GetMain().GetField("thisGUIComponent", AccessTools.all).SetValue(null, value);
            }
        }
        static int trycount = 0;
        internal static Type GetMain()
        {
            Type main = null;
            foreach (Type type in pdmod.Assembly.GetTypes())
            {
                if (type.Name == "Main")
                {
                    main = type;
                    break;
                }
            }
            return main;
        }
        public static void Refresh()
        {
            if (pdMain == null)
            {
                mod.Logger.Log($"{trycount}try.. null");
            }
            if (pdMain != null)
            {
                RefreshComponent(SetSettings);
            }
        }
        public static void SetSettings()
        {
            var settings = pdMain.GetField("settings", AccessTools.all).GetValue(null);
            var set = settings.GetType();
            set.GetField("containerBackgroundColor").SetValue(settings, Main.settings.containerBackgroundColor);
            set.GetField("containerOffset").SetValue(settings, Main.settings.containerOffset);
            set.GetField("containerSize").SetValue(settings, Main.settings.containerSize);
            set.GetField("displayAsInteger").SetValue(settings, Main.settings.displayAsInteger);
            set.GetField("displaying").SetValue(settings, Main.settings.displaying);
            set.GetField("fontBold").SetValue(settings, Main.settings.fontBold);
            set.GetField("fontColor").SetValue(settings, Main.settings.fontColor);
            set.GetField("fontShadow").SetValue(settings, Main.settings.fontShadow);
            set.GetField("fontSize").SetValue(settings, Main.settings.fontSize);
            set.GetField("labelPriorities").SetValue(settings, Main.settings.labelPriorities);
            set.GetField("labelTexts").SetValue(settings, Main.settings.labelTexts);
            set.GetField("resetCombo").SetValue(settings, Main.settings.resetCombo);
            set.GetField("valueRounding").SetValue(settings, Main.settings.valueRounding);
            set.GetField("valueStatic").SetValue(settings, Main.settings.valueStatic);
        }
        public static void AddComp()
        {
            thisGUIComponent = new GameObject().AddComponent(thisGUIComponent.GetType());
            UnityEngine.Object.DontDestroyOnLoad(thisGUIComponent as UnityEngine.Object);
            mod.Logger.Log("Added");
        }
        public static void DestroyComp()
        {
            UnityEngine.Object.Destroy(thisGUIComponent as UnityEngine.Object);
            mod.Logger.Log("Destroyed");
        }
        public static void RefreshComponent(Action action)
        {
            DestroyComp();
            action();
            AddComp();
        }
        public static Settings settings;
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            Main.mod = modEntry;
            modEntry.OnToggle = (entry, value) =>
            {
                IsEnabled = value;
                if(IsEnabled)
                {
                    foreach(UnityModManager.ModEntry m in UnityModManager.modEntries)
                    {
                        if(m.Info.DisplayName == "ProgressDisplayer")
                        {
                            pdmod = m;
                            break;
                        }
                    }
                    foreach(Type type in pdmod.Assembly.GetTypes())
                    {
                        if (type.Name == "Main")
                        {
                            pdMain = type;
                            break;
                        }
                    }
                    UnityEngine.Object.Destroy(thisGUIComponent as UnityEngine.Object);
                    form = new PACSForm();
                    Refresh();
                }
                else
                {
                    entry.OnSaveGUI(entry);
                }
                return true;
            };;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = (entry) =>
            {
                settings.Save(modEntry);
                Refresh();
            };
            return true;
        }
        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
            if (settings.SetPrio)
            {
                GUILayout.BeginHorizontal();
                for (int i = 0; i < cache.Count; i++)
                {
                    if (GUILayout.Button(cache[i].ToString()))
                    {
                        prios.Add(cache[i]);
                        cache.RemoveAt(i);
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Complete"))
                {
                    if (prios.Count != 4)
                    {
                        Main.mod.Logger.Log("Please Press All Buttons!!");
                        return;
                    }
                    for (int i = 0; i < prios.Count; i++)
                    {
                        Main.mod.Logger.Log($"{i + 1}. {Value.GetValue(prios[i])}");
                        tmp.Add(Value.GetValue<Priority, string>(prios[i]));
                    }
                    settings.labelPriorities = tmp.ToArray();
                    settings.SetPriority();
                    Refresh();
                    settings.OnChange();
                    tmp = new List<string>();
                    prios = new List<Priority>();
                    cache = new List<Priority> { Priority.Progress, Priority.Accuracy, Priority.Combo, Priority.Score };
                    settings.Save(mod);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Forms"))
            {
                if (form.IsDisposed)
                {
                    form = new PACSForm();
                }
                form.Show();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        public static List<string> tmp = new List<string>();
        public static List<Priority> prios = new List<Priority>();
        public static List<Priority> cache = new List<Priority> { Priority.Progress, Priority.Accuracy, Priority.Combo, Priority.Score };
    }
}
