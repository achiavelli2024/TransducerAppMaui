using TransducerAppMaui.Helpers;
using TransducerAppMaui.Logs;
using TransducerAppMaui.Resources.Strings;

namespace TransducerAppMaui.Views
{
    public partial class SettingsPage : ContentPage
    {
        private bool _loadingLanguage;

        public SettingsPage()
        {
            InitializeComponent();

            // Logging toggle
            LoggingEnabledSwitch.Toggled += LoggingEnabledSwitch_Toggled;

            // Language picker (textos vêm do resx)
            LanguagePicker.Items.Clear();
            LanguagePicker.Items.Add(AppResources.Settings_Language_Auto);
            LanguagePicker.Items.Add(AppResources.Settings_Language_English);
            LanguagePicker.Items.Add(AppResources.Settings_Language_Portuguese);

            LanguagePicker.SelectedIndexChanged += LanguagePicker_SelectedIndexChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // ===== Logging =====
            var enabled = LoggingSettings.Enabled;

            LoggingEnabledSwitch.Toggled -= LoggingEnabledSwitch_Toggled;
            LoggingEnabledSwitch.IsToggled = enabled;
            LoggingEnabledSwitch.Toggled += LoggingEnabledSwitch_Toggled;

            UpdateStatusLabel(enabled);

            // ===== Language =====
            _loadingLanguage = true;
            try
            {
                var selected = LanguageSettings.Selected;

                if (string.Equals(selected, LanguageSettings.English, StringComparison.OrdinalIgnoreCase))
                    LanguagePicker.SelectedIndex = 1;
                else if (string.Equals(selected, LanguageSettings.PortugueseBrazil, StringComparison.OrdinalIgnoreCase))
                    LanguagePicker.SelectedIndex = 2;
                else
                    LanguagePicker.SelectedIndex = 0;

                UpdateLanguageStatusLabel();
            }
            finally
            {
                _loadingLanguage = false;
            }
        }

        // ---------------- Logging ----------------

        private void LoggingEnabledSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            LoggingSettings.Enabled = e.Value;
            UpdateStatusLabel(e.Value);

            try { TransducerLogAndroid.LogInfo("Settings: LoggingEnabled set to {0}", e.Value); } catch { }
        }

        private void UpdateStatusLabel(bool enabled)
        {
            LoggingStatusLabel.Text = enabled
                ? AppResources.Settings_LogStatus_On
                : AppResources.Settings_LogStatus_Off;
        }

        // ---------------- Language ----------------

        private async void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingLanguage) return;

            try
            {
                switch (LanguagePicker.SelectedIndex)
                {
                    case 1:
                        LanguageSettings.Selected = LanguageSettings.English;
                        break;
                    case 2:
                        LanguageSettings.Selected = LanguageSettings.PortugueseBrazil;
                        break;
                    default:
                        LanguageSettings.Selected = LanguageSettings.Auto;
                        break;
                }

                LocalizationService.ApplyCultureFromSettings();
                UpdateLanguageStatusLabel();

                try { TransducerLogAndroid.LogInfo("Settings: Language set to {0}", LanguageSettings.Selected); } catch { }

                await DisplayAlert(AppResources.Dialog_Info, AppResources.Dialog_LanguageApplied, AppResources.Dialog_Ok);

                if (Application.Current != null)
                    Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                try { TransducerLogAndroid.LogException(ex, "LanguagePicker_SelectedIndexChanged"); } catch { }

                var msg = string.Format(AppResources.Dialog_LanguageFailed, ex.Message);
                await DisplayAlert(AppResources.Dialog_Error, msg, AppResources.Dialog_Ok);
            }
        }

        private void UpdateLanguageStatusLabel()
        {
            var selected = (LanguageSettings.Selected ?? LanguageSettings.Auto).Trim();

            LanguageStatusLabel.Text =
                selected.Equals(LanguageSettings.English, StringComparison.OrdinalIgnoreCase) ? AppResources.Settings_Language_Current_English :
                selected.Equals(LanguageSettings.PortugueseBrazil, StringComparison.OrdinalIgnoreCase) ? AppResources.Settings_Language_Current_Portuguese :
                AppResources.Settings_Language_Current_Auto;
        }
    }
}