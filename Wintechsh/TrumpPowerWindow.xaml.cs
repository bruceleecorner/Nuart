﻿using System.IO.Ports;
using System.Windows;
using NLog;

namespace Wintechsh
{
    /// <summary>
    /// Interaction logic for TrumpPowerWindow.xaml
    /// </summary>
    public partial class TrumpPowerWindow : Window
    {
        private readonly TrumpPower _device = new("COM46", 115200, Parity.None, StopBits.One);
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int _i;
        private int _k;

        public TrumpPowerWindow()
        {
            InitializeComponent();
        }

        private void ResetAlways_OnClick(object sender, RoutedEventArgs e)
        {
            if (_k == 0)
            {
                _k++;
                Task.Run(() =>
               {
                   while (true)
                   {
                       _device.Reset();
                   }
               });
            }
        }

        private void ResetOnce_OnClick(object sender, RoutedEventArgs e)
        {
            _device.Reset();
        }

        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                if (_i == 0)
                {
                    ++_i;
                    while (true)
                    {
                        var r = _device.QueryStatus();
                        if (r.IsSuccess == false || r.Data.Length != 54)
                        {
                            _logger.Error($"{r.IsSuccess}   {r.ErrorMsg}   {r.Data.Length}");
                        }
                    }
                }
            });
        }
    }
}