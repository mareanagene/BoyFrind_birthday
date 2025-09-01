using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace wpf_project
{
    public partial class MainWindow : Window
    {
        // Random generator for animations
        private readonly Random _rnd = new Random();

        // Stage 1 completion state
        private bool _stage1Completed = false;
        
        private bool _stage2Completed = false;
        private bool _stage3Completed = false;
        private bool _stage4Completed = false;


        // Balloons
        private DispatcherTimer _balloonTimer;
        private double _balloonSpeed1 = 26;
        private double _balloonSpeed2 = 20;
        private double _balloonSpeed3 = 32;

        // Balloon per-item info for sway animation
        private class BalloonInfo
        {
            public double Speed;    // vertical px/sec
            public double BaseLeft; // horizontal center line
            public double Phase;    // radians
            public double Amp;      // horizontal amplitude
            public double Omega;    // rad/sec
        }

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                if (_stage1Completed)
                    MarkStage1Completed();
                if (_stage2Completed)
                    MarkStage2Completed();
                if (_stage3Completed)
                    MarkStage3Completed();
                if (_stage4Completed)
                    MarkStage4Completed();

                InitBalloons();
                StartBalloons();
            };
        }

        // ---------- Navigation ----------
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
            Title = title;
        }

        // ---------- ScrollViewer panning (mouse drag) ----------
        private bool _panArmed;
        private bool _isPanning;
        private Point _panStart;
        private double _startH, _startV;
        private const double PanThreshold = 6;

        private static T FindAncestor<T>(DependencyObject d) where T : DependencyObject
        {
            while (d != null)
            {
                if (d is T t) return t;
                if (d is Visual || d is System.Windows.Media.Media3D.Visual3D)
                    d = VisualTreeHelper.GetParent(d);
                else
                    d = LogicalTreeHelper.GetParent(d);
            }
            return null;
        }

        private static bool IsInteractive(DependencyObject d) =>
            FindAncestor<ComboBox>(d) != null ||
            FindAncestor<Button>(d) != null ||
            FindAncestor<TextBoxBase>(d) != null ||
            FindAncestor<Selector>(d) != null;

        private void SV_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var sv = (ScrollViewer)sender;

            // do not start panning from interactive controls
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
                e.Handled = true;
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

        // ---------- Mark stage 1 completion on home ----------
        private void MarkStage1Completed()
        {
            _stage1Completed = true;

            if (BtnStage1 != null)
            {
                BtnStage1.Content = "שלב הראשון ✓ הושלם";
                BtnStage1.IsEnabled = false;
                BtnStage1.Opacity = 0.6;          // subtle "done" look
                BtnStage1.Cursor = Cursors.Arrow; // no hand cursor
            }
        }

        // ---------- Mark stage 2 completion on home ----------
        private void MarkStage2Completed()
        {
            _stage2Completed = true;

            if (BtnStage2 != null)
            {
                BtnStage2.Content = "שלב שני ✓ הושלם";
                BtnStage2.IsEnabled = false;
                BtnStage2.Opacity = 0.6;          // subtle "done" look
                BtnStage2.Cursor = Cursors.Arrow; // no hand cursor
            }
        }

        private void MarkStage3Completed()
        {
            _stage3Completed = true;

            if (BtnStage3 != null)
            {
                BtnStage3.Content = "שלב שלישי ✓ הושלם";
                BtnStage3.IsEnabled = false;
                BtnStage3.Opacity = 0.6;          // subtle "done" look
                BtnStage3.Cursor = Cursors.Arrow; // no hand cursor
            }
        }


        private void MarkStage4Completed()
        {
            _stage4Completed = true;

            if (BtnStage4 != null)
            {
                BtnStage4.Content = "שלב רביעי ✓ הושלם";
                BtnStage4.IsEnabled = false;
                BtnStage4.Opacity = 0.6;          // subtle "done" look
                BtnStage4.Cursor = Cursors.Arrow; // no hand cursor
            }
        }

        // ---------- Quiz check ----------
        private void SubmitQuiz1_Click(object sender, RoutedEventArgs e)
        {
            // keep answers aligned with the exact strings in XAML ComboBoxItems
            string correctQ1 = "אוקטופר";
            string correctQ2 = "ויונו";
            string correctQ3 = "פאשי";
            string correctQ4 = "ניסו";
            string correctQ5 = "יער";
            string correctQ6 = "יוגרט";
            string correctQ7 = "מריאן";

            if (Q1Box.SelectedItem == null || Q2Box.SelectedItem == null || Q3Box.SelectedItem == null ||
                Q4Box.SelectedItem == null || Q5Box.SelectedItem == null || Q6Box.SelectedItem == null ||
                Q7Box.SelectedItem == null)
            {
                MessageBox.Show("בחר/י תשובה לכל השאלות לפני שליחה.", "חסר מידע");
                return;
            }

            string ans1 = (Q1Box.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string ans2 = (Q2Box.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string ans3 = (Q3Box.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string ans4 = (Q4Box.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string ans5 = (Q5Box.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string ans6 = (Q6Box.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string ans7 = (Q7Box.SelectedItem as ComboBoxItem)?.Content?.ToString();

            bool allOk = ans1 == correctQ1 && ans2 == correctQ2 && ans3 == correctQ3 &&
                         ans4 == correctQ4 && ans5 == correctQ5 && ans6 == correctQ6 && ans7 == correctQ7;

            if (allOk)
            {
                // Build the results text shown on the home screen
                string result =
                    "תשובות נכונות:\n" +
                    $"1) {correctQ1}\n" +
                    $"2) {correctQ2}\n" +
                    $"3) {correctQ3}\n" +
                    $"4) {correctQ4}\n" +
                    $"5) {correctQ5}\n" +
                    $"6) {correctQ6}\n" +
                    $"7) {correctQ7}";

                // 1) Mark Stage 1 as completed (visually disables the button on home)
                MarkStage1Completed();

                // 2) Put the answers on the home screen and show the results card
                if (ResultsText != null) ResultsText.Text = result;
                if (ResultsCard != null) ResultsCard.Visibility = Visibility.Visible;

                // 3) Go back to the home screen
                ShowScreen(ScreenMenu, "Main Menu");

                // 4) Celebrate on the home screen with confetti (responsive amount)
                StartConfetti(150);

                return; // No MessageBox; feedback is on the home screen
            }
            else
            {
                MessageBox.Show("לא כל התשובות נכונות... נסו שוב 🙂", "כמעט!");
            }
        }

        private void SubmitQuiz2_Click(object sender, RoutedEventArgs e)
        {
            string correctQ1 = "ציור";

            if (Q1Box2 == null || Q1Box2.SelectedItem == null)
            {
                MessageBox.Show("בחר/י תשובה לכל השאלות לפני שליחה.", "חסר מידע");
                return;
            }

            string ans1 = (Q1Box2.SelectedItem as ComboBoxItem)?.Content?.ToString();
            bool allOk = ans1 == correctQ1;

            if (allOk)
            {
                string result = "תשובה נכונה (שלב 2):\n" + $"1) {correctQ1}\n";

                MarkStage2Completed();

                if (ResultsText2 != null) ResultsText2.Text = result;
                if (ResultsCard2 != null) ResultsCard2.Visibility = Visibility.Visible;

                ShowScreen(ScreenMenu, "Main Menu");
                StartConfetti(150);
                return;
            }

            MessageBox.Show("לא כל התשובות נכונות... נסו שוב 🙂", "כמעט!");
        }


        private void SubmitQuiz3_Click(object sender, RoutedEventArgs e)
        {
            string correctQ1 = "אוכל + ריצה";

            if (Q1Box3 == null || Q1Box3.SelectedItem == null)
            {
                MessageBox.Show("בחר/י תשובה לכל השאלות לפני שליחה.", "חסר מידע");
                return;
            }

            string ans1 = (Q1Box3.SelectedItem as ComboBoxItem)?.Content?.ToString();
            bool allOk = ans1 == correctQ1;

            if (allOk)
            {
                string result = "תשובה נכונה (שלב 3):\n" + $"1) {correctQ1}\n";

                MarkStage3Completed();

                if (ResultsText3 != null) ResultsText3.Text = result;
                if (ResultsCard3 != null) ResultsCard3.Visibility = Visibility.Visible;

                ShowScreen(ScreenMenu, "Main Menu");
                StartConfetti(150);
                return;
            }

            MessageBox.Show("לא כל התשובות נכונות... נסו שוב 🙂", "כמעט!");
        }

        private void SubmitQuiz4_Click(object sender, RoutedEventArgs e)
        {
            string correctQ1 = "מינה טומה";

            if (Q1Box4 == null || Q1Box4.SelectedItem == null)
            {
                MessageBox.Show("בחר/י תשובה לכל השאלות לפני שליחה.", "חסר מידע");
                return;
            }

            string ans1 = (Q1Box4.SelectedItem as ComboBoxItem)?.Content?.ToString();
            bool allOk = ans1 == correctQ1;

            if (allOk)
            {
                string result = "תשובה נכונה (שלב 4):\n" + $"1) {correctQ1}\n";

                MarkStage4Completed();

                if (ResultsText4 != null) ResultsText4.Text = result;
                if (ResultsCard4 != null) ResultsCard4.Visibility = Visibility.Visible;

                ShowScreen(ScreenMenu, "Main Menu");
                StartConfetti(150);
                return;
            }

            MessageBox.Show("לא כל התשובות נכונות... נסו שוב 🙂", "כמעט!");
        }


        // ---------- Party button ----------
        private void Party_Click(object sender, RoutedEventArgs e)
        {
            StartConfetti(140);
        }

        // ---------- Confetti ----------
        private void StartConfettiOn(Canvas target, int particles = 120)
        {
            if (target == null) return;

            target.Children.Clear();

            var brushes = new Brush[]
            {
                TryFindResource("Bunting1") as Brush ?? new SolidColorBrush(Color.FromRgb(239,71,111)),
                TryFindResource("Bunting2") as Brush ?? new SolidColorBrush(Color.FromRgb(255,209,102)),
                TryFindResource("Bunting3") as Brush ?? new SolidColorBrush(Color.FromRgb(6,214,160)),
                TryFindResource("Bunting4") as Brush ?? new SolidColorBrush(Color.FromRgb(17,138,178)),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9B5DE5"))
            };

            double canvasWidth  = target.ActualWidth  > 0 ? target.ActualWidth  : (ActualWidth  > 0 ? ActualWidth  : 900);
            double canvasHeight = target.ActualHeight > 0 ? target.ActualHeight : (ActualHeight > 0 ? ActualHeight : 600);

            // Scale particle count with area (relative to 900x600 baseline)
            double scale = (canvasWidth * canvasHeight) / (900.0 * 600.0);
            int count = (int)Math.Round(particles * scale);
            count = Math.Max((int)(particles * 0.6), Math.Min((int)(particles * 3.0), count));

            // Horizontal sway range scales with width
            int swayRange = (int)Math.Max(40, canvasWidth * 0.05);

            for (int i = 0; i < count; i++)
            {
                var rect = new Rectangle
                {
                    Width  = _rnd.Next(4, 9),
                    Height = _rnd.Next(8, 16),
                    RadiusX = 1,
                    RadiusY = 1,
                    Fill = brushes[_rnd.Next(brushes.Length)],
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new RotateTransform(_rnd.Next(0, 360)),
                    Opacity = 0.9
                };

                double startLeft = _rnd.NextDouble() * Math.Max(40, canvasWidth - 40);
                double startTop  = -_rnd.Next(20, 300);

                Canvas.SetLeft(rect, startLeft);
                Canvas.SetTop(rect, startTop);
                target.Children.Add(rect);

                var fall = new DoubleAnimation
                {
                    From = startTop,
                    To = canvasHeight + 120,
                    Duration = TimeSpan.FromSeconds(_rnd.Next(5, 10)),
                    BeginTime = TimeSpan.FromMilliseconds(_rnd.Next(0, 1200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                Storyboard.SetTarget(fall, rect);
                Storyboard.SetTargetProperty(fall, new PropertyPath("(Canvas.Top)"));

                var sway = new DoubleAnimation
                {
                    From = startLeft,
                    To = startLeft + _rnd.Next(-swayRange, swayRange + 1),
                    Duration = TimeSpan.FromSeconds(_rnd.Next(2, 4)),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                Storyboard.SetTarget(sway, rect);
                Storyboard.SetTargetProperty(sway, new PropertyPath("(Canvas.Left)"));

                var spin = new DoubleAnimation
                {
                    From = 0,
                    To = _rnd.Next(180, 540),
                    Duration = TimeSpan.FromSeconds(_rnd.Next(3, 6)),
                    RepeatBehavior = RepeatBehavior.Forever
                };
                Storyboard.SetTarget(spin, rect);
                Storyboard.SetTargetProperty(spin, new PropertyPath("RenderTransform.(RotateTransform.Angle)"));

                var sb = new Storyboard { FillBehavior = FillBehavior.Stop };
                sb.Children.Add(fall);
                sb.Children.Add(sway);
                sb.Children.Add(spin);
                sb.Completed += (_, __) => target.Children.Remove(rect);
                sb.Begin();
            }
        }

        // Wrapper: home screen canvas (ScreenMenu)
        private void StartConfetti(int particles = 120)
        {
            StartConfettiOn(ConfettiLayer, particles);
        }

        private double GetConfettiWidth()
        {
            if (ConfettiLayer != null && ConfettiLayer.ActualWidth > 0) return ConfettiLayer.ActualWidth;
            if (ActualWidth > 0) return ActualWidth;
            return 900;
        }

        // ---------- Balloons ----------
        private void InitBalloons()
        {
            if (BalloonsLayer == null) return;

            var children = BalloonsLayer.Children.OfType<FrameworkElement>().ToList();
            if (children.Count == 0) return;

            double width = GetConfettiWidth();
            foreach (var child in children)
            {
                // prefer existing speed from Tag if set as double
                double speed = (child.Tag is double d && d > 0) ? d : 24.0;
                double baseLeft = Canvas.GetLeft(child);
                if (double.IsNaN(baseLeft)) baseLeft = width * 0.5;

                // amplitude scales with width (3–7%)
                double amp = Math.Max(20, width * (0.03 + _rnd.NextDouble() * 0.04));
                double omega = 0.6 + _rnd.NextDouble() * 0.8; // 0.6..1.4 rad/s
                double phase = _rnd.NextDouble() * Math.PI * 2;

                child.Tag = new BalloonInfo
                {
                    Speed = speed,
                    BaseLeft = baseLeft,
                    Phase = phase,
                    Amp = amp,
                    Omega = omega
                };
            }
        }

        private void StartBalloons()
        {
            if (BalloonsLayer == null) return;

            _balloonTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) }; // ~60 FPS
            DateTime last = DateTime.Now;

            _balloonTimer.Tick += (s, e) =>
            {
                var now = DateTime.Now;
                double dt = (now - last).TotalSeconds;
                last = now;

                double height = ActualHeight > 0 ? ActualHeight : 600;
                double width  = GetConfettiWidth();

                foreach (var child in BalloonsLayer.Children.OfType<FrameworkElement>())
                {
                    if (child.Tag is not BalloonInfo info)
                    {
                        child.Tag = info = new BalloonInfo
                        {
                            Speed = 24.0,
                            BaseLeft = Canvas.GetLeft(child),
                            Phase = _rnd.NextDouble() * Math.PI * 2,
                            Amp = Math.Max(20, width * 0.04),
                            Omega = 1.0
                        };
                    }

                    // vertical rise
                    double top = Canvas.GetTop(child) - info.Speed * dt;

                    // horizontal sway (sin)
                    info.Phase += info.Omega * dt;
                    double left = info.BaseLeft + info.Amp * Math.Sin(info.Phase);

                    // clamp inside screen
                    left = Math.Max(20, Math.Min(left, width - 40));

                    // respawn when above the top
                    if (top < -120)
                    {
                        top = height + _rnd.Next(40, 140);

                        // slightly randomize base line & sway for variety
                        info.BaseLeft = Math.Max(20, Math.Min(width - 40, info.BaseLeft + _rnd.Next(-120, 121)));
                        info.Amp = Math.Max(20, width * (0.03 + _rnd.NextDouble() * 0.04));
                        info.Omega = 0.6 + _rnd.NextDouble() * 0.8;
                    }

                    Canvas.SetTop(child, top);
                    Canvas.SetLeft(child, left);
                }
            };

            _balloonTimer.Start();
        }
    }
}
