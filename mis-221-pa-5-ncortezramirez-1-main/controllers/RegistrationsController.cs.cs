using Microsoft.AspNetCore.Mvc;
using mis_221_pa_5_ncortezramirez_1;

namespace CrimsonSportsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private static CrimsonUtility utility = new CrimsonUtility();

        static RegistrationsController()
        {
            utility.LoadData();
        }

        // GET: api/registrations - Get all registrations
        [HttpGet]
        public IActionResult GetAllRegistrations()
        {
            Registration[] registrations = utility.GetAllRegistrationsArray();
            
            // Convert to response array
            RegistrationResponse[] response = new RegistrationResponse[100];
            int count = 0;
            
            for (int i = 0; i < registrations.Length; i++)
            {
                if (registrations[i] != null)
                {
                    string sportName = utility.GetSportNameForSession(registrations[i].GetSessionId());
                    
                    response[count] = new RegistrationResponse
                    {
                        Id = registrations[i].GetRegistrationId(),
                        Email = registrations[i].GetAthleteEmail(),
                        Name = registrations[i].GetAthleteName(),
                        SessionId = registrations[i].GetSessionId(),
                        SportName = sportName,
                        Date = registrations[i].GetRegristationDate(),
                        IsPaid = registrations[i].GetIsPaid(),
                        Status = registrations[i].GetStatus()
                    };
                    count++;
                }
            }
            
            return Ok(response);
        }

        // GET: api/registrations/athlete/{email} - Get registrations by email
        [HttpGet("athlete/{email}")]
        public IActionResult GetAthleteRegistrations(string email)
        {
            Registration[] registrations = utility.GetRegistrationsByEmail(email);
            
            // Convert to response array
            RegistrationResponse[] response = new RegistrationResponse[100];
            int count = 0;
            
            for (int i = 0; i < registrations.Length; i++)
            {
                if (registrations[i] != null)
                {
                    string sportName = utility.GetSportNameForSession(registrations[i].GetSessionId());
                    
                    response[count] = new RegistrationResponse
                    {
                        Id = registrations[i].GetRegistrationId(),
                        SessionId = registrations[i].GetSessionId(),
                        SportName = sportName,
                        Date = registrations[i].GetRegristationDate(),
                        Status = registrations[i].GetStatus()
                    };
                    count++;
                }
            }
            
            return Ok(response);
        }

        // POST: api/registrations - Register for session
        [HttpPost]
        public IActionResult RegisterForSession([FromBody] RegistrationDto regDto)
        {
            try
            {
                Registration registration = utility.RegisterForSessionAPI(
                    regDto.Email,
                    regDto.Name,
                    regDto.SessionId,
                    regDto.DiscountApplied
                );
                
                if (registration == null)
                {
                    return BadRequest(new { message = "Session is full or invalid" });
                }
                
                utility.SaveData();
                
                string sportName = utility.GetSportNameForSession(registration.GetSessionId());
                
                RegistrationResponse response = new RegistrationResponse
                {
                    Id = registration.GetRegistrationId(),
                    Email = registration.GetAthleteEmail(),
                    Name = registration.GetAthleteName(),
                    SessionId = registration.GetSessionId(),
                    SportName = sportName,
                    Date = registration.GetRegristationDate(),
                    IsPaid = registration.GetIsPaid(),
                    Status = registration.GetStatus()
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/registrations/reports/daily - Daily sessions report
        [HttpGet("reports/daily")]
        public IActionResult GetDailyReport()
        {
            string[] reportData = utility.GetDailySessionsReportAPI();
            
            // Convert to response array
            DailyReportResponse[] response = new DailyReportResponse[100];
            int count = 0;
            
            for (int i = 0; i < reportData.Length; i++)
            {
                if (reportData[i] != null)
                {
                    string[] parts = reportData[i].Split('#');
                    response[count] = new DailyReportResponse
                    {
                        Date = parts[0],
                        Count = int.Parse(parts[1])
                    };
                    count++;
                }
            }
            
            return Ok(response);
        }

        // GET: api/registrations/reports/daterange - Date range report
        [HttpGet("reports/daterange")]
        public IActionResult GetDateRangeReport([FromQuery] string startDate, [FromQuery] string endDate)
        {
            try
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);
                
                Registration[] registrations = utility.GetDateRangeReportAPI(start, end);
                
                // Convert to response array
                RegistrationResponse[] response = new RegistrationResponse[100];
                int count = 0;
                
                for (int i = 0; i < registrations.Length; i++)
                {
                    if (registrations[i] != null)
                    {
                        string sportName = utility.GetSportNameForSession(registrations[i].GetSessionId());
                        
                        response[count] = new RegistrationResponse
                        {
                            Id = registrations[i].GetRegistrationId(),
                            Email = registrations[i].GetAthleteEmail(),
                            Name = registrations[i].GetAthleteName(),
                            SessionId = registrations[i].GetSessionId(),
                            SportName = sportName,
                            Date = registrations[i].GetRegristationDate(),
                            Status = registrations[i].GetStatus()
                        };
                        count++;
                    }
                }
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // Response classes
    public class RegistrationResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int SessionId { get; set; }
        public string SportName { get; set; }
        public string Date { get; set; }
        public bool IsPaid { get; set; }
        public string Status { get; set; }
    }

    public class DailyReportResponse
    {
        public string Date { get; set; }
        public int Count { get; set; }
    }

    // Data Transfer Object
    public class RegistrationDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int SessionId { get; set; }
        public bool DiscountApplied { get; set; }
    }
}