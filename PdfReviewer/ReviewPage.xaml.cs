using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PdfReviewer
{
    public sealed partial class ReviewPage : Page
    {

        public ReviewPage()
        {
            InitializeComponent();
            inkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                                                      Windows.UI.Core.CoreInputDeviceTypes.Pen |
                                                      Windows.UI.Core.CoreInputDeviceTypes.Touch;

            DataContext = App.ViewModel.SelectedDocument;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            RenderInkImageAsync();
        }

        private async void RenderInkImageAsync()
        {
            var currentStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            if (currentStrokes.Count > 0)
            {
                var storageFolder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
                var file = await storageFolder.CreateFileAsync($"{Guid.NewGuid()}.png", Windows.Storage.CreationCollisionOption.ReplaceExisting);

                if (file != null)
                {
                    Windows.Storage.CachedFileManager.DeferUpdates(file);
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                    using (var outputStream = stream.GetOutputStreamAt(0))
                    {
                        await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                        await outputStream.FlushAsync();
                    }
                    stream.Dispose();
                }

                var bm = new BitmapImage(new Uri($"ms-appdata:///temp/{file.Name}"));
                App.ViewModel.SelectedDocument.AnnotationImage = bm;

            }

            Frame.GoBack();
        }

        private void OnUndoClick(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
