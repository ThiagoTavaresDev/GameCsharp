using System.Media;
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
using System.Windows.Threading;

namespace MatchGame
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    DispatcherTimer timer = new DispatcherTimer();
    int tenthsOfSecondsElapsed;
    int matchesFound;
    double bestTime = double.MaxValue;
    SoundPlayer tickPlayer;
    public MainWindow()
    {
      InitializeComponent();
      timer.Interval = TimeSpan.FromSeconds(.1);
      timer.Tick += Timer_Tick;

      tickPlayer = new SoundPlayer("C:\\Users\\Thzin\\Downloads\\som.wav");

      SetUpGame();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
      tenthsOfSecondsElapsed++;
      timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
      if (tenthsOfSecondsElapsed % 2 == 0)
      {
        tickPlayer.Play();
      }
      if (matchesFound == 8)
      {
        timer.Stop();
        timeTextBlock.Text = timeTextBlock.Text + " - Jogar denovo?";
        if (tenthsOfSecondsElapsed / 10F < bestTime)
        {
          bestTime = tenthsOfSecondsElapsed / 10F;
          bestTimeText.Text = "Melhor Tempo: " + bestTime.ToString("0.0s");
        }
      }
    }
   
    private void SetUpGame()
    {
      List<string> animalEmoji = new List<string>()
      {
        "🐨", "🐨",
        "🐵", "🐵",
        "🐺", "🐺",
        "🐱", "🐱",
        "🐯", "🐯",
        "🐻‍", "🐻‍",
        "🐹", "🐹",
        "🐸", "🐸",
      };
      Random random = new Random();
      foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
      {
        if (textBlock.Name != "timeTextBlock" && textBlock.Name != "bestTimeText")
        {
          textBlock.Visibility = Visibility.Visible;
          int index = random.Next(animalEmoji.Count);
          string nextEmoji = animalEmoji[index];
          textBlock.Text = nextEmoji;
          animalEmoji.RemoveAt(index);
        }
      }
      timer.Start();
      tenthsOfSecondsElapsed = 0;
      matchesFound = 0;
    }
    TextBlock lastTextBlockClicked;
    bool findingMatch = false;

    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
      TextBlock textBlock = sender as TextBlock;
      if (findingMatch == false)
      {
        textBlock.Visibility = Visibility.Hidden;
        lastTextBlockClicked = textBlock;
        findingMatch = true;
      }
      else if (textBlock.Text == lastTextBlockClicked.Text)
      {
        matchesFound++;
        textBlock.Visibility = Visibility.Hidden;
        findingMatch = false;
      }
      else
      {
        lastTextBlockClicked.Visibility = Visibility.Visible;
        findingMatch = false;
      }
    }

    private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if(matchesFound == 8)
      {
        SetUpGame();
      }
    }
  }
}