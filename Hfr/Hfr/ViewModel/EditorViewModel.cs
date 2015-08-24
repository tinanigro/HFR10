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
using Hfr.Utilities;

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
            var url = e.Parameter as string;
            Debug.WriteLine("EditorViewModel OnNavigatedTo " + url);

            if (url == "http://debug") url = HFRUrl.Dbg_Form_QuoteSingleMPFLKURL;

            LoadEditor(url);

        }
        #endregion

    }
}