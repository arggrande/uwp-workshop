using PdfReviewer.Models;
using System.Collections.ObjectModel;

namespace PdfReviewer.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            AllDocuments = new ObservableCollection<PreviewInformation>();
        }
        public ObservableCollection<PreviewInformation> AllDocuments { get; set; }
        public PreviewInformation SelectedDocument { get; set; }


    }
}
