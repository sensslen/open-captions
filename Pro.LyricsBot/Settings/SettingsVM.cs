﻿using NAudio.CoreAudioApi;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pro.LyricsBot.Settings
{
    internal class SettingsVM : INotifyPropertyChanged
    {
        private MMDevice? _selectedDevice;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<MMDevice> Devices { get; private set; } = new();

        public MMDevice? SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (SetField(ref _selectedDevice, value))
                {
                    Settings.SourceDeviceId = _selectedDevice?.ID;
                    Preferences.Default.Set("SourceDeviceId", Settings.SourceDeviceId);
                }
            }
        }

        public string? ProPresenterHost
        {
            get => Settings.ProPresenterHost;
            set
            {
                if (Settings.ProPresenterHost != value)
                {
                    Settings.ProPresenterHost = value;
                    Preferences.Default.Set("ProPresenterHost", value);
                }
            }
        }

        public int ProPresenterPort
        {
            get => Settings.ProPresenterPort;
            set
            {
                if (Settings.ProPresenterPort != value)
                {
                    Settings.ProPresenterPort = value;
                    Preferences.Default.Set("ProPresenterPort", Settings.ProPresenterPort);
                }
            }
        }

        public int LineLength
        {
            get => Settings.LineLength;
            set
            {
                if (Settings.LineLength != value)
                {
                    Settings.LineLength = value;
                    Preferences.Default.Set("LineLength", Settings.LineLength);
                }
            }
        }

        public int LineCount
        {
            get => Settings.LineCount;
            set
            {
                if (Settings.LineCount != value)
                {
                    Settings.LineCount = value;
                    Preferences.Default.Set("LineCount", value);
                }
            }
        }

        public SettingsVM()
        {
            var enumerator = new MMDeviceEnumerator();
            foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All))
            {
                if (device.State == DeviceState.Active)
                {
                    Devices.Add(device);
                    if (Settings.SourceDeviceId is not null && device.ID == Settings.SourceDeviceId)
                    {
                        SelectedDevice = device;
                    }
                }
            }

            if (SelectedDevice is null)
            {
                Settings.SourceDeviceId = null;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
