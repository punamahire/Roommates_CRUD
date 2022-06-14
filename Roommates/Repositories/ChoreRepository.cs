using System;
using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        // pass the connection string to the base class constructor
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public List<Chore> GetAllChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();

                        while(reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };
                           
                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }

        public Chore GetChoreById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore where Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;

                        if(reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),   
                            };
                        }
                        return chore;
                    }
                }
            }
        }

        public List<Chore> GetChoresByRoommateId(int roommateId)
        {
            List<Chore> chores = new List<Chore>();
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT ch.Id, ch.Name FROM Chore ch
                                            INNER JOIN RoommateChore rmch on ch.Id = rmch.ChoreId
                                            where rmch.RoommateId = @roommateId";
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chore chore = new Chore
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }
        public void Insert (Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                            OUTPUT INSERTED.Id
                                            VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;

                }
            }
        }

        public List<Chore> GetUnassignedChores()
        {
            List<Chore> chores = new List<Chore>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Name, c.Id 
                                            FROM Chore c
                                            LEFT JOIN RoommateChore rc on c.Id = rc.ChoreId
                                            LEFT JOIN Roommate r on r.Id = rc.RoommateId
                                            WHERE r.Id is null";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {   
                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }

        public List<Chore> GetAssignedChores()
        {
            List<Chore> chores = new List<Chore>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT DISTINCT c.Name, c.Id 
                                            FROM Chore c
                                            INNER JOIN RoommateChore rc on c.Id = rc.ChoreId
                                            WHERE c.Id = rc.ChoreId";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (RoommateId, ChoreId) 
                                            VALUES (@roommateId, @choreId)";
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<RoommateChores> GetChoreCounts()
        {
            List<RoommateChores> roommateChores = new List<RoommateChores>(); 
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT r.FirstName, COUNT(rc.ChoreId) as Count
                                            FROM Roommate r
                                            LEFT JOIN RoommateChore rc on r.Id = rc.RoommateId
                                            GROUP BY r.FirstName";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            int nameColumnPosition = reader.GetOrdinal("FirstName");
                            string nameValue = reader.GetString(nameColumnPosition);

                            int countColumnPosition = reader.GetOrdinal("Count");
                            int choreCountValue = reader.GetInt32(countColumnPosition);

                            RoommateChores roomieChore = new RoommateChores
                            {
                                RoommateName = nameValue,
                                ChoreCount = choreCountValue
                            };

                            roommateChores.Add(roomieChore);
                        }
                        return roommateChores;
                    }
                }
            }
        }

        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                    SET Name = @name,     
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // What happens when a user tries to delete a Chore that's been assigned to someone?
                    // Why does this happen? What can/should be done about this?

                    cmd.CommandText = "UPDATE RoommateChore SET RoommateId = NULL and ChoreId = NULL where ChoreId = @chore_id";
                    cmd.Parameters.AddWithValue("@chore_id", id);
                    cmd.ExecuteNonQuery();

                    // the below query WON'T WORK because there is a constraint added in RoommateChore table.
                    // So, first we have to run an alter query on RoommateChore table to drop that constraint 
                    // and then run the delete query below.
                    cmd.CommandText = "DELETE FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    // When a row corresponsing to ChoreId or RoommateId is deleted from either tables Chore
                    // or Roommate then it doesn't make sense to keep a record for it in the join table
                    // RoommateChore. In such a case, remove the row from RoommateChore using cascade delete.
                    // For that we need to run query in a separate query window to alter the RoommateChore table.
                    // First, drop the existing constraint and then add a new constraint - on delete cascade.
                }
            }
        }

        public void DeleteChoreForRmt(int choreId, int roommateId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // What happens when a user tries to delete a Chore that's been assigned to someone?
                    // Why does this happen? What can/should be done about this?
                    cmd.CommandText = @"DELETE FROM RoommateChore 
                                        WHERE ChoreId = @choreId and RoommateId = @roommateId";
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
