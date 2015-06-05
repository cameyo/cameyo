using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.ComponentModel;

namespace Cameyo.Player
{
    public class AppDisplay : INotifyPropertyChanged
    {
        public string PkgId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Version { get; set; }
        public long Size { get; set; }
        public bool ImageLoaded { get; set; }
        public string ImageUrl { get; set; }    // Used by async image loader
        public string ImagePath   // Used by XAML display
        {
            get { return _ImagePath; }
            set
            {
                _ImagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        private string _ImagePath;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string value)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(value));
            }
        }

        public AppDisplay(ServerApp serverApp)
        {
            this.PkgId = serverApp.PkgId;
            this.Name = serverApp.AppID;
            this.Version = serverApp.Version;
            this.Size = serverApp.Size;
            this.Category = serverApp.Category;
            var info = serverApp.InfoStr.Split('\n');
            /*if (info.Count() >= 1)
                this.Category = info[0];
            if (info.Count() >= 2)
                this.Version = info[1];*/
            this.ImageUrl = serverApp.IconUrl;

            var localIconFile = ServerSingleton.Instance.ServerClient.LocalIconFile(PkgId);
            if (File.Exists(localIconFile))
            {
                this.ImagePath = localIconFile;
                this.ImageLoaded = true;
            }
            else
            {
                this.ImagePath = "gfx\\Pending.png";
                this.ImageLoaded = false;
            }
        }
    }

    public class LibDisplay
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public LibDisplay(ServerLib lib)
        {
            this.Id = lib.Id;
            this.Name = lib.DisplayName;
            //this.ImagePath = @"V:\MyProjects\Cameyo\Debug\LibCache.Icons\" + serverApp.PkgId + ".icon";
        }
    }
}
