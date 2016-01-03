using GalaSoft.MvvmLight;
using Hfr.Model;
using Hfr.Helpers;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Hfr.Utilities;
using Hfr.Commands;

namespace Hfr.ViewModel
{
    public class EditorViewModel : ViewModelBase, IDisposable
    {
        private Editor _currentEditor;
        private bool _isBusy = false;

        public Editor CurrentEditor
        {
            get { return _currentEditor; }
            set
            {
                Set(ref _currentEditor, value);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                Set(ref _isBusy, value);
                RaisePropertyChanged(nameof(IsBusyScreenVisible));
                RaisePropertyChanged(nameof(IsEditorEnabled));
            }
        }

        public bool IsEditorEnabled
        {
            get { return !IsBusy; }
        }

        public Visibility IsBusyScreenVisible
        {
            get { return IsBusy ? Visibility.Visible : Visibility.Collapsed; }
        }

        #region methods
        public void LoadEditor(string url)
        {
            Task.Run(async () => await FormFetcher.GetEditor(url));
        }

        public async void SubmitEditor()
        {
            await ThreadUI.Invoke(() =>
            {
                IsBusy = true;
            });

            CurrentEditor.PrepareForSubmit(); //Required!!!
            var result = await HttpClientHelper.Post(CurrentEditor.SubmitUrl, CurrentEditor.Data);

#warning "no feedback on submit"
            Debug.WriteLine(result);
            if (result.Length > 0)
            {
                await ThreadUI.Invoke(() =>
                {
                    IsBusy = false;
                    Loc.NavigationService.GoBack();
                    ApplicationView.GetForCurrentView().SuppressSystemOverlays = false;
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