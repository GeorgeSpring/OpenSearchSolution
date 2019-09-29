using CbrHackaton.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CbrHackaton.Models
{
    public class QuestionModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public List<ServCont> ContentPaths
        {
            get;set;
        }
        string _DateFrom = "";
        [JsonProperty("dateBegin")]
        public string DateFrom
        {
            get => _DateFrom.Replace(" 00:00:00", "");
            set
            {
                _DateFrom = value;
            }
        }

        public void Parse()
        {
            int i = 1;
            ContentPaths?.Where(x => x.type == "mp4" || x.type == "avi").ToList().ForEach((item) =>
            {
                Videos.Add(new ImageItem()
                {
                    Name = "Видео №" + i,
                    Id = i,
                    ImageSource = item.value,
                });
                i++;
            });
            i = 1;
            ContentPaths?.Where(x => x.type == "jpg" || x.type == "png").ToList().ForEach((item) =>
            {
                Images.Add(new ImageItem()
                {
                    Name = "Изображение №" + i,
                    Id = i,
                    ImageSource = item.value,
                });
                i++;
            });
        }

        string _DateTo = "";

        public void OnPropertyChanged([CallerMemberName]string text = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(text));
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("dateEnd")]
        public string DateTo 
        {
            get => _DateTo.Replace(" 00:00:00", "");
            set
            {
                _DateTo = value;
            }
        }

        public bool HasMoreQuestion { get; set; } = true;
        public bool VideoIsVisible => Videos?.Count > 0;
        public bool AnswerIsVisible => Answers?.Count > 0;
        public bool ImageIsVisible => Images?.Count > 0;
        public bool QuestionIsVisible => Questions?.Count > 0;
        public bool HasStat { get; set; }

        [JsonProperty("answers")]
        public List<ImageItem> Answers { get; set; } = new List<ImageItem>();
        public List<ImageItem> Images { get; set; } = new List<ImageItem>();
        [JsonProperty("childrenQuestions")]
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
        public ICommand AnswersTap
        {
            get
            {
                return new Command((p) =>
                {
                    var par = p as ImageItem;
                    var item = Answers?.FirstOrDefault(x => x.IsSelected);
                    if (item != null)
                        item.IsSelected = false;
                    par.IsSelected = true;
                    par?.OnPropertyChanged(nameof(par.IsSelected));
                    item?.OnPropertyChanged(nameof(par.IsSelected));
                });
            }
        }
        public ICommand ImagesTap
        {
            get
            {
                return new Command((p) =>
                {
                    var par = p as ImageItem;
                    var item = Images?.FirstOrDefault(x => x.IsSelected);
                    if (item != null)
                        item.IsSelected = false;
                    par.IsSelected = true;
                    par?.OnPropertyChanged(nameof(par.IsSelected));
                    item?.OnPropertyChanged(nameof(par.IsSelected));
                });
            }
        }
        public ICommand UrlTapped
        {
            get
            {
                return new Command(async () =>
                {
                    await Share.RequestAsync(new ShareTextRequest()
                    {
                        Title = "Тесты важные для всех!",
                        Text = "Пройди тест вместе со мной!",
                        Uri = $@"http://cbr.opensolutionsearch.ru/index.html?userid=1&questionid={Id}",
                    });
                });
            }
        }
        public ICommand VideosTap
        {
            get
            {
                return new Command((p) =>
                {
                    var par = p as ImageItem;
                    var item = Videos?.FirstOrDefault(x => x.IsSelected);
                    if (item != null)
                        item.IsSelected = false;
                    par.IsSelected = true;
                    par?.OnPropertyChanged(nameof(par.IsSelected));
                    item?.OnPropertyChanged(nameof(par.IsSelected));
                });
            }
        }

        public ICommand MainTap
        {
            get; set;
        }
        public ICommand SecondTap
        {
            get; set;
        }
        public List<ImageItem> Videos { get; set; } = new List<ImageItem>();
    }
    public class ServCont
    {
        public string type { get; set; }
        public string value { get; set; }
    }
}
