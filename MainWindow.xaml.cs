using FAR.Models;
using FAR.Services;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;

namespace FAR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.DefaultExt = ".xml";
            fileDialog.Filter = "XML Files (*.xml)|*.xml";
            fileDialog.Multiselect = false;

            if (fileDialog.ShowDialog() == true)
            {
                InvoiceService invoiceService = InvoiceService.getIstance();
                Task<Answer<Invoice[]>> task = invoiceService.GetInvoice(fileDialog.FileName);
                await task;
                if (task.IsCompleted)
                {
                    var answer = task.Result;
                    if (answer.Result == Result.Ok)
                    {
                        var invoices = answer.Attachment;
                        foreach (var invoice in invoices)
                        {
                            MessageBox.Show(invoice.Number, answer.Result.ToString(), MessageBoxButton.OK);

                        }

                    }

                }
                else
                {
                    MessageBox.Show("Why are you gay?", "Выбор накладных", MessageBoxButton.OK);
                }


            }

        }
    }
}
