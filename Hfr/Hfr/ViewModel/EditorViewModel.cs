using GalaSoft.MvvmLight;
using Hfr.Model;
using Hfr.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Hfr.Utilities;
using Hfr.Commands;
using Hfr.Commands.Editor;
using Hfr.Models;
using System.Collections.Generic;

namespace Hfr.ViewModel
{
    public delegate void EditorCancelledMessage();
    public delegate void SmileyChosen(Smiley smiley);
    public class EditorViewModel : ViewModelBase, IDisposable
    {
        #region private props
        private Editor _currentEditor;
        private bool _isBusy = false;

        private string _smileySearch;
        private string _uploadedPicId;

        private bool _showSmileyUI = false;
        private bool _isMainEditorVisible = false;
        private bool _isExtraEditorVisibleVisible = false;
        private bool _isCompleteEditorVisible = false;
        #endregion
        #region private fields
        private List<Smiley> _wikiSmileys;
        #endregion
        #region public props
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

        public string SmileySearch
        {
            get { return _smileySearch; }
            set
            {
                Set(ref _smileySearch, value);
                if (value?.Length >= 3)
                {
                    Task.Run(() => GetSmileys());
                }
                else
                {
                    WikiSmileys?.Clear();
                }
            }
        }

        public string UploadedPicId
        {
            get { return _uploadedPicId; }
            set
            {
                Set(ref _uploadedPicId, value);
                RaisePropertyChanged(nameof(UploadImageUrlTextBoxsVisibility));
            }
        }
        #region UI
        public bool ShowSmileyUI
        {
            get { return _showSmileyUI; }
            set
            {
                Set(ref _showSmileyUI, value);
                TriggerUIAdapter();
            }
        }
        public bool IsMainEditorVisible
        {
            get { return _isMainEditorVisible; }
            set { Set(ref _isMainEditorVisible, value); }
        }

        public bool IsExtraEditorVisibleVisible
        {
            get { return _isExtraEditorVisibleVisible; }
            set { Set(ref _isExtraEditorVisibleVisible, value); }
        }

        public bool IsCompleteEditorVisible
        {
            get { return _isCompleteEditorVisible; }
            set { Set(ref _isCompleteEditorVisible, value); }
        }

        public Visibility UploadImageUrlTextBoxsVisibility => string.IsNullOrEmpty(_uploadedPicId) ? Visibility.Collapsed : Visibility.Visible;

        #endregion
        #endregion
        #region public fields
        public List<Smiley> WikiSmileys
        {
            get { return _wikiSmileys; }
            set { Set(ref _wikiSmileys, value); }
        }
        #endregion
        #region methods

        public void TriggerUIAdapter()
        {
            IsExtraEditorVisibleVisible = false;
            IsMainEditorVisible = false;
            IsCompleteEditorVisible = false;

            var width = Window.Current.Bounds.Width;
            if (width < Strings.PortraitWidth)
            {
                if (ShowSmileyUI)
                {
                    IsExtraEditorVisibleVisible = true;
                }
                else
                {
                    IsMainEditorVisible = true;
                }
            }
            else
            {
                IsCompleteEditorVisible = true;
            }
        }

        public async Task RemoveQuoteFromMultiQuote(EditorPackage package)
        {
            if (string.IsNullOrEmpty(MultiQuoteTemporaryContent)) return;
            var quote = await FormFetcher.FetchMessageContent(package);

            Loc.Editor.MultiQuoteTemporaryContent = Loc.Editor.MultiQuoteTemporaryContent.Replace(quote, "").Replace(Environment.NewLine, "");
        }

        public async Task LoadEditor(EditorPackage package)
        {
            await ThreadUI.Invoke(() => IsBusy = true);
            await Task.Run(async () => await FormFetcher.GetEditor(package));
            await ThreadUI.Invoke(() =>
            {
                IsBusy = false;
                RaisePropertyChanged(nameof(IsEditUIVisible));
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

        public async Task GetSmileys()
        {
            var smileys = await SmileyFetcher.Fetch(_smileySearch);
            await ThreadUI.Invoke(() => WikiSmileys = smileys);
        }

        public void InsertSmiley(Smiley smiley)
        {
            SmileyChosen?.Invoke(smiley);
        }
        #endregion

        #region navigation
        public async Task OnNavigatedTo(object parameter)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            TriggerUIAdapter();
            var editorPackage = parameter as EditorPackage;
            _isBusy = false;
            Debug.WriteLine("EditorViewModel OnNavigatedTo " + editorPackage.PostUriForm);

            if (editorPackage.PostUriForm == "http://debug")
            {
                editorPackage.PostUriForm = HFRUrl.Dbg_Form_QuoteSingleURL;
            }
            await LoadEditor(editorPackage);
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            TriggerUIAdapter();
        }

        public void OnNavigatedFrom()
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            EditorCancelledMessage?.Invoke();
            Dispose();
        }
        #endregion

        #region commands
        public SubmitEditorCommand SubmitEditorCommand { get; } = new SubmitEditorCommand();
        public DeleteMessageCommand DeleteMessageCommand { get; } = new DeleteMessageCommand();
        public CancelMessageCommand CancelMessageCommand { get; } = new CancelMessageCommand();

        public SmileyChosenCommand SmileyChosenCommand { get; } = new SmileyChosenCommand();
        public PickAndUploadImgCommand PickAndUploadImgCommand { get; } = new PickAndUploadImgCommand();
        #endregion
        #region events
        public event EditorCancelledMessage EditorCancelledMessage;
        public event SmileyChosen SmileyChosen;
        #endregion
        public void Dispose()
        {
            SmileySearch = "";
            UploadedPicId = "";
            ShowSmileyUI = false;
        }
    }
}