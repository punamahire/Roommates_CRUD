using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }
        public Roommate GetRoommateById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT top 1 rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, r.Name
                                             FROM Roommate rm
                                             LEFT JOIN Room r on r.Id = rm.RoomId
                                             WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                                Room = new Room { Name = reader.GetString(reader.GetOrdinal("Name")) }
                                
                            };
                        }
                        return roommate;
                    }

                }
            }
        }

        public List<Roommate> GetAll()
        {
            List<Roommate> roommates = new List<Roommate>();
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT rm.Id, rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, r.Name
                                             FROM Roommate rm
                                             LEFT JOIN Room r on r.Id = rm.RoomId";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        while (reader.Read())
                        {   
                            roommate = new Roommate
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                                Room = new Room { Name = reader.GetString(reader.GetOrdinal("Name")) }

                            };

                            // ...and add that room object to our list.
                            roommates.Add(roommate);
                        }
                        // Return the list of rooms who whomever called this method.
                        return roommates;
                    }

                }
            }
        }

        public List<Roommate> GetRoommatesByChoreId(int choreId)
        {
            List<Roommate> roommates = new List<Roommate>();
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT rm.Id, rm.FirstName, rmch.ChoreId
                                        FROM Roommate rm
                                        INNER JOIN RoommateChore rmch on rm.Id = rmch.RoommateId
                                        INNER JOIN Chore ch on ch.Id = rmch.ChoreId
                                        WHERE rmch.ChoreId = @choreId";
                    cmd.Parameters.AddWithValue("@choreId", choreId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        while (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName"))
                            };
                            roommates.Add(roommate);
                        }
                        return roommates;
                    }

                }
            }
        }
    }
}

