using System.Collections.Concurrent;
using Microsoft.VisualBasic;

namespace mis_221_pa_5_ncortezramirez_1
{
    public class Session
    {
        private string sportName;
        private int sessionId;
        private int lengthMinutes;
        private int numSeats;
        private double sessionPrice;
        private bool isFull;
        private string coachName;
        private bool isDeleted;

        public Session(string sportName, int sessionId, int lengthMinutes, int numSeats, double sessionPrice, bool isFull, string coachName, bool isDeleted)
        {
            this.sportName = sportName;
            this.sessionId = sessionId;
            this.lengthMinutes = lengthMinutes;
            this.numSeats = numSeats;
            this.sessionPrice = sessionPrice;
            this.coachName = coachName;
            isDeleted = isDeleted;
            isFull = isFull;
        }
        
        public Session()
        {

        }

    
        
        public string GetSportName()
        {
            return sportName;
        }
        public void SetSportName(string sportName)
        {
            this.sportName = sportName;
        }
        

        public int GetSessionId()
        {
            return sessionId;
        }
        public void SetId(int sessionId)
        {
            this.sessionId = sessionId;
        }

        public int GetLengthMinutes()
        {
            return lengthMinutes;
        }
        public void SetLengthMinutes(int lengthMinutes)
        {
            this.lengthMinutes = lengthMinutes;
        }

           public string GetCoachName()
        {
            return coachName;
        }
        public void SetCoachName(string coachName)
        {
            this.coachName = coachName;
        }

        public int GetNumSeats()
        {
            return numSeats;
        }
        public void SetNumSeats(int numSeats)
        {
            this.numSeats = numSeats;
        }

        public double GetSessionPrice()
        {
            return sessionPrice;
        }
        public void SetSessionPrice(double sessionPrice)
        {
            this.sessionPrice = sessionPrice;
        }

         public bool GetIsDeleted()
        {
            return isDeleted;
        }
        
        public void DeleteSession() 
        {
            this.isDeleted = true;
        }

         public bool GetIsFull()
        {
            return isFull;
        }
        public void SetIsFull(bool isFull)
        {
            this.isFull = isFull;
        }

        public void CheckIfFull(int currentRegistrations)
        {
            if(currentRegistrations >= numSeats)
            {
                isFull = true;
            }
            else
            {
                isFull = false;
            }
        }

        public override string ToString()
        {
            return $"{sessionId}\t{sportName}\t{lengthMinutes}\t{coachName}\t{sessionPrice}\t{numSeats}\t{isFull}\t{isDeleted}";
        }

    }
}