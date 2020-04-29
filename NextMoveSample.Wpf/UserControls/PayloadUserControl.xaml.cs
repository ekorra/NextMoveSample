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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NextMoveSample.Wpf.ViewModels;

namespace NextMoveSample.Wpf.UserControls
{
    /// <summary>
    /// Interaction logic for PayloadUserControl.xaml
    /// </summary>
    public partial class PayloadUserControl : UserControl
    {
        public PayloadUserControl()
        {
            InitializeComponent();
        }

        private void UIElement_OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            // Note that you can have more than one file.

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Assuming you have one file that you care about, pass it off to whatever
            // handling code you have defined.
            foreach (var file in files)
            {
                ((ShellViewModel)this.DataContext).AddFiles(new FileInfo(file));
            }


        }
    }
}
