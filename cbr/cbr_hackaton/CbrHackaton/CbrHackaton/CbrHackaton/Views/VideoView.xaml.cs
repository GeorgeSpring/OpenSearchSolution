﻿using CbrHackaton.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CbrHackaton.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VideoView : ContentPage
	{
        ImageItem source;

        public VideoView(ImageItem source)
        {
            InitializeComponent();
            this.source = source;
            videoPlayer1.Source = source.ImageSource;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            source.TapCommand.Execute(source);
            Navigation.PopAsync();
        }
    }
}