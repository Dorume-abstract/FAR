using FAR.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FAR
{
    /// <summary>
    /// Interaction logic for InvoiceModal.xaml
    /// </summary>
    public partial class InvoiceModal : Window
    {
        public InvoiceModal()
        {
            InitializeComponent();
        }

        private Invoice invoice;

        public Invoice GetInvoice()
        {
            return invoice;
        }

        public void SetInvoice(Invoice value)
        {
            invoice = value;
        }

        public void ShowInvoice()
        {
            if (invoice != null)
            {
                this.Title = invoice.Number;
                Number.Content += invoice.Number;
                Warehouse.Content += invoice.Warehouse.Name;
                CounterActor.Content += invoice.Buyer.Name;
                Amount.Content += invoice.Amount.ToString();
                if (invoice.Taxes.Length == 0)
                {
                    AmountVat.Content += "Без ПДВ";

                }
                else
                {
                    AmountVat.Content += invoice.Taxes[0].Amount.ToString();

                }
                CountOfPos.Content += invoice.Products.Length.ToString();
                DataGrid.ItemsSource = invoice.Products;
            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
