using System;
using System.Linq;
using Hfr.ViewModel;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;
using Hfr.Helpers;

namespace Hfr.Views.MainPages
{
    public sealed partial class EditorPage : Page
    {
        public EditorPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Loc.Editor.PropertyChanged += Editor_PropertyChanged;
            Loc.Editor.SmileyChosen += Editor_SmileyChosen;
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = true;
            App.TelemetryClient.TrackPageView(nameof(EditorPage));
        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Loc.Editor.PropertyChanged -= Editor_PropertyChanged;
            Loc.Editor.SmileyChosen -= Editor_SmileyChosen;
            Loc.Editor.OnNavigatedFrom();
            ApplicationView.GetForCurrentView().SuppressSystemOverlays = false;
        }

        private void Editor_SmileyChosen(Models.Smiley smiley)
        {
            var tagToInsert = " " + smiley.Tag + " ";
            InsertInMessageTextBlock(tagToInsert);
        }

        private void Editor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Loc.Editor.IsEditorEnabled))
            {
                if (Loc.Editor.IsEditorEnabled)
                {
                    MessageTextBlock.IsEnabled = true;
                    MessageTextBlock.Focus(FocusState.Keyboard);
                    if (!string.IsNullOrEmpty(MessageTextBlock.Text) && MessageTextBlock.Text.Length > 0)
                    {
                        MessageTextBlock.SelectionStart = MessageTextBlock.Text.Length - 1;
                    }
                    else
                    {
                        MessageTextBlock.SelectionStart = 0;
                    }
                }
                else
                {
                    MessageTextBlock.IsEnabled = false;
                }
            }
        }

        private void IncludeGrandePic(object sender, RoutedEventArgs e)
        {
            var url = BuildImgUrlAnchor("full");
            Loc.Editor.UploadedPicId = "";
            InsertInMessageTextBlock(url);
        }
        
        private void IncludePreviewPic(object sender, RoutedEventArgs e)
        {
            var url = BuildImgUrlAnchor("preview");
            Loc.Editor.UploadedPicId = "";
            InsertInMessageTextBlock(url);
        }

        private void IncludeMiniPic(object sender, RoutedEventArgs e)
        {
            var url = BuildImgUrlAnchor("thumb");
            Loc.Editor.UploadedPicId = "";
            InsertInMessageTextBlock(url);
        }

        private string BuildImgUrlAnchor(string size)
        {
            string url = "";
            if (IncludeImgUrlLink.IsOn && IncludeImgUrlLink.IsOn)
            {
                url += "[url=http://reho.st/view/self/" + Loc.Editor.UploadedPicId + "]";
            }
            url += "[img]http://reho.st/";
            switch (size)
            {
                case "preview":
                    url += "preview/";
                    break;
                case "thumb":
                    url += "thumb/";
                    break;
            }
            url += "self/" + Loc.Editor.UploadedPicId + "[/img]";

            if (IncludeImgUrlLink.IsOn && IncludeImgUrlLink.IsOn)
            {
                url += "[/url]";
            }
            return url;
        }

        void InsertInMessageTextBlock(string text)
        {
            Loc.Editor.ShowSmileyUI = false;
            MessageTextBlock.SelectedText = text;
            MessageTextBlock.SelectionLength = 0;
            MessageTextBlock.SelectionStart += text.Length;
            MessageTextBlock.Focus(FocusState.Keyboard);
        }
    }
}
