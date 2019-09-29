using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CbrHackaton
{
    public interface IShare
    {
        Task Show(string subject, string message, string imageSource);
    }
}
