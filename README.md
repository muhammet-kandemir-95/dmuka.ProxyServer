# dmuka.ProxyServer

Demo : [Download Project](https://github.com/muhammet-kandemir-95/dmuka.ProxyServer/archive/master.zip)

 You can create proxy server on your pc using this library. So the server can proxy on any port.

## Used Library
* [dmuka.Semaphore](https://github.com/muhammet-kandemir-95/dmuka.Semaphore)

## Public Variables

### ProxyServerPort
 New port on the server.
```csharp
public int ProxyServerPort { get; }
```

### Hostname
 Which host do the server listening? 
```csharp
public string Hostname { get; }
```

### Port
 Which port do the server listening? 
```csharp
public int Port { get; }
```

### ClientCount
 Thread count(With semaphore).
```csharp
public int ClientCount { get; }
```

### Disposed
 Is the current instance dispose?
```csharp
public bool Disposed { get; }
```

### Started
 Was the current instance start?
```csharp
public bool Started { get; }
```

## Public Methods

### Start
 This is for start to current instance.
```csharp
void Start()
```

### Dispose
 This is for dispose to current instance.
```csharp
public void Dispose()
```

## Example Usage

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
