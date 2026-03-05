using System;
using System.IO;

namespace mis_221_pa_5_ncortezramirez_1
{
    public class CrimsonUtility
    {
        private Session[] sessions = new Session[100];
        private int sessionCount = 0;
        private Registration[] registrations = new Registration[100];
        private int registrationCount = 0;

        public CrimsonUtility()
        {
            
        }

        // --- FILE I/O ---
        public void LoadData()
        {
            if (File.Exists("sessions.txt"))
            {
                string[] lines = File.ReadAllLines("sessions.txt");
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split('#');
                    // Ensure we have enough parts to avoid crash
                    if (parts.Length < 7) continue; 

                    sessions[sessionCount] = new Session(
                        parts[1], // Sport
                        int.Parse(parts[0]), // ID
                        int.Parse(parts[2]), // Length
                        int.Parse(parts[5]), // Seats
                        double.Parse(parts[4]), // Price
                        bool.Parse(parts[6]), // IsFull
                        parts[3], // Coach
                        bool.Parse(parts[7]) // IsDeleted
                    );
                    sessionCount++;
                }
            }
            if (File.Exists("registration.txt"))
            {
                string[] lines = File.ReadAllLines("registration.txt");
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split('#');
                    registrations[registrationCount] = new Registration(
                        int.Parse(parts[0]), parts[1], parts[2], int.Parse(parts[3]), 
                        parts[4], bool.Parse(parts[5]), parts[6]
                    );
                    registrationCount++;
                }
            }
        }

        public void SaveData()
        {
            using (StreamWriter writer = new StreamWriter("sessions.txt"))
            {
                for (int i = 0; i < sessionCount; i++) writer.WriteLine(sessions[i].ToString());
            }
            using (StreamWriter writer = new StreamWriter("registration.txt"))
            {
                for (int i = 0; i < registrationCount; i++) writer.WriteLine(registrations[i].ToString());
            }
        }

        // --- SEARCH (Binary Search Requirement) ---
        public int FindSessionIndexById(int targetId)
        {
            // Note: Binary Search requires sorted array. Assuming sessions added in order of ID.
            int low = 0;
            int high = sessionCount - 1;
            while (low <= high)
            {
                int mid = (low + high) / 2;
                if (sessions[mid].GetSessionId() == targetId) return mid;
                else if (sessions[mid].GetSessionId() < targetId) low = mid + 1;
                else high = mid - 1;
            }
            return -1;
        }

        // --- MANAGER FUNCTIONS ---
        public void AddSession()
        {
            Console.WriteLine("Enter Sport Name:");
            string sport = Console.ReadLine();
            Console.WriteLine("Enter Coach Name:");
            string coach = Console.ReadLine();
            Console.WriteLine("Enter Duration (min):");
            int duration = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Price:");
            double price = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter Max Seats:");
            int seats = int.Parse(Console.ReadLine());

            int newId = sessionCount + 1;
            sessions[sessionCount] = new Session(sport, newId, duration, seats, price, false, coach, false);
            sessionCount++;
            Console.WriteLine("Session Added Successfully!");
        }

        public void DeleteSession()
        {
            Console.Write("Enter Session ID to delete: ");
            int id = int.Parse(Console.ReadLine());
            int index = FindSessionIndexById(id);
            if (index != -1)
            {
                // Soft Delete Requirement
                sessions[index].DeleteSession();
                Console.WriteLine("Session marked as deleted (Soft Delete).");
            }
            else Console.WriteLine("Session not found.");
        }

        public void EditSession()
        {
            Console.Write("Enter Session ID to edit: ");
            int id = int.Parse(Console.ReadLine());
            int index = FindSessionIndexById(id);
            if (index == -1) { Console.WriteLine("Not found."); return; }

            Console.WriteLine($"Editing {sessions[index].GetSportName()}. Enter new Price (or -1 to keep current):");
            double newPrice = double.Parse(Console.ReadLine());
            if (newPrice != -1) sessions[index].SetSessionPrice(newPrice);
            
            Console.WriteLine("Session updated.");
        }

        public void MarkSessionComplete()
        {
            Console.Write("Enter Session ID to mark complete: ");
            int id = int.Parse(Console.ReadLine());
            // This updates registrations for this session
            int updateCount = 0;
            for(int i=0; i<registrationCount; i++)
            {
                if(registrations[i].GetSessionId() == id)
                {
                    registrations[i].SetStatus("Completed");
                    updateCount++;
                }
            }
            Console.WriteLine($"{updateCount} registrations marked as Completed.");
        }

        // --- ATHLETE FUNCTIONS ---
        public void ViewAvailableSessions()
        {
            Console.WriteLine("\n--- Available Sessions ---");
            Console.WriteLine("ID\tSport\tPrice\tSeats Left");
            for (int i = 0; i < sessionCount; i++)
            {
                if (!sessions[i].GetIsDeleted() && !sessions[i].GetIsFull())
                {
                    // Calculate remaining seats
                    int taken = 0;
                    for(int j=0; j<registrationCount; j++)
                    {
                        if(registrations[j].GetSessionId() == sessions[i].GetSessionId()) taken++;
                    }
                    int left = sessions[i].GetNumSeats() - taken;
                    if(left > 0)
                        Console.WriteLine($"{sessions[i].GetSessionId()}\t{sessions[i].GetSportName()}\t${sessions[i].GetSessionPrice()}\t{left}");
                }
            }
        }

        public void RegisterForSession()
        {
            ViewAvailableSessions();
            Console.Write("\nEnter Session ID to register: ");
            int sessId = int.Parse(Console.ReadLine());
            int sessIndex = FindSessionIndexById(sessId);

            if(sessIndex == -1 || sessions[sessIndex].GetIsDeleted())
            {
                Console.WriteLine("Invalid Session.");
                return;
            }

            Console.Write("Enter Your Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Your Name: ");
            string name = Console.ReadLine();

            // --- GAME INTEGRATION (The Winning Feature) ---
            Console.WriteLine("\n*** SPECIAL OFFER ***");
            Console.WriteLine("Would you like to play 'Crimson 2048' to win a 10% discount? (y/n)");
            string play = Console.ReadLine().ToLower();
            bool discountWon = false;

            if (play == "y")
            {
                Game2048 game = new Game2048();
                discountWon = game.PlayForDiscount();
            }

            double finalPrice = sessions[sessIndex].GetSessionPrice();
            if (discountWon)
            {
                finalPrice *= 0.90; // Apply 10% off
                Console.WriteLine($"Discount Applied! New Price: ${finalPrice}");
            }
            else
            {
                Console.WriteLine($"Standard Price: ${finalPrice}");
            }

            // Create Registration
            int newRegId = registrationCount + 1;
            string date = DateTime.Now.ToString("MM/dd/yyyy");
Console.WriteLine($"Registration Date Recorded: {date}");
            
            // Add to array
            registrations[registrationCount] = new Registration(newRegId, email, name, sessId, date, true, "Pending");
            registrationCount++;

            Console.WriteLine("Registration Successful!");
        }

        public void ViewPastSessions()
        {
            Console.Write("Enter your email: ");
            string email = Console.ReadLine();
            Console.WriteLine("\n--- Your History ---");
            for(int i=0; i<registrationCount; i++)
            {
                if(registrations[i].GetAthleteEmail() == email)
                {
                    // Find sport name for better display
                    string sport = "Unknown";
                    int sIdx = FindSessionIndexById(registrations[i].GetSessionId());
                    if(sIdx != -1) sport = sessions[sIdx].GetSportName();

                    Console.WriteLine($"Date: {registrations[i].GetRegristationDate()} | Sport: {sport} | Status: {registrations[i].GetStatus()}");
                }
            }
        }

        // --- REPORTS (Hidden Requirements) ---
      public void RunReports()
{
    Console.WriteLine("\n--- Select Report ---");
    Console.WriteLine("1. Remaining Seats");
    Console.WriteLine("2. Session Pairs (>1 Hour)");
    Console.WriteLine("3. Registrations by Date Range");
    Console.WriteLine("4. Daily Sessions Report"); // NEW OPTION
    string input = Console.ReadLine();

    if (input == "1") ShowRemainingSeats();
    else if (input == "2") ShowSessionPairs();
    else if (input == "3") ShowDateRangeReport();
    else if (input == "4") ShowDailySessionsReport(); // Call the new method
}

        private void ShowRemainingSeats()
        {
            Console.WriteLine("\n--- Seat Availability ---");
            for (int i = 0; i < sessionCount; i++)
            {
                if (sessions[i].GetIsDeleted()) continue;
                int count = 0;
                for (int j = 0; j < registrationCount; j++)
                    if (registrations[j].GetSessionId() == sessions[i].GetSessionId()) count++;
                
                int left = sessions[i].GetNumSeats() - count;
                Console.WriteLine($"{sessions[i].GetSportName()}: {left} seats left");
            }
        }

        private void ShowSessionPairs()
        {
            Console.WriteLine("\n--- Valid Pairs (>60min) ---");
            for (int i = 0; i < sessionCount; i++)
            {
                for (int j = i + 1; j < sessionCount; j++)
                {
                    if (sessions[i].GetLengthMinutes() + sessions[j].GetLengthMinutes() >= 60)
                    {
                        Console.WriteLine($"{sessions[i].GetSportName()} + {sessions[j].GetSportName()}");
                    }
                }
            }
        }

        private void ShowDateRangeReport()
        {
            // Hidden Requirement: Date Range
            Console.WriteLine("Enter Start Date (MM/dd/yyyy):");
            DateTime start = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter End Date (MM/dd/yyyy):");
            DateTime end = DateTime.Parse(Console.ReadLine());

            Console.WriteLine($"\n--- Registrations between {start.ToShortDateString()} and {end.ToShortDateString()} ---");
            for (int i = 0; i < registrationCount; i++)
            {
                DateTime regDate = DateTime.Parse(registrations[i].GetRegristationDate());
                if (regDate >= start && regDate <= end)
                {
                    Console.WriteLine($"ID: {registrations[i].GetRegistrationId()} | Athlete: {registrations[i].GetAthleteName()}");
                }
            }
        }

        public void ShowDailySessionsReport()
{
    Console.WriteLine("\n--- Daily Sessions Report (Registrations per Day) ---");

    // 1. Get all dates from registrations
    string[] dates = new string[registrationCount];
    for (int i = 0; i < registrationCount; i++)
    {
        dates[i] = registrations[i].GetRegristationDate();
    }

    // 2. Sort the dates so we can count them easily
    Array.Sort(dates);

    // 3. Loop through and count duplicates
    if (dates.Length > 0)
    {
        string currentDate = dates[0];
        int count = 1;

        for (int i = 1; i < dates.Length; i++)
        {
            if (dates[i] == currentDate)
            {
                count++;
            }
            else
            {
                // Print the previous group
                Console.WriteLine($"{currentDate}: {count}");
                
                // Reset for the new group
                currentDate = dates[i];
                count = 1;
            }
        }
        // Print the final group
        Console.WriteLine($"{currentDate}: {count}");
    }
    else
    {
        Console.WriteLine("No registrations found.");
    }
}
    


    // ===== API METHODS (Add these to the bottom of your CrimsonUtility class) =====

public Session[] GetAllSessionsArray()
{
    Session[] availableSessions = new Session[sessionCount];
    int count = 0;
    
    for (int i = 0; i < sessionCount; i++)
    {
        if (!sessions[i].GetIsDeleted())
        {
            availableSessions[count] = sessions[i];
            count++;
        }
    }
    
    return availableSessions;
}

public int GetRemainingSeats(int sessionId)
{
    int sessionIndex = FindSessionIndexById(sessionId);
    if (sessionIndex == -1)
    {
        return 0;
    }
    
    int taken = 0;
    for (int j = 0; j < registrationCount; j++)
    {
        if (registrations[j].GetSessionId() == sessionId)
        {
            taken++;
        }
    }
    
    int seatsLeft = sessions[sessionIndex].GetNumSeats() - taken;
    return seatsLeft;
}

public Session GetSessionByIdAPI(int id)
{
    int index = FindSessionIndexById(id);
    if (index != -1 && !sessions[index].GetIsDeleted())
    {
        return sessions[index];
    }
    return null;
}

public Session AddSessionAPI(string sport, string coach, int duration, double price, int seats)
{
    int newId = sessionCount + 1;
    sessions[sessionCount] = new Session(sport, newId, duration, seats, price, false, coach, false);
    sessionCount++;
    return sessions[sessionCount - 1];
}

public bool UpdateSessionAPI(int id, double newPrice)
{
    int index = FindSessionIndexById(id);
    if (index == -1)
    {
        return false;
    }

    sessions[index].SetSessionPrice(newPrice);
    return true;
}

public bool DeleteSessionAPI(int id)
{
    int index = FindSessionIndexById(id);
    if (index == -1)
    {
        return false;
    }

    sessions[index].DeleteSession();
    return true;
}

public int MarkSessionCompleteAPI(int sessionId)
{
    int count = 0;
    for (int i = 0; i < registrationCount; i++)
    {
        if (registrations[i].GetSessionId() == sessionId)
        {
            registrations[i].SetStatus("Completed");
            count++;
        }
    }
    return count;
}

public Registration[] GetAllRegistrationsArray()
{
    Registration[] allRegs = new Registration[registrationCount];
    for (int i = 0; i < registrationCount; i++)
    {
        allRegs[i] = registrations[i];
    }
    return allRegs;
}

public Registration[] GetRegistrationsByEmail(string email)
{
    Registration[] athleteRegs = new Registration[registrationCount];
    int count = 0;
    
    for (int i = 0; i < registrationCount; i++)
    {
        if (registrations[i].GetAthleteEmail() == email)
        {
            athleteRegs[count] = registrations[i];
            count++;
        }
    }
    
    return athleteRegs;
}

public Registration RegisterForSessionAPI(string email, string name, int sessionId, bool discountApplied)
{
    int sessIndex = FindSessionIndexById(sessionId);
    if (sessIndex == -1)
    {
        return null;
    }
    
    if (sessions[sessIndex].GetIsDeleted())
    {
        return null;
    }

    // Check if full
    int taken = 0;
    for (int i = 0; i < registrationCount; i++)
    {
        if (registrations[i].GetSessionId() == sessionId)
        {
            taken++;
        }
    }

    if (taken >= sessions[sessIndex].GetNumSeats())
    {
        return null;
    }

    // Create registration
    int newRegId = registrationCount + 1;
    string date = DateTime.Now.ToString("MM/dd/yyyy");

    registrations[registrationCount] = new Registration(
        newRegId, email, name, sessionId, date, true, "Pending"
    );
    registrationCount++;

    return registrations[registrationCount - 1];
}

public string[] GetDailySessionsReportAPI()
{
    // Get all unique dates
    string[] allDates = new string[registrationCount];
    int[] dateCounts = new int[registrationCount];
    int uniqueDateCount = 0;
    
    for (int i = 0; i < registrationCount; i++)
    {
        string currentDate = registrations[i].GetRegristationDate();
        bool found = false;
        int foundIndex = -1;
        
        // Check if date already exists
        for (int j = 0; j < uniqueDateCount; j++)
        {
            if (allDates[j] == currentDate)
            {
                found = true;
                foundIndex = j;
                break;
            }
        }
        
        if (found)
        {
            dateCounts[foundIndex]++;
        }
        else
        {
            allDates[uniqueDateCount] = currentDate;
            dateCounts[uniqueDateCount] = 1;
            uniqueDateCount++;
        }
    }
    
    // Create report array
    string[] report = new string[uniqueDateCount];
    for (int i = 0; i < uniqueDateCount; i++)
    {
        report[i] = allDates[i] + "#" + dateCounts[i];
    }
    
    return report;
}

public Registration[] GetDateRangeReportAPI(DateTime start, DateTime end)
{
    Registration[] dateRangeRegs = new Registration[registrationCount];
    int count = 0;
    
    for (int i = 0; i < registrationCount; i++)
    {
        DateTime regDate = DateTime.Parse(registrations[i].GetRegristationDate());
        if (regDate >= start && regDate <= end)
        {
            dateRangeRegs[count] = registrations[i];
            count++;
        }
    }
    
    return dateRangeRegs;
}

public string GetSportNameForSession(int sessionId)
{
    int index = FindSessionIndexById(sessionId);
    if (index != -1)
    {
        return sessions[index].GetSportName();
    }
    return "Unknown";
}
}
}