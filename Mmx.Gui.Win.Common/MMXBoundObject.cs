﻿namespace Mmx.Gui.Win.Common
{
    public class MMXBoundObject
    {
        public string Locale { get; set; } = Properties.Settings.Default.LanguageCode;

        public delegate void CopyKeysToPlotterEventHandler(string json);

        public event CopyKeysToPlotterEventHandler KeysToPlotter;

        public void CopyKeysToPlotter(string json)
        {
            KeysToPlotter?.Invoke(json);
        }

    }


}
