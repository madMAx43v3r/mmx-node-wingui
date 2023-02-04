﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mmx.Gui.Win.Common.Plotter
{
    public enum Plotters : int
    {
        [Description("CPU Plotter")]
        ChiaPlotter = 1 << 0,
        [Description("CPU Plotter with compression")]
        ChiaPlotterWithCompression = 1 << 1,
        [Description("Gigahorse GPU Plotter")]
        CudaPlotter = 1 << 2,
        [Description("Bladebit")]
        Bladebit = 1 << 8,
    };

    public enum ItemType
    {
        CmdParameter,
        EnvParameter,
        Hidden,
        Other
    };

    public class IntItem : Item<int> {}
    public class StringItem : Item<string> { }

    public class PathItem : Item<string>
    {
        public new string GetParam()
        {
            var result = "";

            if (Value != null)
            {
                var value = Value.ToString();

                if (value.Contains(" "))
                {
                    value = $"\"{value}\"";
                    value = value.Replace("\\\"", "\\\\\"");
                }

                if (!string.IsNullOrEmpty(value))
                {
                    result = FormatParam(value);
                }

            }

            return result;
        }
    }

    public class BoolItem : Item<bool>
    {
        public BoolItem()
        {
            SkipValue = true;
        }
        public new string GetParam()
        {
            return Value ? base.GetParam() : "";
        }
    }

    public class ItemBase<T>: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool _valueInitialized;
        protected T _value;

        public string Name { get; set; }
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueInitialized = true;
                NotifyPropertyChanged();
            }
        }
    }

    public abstract class Item<T> : ItemBase<T>
    {
        public string LongName { get; internal set; }

        public void SetValue(object obj)
        {
            Value = (T)Convert.ChangeType(obj, typeof(T));
        }

        private T _defaultValue;
        public T DefaultValue
        {  
            get => _defaultValue;
            internal set {
                _defaultValue = value;
                if (_valueInitialized == false)
                {
                    _value = _defaultValue;
                }
                NotifyPropertyChanged();
            }
        }

        private bool _isVisible;
        public bool IsVisible { 
            get => _isVisible;
            internal set
            {
                _isVisible = value;
                NotifyPropertyChanged();
            }
        }

        public ItemType Type { get; internal set; } = ItemType.CmdParameter;

        public PlotterOptions.Scopes Scope { get; internal set; } = PlotterOptions.Scopes.None;

        public ObservableCollection<ItemBase<T>> Items { get; internal set; }
        public T Minimum { get; internal set; }
        public T Maximum { get; internal set; }
        public bool SkipName { get; internal set; }
        public bool SkipValue { get; internal set; }
        public bool Persistent { get; internal set; } = true;
        public bool SuppressDefaultValue { get; internal set; } = false;        

        public string GetParam()
        {
            var result = "";

            if (Value != null)
            {
                var value = Value.ToString();
                if (!string.IsNullOrEmpty(value))
                {                    
                    result = FormatParam(value);
                }
            }

            return result;
        }

        protected string FormatParam(string value)
        {
            string result = "";

            if(SuppressDefaultValue && value == DefaultValue.ToString())
            {
                return result;
            }

            if (SkipName)
            {
                result = value;
            } else
            {
                if(SkipValue)
                {
                    result = $"-{Name}";
                } else
                {
                    result = $"-{Name} {value}";
                }                
            }

            return result;
        }

    }

}
