using Windows.UI.Xaml.Media.Imaging;

namespace PdfReviewer.Models
{
    public class PreviewInformation
    {
        public int PageNumber { get; set; }
        public BitmapImage PageImage { get; set; }
        public BitmapImage AnnotationImage { get; set; }
    }
}
