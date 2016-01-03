using GalaSoft.MvvmLight;
using Hfr.Model;
using Hfr.Helpers;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;
using Hfr.Utilities;
using Hfr.Commands;

namespace Hfr.ViewModel
{
    public class EditorViewModel : ViewModelBase, IDisposable
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

        public async void SubmitEditor()
        {
            CurrentEditor.PrepareForSubmit(); //Required!!!
            var result = await HttpClientHelper.Post(CurrentEditor.SubmitUrl, CurrentEditor.Data);

#warning "no feedback on submit"
            Debug.WriteLine(result);
            if (result.Length > 0)
            {
                await ThreadUI.Invoke(() =>
                {
                    Loc.NavigationService.GoBack();
                });
                await Loc.Topic.RefreshPage();
            }
            else
            {
                await ThreadUI.Invoke(() =>
                {
                });
            }
        }
        #endregion

        #region navigation
        public void OnNavigatedTo(object parameter)
        {
            var url = parameter as string;
            Debug.WriteLine("EditorViewModel OnNavigatedTo " + url);

            if (url == "http://debug")
            {
                url = HFRUrl.Dbg_Form_QuoteSingleURL;
            }
            LoadEditor(url);
        }

        public void OnNavigatedFrom()
        {
            Dispose();
        }
        #endregion

        #region commands
        public SubmitEditorCommand SubmitEditorCommand { get; } = new SubmitEditorCommand();
        #endregion

        public void Dispose()
        {

        }
    }
}