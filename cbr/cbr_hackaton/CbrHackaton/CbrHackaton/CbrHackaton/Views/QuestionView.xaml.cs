using CbrHackaton.API;
using CbrHackaton.Classes;
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
	public partial class QuestionView : ContentPage
	{
        QuestionModel question;
        public QuestionView(QuestionModel question)
        {
            InitializeComponent();
            this.question = question;
            this.BindingContext = question;
        }

        private async void MaterialButton_Clicked(object sender, EventArgs e)
        {
            var asd = GetAnsweres(question);
            try
            {
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Пожалуйста, подождите"))
                {
                    var isSuccess = (await API.API.SetAnswer(CurrentProperties.CurrentUser.Token, asd));
                    if(isSuccess)
                    {
                        await DisplayAlert("Внимание!", "Ответ засчитан!", "Ок");
                        if (question.QuestionIsVisible)
                        {
                            question.Questions?.ForEach((itemVid) =>
                            {
                                itemVid.HasStat = true;
                            });
                        }
                        if (!question.QuestionIsVisible)
                        {
                            var page = new StatisticsView(question);
                            await Navigation.PushAsync(page);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Внимание!", "Произошла ошибка!", "Ок");
                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception exc)
            {
                await DisplayAlert("Внимание!", exc.Message, "Ок");
                return;
            }
        }
        
        List<int> GetAnsweres(QuestionModel question)
        {
            List<int> answers = new List<int>();
            //question?.Images?.ForEach((item) =>
            //{
            //    if (item.IsSelected)
            //    {
            //        answers.Add(item.Id);
            //        return;
            //    }
            //});
            // question?.Videos?.ForEach((item)=>
            //{
            //    if (item.IsSelected)
            //    {
            //        answers.Add(item.Id);
            //        return;
            //    }
            //});
            question?.Answers?.ForEach((item) =>
            {
                if (item.IsSelected)
                {
                    answers.Add(item.Id);
                    return;
                }
            });
            question?.Questions?.ForEach((item) =>
            {
                answers.AddRange(GetAnsweres(item));
            });
            return answers;
        }
    }
}