using System;
using System.Collections.Generic;

namespace Roommates.Models
{
    // C# representation of the Roommate table
    public class RoommateChores
    {
        public int Id { get; set; }
        public string RoommateName { get; set; }
        public int ChoreCount { get; set; }       
    }
}
