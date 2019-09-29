using Acr.UserDialogs;
using CbrHackaton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace CbrHackaton.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationVIew : ContentPage
    {
        public UserModel Model { get; set; } = new UserModel();
        public RegistrationVIew(UserModel userModel)
        {
            InitializeComponent();
            Model = userModel;
            this.BindingContext = this;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Model.FirstName) ||
                        string.IsNullOrWhiteSpace(Model.Password) ||
                        string.IsNullOrWhiteSpace(Model.SecondPassword) ||
                        string.IsNullOrWhiteSpace(Model.Phone))
            {
                await DisplayAlert("Внимание!", "Заполнены не все поля!", "Ок");
                return;
            }
            if (Model.Password != Model.SecondPassword)
            {
                await DisplayAlert("Внимание!", "Введеные пароли не совпадают!", "Ок");
                return;
            }
            try
            {
                using (var snackbar = await MaterialDialog.Instance.LoadingSnackbarAsync(message: "Пожалуйста, подождите..."))
                {
                    string Complete = (await API.API.Register(Model));
                    await Navigation.PopAsync();
                }
            }
            catch (Exception exc)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Внимание!", exc.Message, "Ок");
                return;
            }
        }
    }
}