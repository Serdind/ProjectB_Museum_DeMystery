public class QRVisitor : View
{
    private static IMuseum museum = Program.Museum;
    public static string WhichVisitorQr()
    {
        museum.WriteLine("Barcode visitor:");
        return ReadLineString();
    }

    public static string ScanQr()
    {
        museum.WriteLine("Scan the barcode that is located on the ticket you bought with the given device. Press the button and hold the scanner of the device closely to the barcode:");
        return ReadLineString();
    }
}