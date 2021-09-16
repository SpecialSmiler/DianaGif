using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DianaGif.Views
{
	/// <summary>
	/// Interaction logic for DianaMessageBox.xaml
	/// </summary>
	public partial class DianaMessageBox : Window
	{

		public DianaMessageBox(string title, string msg)
		{
			InitializeComponent();
            DianaMessageBoxView.Title = title;
            MessageText.Text = msg;
            IconImage.Source = ResourceManager.Instance.GetWarningImageRandomly();
            ResourceManager.Instance.PlaySoundRandomly();
        }

        public static bool? Show(string title, string msg)
        {
            var msgBox = new DianaMessageBox(title, msg);
            return msgBox.ShowDialog();
        }

 
        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
   

	}
}
