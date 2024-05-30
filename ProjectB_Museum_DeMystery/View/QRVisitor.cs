public class QRVisitor : View
{
    private static IMuseum museum = Program.Museum;
    public static string WhichVisitorQr()
    {
        museum.WriteLine("QR visitor:");
        return ReadLineString();
    }

    public static string ScanQr()
    {
        museum.WriteLine("Scan your QR code:");
        return ReadLineString();
    }
}