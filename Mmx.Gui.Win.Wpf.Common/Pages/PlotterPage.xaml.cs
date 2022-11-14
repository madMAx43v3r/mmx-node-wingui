﻿using Microsoft.WindowsAPICodePack.Dialogs;
using Mmx.Gui.Win.Common.Plotter;
using Mmx.Gui.Win.Wpf.Common.Dialogs;
using ModernWpf.Controls;
using System.Windows.Controls;

namespace Mmx.Gui.Win.Wpf.Common.Pages
{
    /// <summary>
    /// Interaction logic for PlotterPage.xaml
    /// </summary>
    ///     
    public partial class PlotterPage
    {
        public PlotterOptions PlotterOptions { get => PlotterOptions.Instance; }

        public PlotterPage()
        {
            InitializeComponent();
            DataContext = this;

            //NFT PLOTS DISABLED
            PlotterOptions.Instance.nftplot.Value = false;
            createPlotNFT.IsEnabled = false;

        }

        private void ChooseFolderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;
            var property = typeof(PlotterOptions).GetProperty(button.Tag as string);
            dynamic item = property.GetValue(PlotterOptions.Instance);

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = string.IsNullOrEmpty(item.Value) ? "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}" : item.Value;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                item.Value = PlotterOptions.FixDir(dialog.FileName);
            }
        }

        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PlotterDialog.ShowAsync(ContentDialogPlacement.InPlace);
            PlotterDialog.StartPlotter();
        }

    }
}
