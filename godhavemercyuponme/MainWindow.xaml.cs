using Microsoft.Win32;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace godhavemercyuponme
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {


        public string CurrentFileName
        {
            get;
            set;
        } = "test.bmp";


        private IntPtr Handle => new WindowInteropHelper(this).Handle;

        public MainWindow()
        {
            InitializeComponent();

        }

        bool mouseDown = false;
        Point mouseDownPos;


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

            mouseDown = true;
            mouseDownPos = e.GetPosition(theGrid);
            theGrid.CaptureMouse();


            Canvas.SetLeft(selectionBox, mouseDownPos.X);
            Canvas.SetTop(selectionBox, mouseDownPos.Y);
            selectionBox.Width = 0;
            selectionBox.Height = 0;


            selectionBox.Visibility = Visibility.Visible;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Canvas.SetLeft(selectionBox, 0);
                Canvas.SetTop(selectionBox, 0);
                this.selectionBox.Height = MainCanvas.ActualHeight;
                selectionBox.Width = MainCanvas.ActualWidth;


            }

            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Bitmap.ClientRectToBmp(Handle, CurrentFileName, GetSelectionBounds());

                BitmapStatus.Text = $"Сохранено в {CurrentFileName}";

            }


            if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control)
            {
                 
                var rect = GetSelectionBounds().ToRECT();
                Bitmap.ShowBitMap(Handle, CurrentFileName, rect.Left, rect.Top);
            //    CompositionTarget.Rendering +=  (_, _) =>
            //        Bitmap.ShowBitMap(Handle, CurrentFileName, rect.Left, rect.Top);


                BitmapStatus.Text = $"Вставлено из {CurrentFileName}";




            }
        }

     
        public Rect GetSelectionBounds()
        {
            var topLeft = selectionBox.TranslatePoint(new Point(0, 0), this);
            var bottomRight = selectionBox.TranslatePoint(new Point(selectionBox.Width, selectionBox.Height), this);
            return new Rect(topLeft, bottomRight);
        }


        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {

            mouseDown = false;
            theGrid.ReleaseMouseCapture();

            // пускай будет persistent
            // selectionBox.Visibility = Visibility.Collapsed;



        }



        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {


            if (mouseDown)
            {



                Point mousePos = e.GetPosition(theGrid);

                //
                mousePos.Y = Math.Min(mousePos.Y, MainCanvas.ActualHeight);
                mousePos.Y = Math.Max(mousePos.Y, 0);
                //  держим в границах

                if (mouseDownPos.X < mousePos.X)
                {
                    Canvas.SetLeft(selectionBox, mouseDownPos.X);
                    selectionBox.Width = mousePos.X - mouseDownPos.X;
                }
                else
                {
                    Canvas.SetLeft(selectionBox, mousePos.X);
                    selectionBox.Width = mouseDownPos.X - mousePos.X;
                }

                if (mouseDownPos.Y < mousePos.Y)
                {
                    Canvas.SetTop(selectionBox, mouseDownPos.Y);
                    selectionBox.Height = mousePos.Y - mouseDownPos.Y ;
                }
                else
                {
                    Canvas.SetTop(selectionBox, mousePos.Y);
                    selectionBox.Height = mouseDownPos.Y - mousePos.Y;
                }

                var rect = GetSelectionBounds().ToRECT();

                this.BitmapStatus.Text = $"Top: {rect.Top}, Right: {rect.Right}, Bottom: {rect.Bottom}, Left: {rect.Left}";

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                CurrentFileName = openFileDialog.FileName;
            }
        }
    }
}
