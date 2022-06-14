using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        List<Room> allRooms = roomRepo.GetAll();
                        foreach (Room r in allRooms)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to remove? ");
                        int chosenRoomId = int.Parse(Console.ReadLine());
                        Room chosenRoom = allRooms.FirstOrDefault(r => r.Id == chosenRoomId);
                        roomRepo.Delete(chosenRoom.Id);

                        Console.WriteLine("Room has been successfully removed");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetRoommateById(roommateId);

                        Console.WriteLine($"{roommate.Id} - {roommate.FirstName} {roommate.RentPortion} - {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Shows all chores"):
                        List<Chore> chores = choreRepo.GetAllChores();
                        foreach (Chore r in chores)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetChoreById(choreId);

                        Console.WriteLine($"{chore.Id} - {chore.Name})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAllChores();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name})");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(c => c.Id == selectedChoreId);

                        Console.Write("New Name: ");
                        selectedChore.Name = Console.ReadLine(); 

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a chore"):
                        List<Chore> theChores = choreRepo.GetAllChores();
                        foreach (Chore c in theChores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to remove? ");
                        int chosenChoreId = int.Parse(Console.ReadLine());
                        Chore chosenChore = theChores.FirstOrDefault(r => r.Id == chosenChoreId);
                        roomRepo.Delete(chosenChore.Id);

                        Console.WriteLine("Chore has been successfully removed");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        Console.WriteLine("Unassigned chore:");
                        foreach (Chore r in unassignedChores)
                        {
                            Console.WriteLine($"{r.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):
                        List<Chore> allChores = choreRepo.GetAllChores();
                        foreach (Chore r in allChores)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id}");
                        }
                        Console.Write("Select a chore - Enter chore Id: ");
                        int inputChoreId = Int32.Parse(Console.ReadLine());

                        List<Roommate> roommates = roommateRepo.GetAll();
                        foreach (Roommate r in roommates)
                        {
                            Console.WriteLine($"{r.FirstName} has an Id of {r.Id} ");
                        }
                        Console.Write("Select a roomate - Enter roomate Id: ");
                        int inputRoomateId = Int32.Parse(Console.ReadLine());
  
                        choreRepo.AssignChore(inputRoomateId, inputChoreId);
                        Console.WriteLine("Successfully assigned a chore to the roomate");

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Re-assign a chore"):
                        List<Chore> assignedChores = choreRepo.GetAssignedChores();

                        Console.WriteLine();
                        Console.WriteLine("Current assigned Chores:");

                        foreach(Chore ch in assignedChores)
                        {
                            Console.WriteLine($"{ch.Name} has id of {ch.Id}");
                        }

                        Console.Write("Enter the id of the chore to re-assign: ");
                        int pickedChoreId = int.Parse(Console.ReadLine());

                        List<Roommate> rmtsDoingThisChore = roommateRepo.GetRoommatesByChoreId(pickedChoreId);
                        Console.WriteLine();
                        Console.WriteLine($"This chore with id {pickedChoreId} is assigned to following roommates:");
                        foreach(Roommate r in rmtsDoingThisChore)
                        {
                            Console.WriteLine(r.FirstName);
                        }

                        Console.WriteLine("Here is the list of roommates with thier ids:");
                        List<Roommate> allRoomies = roommateRepo.GetAll();
                        foreach(Roommate r in allRoomies)
                        {
                            Console.WriteLine($"{r.FirstName} has id of {r.Id}");
                        }
                        Console.Write("Select roommate's id to who would you like to assign this chore? > ");
                        int toRoommateId = Int32.Parse(Console.ReadLine());

                        Console.WriteLine();
                        Console.Write("Enter roommate's id from whom you want to re-assign this chore? > ");
                        int fromRoommateId = Int32.Parse(Console.ReadLine());

                        choreRepo.AssignChore(toRoommateId, pickedChoreId);
                        choreRepo.DeleteChoreForRmt(pickedChoreId, fromRoommateId);
                        Console.WriteLine("Successfully assigned this chore to the roommate");

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show chore report"):
                        List<RoommateChores> choresOfRoommates = choreRepo.GetChoreCounts();
                        foreach(RoommateChores r in choresOfRoommates)
                        {
                            Console.WriteLine($"{r.RoommateName} has been assigned {r.ChoreCount} chores");
                        }

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Search for roommate",
                "Shows all chores",
                "Search for chore", 
                "Add a chore", 
                "Update a chore",
                "Delete a chore",
                "Show unassigned chores",
                "Assign chore to roommate",
                "Re-assign a chore",
                "Show chore report",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}