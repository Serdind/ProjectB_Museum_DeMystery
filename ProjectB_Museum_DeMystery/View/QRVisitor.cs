public class QRVisitor : View
{
    public static string WhichVisitorQr()
    {
        Console.WriteLine("QR visitor:");
        return ReadLineString();
    }

    public static string ScanQr()
    {
        Console.WriteLine("Scan your QR code:");
        return ReadLineString();
    }
}