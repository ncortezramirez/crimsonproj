namespace mis_221_pa_5_ncortezramirez_1
{
    public class Registration
    {
     private int registrationId;
        private string athleteEmail;
        private string athleteName;
        private int sessionId; // This links to the Session Class (Foreign Key)
        private string registrationDate; // "Date are gertation the customer can enter"
        private bool isPaid; // "Payment status"
        private string status;

        public Registration(int registrationId, string athleteEmail, string athleteName, int sessionId, string registrationDate, bool isPaid, string status)
        {
            this.registrationId = registrationId;
            this.athleteEmail = athleteEmail;
            this.athleteName = athleteName;
            this.sessionId = sessionId;
            this.registrationDate = registrationDate;
            this.isPaid = isPaid;
            this.status = status;
        }
        
        public Registration()
        {
            
        }
        public int GetRegistrationId()
        {
            return registrationId;
        }
        public void SetRegistrationId(int registrationId)
        {
            this.registrationId = registrationId;
        }
        public string GetAthleteEmail()
        {
            return athleteEmail;
        }
        public void SetAthleteEmail(string athleteEmail)
        {
            this.athleteEmail = athleteEmail;
        }
        public string GetAthleteName()
        {
            return athleteName;
        }
        public void SetAthleteName(string athleteName)
        {
            this.athleteName = athleteName;
        }
        public int GetSessionId()
        {
            return sessionId;
        }
        public void SetSessionId(int sessionId)
        {
            this.sessionId = sessionId;
        }
        public string GetRegristationDate()
        {
            return registrationDate;
        }
        public void SetRegristationDate(string registrationDate)
        {
            this.registrationDate = registrationDate;
        }
        public bool GetIsPaid()
        {
            return isPaid;
        }
        
        public void SetIsPaid() 
        {
            this.isPaid = true;
        }

         public string GetStatus()
        {
            return status;
        }
        public void SetStatus(string status)
        {
            this.status = status;
        }
         public override string ToString()
        {
            return $"{registrationId}#{athleteEmail}#{athleteName}#{sessionId}#{registrationDate}#{isPaid}#{status}";
        }



    }
}