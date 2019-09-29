using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CbrHackaton.Classes
{
    public class ImageItem : INotifyPropertyChanged
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("text")]
        public string Name { get; set; }
        public string ImageSource { get; set; }
        public bool IsSelected { get; set; }
        public ICommand TapCommand
        {
            get;set;
        }
        public ICommand SecondTap
        {
            get; set;
        }
        public void OnPropertyChanged([CallerMemberName] string text = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(text));
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
