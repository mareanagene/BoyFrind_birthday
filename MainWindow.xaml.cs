using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace wpf_project
{
    public partial class MainWindow : Window
    {
        // ==== שדות ניווט בין המסכים ====
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Go1_Click(object sender, RoutedEventArgs e) => ShowScreen(Screen1, "Screen 1");
        private void Go2_Click(object sender, RoutedEventArgs e) => ShowScreen(Screen2, "Screen 2");
        private void Go3_Click(object sender, RoutedEventArgs e) => ShowScreen(Screen3, "Screen 3");
        private void Go4_Click(object sender, RoutedEventArgs e) => ShowScreen(Screen4, "Screen 4");
        private void BackToMenu_Click(object sender, RoutedEventArgs e) => ShowScreen(ScreenMenu, "Main Menu");

        private void ShowScreen(Grid target, string title)
        {
            ScreenMenu.Visibility = Visibility.Collapsed;
            Screen1.Visibility = Visibility.Collapsed;
            Screen2.Visibility = Visibility.Collapsed;
            Screen3.Visibility = Visibility.Collapsed;
            Screen4.Visibility = Visibility.Collapsed;

            target.Visibility = Visibility.Visible;
            this.Title = title;
        }

        // ==== גלילה בגרירה (פאנינג) – עם סף תנועה והתעלמות מבקרות אינטראקטיביות ====
        private bool _panArmed;                     // לחיצה שהוכנה לגרירה
        private bool _isPanning;                    // גרירה פעילה
        private Point _panStart;
        private double _startH, _startV;
        private const double PanThreshold = 6;      // פיקסלים

        private static T FindAncestor<T>(DependencyObject d) where T : DependencyObject
        {
            while (d != null)
            {
                if (d is T t) return t;
                d = VisualTreeHelper.GetParent(d);
            }
            return null;
        }

        private static bool IsInteractive(DependencyObject d) =>
            FindAncestor<ComboBox>(d) != null ||
            FindAncestor<Button>(d) != null ||
            FindAncestor<TextBoxBase>(d) != null ||
            FindAncestor<Selector>(d) != null; // ListBox/ListView/ComboBoxItem וכו'

        private void SV_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var sv = (ScrollViewer)sender;

            // אם לחצו על בקרה אינטראקטיבית – אל תתחילי גרירה
            if (IsInteractive((DependencyObject)e.OriginalSource))
            {
                _panArmed = _isPanning = false;
                return;
            }

            _panArmed = true;
            _isPanning = false;
            _panStart = e.GetPosition(sv);
            _startH = sv.HorizontalOffset;
            _startV = sv.VerticalOffset;
        }

        private void SV_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var sv = (ScrollViewer)sender;

            if (!_isPanning && _panArmed && e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(sv);
                if (Math.Abs(pos.X - _panStart.X) + Math.Abs(pos.Y - _panStart.Y) > PanThreshold)
                {
                    _isPanning = true;
                    sv.CaptureMouse();
                    sv.Cursor = Cursors.SizeAll;
                }
            }

            if (_isPanning)
            {
                var pos = e.GetPosition(sv);
                sv.ScrollToHorizontalOffset(_startH - (pos.X - _panStart.X));
                sv.ScrollToVerticalOffset(_startV - (pos.Y - _panStart.Y));
                e.Handled = true; // לא להעביר הלאה
            }
        }

        private void SV_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var sv = (ScrollViewer)sender;
            _panArmed = false;

            if (_isPanning)
            {
                _isPanning = false;
                sv.ReleaseMouseCapture();
                sv.Cursor = Cursors.Arrow;
                e.Handled = true;
            }
        }

        // ==== בדיקת החידון ====
        private void SubmitQuiz_Click(object sender, RoutedEventArgs e)
        {
            // תשובות נכונות – עדכני למה שנכון אצלך
            string correctQ1 = "אוקטופר";
            string correctQ2 = "ויונו";
            string correctQ3 = "פאשי";
            string correctQ4 = "ניסו";
            string correctQ5 = "יער";
            string correctQ6 = "יוגרט";
            string correctQ7 = "מריאן";

            // אם חסרה בחירה באחת השאלות – הודעה ידידותית
            if (Q1Box.SelectedItem == null || Q2Box.SelectedItem == null || Q3Box.SelectedItem == null ||
                Q4Box.SelectedItem == null || Q5Box.SelectedItem == null || Q6Box.SelectedItem == null ||
                Q7Box.SelectedItem == null)
            {
                MessageBox.Show("בחר/י תשובה לכל השאלות לפני שליחה.", "חסר מידע");
                return;
            }

            string ans1 = (Q1Box.SelectedItem as ComboBoxItem)?.Content.ToString();
            string ans2 = (Q2Box.SelectedItem as ComboBoxItem)?.Content.ToString();
            string ans3 = (Q3Box.SelectedItem as ComboBoxItem)?.Content.ToString();
            string ans4 = (Q4Box.SelectedItem as ComboBoxItem)?.Content.ToString();
            string ans5 = (Q5Box.SelectedItem as ComboBoxItem)?.Content.ToString();
            string ans6 = (Q6Box.SelectedItem as ComboBoxItem)?.Content.ToString();
            string ans7 = (Q7Box.SelectedItem as ComboBoxItem)?.Content.ToString();

            bool allOk = ans1 == correctQ1 && ans2 == correctQ2 && ans3 == correctQ3 &&
                         ans4 == correctQ4 && ans5 == correctQ5 && ans6 == correctQ6 && ans7 == correctQ7;

            if (allOk)
            {
                string result =
                    "תשובות נכונות:\n" +
                    $"1) {correctQ1}\n" +
                    $"2) {correctQ2}\n" +
                    $"3) {correctQ3}\n" +
                    $"4) {correctQ4}\n" +
                    $"5) {correctQ5}\n" +
                    $"6) {correctQ6}\n" +
                    $"7) {correctQ7}";

                MessageBox.Show(result, "כל הכבוד! 🎉");
            }
            else
            {
                MessageBox.Show("לא כל התשובות נכונות... נסו שוב 🙂", "כמעט!");
            }
        }
    }
}
