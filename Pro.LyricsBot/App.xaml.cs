namespace Pro.LyricsBot
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            FillPreferences();
        }

        private void FillPreferences()
        {
            Settings.Settings.SourceDeviceId = Preferences.Default.Get("SourceDeviceId", string.Empty);
            Settings.Settings.ProPresenterHost = Preferences.Default.Get("ProPresenterHost", "localhost");
            Settings.Settings.ProPresenterPort = Preferences.Default.Get("ProPresenterPort", 0);
            Settings.Settings.LineLength = Preferences.Default.Get("LineLength", 0);
            Settings.Settings.LineCount = Preferences.Default.Get("LineCount", 0);
            Settings.Settings.MessageId = Preferences.Default.Get("MessageId", string.Empty);
            Settings.Settings.TokenName = Preferences.Default.Get("TokenName", string.Empty);
        }
    }
}
