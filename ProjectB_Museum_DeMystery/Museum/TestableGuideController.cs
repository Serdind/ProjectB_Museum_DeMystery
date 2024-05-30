using System.Text.Json.Serialization;
using Spectre.Console;
using Newtonsoft.Json;
using System.Globalization;
using NUnit.Framework.Internal;

public class TestableGuideController
{
    public readonly IMuseum Museum;

    public TestableGuideController(IMuseum museum)
    {
        Museum = museum;
    }
    public void ViewVisitorsTour(int tourId, GuidedTour tour, TestableGuide testableGuide)
    {
        string subdirectory = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery";
        string fileName = "unique_codes.json";
        string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(userDirectory, subdirectory, fileName);

        TestableTour testableTour = new TestableTour(Museum);

        List<string> uniqueCodes = TestJsonData.LoadUniqueCodesFromTestFile(Museum);

        

        if (tour.ID == tourId && tour.Status)
        {
            testableTour.OverviewVisitorsTour(tourId);

            while (true)
            {
                string subdirectory1 = @"ProjectB\ProjectB_Museum_DeMystery\ProjectB_Museum_DeMystery\TestData";
                string fileName1 = "visitorsTest.json";
                string userDirectory1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath1 = Path.Combine(userDirectory1, subdirectory1, fileName1);
                
                if (Museum.FileExists(filePath))
                {
                    string json = Museum.ReadAllText(filePath);
                    var visitors = JsonConvert.DeserializeObject<List<Visitor>>(json);
                    
                    visitors = visitors.Where(v => v.TourId == tourId).OrderBy(t => t.TourId).ToList();

                    if (visitors.Any())
                    {
                        Museum.WriteLine("Add visitor(A)\nRemove visitor(R)\nGo back(B)");
                    }
                    else
                    {
                        Museum.WriteLine("Add visitor(A)\nGo back(B)");
                    }
                }
                string option = Museum.ReadLine();

                if (option.ToLower() == "a" || option.ToLower() == "add visitor")
                {
                    bool codeValid = false;

                    while (!codeValid)
                    {
                        Museum.WriteLine("Insert (Back or B) if you want to go back");
                        Museum.WriteLine("Press any key to continue...\n");
                        Museum.ReadKey();
                        Museum.WriteLine("QR visitor:");
                        string qr = Museum.ReadLine();

                        if (qr.ToLower() == "b" || qr.ToLower() == "back")
                        {
                            break;
                        }

                        if (uniqueCodes.Contains(qr))
                        {
                            testableGuide.AddVisitorToTour(tourId, qr);
                            codeValid = true;
                        }
                        else
                        {
                            Museum.WriteLine("Code is not valid.");
                            Museum.WriteLine("Press any key to continue...\n");
                            Museum.ReadKey();
                        }
                    }
                }
                else if (option.ToLower() == "r" || option.ToLower() == "remove visitor")
                {
                    bool codeValid = false;

                    while (!codeValid)
                    {
                        Museum.WriteLine("Insert (Back or B) if you want to go back");
                        Museum.WriteLine("Press any key to continue...\n");
                        Museum.ReadKey();
                        Museum.WriteLine("QR visitor:");
                        string qr = Museum.ReadLine();

                        if (qr.ToLower() == "b" || qr.ToLower() == "back")
                        {
                            break;
                        }

                        if (uniqueCodes.Contains(qr))
                        {
                            testableGuide.RemoveVisitorFromTour(tourId, qr);
                            codeValid = true;
                        }
                        else
                        {
                            Museum.WriteLine("Code is not valid.");
                            Museum.WriteLine("Press any key to continue...\n");
                            Museum.ReadKey();
                        }
                    }
                }
                else if (option.ToLower() == "b" || option.ToLower() == "go back")
                {
                    break;
                }
                else
                {
                    Museum.WriteLine("Wrong input. Try again.");
                }
            }
        }
        else
        {
            Museum.WriteLine("Tour is not available.");
            Museum.WriteLine("Press any key to continue...\n");
            Museum.ReadKey();
        }
    }

    public void OptionsGuide(List<GuidedTour> tours, TestableGuide testableGuide)
    {
        while (true)
        {
            Museum.WriteLine("View visitors(V)\nStart tour(S)\nGo back(B)");
            string option = Museum.ReadLine();
            int tourID;

            if (option.ToLower() == "v" || option.ToLower() == "view visitors")
            {
                Museum.WriteLine("Insert (Back or B) if you want to go back");
                tourID = TourId.WhichTourId();

                if (tourID == -1)
                {
                    break;
                }

                bool tourFound = false;
                foreach (var tour in tours)
                {
                    if (tour.ID == tourID)
                    {
                        ViewVisitorsTour(tourID, tour, testableGuide);
                        tourFound = true;
                        break;
                    }
                }

                if (!tourFound)
                {
                    Museum.WriteLine("Tour not found.");
                }
            }
            else if (option.ToLower() == "s" || option.ToLower() == "start tour")
            {
                Museum.WriteLine("Insert (Back or B) if you want to go back");
                tourID = TourId.WhichTourId();

                if (tourID == -1)
                {
                    break;
                }
                testableGuide.StartTour(tourID);
                break;
            }
            else if (option.ToLower() == "b" || option.ToLower() == "go back")
            {
                break;
            }
            else
            {
                Museum.WriteLine("Wrong input. Try again.");
            }
        }
    }
}