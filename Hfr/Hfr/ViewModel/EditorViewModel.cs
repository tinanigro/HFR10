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
using Hfr.Commands.Editor;
using Hfr.Models;

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

        public Visibility IsEditUIVisible
        {
            get { return CurrentEditor?.Intent == EditorIntent.Edit ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string MultiQuoteTemporaryContent { get; set; }
        #region methods
        public async Task LoadEditor(EditorPackage package)
        {
            await ThreadUI.Invoke(() => IsBusy = true);
            await Task.Run(async () => await FormFetcher.GetEditor(package));
            await ThreadUI.Invoke(() =>
            {
                IsBusy = false;
                RaisePropertyChanged(nameof(IsEditUIVisible));
                if (package.Intent == EditorIntent.MultiQuote)
                {
                    Loc.NavigationService.GoBack();
                }
            });
        }

        public async Task SubmitEditor()
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
                    Loc.NavigationService.GoBack();
                    ApplicationView.GetForCurrentView().SuppressSystemOverlays = false;
                    _isBusy = false;
                });
                await Loc.Topic.RefreshPage(CurrentEditor.Intent);
            }
            else
            {
                await ThreadUI.Invoke(() =>
                {
                    IsBusy = false;
                });
            }
        }
        #endregion

        #region navigation
        public async Task OnNavigatedTo(object parameter)
        {
            var editorPackage = parameter as EditorPackage;
            _isBusy = false;
            await ThreadUI.Invoke(() =>
            {
                ApplicationView.GetForCurrentView().SuppressSystemOverlays = true;
            });
            Debug.WriteLine("EditorViewModel OnNavigatedTo " + editorPackage.PostUriForm);
            
            if (editorPackage.PostUriForm == "http://debug")
            {
                editorPackage.PostUriForm = HFRUrl.Dbg_Form_QuoteSingleURL;
            }
            await LoadEditor(editorPackage);
        }

        public void OnNavigatedFrom()
        {
            Dispose();
        }
        #endregion

        #region commands
        public SubmitEditorCommand SubmitEditorCommand { get; } = new SubmitEditorCommand();
        public DeleteMessageCommand DeleteMessageCommand { get; } = new DeleteMessageCommand();
        public CancelMessageCommand CancelMessageCommand { get; } = new CancelMessageCommand();
        #endregion

        public void Dispose()
        {

        }
    }
}