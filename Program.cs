using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");


string file = "mario.csv";
// make sure movie file exists
if (!File.Exists(file))
{
    logger.Error("File does not exist: {File}", file);
}
else
{
        // create parallel lists of character details
    // lists are used since we do not know number of lines of data
   List<Character> characters=new();
    // to populate the lists with data, read from the data file
    try
    {
        StreamReader sr = new(file);
        // first line contains column headers
        sr.ReadLine();
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
                     if (line is not null)
            {
                Character character = new();
                // character details are separated with comma(,)
                string[] characterDetails = line.Split(',');
                // 1st array element contains id
                character.Id = UInt64.Parse(characterDetails[0]);
                // 2nd array element contains character name
                character.Name = characterDetails[1];
                // 3rd array element contains character description
                character.Description = characterDetails[2];
                character.Species = characterDetails[3];
                character.FirstAppearance = characterDetails[4];
                character.YearCreated = Convert.ToInt32(characterDetails[5]);
                characters.Add(character);
            }
        }
        sr.Close();
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
    }

       string? choice;
    do
    {
        // display choices to user
        Console.WriteLine("1) Add Character");
        Console.WriteLine("2) Display All Characters");
        Console.WriteLine("Enter to quit");

        // input selection
        choice = Console.ReadLine();
        logger.Info("User choice: {Choice}", choice);

        if (choice == "1")
        {
            // Add Character
            Character character = new();
                        Console.WriteLine("Enter new character name: ");
         character.Name = Console.ReadLine() ?? string.Empty;
         if (!string.IsNullOrEmpty(character.Name)){
                            // check for duplicate name
          List<string>LowerCaseNames =
          characters.ConvertAll(character =>character.Name.ToLower());
                if (LowerCaseNames.Contains(character.Name.ToLower()))
                {
                    logger.Info($"Duplicate name {character.Name}");
                }
                else
                {
                    // generate id - use max value in Ids + 1
                    character.Id = characters.Max(character => character.Id) + 1;
                                       // input character description
                    Console.WriteLine("Enter description:");
                  character.Description = Console.ReadLine() ?? string.Empty;
                    // Input Species
                    Console.WriteLine("Enter species:");
                    character.Species = Console.ReadLine() ?? string.Empty;
                    // Input First Appearance
                    Console.WriteLine("Enter first appearance:");
                  character.FirstAppearance = Console.ReadLine() ?? string.Empty;
                    // Input Year Created
                    Console.WriteLine("Enter year created:");
                   character.YearCreated = Convert.ToInt32(Console.ReadLine());

                    // create file from data
                    StreamWriter sw = new(file, true);
                    sw.WriteLine($"{character.Id},{character.Name},{character.Description},{character.Species},{character.FirstAppearance},{character.YearCreated}");
                    sw.Close();
                    // add new character details to Lists
                    characters.Add(character);
                    // log transaction
                    logger.Info($"Character id {character.Id} added");                }
            } else {
                logger.Error("You must enter a name");
            }
        }
        else if (choice == "2")
        {
            // Display All Characters
                        // loop thru Lists
            foreach (Character character in characters)
            {
                // display character details
                Console.WriteLine(character.Display());
            }
        }
    } while (choice == "1" || choice == "2");
}

logger.Info("Program ended");