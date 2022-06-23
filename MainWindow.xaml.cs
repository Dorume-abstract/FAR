﻿using FAR.Database;
using FAR.Models;
using FAR.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        InvoiceService invoiceService = InvoiceService.getIstance();
        List<Invoice> invoices = new List<Invoice>();
        private async void OpenInvoicesClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.DefaultExt = ".xml";
            fileDialog.Filter = "XML Files (*.xml)|*.xml";
            fileDialog.Multiselect = false;

            if (fileDialog.ShowDialog() == true)
            {

                Task<Answer<Invoice[]>> task = invoiceService.GetInvoice(fileDialog.FileName);
                await task;
                if (task.IsCompleted)
                {
                    var answer = task.Result;
                    if (answer.Result == Result.Ok)
                    {
                        DataGrid.ItemsSource = answer.Attachment;
                        invoices = new List<Invoice>(answer.Attachment);

                    }
                    else
                    {
                        MessageBox.Show(answer.Description);
                    }


                }
            }
        }

        private async void SaveAsClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                _ = await invoiceService.SaveInvoice(saveFileDialog.FileName, invoices.ToArray());
            }
        }

        private void TestDB()
        {
            using (MyContext itemContext = new MyContext())
            {

                itemContext.ProductNames.Add(new ProductName()
                {
                    AmbarName = "AmbName",
                    RealName = "RealName"
                });
                try
                {
                    int i = itemContext.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
