using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Android.OS;

namespace PhoneReminderAI
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Button myButton = this.FindByName<Button>("Connect");
            myButton.BackgroundColor = Color.FromRgb(0, 122, 255);
        }


        private void Connect_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ListPage());
        }
    }
}
