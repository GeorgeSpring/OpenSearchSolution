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
	public partial class StatisticsView : ContentPage
	{
        QuestionModel quest;
		public StatisticsView(QuestionModel id)
		{
			InitializeComponent ();
            this.quest = id;
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Пожалуйста, подождите"))
                {
                    var questions = (await API.API.GetStat(quest.Id));
                    questions?.ForEach((item) =>
                    {
                        item.Name = quest?.Answers?.FirstOrDefault(x => x.Id == item.AnswerId)?.Name;
                    });
                    MainLV.ItemsSource = questions;
                }
            }
            catch (Exception exc)
            {

                await DisplayAlert("Внимание!", exc.Message, "Ок");
                return;
            }
        }
    }
}