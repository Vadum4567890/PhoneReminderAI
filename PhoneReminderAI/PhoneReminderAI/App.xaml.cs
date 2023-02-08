using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhoneReminderAI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }


        //protected override async void OnStart()
        //{
        //    base.OnStart();

        //    // Load your data here
        //    await Task.Delay(3000); // Wait for 3 seconds

        //    // Set MainPage to your MainPage after data is loaded
        //    MainPage = new MainPage();
        //}
        

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
