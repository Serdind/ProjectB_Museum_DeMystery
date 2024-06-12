public class QRVisitor : View
{
    
    public static string WhichVisitorQr()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Barcode visitor:");
        return ReadLineString();
    }

    public static string ScanQr()
    {
        IMuseum museum = Program.Museum;
        museum.WriteLine("");
        museum.WriteLine("Tip: Press the button and hold the scanner of the device closely to the barcode.\n\nScan the barcode that is located on the ticket you bought with the given device:");
        return ReadLineString();
    }
}