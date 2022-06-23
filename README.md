# Cross Platform frameworks benchmarks

![Cross Platform frameworks benchmarks](images/perf-banner.png)

This repository is a set of tests and **benchmarks** to analyze the evolution in different aspects related to performance (boot time, package size, memory consumption, layout performance, rendering perfomance, etc.) in different **cross platform frameworks**.

The following frameworks are measured in this repository:
- [.NET MAUI](https://github.com/dotnet/maui)
- [Avalonia](https://github.com/AvaloniaUI/Avalonia)
- [Flutter](https://github.com/flutter/flutter)
- [UNO Platform](https://github.com/unoplatform/uno)
- [Xamarin.Forms](https://github.com/xamarin/Xamarin.Forms)

_(**Work in progress**, missing examples, missing metrics, etc.)_

## Benchmarks

At the moment, we have three different test blocks:
- **EmptyApp**: Yes, something as simple as an empty App can give us a lot of information. It can give us information such as the minimum startup time in each framework, etc.
- **ChatApp**: An empty application cannot give us all the information we are looking for, what is the performance with real applications? For that reason, we measure again startup time along with other parameters in a real application.
- **DopeTest**: This test is basically based on rendering hundreds of texts and measuring the performance of Layouts, rendering, etc.

_NOTE: A Poco F2 Pro has been used to obtain the data._

### Empty App

![Empty App](images/emptyapp-banner.png)

#### Startup time

| Android App | Framework           | Profiled AOT Time (ms) |
|-------------|---------------------| ---------------------:|
| EmptyApp    |  .NET MAUI GA SR1          |                 [380.4](https://github.com/jsuarezruiz/cross-platform-performance/blob/main/results/revision-1/EmptyApp/NET%20MAUI/dotnet-maui.txt) |
| EmptyApp    |  Avalonia 0.10.15           |                 - |
| EmptyApp    |  Flutter 3.0.2            |                 [239.7](https://github.com/jsuarezruiz/cross-platform-performance/blob/main/results/revision-1/EmptyApp/Flutter/flutter.txt) |
| EmptyApp    |  UNO Platform 4.3       |                 [1026.1](https://github.com/jsuarezruiz/cross-platform-performance/blob/main/results/revision-1/EmptyApp/UNO%20Platform/uno-platform.txt) |
| EmptyApp    |  Xamarin.Forms 5.0 SR11     |                 [347.2](https://github.com/jsuarezruiz/cross-platform-performance/blob/main/results/revision-1/EmptyApp/Xamarin.Forms/xamarin-forms.txt) |

_(lower values are better)_

![EmptyApp Startup time](images/empty-app-startup.png)

#### Package size

| Android App | Framework           | Profiled AOT Time (MBs) |
|-------------|---------------------| ---------------------:|
| EmptyApp    |  .NET MAUI GA SR1          |                 29.1 |
| EmptyApp    |  Avalonia 0.10.15           |                 - |
| EmptyApp    |  Flutter 3.0.2            |                 16.3 |
| EmptyApp    |  UNO Platform 4.3       |                 28.2 |
| EmptyApp    |  Xamarin.Forms 5.0 SR11    |                 45.0 |

![EmptyApp Package size](images/empty-app-size.png)

### Chat App

![Chat App](images/chatapp-banner.png)

#### Startup time

| Android App | Framework           | Profiled AOT Time(ms) |
|-------------|---------------------| ---------------------:|
| ChatApp    |  .NET MAUI GA SR1          |                 [716.9](https://github.com/jsuarezruiz/cross-platform-performance/blob/main/results/revision-1/ChatApp/NET%20MAUI/dotnet-maui.txt) |
| ChatApp    |  Avalonia 0.10.15           |                 - |
| ChatApp    |  Flutter 3.0.2            |                 [257.4](https://github.com/jsuarezruiz/cross-platform-performance/blob/main/results/revision-1/ChatApp/Flutter/flutter.txt) |
| ChatApp    |  UNO Platform 4.3       |                 [1910.2](https://github.com/jsuarezruiz/cross-platform-performance/blob/main/results/revision-1/ChatApp/UNO%20Platform/uno-platform.txt) |
| ChatApp    |  Xamarin.Forms 5.0 SR11     |                 [377.0](https://github.com/jsuarezruiz/cross-platform-performance/blob/main/results/revision-1/ChatApp/Xamarin.Forms/xamarin-forms.txt) |

_(lower values are better)_

![ChatApp Startup time](images/chat-app-startup.png)

#### Package size

| Android App | Framework           | Profiled AOT Time(MBs) |
|-------------|---------------------| ---------------------:|
| ChatApp    |  .NET MAUI GA SR1          |                 30.7 |
| ChatApp    |  Avalonia 0.10.15           |                 - |
| ChatApp    |  Flutter 3.0.2            |                 19.6 |
| ChatApp    |  UNO Platform 4.3       |                 29.8 |
| ChatApp    |  Xamarin.Forms 5.0 SR11     |                 46.7 |

![ChatApp Package size](images/chat-app-size.png)

#### Memory consumption

| Android App | Framework           | Profiled AOT Time(MBs) |
|-------------|---------------------| ---------------------:|
| ChatApp    |  .NET MAUI GA SR1          |                 240.876 |
| ChatApp    |  Avalonia 0.10.15           |                 - |
| ChatApp    |  Flutter 3.0.2            |                 175.596 |
| ChatApp    |  UNO Platform 4.3       |                 - |
| ChatApp    |  Xamarin.Forms 5.0 SR11     |                 197.504 |

_(lower values are better)_

![ChatApp Memory consumption](images/chat-app-memory.png)

## Dope Test

![Dope Test](images/dopetest-banner.png)

This test is based on rendering hundreds of texts and measuring the performance of Layouts, rendering, etc.

Inspired by https://github.com/maxim-saplin/dopetest_flutter by Maxim Saplin.

| Android App | Framework           | Build |
|-------------|---------------------| ---------------------:|
| DopeTest    |  .NET MAUI GA SR1         |                 42.3 |
| DopeTest    |  Avalonia 0.10.15           |                 - |
| DopeTest    |  Flutter 3.0.2            |                 11850.8 |
| DopeTest    |  UNO Platform 4.3       |                 91.9 |
| DopeTest    |  Xamarin.Forms 5.0 SR11      |                 68.5 |

_(higher values are better)_

![Dope Test](images/dope-test-build.png)

![Dope Test](images/dope-test-build-2.png)
## Contribute

_"The data in [insert device here] is different. Why isn't  [insert cross platform framework here] here?"_

Contributions are welcome, you can launch the same benchmarks on another device and add the data to the repository; you can create the examples used in another framework; you can add more tests to measure memory etc.

## Copyright and license

Code released under the [MIT license](https://opensource.org/licenses/MIT).