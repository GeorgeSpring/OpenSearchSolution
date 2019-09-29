using CbrHackaton.Classes;
using CbrHackaton.CustomControls;
using CbrHackaton.Models;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;
using XF.Material.Forms.UI.Dialogs;

namespace CbrHackaton.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PollView : ContentPage
    {
        public PollView()
        {
            InitializeComponent();
        }
        private void MainLV_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new QuestionView(e.Item as QuestionModel) { Title = "Подробности" });
            MainLV.SelectedItem = null;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Пожалуйста, подождите"))
                {
                    var questions = (await API.API.GetQuestions(CurrentProperties.CurrentUser.Token));
                    PrepareQuestions(questions);
                    MainLV.ItemsSource = questions;
                }
            }
            catch (Exception exc)
            {

                await DisplayAlert("Внимание!", exc.Message, "Ок");
                return;
            }
        }
        void PrepareQuestions(List<QuestionModel> questions)
        {
            Command maincmd = new Command(async (p) =>
            {
                if (!(p as QuestionModel).HasStat)
                    await Navigation.PushAsync(new QuestionView(p as QuestionModel) { Title = "Подробности" });
                else
                    await Navigation.PushAsync(new StatisticsView(p as QuestionModel));
            });

            questions?.ForEach((item) =>
            {
                item.Parse();
                item.MainTap = maincmd;
                item.Images?.ForEach((itemImg) =>
                {
                    itemImg.TapCommand = item.ImagesTap;
                    itemImg.SecondTap = new Command((p) => { Navigation.PushAsync(new ImageView(p as ImageItem) { Title = "Изображение" }); });
                });
                item.Videos?.ForEach((itemVid) =>
                {
                    itemVid.TapCommand = item.VideosTap;
                });
                item.Answers?.ForEach((itemVid) =>
                {
                    itemVid.TapCommand = item.AnswersTap;
                });
                item.Questions?.ForEach((itemVid) =>
                {
                    itemVid.SecondTap = maincmd;
                    itemVid.HasMoreQuestion = false;
                });
                PrepareQuestions(item.Questions);
            });
        }
    }
}