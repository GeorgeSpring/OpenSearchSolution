using Acr.UserDialogs;
using CbrHackaton.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace CbrHackaton.Views
{
    public partial class MainPage : ContentPage
    {
        public UserModel User { get; set; } = new UserModel() { Phone = "+78005001045", Password = "123456789" };
        public MainPage()
        {
            InitializeComponent(); 
            this.BindingContext = this;
            MainScrollView.InputTransparent = false;
            RegLabel.BindingContext = this;
        }
        public ICommand TapCommand
        {
            get
            {
                return new Command( async () =>
                {
                    var actions = new string[] { "Физ. лицо", "Юр. лицо" };
                    //Show simple dialog with title
                    var result1 = await MaterialDialog.Instance.SelectActionAsync(title: "Выберите тип регистрации",
                                                                                 actions: actions);
                    if(result1 == 0)
                    {
                        await Navigation.PushAsync(new RegistrationVIew(new UserModel()) { Title = "Регистрация" });
                    }
                    else
                    {
                        await Navigation.PushAsync(new RegistrationVIew(new UserModel() { UserType = "LegalEntity" }) { Title = "Регистрация" });
                    }
                });
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {

            //PostOnWall pow = new PostOnWall();
            //pow.Auth();
            //var cacheFile = Path.Combine(FileSystem.CacheDirectory, "share.png");
            //if (File.Exists(cacheFile))
            //    File.Delete(cacheFile);
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //var resources = assembly.GetManifestResourceNames().ToList();
            //using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resources.FirstOrDefault(x => x.Contains(".png"))))
            //using (var file = new FileStream(cacheFile, FileMode.Create, FileAccess.Write))
            //{
            //    resource.CopyTo(file);
            //}
            //await DependencyService.Get<IShare>().Show("gei", "pidor", cacheFile);
            //
            //
            if (string.IsNullOrWhiteSpace(User.Phone) ||
                    string.IsNullOrWhiteSpace(User.Password))
            {
                await DisplayAlert("Внимание!", "Заполнены не все поля!", "Ок");
                return;
            }
            
            try
            {
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Пожалуйста, подождите"))
                {
                    string token = (await API.API.Login(User));
                    CurrentProperties.CurrentUser = User;
                    CurrentProperties.CurrentUser.Token = token;
                    await Navigation.PushAsync(new PollView());
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
    public static class DoubleResources
    {
        public static readonly Thickness ButtonGroupPadding = new Thickness(0, Device.OnPlatform(12, 0, 0), 0, 0);
        public static readonly double SignUpButtonHeight = Device.OnPlatform(60, 60, 80);
        public static readonly double FBButtonHeight = Device.OnPlatform(135, 135, 64);
    }
}
