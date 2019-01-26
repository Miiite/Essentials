﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class BrowserViewModel : BaseViewModel
    {
        string browserStatus;
        string uri = "http://xamarin.com";
        int browserType = (int)BrowserLaunchMode.SystemPreferred;
        int browserTitleType = (int)BrowserTitleMode.Default;
        int controlColor = 0;
        int toolbarColor = 0;

        Dictionary<string, Color> colorDictionary;

        public List<string> AllColors { get; }

        public BrowserViewModel()
        {
            OpenUriCommand = new Command(OpenUri);

            colorDictionary = typeof(Color)
                .GetFields()
                .Where(f => f.FieldType == typeof(Color) && f.IsStatic && f.IsPublic)
                .ToDictionary(f => f.Name, f => (Color)f.GetValue(null));

            var colors = colorDictionary.Keys.ToList();
            colors.Insert(0, "None");

            AllColors = colors;
        }

        public ICommand OpenUriCommand { get; }

        public string BrowserStatus
        {
            get => browserStatus;
            set => SetProperty(ref browserStatus, value);
        }

        public string Uri
        {
            get => uri;
            set => SetProperty(ref uri, value);
        }

        public List<string> BrowserLaunchModes { get; } =
            new List<string>
            {
                $"Use Default Browser App",
                $"Use System-Preferred Browser",
            };

        public int BrowserType
        {
            get => browserType;
            set => SetProperty(ref browserType, value);
        }

        public List<string> BrowserTitleModes { get; } =
            new List<string>
            {
                $"Use Default Mode",
                $"Show Title",
                $"Hide Title"
            };

        public int BrowserTitleType
        {
            get => browserTitleType;
            set => SetProperty(ref browserTitleType, value);
        }

        public int ToolbarColor
        {
            get => toolbarColor;
            set => SetProperty(ref toolbarColor, value);
        }

        public int ControlColor
        {
            get => controlColor;
            set => SetProperty(ref controlColor, value);
        }

        async void OpenUri()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await Browser.OpenAsync(uri, new BrowserLaunchOptions
                {
                    LaunchMode = (BrowserLaunchMode)BrowserType,
                    TitleMode = (BrowserTitleMode)BrowserTitleType,
                    PreferredToolbarColor = TitleColor == 0 ? (Color?)null : (System.Drawing.Color)colorDictionary[AllColors[TitleColor]],
                    PreferredControlColor = ControlColor == 0 ? (Color?)null : (System.Drawing.Color)colorDictionary[AllColors[ControlColor]]
                });
            }
            catch (Exception e)
            {
                BrowserStatus = $"Unable to open Uri {e.Message}";
                Debug.WriteLine(browserStatus);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
