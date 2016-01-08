using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Hfr.Helpers;
using Hfr.Utilities;
using Hfr.ViewModel;

namespace Hfr.Commands.Editor
{
    public class PickAndUploadImgCommand : Command
    {
        public override async void Execute(object parameter)
        {
            var fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".gif");
            var file = await fileOpenPicker.PickSingleFileAsync();

            await Task.Run(async () =>
            {
                var url = await HfrRehostHelper.Upload(file);
                if (string.IsNullOrEmpty(url)) return;
                await ThreadUI.Invoke(() =>
                {
                    Loc.Editor.UploadedPicId = url;
                });
            });
        }
    }
}
