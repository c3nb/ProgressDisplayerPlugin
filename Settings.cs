using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using System.Linq;

namespace ProgressDisplayerPlugin
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        public void OnChange()
        {
            containerSize[0] = CX;
            containerSize[1] = CY;
            containerOffset[0] = COX;
            containerOffset[1] = COY;
            displaying[0] = Progress;
            displaying[1] = Accuracy;
            displaying[2] = Combo;
            displaying[3] = Score;
            labelTexts[0] = ProgressNotPlayingForm;
            labelTexts[1] = ProgressPlayingForm;
            labelTexts[2] = AccuracyNotPlayingForm;
            labelTexts[3] = AccuracyPlayingForm;
            labelTexts[4] = PerfectsComboNotPlayingForm;
            labelTexts[5] = PerfectsComboPlayingForm;
            labelTexts[6] = ScoreNotPlayingForm;
            labelTexts[7] = ScorePlayingForm;
            Main.Refresh();
            Save(Main.mod);
        }
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
        public static string[] allSettingsKeys = new string[25]
        {
      "OffsetX",
      "OffsetY",
      "ContainerWidth",
      "ContainerHeight",
      "ContainerBackgroundColor",
      "FontSize",
      "FontColor",
      "FontBold",
      "FontShadow",
      "ValueAsStatic",
      "ValueRoundPoint",
      "DisplayProgress",
      "DisplayAccuracy",
      "DisplayPerfectsCombo",
      "DisplayScore",
      "DisplayedPriorities",
      "ResetPerfectsComboOnLevelRestart",
      "String - ProgressNull",
      "String - ProgressSet",
      "String - AccuracyNull",
      "String - AccuracySet",
      "String - PerfectsComboNull",
      "String - PerfectsComboSet",
      "String - ScoreNull",
      "String - ScoreSet"
        };
        public List<string> storedKeys;
        public static string DefaultFileContent;
        public int[] containerSize = new int[2] { 600, 400 };
        public int[] containerOffset = new int[2];
        [Draw("Container Size X")]
        public int CX = 600;
        [Draw("Container Size Y")]
        public int CY = 400;
        [Draw("Container Offset X")]
        public int COX = 0;
        [Draw("Container Offset Y")]
        public int COY = 0;
        [Draw("Container BackgroudColor")]
        public Color containerBackgroundColor = new Color(1, 1, 1, 0.0f);
        [Draw("FontSize")]
        public int fontSize = 20;
        [Draw("Font Color")]
        public Color fontColor = new Color(1, 1, 1);
        [Draw("Font Bold")]
        public bool fontBold = false;
        [Draw("Font Shadow")]
        public bool fontShadow = true;
        [Draw("Value Static")]
        public bool valueStatic = false;
        [Draw("Value Rounding")]
        public int valueRounding = 6;
        [Draw("Display Progress")]
        public bool Progress = true;
        [Draw("Display Accuracy")]
        public bool Accuracy = false;
        [Draw("Display Combo")]
        public bool Combo = false;
        [Draw("Display Score")]
        public bool Score = false;
        [Draw("Reset Combo")]
        public bool resetCombo = false;
        [Draw("Set Priority (Not Recommended)")]
        public bool SetPrio = false;
        public string AccuracyPlayingForm = "Accuracy: @value%";
        public string AccuracyNotPlayingForm = "Accuracy: not playing";
        public string ProgressPlayingForm = "Progress: @value%";
        public string ProgressNotPlayingForm = "Progress: not playing";
        public string PerfectsComboPlayingForm = "Combo: @value";
        public string PerfectsComboNotPlayingForm = "Combo: @value";
        public string ScorePlayingForm = "Score: @value";
        public string ScoreNotPlayingForm = "Score: not playing";

        public string[] labelPriorities = Settings.originalPriority;
        public bool[] displaying = new bool[4]
        {
      true,
      false,
      false,
      false
        };
        public static string[] originalPriority = new string[4]
        {
      "Progress",
      "Accuracy",
      "PerfectsCombo",
      "Score"
        };
        public string[] labelTexts = new string[8]
        {
      "Progress: not playing",
      "Progress: @value%",
      "Accuracy: not playing",
      "Accuracy: @value%",
      "Combo: @value",
      "Combo: @value",
      "Score: not playing",
      "Score: @value"
        };
        public bool[] displayAsInteger = new bool[4]
        {
      false,
      false,
      true,
      true
        };
        public void SetPriority()
        {
            string[] array4 = (string[])this.labelTexts.Clone();
            bool[] array5 = (bool[])this.displayAsInteger.Clone();
            int[] array6 = (int[])Main.VariableTable.Clone();
            for (int k = 0; k < Settings.originalPriority.Length; k++)
            {
                int num10 = Array.IndexOf<string>(Settings.originalPriority, this.labelPriorities[k]);
                for (int l = 0; l < 2; l++)
                {
                    this.labelTexts[k * 2 + l] = array4[num10 * 2 + l];
                }
                Main.VariableTable[k] = array6[num10];
                this.displayAsInteger[k] = array5[num10];
            }
        }
    }
    public enum Priority
    {
        [Value("Progress")]
        Progress,
        [Value("Accuracy")]
        Accuracy,
        [Value("PerfectsCombo")]
        Combo,
        [Value("Score")]
        Score
    }
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class Value : Attribute
    {
        public static BindingFlags ab = BindingFlags.CreateInstance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.OptionalParamBinding | BindingFlags.Public | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Static | BindingFlags.SuppressChangeType;
        public object value { get; set; }
        public object[] values { get; set; }
        public Value(params object[] values)
        {
            value = values[0];
            this.values = values;
        }
        public static object GetValue<T>(T obj) where T : Enum
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(obj.ToString(), ab);
            Value val = field.GetCustomAttributes<Value>(true).ToArray()[0];
            return val.value;
        }
        public static R GetValue<T, R>(T obj) where T : Enum
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(obj.ToString(), ab);
            Value val = field.GetCustomAttributes<Value>(true).ToArray()[0];
            return (R)val.value;
        }
        public static object SetValue<T>(T obj, object value) where T : Enum
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(obj.ToString(), ab);
            Value val = field.GetCustomAttributes<Value>(true).ToArray()[0];
            val.value = value;
            return val.value;
        }
        public static object[] GetValues<T>(T obj) where T : Enum
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(obj.ToString(), ab);
            Value val = field.GetCustomAttributes<Value>(true).ToArray()[0];
            return val.values;
        }
        public static object[] SetValues<T>(T obj, params object[] values) where T : Enum
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(obj.ToString(), ab);
            Value val = field.GetCustomAttributes<Value>(true).ToArray()[0];
            val.values = values;
            return val.values;
        }
    }
    internal static class Utils
    {
        public static void Shift<T>(this T[] array, int oldIndex, int newIndex)
        {
            // TODO: Argument validation
            if (oldIndex == newIndex)
            {
                return; // No-op
            }
            T tmp = array[oldIndex];
            if (newIndex < oldIndex)
            {
                // Need to move part of the array "up" to make room
                Array.Copy(array, newIndex, array, newIndex + 1, oldIndex - newIndex);
            }
            else
            {
                // Need to move part of the array "down" to fill the gap
                Array.Copy(array, oldIndex + 1, array, oldIndex, newIndex - oldIndex);
            }
            array[newIndex] = tmp;
        }
        public static void Swap<T>(this ref T lhs, ref T rhs) where T : struct
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        public static void Swap(this ref Priority lhs, ref Priority rhs)
        {
            Priority temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}
