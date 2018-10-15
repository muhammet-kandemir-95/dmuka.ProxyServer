# dmuka.ProxyServer

Demo : [Download Project](https://github.com/muhammet-kandemir-95/dmuka.ProxyServer/archive/master.zip)

 Bilgisayarınızda proxy server kurmanızı sağlar. Bu sayede belli bir port üzerindeki veriyi başka bir porta aktarabilirsiniz.

## Bağımlı Olduğu Kütüphaneler
* [dmuka.Semaphore](https://github.com/muhammet-kandemir-95/dmuka.Semaphore)

## Public Variables

### ProxyServerPort
 Sunucu hangi port üzerinden veri aktarıyor.
```csharp
public int ProxyServerPort { get; }
```

### Hostname
 Sunucu hangi sunucuyu dinliyor.
```csharp
public string Hostname { get; }
```

### Port
 Sunucu hangi port üzerinden dinliyor.
```csharp
public int Port { get; }
```

### ClientCount
 Aynı anda yürütülen "Client" sayısını belirler.
```csharp
public int ClientCount { get; }
```

### Disposed
 Aktif instance daha önceden "Dispose" oldumu.
```csharp
public bool Disposed { get; }
```

### Started
 Aktif instance daha önceden başlatıldımı.
```csharp
public bool Started { get; }
```

## Public Methods

### Start
 Aktif instance işlemini başlatır.
```csharp
void Start()
```

### Dispose
 Aktif instance işlemini "Dispose" eder.
```csharp
public void Dispose()
```

## Örnek Kullanım

```csharp
using System;

namespace dmuka.ProxyServer.App
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Write("Type host name = ");
                    string hostName = Console.ReadLine();

                    Console.Write("Type host port = ");
                    int port = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Type proxy port = ");
                    int proxyPort = Convert.ToInt32(Console.ReadLine());

                    Server server = new Server(hostName, port, proxyPort, coreCount: 100);
                    server.Start();
                }
                catch (Exception)
                {
                    Console.WriteLine("Server crached!");
                }
            }
        }
    }
}
```
