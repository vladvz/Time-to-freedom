using System;
using System.Configuration;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace TimeToFreedom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeDates();
        }

        private void InitializeDates()
        {
            this.dpStartDate.SelectedDate = DateTime.Now;

            var endDate = ConfigurationManager.AppSettings.Get("EndDate");

            if (string.IsNullOrEmpty(endDate))
            {
                this.dpEndDate.SelectedDate = DateTime.Now;
            }
            else
            {
                this.dpEndDate.SelectedDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }

        private void dpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateDifference();
        }

        private void dpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.dpEndDate.SelectedDate < this.dpStartDate.SelectedDate)
            {
                this.dpEndDate.SelectedDate = this.dpStartDate.SelectedDate;
                SaveEndDate(this.dpEndDate.SelectedDate);

                return;
            }

            CalculateDifference();
            SaveEndDate(this.dpEndDate.SelectedDate);
        }

        private void SaveEndDate(DateTime? selectedDate)
        {
            var value = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", selectedDate.Value.Date);
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["EndDate"].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void CalculateDifference()
        {
            if (this.dpEndDate.SelectedDate == null || this.dpStartDate.SelectedDate == null)
                return;

            this.txtLeftDays.Text = string.Format("{0} {1}", (this.dpEndDate.SelectedDate - this.dpStartDate.SelectedDate).Value.TotalDays.ToString(), "days");
        }
    }
}
