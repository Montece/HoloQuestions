using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HoloQuestions
{
    public partial class ProgressWindow : Window
    {
        private static ProgressWindow Instance;

        public ProgressWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        void Window_Closed(object sender, System.EventArgs e)
        {
            Stop();
        }

        public static void PrintMessage(object text)
        {
            Instance.Log_richbox.Document.Blocks.FirstBlock.ContentEnd.Paragraph.Inlines.Add(new Run(text.ToString() + "\r"));
            Instance.Log_richbox.ScrollToEnd();
        }

        public static void PrintAlert(object text)
        {
            Instance.Log_richbox.Document.Blocks.FirstBlock.ContentEnd.Paragraph.Inlines.Add(new Run(text.ToString() + "\r")
            {
                Foreground = Brushes.Yellow
            });
            Instance.Log_richbox.ScrollToEnd();
        }

        public static void PrintError(object text)
        {
            Instance.Log_richbox.Document.Blocks.FirstBlock.ContentEnd.Paragraph.Inlines.Add(new Run(text.ToString() + "\r")
            {
                Foreground = Brushes.Red
            });
            Instance.Log_richbox.ScrollToEnd();
        }

        public static void PrintGood(object text)
        {
            Instance.Log_richbox.Document.Blocks.FirstBlock.ContentEnd.Paragraph.Inlines.Add(new Run(text.ToString() + "\r")
            {
                Foreground = Brushes.Green
            });
            Instance.Log_richbox.ScrollToEnd();
        }

        void Stop_button_Click(object sender, RoutedEventArgs e)
        {

        }

        void Ready_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
