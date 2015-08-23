using GalaSoft.MvvmLight;
using Hfr.Models;
using Hfr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Ioc;

namespace Hfr.ViewModel
{
    public class EditorViewModel : ViewModelBase
    {
        private Editor _currentEditor;

        public Editor CurrentEditor
        {
            get { return _currentEditor; }
            set
            {
                Set(ref _currentEditor, value);
                RaisePropertyChanged(nameof(CurrentEditor));
            }
        }

        #region methods
        public void LoadEditor(string url)
        {
            Task.Run(async () => await FormFetcher.GetEditor(url));
        }
        #endregion

        #region navigation
        public void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine("EditorViewModel OnNavigatedTo");

            var parameter = e.Parameter as string;
            Debug.WriteLine(parameter);

            CurrentEditor = new Editor()
            {
                FromUrl = "http://",
                Text = "Dummy\r\n\r\nFoobar",
                idxTopic = 0,
            };

        }
        #endregion

    }
}