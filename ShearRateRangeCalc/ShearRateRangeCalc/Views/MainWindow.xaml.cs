using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShearRateRangeCalc.ViewModels;

namespace ShearRateRangeCalc.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ShearRateRangeCalcViewModel();
        }

        private void btnSuggestSyringes_Click(object sender, RoutedEventArgs e)
        {
            ShearRateRangeCalcViewModel srrcvm = (DataContext as ShearRateRangeCalcViewModel);
            if (srrcvm != null)
                srrcvm.SuggestBtnClicked = true;       
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {

            ShearRateRangeCalcViewModel srrcvm = (DataContext as ShearRateRangeCalcViewModel);
            if (srrcvm != null)
            {
                srrcvm.IsWindowLoaded = true;

                srrcvm.CanvasWidth = cnvOutput.ActualWidth;

                srrcvm.UpdateFlowRateToScreenUnitsFactor();

                srrcvm.UpdateReferenceSyringeBar();
            }      
        }

        private void cbChipType_DropDownClosed(object sender, EventArgs e)
        {
            ShearRateRangeCalcViewModel srrcvm = (DataContext as ShearRateRangeCalcViewModel);
            if (srrcvm != null)
                srrcvm.CurrentChip = srrcvm.CurrentChip;
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tb = sender as TextBox;

                if (tb != null)
                {
                    BindingExpression be = tb.GetBindingExpression(TextBox.TextProperty);

                    be.UpdateSource();
                }
            }
        }
    }
}
