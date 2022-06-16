using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MauiDopeBenchmark;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    volatile bool breakTest = false;
    const int max = 100;

    void StartTestST()
    {
        var rand = new Random2(0);

        breakTest = false;

        var width = AbsoluteLayout.Width;
        var height = AbsoluteLayout.Height;

        const int step = 20;
        var labels = new Label[step * 2];

        var processed = 0;

        long prevTicks = 0;
        long prevMs = 0;
        int prevProcessed = 0;
        double avgSum = 0;
        int avgN = 0;
        var sw = new Stopwatch();

        Action loop = null;

        loop = () =>
        {
            var now = sw.ElapsedMilliseconds;

            if (breakTest)
            {
                var avg = avgSum / avgN;
                DopesLabel.Text = string.Format("{0:0.00} Dopes/s (AVG)", avg).PadLeft(21);
                return;
            }

            //60hz, 16ms to build the frame
            while (sw.ElapsedMilliseconds - now < 16)
            {
                var label = new Label()
                {
                    Text = "Dope",
                    TextColor = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()),
                    Rotation = rand.NextDouble() * 360
                };

                Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
                Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(label, new Rect(rand.NextDouble(), rand.NextDouble(), 80, 24));

                if (processed > max)
                {
                    AbsoluteLayout.Children.RemoveAt(0);
                }

                AbsoluteLayout.Children.Add(label);

                processed++;

                if (sw.ElapsedMilliseconds - prevMs > 500)
                {

                    var r = (processed - prevProcessed) / ((double)(sw.ElapsedTicks - prevTicks) / Stopwatch.Frequency);
                    prevTicks = sw.ElapsedTicks;
                    prevProcessed = processed;

                    if (processed > max)
                    {
                        DopesLabel.Text = string.Format("{0:0.00} Dopes/s", r).PadLeft(15);
                        avgSum += r;
                        avgN++;
                    }

                    prevMs = sw.ElapsedMilliseconds;
                }
            }

            Device.BeginInvokeOnMainThread(loop);
        };

        sw.Start();

        Device.BeginInvokeOnMainThread(loop);
    }

    void StartTestGridST()
    {
        var rand = new Random2(0);

        breakTest = false;

        var width = GridLayout.Width;
        var height = GridLayout.Height;

        const int step = 20;
        var labels = new Label[step * 2];

        var processed = 0;

        long prevTicks = 0;
        long prevMs = 0;
        int prevProcessed = 0;
        double avgSum = 0;
        int avgN = 0;
        var sw = new Stopwatch();

        Action loop = null;

        loop = async () =>
        {
            await Task.Delay(1);

            if (breakTest)
            {
                var avg = avgSum / avgN;
                DopesLabel.Text = string.Format("{0:0.00} Dopes/s (AVG)", avg).PadLeft(21);
                return;
            }

            var now = sw.ElapsedMilliseconds;

            // 60hz, 16ms to build the frame
            while (sw.ElapsedMilliseconds - now < 16)
            {
                var label = new Label()
                {
                    Text = "Dope",
                    TextColor = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()),
                    Rotation = rand.NextDouble() * 360,
                    TranslationX = rand.NextDouble() * width,
                    TranslationY = rand.NextDouble() * height
                };

                if (processed > max)
                {
                    GridLayout.Children.RemoveAt(0);
                }

                GridLayout.Children.Add(label);

                processed++;

                if (sw.ElapsedMilliseconds - prevMs > 500)
                {
                    var r = (processed - prevProcessed) / ((double)(sw.ElapsedTicks - prevTicks) / Stopwatch.Frequency);
                    prevTicks = sw.ElapsedTicks;
                    prevProcessed = processed;

                    if (processed > max)
                    {
                        DopesLabel.Text = string.Format("{0:0.00} Dopes/s", r).PadLeft(15);
                        avgSum += r;
                        avgN++;
                    }

                    prevMs = sw.ElapsedMilliseconds;
                }
            }

            Device.BeginInvokeOnMainThread(loop);
        };

        sw.Start();

        Device.BeginInvokeOnMainThread(loop);
    }

    void StartTestChangeST()
    {
        var rand = new Random2(0);

        breakTest = false;

        var width = GridLayout.Width;
        var height = GridLayout.Height;

        const int step = 20;
        var labels = new Label[step * 2];

        var processed = 0;

        long prevTicks = 0;
        long prevMs = 0;
        int prevProcessed = 0;
        double avgSum = 0;
        int avgN = 0;
        var sw = new Stopwatch();

        var texts = new string[] { "dOpe", "Dope", "doPe", "dopE" };

        Action loop = null;

        var sw2 = Stopwatch.StartNew();

        loop = async () =>
        {
            if (breakTest || sw2.Elapsed > TimeSpan.FromSeconds(15))
            {
                var avg = avgSum / avgN;
                DopesLabel.Text = string.Format("{0:0.00} Dopes/s (AVG)", avg).PadLeft(21);
                return;
            }

            var now = sw.ElapsedMilliseconds;

            //60hz, 16ms to build the frame
            while (sw.ElapsedMilliseconds - now < 16)
            {
                if (processed > max)
                {
                    (AbsoluteLayout.Children[processed % max] as Label).Text = texts[(int)Math.Floor(rand.NextDouble() * 4)];
                }
                else
                {
                    var label = new Label()
                    {
                        Text = "Dope",
                        TextColor = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()),
                        Rotation = rand.NextDouble() * 360
                    };

                    Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
                    Microsoft.Maui.Controls.AbsoluteLayout.SetLayoutBounds(label, new Rect(rand.NextDouble(), rand.NextDouble(), 80, 24));

                    AbsoluteLayout.Children.Add(label);
                }

                processed++;

                if (sw.ElapsedMilliseconds - prevMs > 500)
                {

                    var r = (processed - prevProcessed) / ((double)(sw.ElapsedTicks - prevTicks) / Stopwatch.Frequency);
                    prevTicks = sw.ElapsedTicks;
                    prevProcessed = processed;

                    if (processed > max)
                    {
                        DopesLabel.Text = string.Format("{0:0.00} Dopes/s", r).PadLeft(15);
                        avgSum += r;
                        avgN++;
                    }

                    prevMs = sw.ElapsedMilliseconds;
                }
            }

            Device.BeginInvokeOnMainThread(loop);
        };

        sw.Start();

        Device.BeginInvokeOnMainThread(loop);
    }

    void SetControlsAtStart()
    {
        StartChangeST.IsVisible = StartSTButton.IsVisible = StartGridST.IsVisible = false;
        StopButton.IsVisible = DopesLabel.IsVisible = true;
        AbsoluteLayout.Children.Clear();
        GridLayout.Children.Clear();
        DopesLabel.Text = "Warming up...";
    }

    void OnStartSTClicked(object sender, EventArgs e)
    {
        SetControlsAtStart();
        StartTestST();
    }

    void OnStartGridSTClicked(object sender, System.EventArgs e)
    {
        SetControlsAtStart();
        StartTestGridST();
    }

    void OnStartChangeSTClicked(object sender, EventArgs e)
    {
        SetControlsAtStart();
        StartTestChangeST();
    }

    void OnStopButtonClicked(object sender, EventArgs e)
    {
        breakTest = true;
        StopButton.IsVisible = false;
        StartChangeST.IsVisible = StartSTButton.IsVisible = StartGridST.IsVisible = true;
    }

    async void OnStartAllClicked(object sender, EventArgs e)
    {
        int testLengthMs = 60000;
        int pauseLengthMs = 100;

        OnStartSTClicked(default, default);
        await Task.Delay(testLengthMs);
        OnStopButtonClicked(default, default);
        await Task.Delay(pauseLengthMs);
        _ = decimal.TryParse(DopesLabel.Text.Replace(" Dopes/s (AVG)", "").Trim(), out var resultST);

        OnStartChangeSTClicked(default, default);
        await Task.Delay(testLengthMs);
        OnStopButtonClicked(default, default);
        await Task.Delay(pauseLengthMs);
        _ = decimal.TryParse(DopesLabel.Text.Replace(" Dopes/s (AVG)", "").Trim(), out var resultChangeST);

        var platformVersion = "Maui GA";

#if ANDROID
        var operatingSystem = "Android";
#elif IOS
        var operatingSystem = "iOS";
#elif MACCATALYST
        var operatingSystem = "MacCatalyst";
#elif WINDOWS
        var operatingSystem = "WinUI";
#else
        var operatingSystem = "Unknown";
#endif

        var results = new { OS = operatingSystem, Platform = platformVersion, Build = resultST, Change = resultChangeST, Reuse = 0, Grid = 0 };
        string jsonString = JsonConvert.SerializeObject(results);

        DopesLabel.Text = $"Build: {results.Build}; Change: {results.Change}";
        Console.WriteLine(jsonString);
    }
}