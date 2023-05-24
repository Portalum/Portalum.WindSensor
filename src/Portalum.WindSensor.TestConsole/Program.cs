using Nager.TcpClient;
using System.Text;


void OnDataReceived(byte[] receivedData)
{
    if (receivedData[0] == 0x02 && receivedData[receivedData.Length - 5] == 0x03)
    {
        Console.WriteLine("valid package");
    }

    Console.WriteLine(Encoding.ASCII.GetString(receivedData));
    Console.WriteLine(BitConverter.ToString(receivedData));
}

/*
 * USR CONFIGURATION
 * 
 * GILL Wind Sensor
 * Connector A: Green and Pink Cable
 * Connector B: Yellow and Violet Cable
 * 
 * 
 * BaudRate 9600
 * DataSize 8bit
 * Parity: None
 * StopBits: 1
 * Local Port Number: 8005
 * Work Mode: TCP Server
 * Reset: false
 * Link: true
 * Index: false
 * Similar: RFC2217: true
 */


using var cancellationTokenSource = new CancellationTokenSource(1000);

using var tcpClient = new TcpClient();
tcpClient.DataReceived += OnDataReceived;
Console.WriteLine("Try connect");
await tcpClient.ConnectAsync("10.14.20.19", 8005, cancellationTokenSource.Token);
Console.WriteLine("Connected");
//Console.WriteLine("Send");
///await tcpClient.SendAsync(new byte[] { 0x01, 0x0A });
await Task.Delay(300000);
tcpClient.Disconnect();
tcpClient.DataReceived -= OnDataReceived;


Console.WriteLine("done");