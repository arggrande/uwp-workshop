using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using PdfReviewer.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PdfReviewer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loading += MainPage_Loading;
        }

        private void MainPage_Loading(FrameworkElement sender, object args)
        {
            DataContext = App.ViewModel;

        }

        private void OnOpenFileClick(object sender, RoutedEventArgs e)
        {
            BrowseForFileAsync();
        }

        private async void BrowseForFileAsync()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".pdf");

            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                App.ViewModel.AllDocuments.Clear();
                var pdf = await PdfDocument.LoadFromFileAsync(file);

                var pageCount = pdf.PageCount;

                loadingProgress.Visibility = Visibility.Visible;
                loadingProgress.Minimum = 1;
                loadingProgress.MaxHeight = pageCount;

                for (int i = 0; i < pageCount; i++)
                {
                    using (var page = pdf.GetPage((uint) i))
                    {
                        loadingProgress.Value = i + 1;
                        var stream = new InMemoryRandomAccessStream();
                        var options = new PdfPageRenderOptions();
                        await page.RenderToStreamAsync(stream);

                        var src = new BitmapImage();
                        await src.SetSourceAsync(stream);

                        App.ViewModel.AllDocuments.Add(new Models.PreviewInformation()
                        {
                            PageNumber = i + 1,
                            PageImage = src
                        });
                    }
                }
                loadingProgress.Visibility = Visibility.Collapsed;
            }
        }

        private void OnDocumentClick(object sender, ItemClickEventArgs e)
        {
            App.ViewModel.SelectedDocument = e.ClickedItem as PreviewInformation;
            Frame.Navigate(typeof(ReviewPage));
        }
    }
}
