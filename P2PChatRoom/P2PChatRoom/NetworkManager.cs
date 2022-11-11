using System;
using System.Windows;
using System.ComponentModel;

namespace P2PChatRoom
{
    public class NetworkWorker
    {

        private BackgroundWorker networkBackgroundWorker;

        public NetworkWorker()
        {
            this.networkBackgroundWorker = new BackgroundWorker();
            
        }
        
    }
}