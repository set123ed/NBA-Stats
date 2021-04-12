using System;
using System.Collections.Generic;
using System.Text;

namespace NBAStats
{
    public class NoInternetConnectionException : Exception
    {
        public NoInternetConnectionException() : base("There's no internet connection")
        {
        }
    }
}
