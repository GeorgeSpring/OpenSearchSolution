using CbrHackaton.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CbrHackaton
{
    public class CurrentProperties
    {
        private static UserModel _CurrentUser = new UserModel();
        public static UserModel CurrentUser
        {
            get => _CurrentUser;
            set
            {
                if (_CurrentUser != value)
                {
                    _CurrentUser = value;
                    OnPropertyChanged("CurrentUser");
                }
            }
        }

        public static void OnPropertyChanged([CallerMemberName] string text = "")
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(text));
        }
        public static EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;


    }
}
