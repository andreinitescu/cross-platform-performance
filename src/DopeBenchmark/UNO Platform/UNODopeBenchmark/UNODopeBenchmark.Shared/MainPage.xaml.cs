using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Uno.Foundation;
using Windows.UI;

namespace UNODopeBenchmark
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        volatile bool breakTest = false;
        const int max = 100;

        public static Brush ConvertColor(Color c) => new SolidColorBrush(c);

        void StartTestST()
        {
            var rand = new Random2(0);

            breakTest = false;

            var width = AbsoluteLayout.ActualWidth;
            var height = AbsoluteLayout.ActualHeight;

            const int step = 20;
            var labels = new TextBlock[step * 2];

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
                    DopesTextBlock.Text = string.Format("{0:0.00} Dopes/s (AVG)", avg).PadLeft(21);
                    return;
                }

                //60hz, 16ms to build the frame
                while (sw.ElapsedMilliseconds - now < 16 && !breakTest)
                {
                    var label = new TextBlock()
                    {
                        Text = "Dope",
                        Foreground = new SolidColorBrush(Color.FromArgb(0xFF, (byte)(rand.NextDouble() * 255), (byte)(rand.NextDouble() * 255), (byte)(rand.NextDouble() * 255)))
                    };

                    label.RenderTransform = new RotateTransform() { Angle = rand.NextDouble() * 360 };

                    Canvas.SetLeft(label, rand.NextDouble() * width);
                    Canvas.SetTop(label, rand.NextDouble() * height);

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
                            DopesTextBlock.Text = string.Format("{0:0.00} Dopes/s", r).PadLeft(15);
                            avgSum += r;
                            avgN++;
                        }

                        prevMs = sw.ElapsedMilliseconds;
                    }
                }

                _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => loop());
            };

            sw.Start();

            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => loop());
        }

        void StartTestReuseST()
        {
            var rand = new Random2(0);

            breakTest = false;

            var width = AbsoluteLayout.ActualWidth;
            var height = AbsoluteLayout.ActualHeight;

            const int step = 20;
            var labels = new TextBlock[step * 2];

            var processed = 0;

            long prevTicks = 0;
            long prevMs = 0;
            int prevProcessed = 0;
            double avgSum = 0;
            int avgN = 0;
            var sw = new Stopwatch();

            Action loop = null;

            Stack<TextBlock> _cache = new Stack<TextBlock>();

            loop = () =>
            {
                var now = sw.ElapsedMilliseconds;

                if (breakTest)
                {
                    var avg = avgSum / avgN;
                    DopesTextBlock.Text = string.Format("{0:0.00} Dopes/s (AVG)", avg).PadLeft(21);
                    return;
                }

                //60hz, 16ms to build the frame
                while (sw.ElapsedMilliseconds - now < 16 && !breakTest)
                {
                    var label = _cache.Count == 0 ? new TextBlock() { Foreground = new SolidColorBrush() } : _cache.Pop();

                    label.Text = "Dope";
                    (label.Foreground as SolidColorBrush).Color = Color.FromArgb(0xFF, (byte)(rand.NextDouble() * 255), (byte)(rand.NextDouble() * 255), (byte)(rand.NextDouble() * 255));

                    label.RenderTransform = new RotateTransform() { Angle = rand.NextDouble() * 360 };

                    Canvas.SetLeft(label, rand.NextDouble() * width);
                    Canvas.SetTop(label, rand.NextDouble() * height);

                    if (processed > max)
                    {
                        _cache.Push(AbsoluteLayout.Children[0] as TextBlock);
                        AbsoluteLayout.Children.RemoveAt(0);
                    }

                    AbsoluteLayout.Children.Add(label);

                    processed++;

                    if (sw.ElapsedMilliseconds - prevMs > 500)
                    {

                        var r = (double)(processed - prevProcessed) / ((double)(sw.ElapsedTicks - prevTicks) / Stopwatch.Frequency);
                        prevTicks = sw.ElapsedTicks;
                        prevProcessed = processed;

                        if (processed > max)
                        {
                            DopesTextBlock.Text = string.Format("{0:0.00} Dopes/s", r).PadLeft(15);
                            avgSum += r;
                            avgN++;
                        }

                        prevMs = sw.ElapsedMilliseconds;
                    }
                }

                _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => loop());
            };

            sw.Start();

            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => loop());
        }

        void StartTestBindings()
        {
            var rand = new Random2(0);

            breakTest = false;

            var width = AbsoluteLayout.ActualWidth;
            var height = AbsoluteLayout.ActualHeight;

            const int step = 20;
            var labels = new TextBlock[step * 2];

            var processed = 0;

            long prevTicks = 0;
            long prevMs = 0;
            int prevProcessed = 0;
            double avgSum = 0;
            int avgN = 0;
            var sw = new Stopwatch();

            var source = Enumerable.Range(0, max).Select(i => new BindingItem() { Color = Colors.Red }).ToArray();
            ItemsDope.ItemsSource = source;

            Action loop = null;
            var current = 0;

            loop = () =>
            {
                var now = sw.ElapsedMilliseconds;

                if (breakTest)
                {
                    var avg = avgSum / avgN;
                    DopesTextBlock.Text = string.Format("{0:0.00} Dopes/s (AVG)", avg).PadLeft(21);
                    return;
                }

                //60hz, 16ms to build the frame
                while (sw.ElapsedMilliseconds - now < 16 && !breakTest)
                {
                    var index = current++ % source.Length;

                    source[index].Color = Color.FromArgb(0xFF, (byte)(rand.NextDouble() * 255), (byte)(rand.NextDouble() * 255), (byte)(rand.NextDouble() * 255));
                    source[index].Rotation = rand.NextDouble() * 360;
                    source[index].Top = rand.NextDouble() * height;
                    source[index].Left = rand.NextDouble() * width;

                    processed++;

                    if (sw.ElapsedMilliseconds - prevMs > 500)
                    {

                        var r = (processed - prevProcessed) / ((double)(sw.ElapsedTicks - prevTicks) / Stopwatch.Frequency);
                        prevTicks = sw.ElapsedTicks;
                        prevProcessed = processed;

                        if (processed > max)
                        {
                            DopesTextBlock.Text = string.Format("{0:0.00} Dopes/s", r).PadLeft(15);
                            avgSum += r;
                            avgN++;
                        }

                        prevMs = sw.ElapsedMilliseconds;
                    }
                }

                _ = Dispatcher.RunIdleAsync(_ => loop());
            };

            sw.Start();

            _ = Dispatcher.RunIdleAsync(_ => loop());
        }

        public void StartTestChangeST()
        {
            var rand = new Random2(0);

            breakTest = false;

            var width = grid.ActualWidth;
            var height = grid.ActualHeight;

            const int step = 20;
            var labels = new TextBlock[step * 2];

            var processed = 0;

            long prevTicks = 0;
            long prevMs = 0;
            int prevProcessed = 0;
            double avgSum = 0;
            int avgN = 0;
            var sw = new Stopwatch();

            var texts = new string[] { "dOpe", "Dope", "doPe", "dopE" };

            Action loop = null;

            loop = () =>
            {
                if (breakTest)
                {
                    var avg = avgSum / avgN;
                    DopesTextBlock.Text = string.Format("{0:0.00} Dopes/s (AVG)", avg).PadLeft(21);
                    return;
                }

                var now = sw.ElapsedMilliseconds;

                // 60hz, 16ms to build the frame
                while (sw.ElapsedMilliseconds - now < 16 && !breakTest)
                {
                    if (processed > max)
                    {
                        (AbsoluteLayout.Children[processed % max] as TextBlock).Text = texts[(int)Math.Floor(rand.NextDouble() * 4)];
                    }
                    else
                    {
                        var label = new TextBlock()
                        {
                            Text = "Dope",
                            Foreground = new SolidColorBrush(Color.FromArgb(0xFF, (byte)(rand.NextDouble() * 255), (byte)(rand.NextDouble() * 255), (byte)(rand.NextDouble() * 255)))
                        };

                        label.RenderTransform = new RotateTransform() { Angle = rand.NextDouble() * 360 };

                        Canvas.SetLeft(label, rand.NextDouble() * width);
                        Canvas.SetTop(label, rand.NextDouble() * height);

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
                            DopesTextBlock.Text = string.Format("{0:0.00} Dopes/s", r).PadLeft(15);
                            avgSum += r;
                            avgN++;
                        }

                        prevMs = sw.ElapsedMilliseconds;
                    }
                }

                _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => loop());
            };

            sw.Start();

            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => loop());
        }

        void SetControlsAtStart()
        {
            StartChangeST.Visibility = StartST.Visibility = StartGridST.Visibility = Visibility.Collapsed;
            stop.Visibility = DopesTextBlock.Visibility = Visibility.Visible;
            AbsoluteLayout.Children.Clear();
            grid.Children.Clear();
            DopesTextBlock.Text = "Warming up...";
        }

        void OnStartSTClicked(object sender, object e)
        {
            SetControlsAtStart();
            StartTestST();
        }

        void OnStartGridSTClicked(object sender, object e)
        {
            SetControlsAtStart();
            StartTestBindings();
        }

        void OnStartChangeSTClicked(object sender, object e)
        {
            SetControlsAtStart();
            StartTestChangeST();
        }

        void OnStartChangeReuseClicked(object sender, object e)
        {
            SetControlsAtStart();
            StartTestReuseST();
        }

        void OnStopClicked(object sender, object e)
        {
            breakTest = true;
            stop.Visibility = Visibility.Collapsed;
            StartChangeST.Visibility = StartST.Visibility = StartGridST.Visibility = Visibility.Visible;
        }

        async void OnStartAllClicked(object sender, object e)
        {
#if HAS_UNO_SKIA
            var deviceFamilyInfo = Windows.System.Profile.AnalyticsInfo.VersionInfo;
            var deviceInfo = new
            {
                OS = "Skia",
                OSVersion = deviceFamilyInfo.DeviceFamily,
                DeviceModel = deviceFamilyInfo.DeviceFamily,
            };
#elif HAS_UNO_WASM
            var deviceFamilyInfo = Windows.System.Profile.AnalyticsInfo.VersionInfo;
            var browserDeviceIdiom = Windows.System.Profile.AnalyticsInfo.DeviceForm;
            var browserUserAgent = WebAssemblyRuntime.InvokeJS("navigator.userAgent;");
            var deviceInfo = new
            {
                OS = "WebAssembly",
                DeviceModel = browserUserAgent,
                DeviceIdiom = browserDeviceIdiom
            };
#else
            var deviceInfo = new
            {
                OS = "Mac Catalyst",
                OSVersion = string.Empty,
                DeviceModel = string.Empty,
                DeviceManufacturer = string.Empty,
                DeviceName = string.Empty,
                DeviceIdiom = string.Empty,
                DeviceType = string.Empty
            };
            try
            {
                deviceInfo = new
                {
                    OS = "Unknown",
                    OSVersion = string.Empty,
                    DeviceModel = string.Empty,
                    DeviceManufacturer = string.Empty,
                    DeviceName = string.Empty,
                    DeviceIdiom = string.Empty,
                    DeviceType = string.Empty
                };
            } catch { }
#endif

#if DEBUG
            int testLengthMs = 5000;
#else
            int testLengthMs = 60000;
#endif
            int pauseLengthMs = 100;

            OnStartSTClicked(default, default);
            await Task.Delay(testLengthMs);
            OnStopClicked(default, default);
            await Task.Delay(pauseLengthMs);
            _ = decimal.TryParse(DopesTextBlock.Text.Replace(" Dopes/s (AVG)", "").Trim(), out var resultST);

            OnStartChangeSTClicked(default, default);
            await Task.Delay(testLengthMs);
            OnStopClicked(default, default);
            await Task.Delay(pauseLengthMs);
            _ = decimal.TryParse(DopesTextBlock.Text.Replace(" Dopes/s (AVG)", "").Trim(), out var resultChangeST);

            var platformVersion = "Uno Platform 4.2 .NET 6";

            var results = new
            {
                Date = DateTime.Today,
                DeviceInfo = deviceInfo,
                Platform = platformVersion,
                Build = resultST,
                Change = resultChangeST
            };

            DopesTextBlock.Text = $"Build: {results.Build}; Change: {results.Change}";
        }
    }

    public class BindingItem : INotifyPropertyChanged
    {
        double top;
        double left;
        double rotation;
        Color color;

        public double Top
        {
            get => top; set
            {
                top = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
            }
        }
        public double Left
        {
            get => left; set
            {
                left = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
            }
        }
        public double Rotation
        {
            get => rotation; set
            {
                rotation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rotation)));
            }
        }
        public Color Color
        {
            get => color; set
            {
                color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}