using Microsoft.AspNetCore.Mvc;
using mis_221_pa_5_ncortezramirez_1;

namespace CrimsonSportsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private static CrimsonUtility utility = new CrimsonUtility();

        // Load data when API starts
        static SessionsController()
        {
            utility.LoadData();
        }

        // GET: api/sessions - Get all available sessions
        [HttpGet]
        public IActionResult GetAvailableSessions()
        {
            Session[] sessions = utility.GetAllSessionsArray();
            
            // Convert to array of objects with seat information
            SessionResponse[] response = new SessionResponse[100];
            int count = 0;
            
            for (int i = 0; i < sessions.Length; i++)
            {
                if (sessions[i] != null)
                {
                    int seatsLeft = utility.GetRemainingSeats(sessions[i].GetSessionId());
                    
                    response[count] = new SessionResponse
                    {
                        Id = sessions[i].GetSessionId(),
                        Sport = sessions[i].GetSportName(),
                        Coach = sessions[i].GetCoachName(),
                        Duration = sessions[i].GetLengthMinutes(),
                        Price = sessions[i].GetSessionPrice(),
                        TotalSeats = sessions[i].GetNumSeats(),
                        SeatsLeft = seatsLeft,
                        IsFull = sessions[i].GetIsFull()
                    };
                    count++;
                }
            }
            
            return Ok(response);
        }

        // GET: api/sessions/{id} - Get specific session
        [HttpGet("{id}")]
        public IActionResult GetSession(int id)
        {
            Session session = utility.GetSessionByIdAPI(id);
            if (session == null)
            {
                return NotFound(new { message = "Session not found" });
            }
            
            int seatsLeft = utility.GetRemainingSeats(session.GetSessionId());
            
            SessionResponse response = new SessionResponse
            {
                Id = session.GetSessionId(),
                Sport = session.GetSportName(),
                Coach = session.GetCoachName(),
                Duration = session.GetLengthMinutes(),
                Price = session.GetSessionPrice(),
                TotalSeats = session.GetNumSeats(),
                SeatsLeft = seatsLeft,
                IsFull = session.GetIsFull()
            };
            
            return Ok(response);
        }

        // POST: api/sessions - Add new session
        [HttpPost]
        public IActionResult AddSession([FromBody] SessionDto sessionDto)
        {
            try
            {
                Session newSession = utility.AddSessionAPI(
                    sessionDto.Sport,
                    sessionDto.Coach,
                    sessionDto.Duration,
                    sessionDto.Price,
                    sessionDto.Seats
                );
                utility.SaveData();
                
                SessionResponse response = new SessionResponse
                {
                    Id = newSession.GetSessionId(),
                    Sport = newSession.GetSportName(),
                    Coach = newSession.GetCoachName(),
                    Duration = newSession.GetLengthMinutes(),
                    Price = newSession.GetSessionPrice(),
                    TotalSeats = newSession.GetNumSeats(),
                    SeatsLeft = newSession.GetNumSeats(),
                    IsFull = false
                };
                
                return CreatedAtAction(nameof(GetSession), new { id = newSession.GetSessionId() }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/sessions/{id} - Edit session
        [HttpPut("{id}")]
        public IActionResult UpdateSession(int id, [FromBody] SessionUpdateDto updateDto)
        {
            try
            {
                bool success = utility.UpdateSessionAPI(id, updateDto.Price);
                if (!success)
                {
                    return NotFound(new { message = "Session not found" });
                }
                
                utility.SaveData();
                return Ok(new { message = "Session updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/sessions/{id} - Soft delete session
        [HttpDelete("{id}")]
        public IActionResult DeleteSession(int id)
        {
            bool success = utility.DeleteSessionAPI(id);
            if (!success)
            {
                return NotFound(new { message = "Session not found" });
            }
            
            utility.SaveData();
            return Ok(new { message = "Session deleted successfully" });
        }

        // POST: api/sessions/complete/{id} - Mark session complete
        [HttpPost("complete/{id}")]
        public IActionResult MarkComplete(int id)
        {
            int count = utility.MarkSessionCompleteAPI(id);
            utility.SaveData();
            return Ok(new { message = count + " registrations marked as completed" });
        }
    }

    // Response and DTO classes
    public class SessionResponse
    {
        public int Id { get; set; }
        public string Sport { get; set; }
        public string Coach { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public int TotalSeats { get; set; }
        public int SeatsLeft { get; set; }
        public bool IsFull { get; set; }
    }

    public class SessionDto
    {
        public string Sport { get; set; }
        public string Coach { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public int Seats { get; set; }
    }

    public class SessionUpdateDto
    {
        public double Price { get; set; }
    }
}