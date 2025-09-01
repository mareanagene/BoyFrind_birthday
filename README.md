WPF Birthday Adventure

A small WPF app that turns a birthday day into a playful multi-stage quiz.
Pass each stage to unlock the next surprise, see results on the home screen, and celebrate with confetti and floating balloons. Fully supports Hebrew (RTL).

Features

Full-screen, responsive window (maximized by default with sensible minimum size)

Themed background gradient and shared resource palette (colors, brushes, styles)

Reusable Card style (rounded corners, soft shadow)

Primary and Ghost button templates with hover/press/disabled states

Styled ComboBox with RTL support for Hebrew content

Home screen (ScreenMenu) shows four stage buttons and separate results cards per stage

Quiz flow per stage: solve → return to Home → show that stage’s results card → mark stage as completed (✓) → trigger confetti

Confetti engine that scales particle count and sway to window size

Balloons animation with vertical rise and sinusoidal horizontal sway (per-balloon parameters)

ScrollViewer drag-to-pan that ignores interactive controls (ComboBox, Button, TextBox, Selector)

Stage answers (current defaults)

Update these as needed in MainWindow.xaml.cs (see “Customize”).

Stage 1
1) אוקטופר
2) ויונו
3) פאשי
4) ניסו
5) יער
6) יוגרט
7) מריאן
Stage 2
1) ציור
Stage 3
1) אוכל + ריצה
Stage 4
1) מינה טומה
Tech

.NET (WPF), C#, XAML

Animations via Storyboard and DispatcherTimer

Single-window app: MainWindow.xaml and MainWindow.xaml.cs

Getting started
Prerequisites

Windows 10/11

Visual Studio 2022 or later with “.NET desktop development” workload
or the .NET SDK (dotnet --version)

Run with Visual Studio

Open the solution.

Set the WPF project as the Startup Project.

Build and Run (F5). The app opens maximized.

Run from command line
git clone <your-repo-url>
cd <repo-folder>
dotnet build
dotnet run --project wpf_project

Project layout
wpf_project/
├─ MainWindow.xaml          # UI: resources, styles, screens, results cards
├─ MainWindow.xaml.cs       # Navigation, quizzes, confetti, balloons, panning
└─ ...                      # standard WPF project files


Named elements used in code:

Home: BtnStage1, BtnStage2, BtnStage3, BtnStage4, ResultsCard, ResultsCard2, ResultsCard3, ResultsCard4, ResultsText, ResultsText2, ResultsText3, ResultsText4, ConfettiLayer, BalloonsLayer

Screens: Screen1, Screen2, Screen3, Screen4

Stage inputs: Q1Box (stage 1), Q1Box2, Q1Box3, Q1Box4 (stages 2–4)

Customize
Change correct answers

Edit the handlers in MainWindow.xaml.cs:

SubmitQuiz1_Click (stage 1)

SubmitQuiz2_Click (stage 2)

SubmitQuiz3_Click (stage 3)

SubmitQuiz4_Click (stage 4)

Example (stage 2):

private void SubmitQuiz2_Click(object sender, RoutedEventArgs e)
{
    string correctQ1 = "ציור";
    string ans1 = (Q1Box2.SelectedItem as ComboBoxItem)?.Content?.ToString();
    bool allOk = ans1 == correctQ1;
    if (allOk)
    {
        MarkStage2Completed();
        // show results on home (ResultsText2/ResultsCard2), navigate back, start confetti
    }
}

Add more questions

In each screen’s XAML, add more ComboBoxes (for example, Q2Box2, Q3Box2).

In the matching submit handler, read those values and extend the correctness check.

Tweak confetti and balloons

Confetti: StartConfettiOn(Canvas target, int particles = 120)
Particle count auto-scales to area; increase the particles base for a denser effect.

Balloons: InitBalloons() and StartBalloons()
Each balloon uses BalloonInfo { Speed, BaseLeft, Amp, Omega, Phase }.
Adjust these ranges to change rise speed or sway width.

Persist stage completion

Stage completion flags (_stage1Completed … _stage4Completed) are in-memory.
Persist them (for example, to application settings or a file) if you want ✓ to survive restarts.

Current behavior checklist

Window opens maximized with themed gradient background

“Start Party” button triggers confetti on the home screen

Each stage has its own quiz; on success:

The app navigates back to Home

The corresponding stage button shows “✓ הושלם” and is disabled/faded

The matching results card (ResultsCard, ResultsCard2, ResultsCard3, or ResultsCard4) appears with the answers

Confetti plays

Dragging inside long content pans smoothly; interactive controls do not initiate panning

Testing

Open Stage 1, select the exact answers listed above, and submit.

Verify that Home shows Stage 1 as completed, the Stage 1 results card appears, and confetti plays.

Repeat for Stages 2–4 with their respective answers.

License

MIT (or update to your preferred license).

Credits

Designed and implemented as a playful WPF demo featuring a clean theme, stage flow, confetti, and balloons, with Hebrew (RTL) support.
