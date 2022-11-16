using System;
using System.IO;
using System.Linq;

namespace LibraryProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.ShowMenu();
        }
        public void ShowMenu()
        {
            //Outputs a guide for the user
            Console.WriteLine("Choose from the options below:");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Edit Book");
            Console.WriteLine("3. Search for Book");
            Console.WriteLine("4. Import Books");
            Console.WriteLine("Press any other key to exit\n");

            //Getting the input from the user
            string choice = Console.ReadLine();

            //Like a if statement where it is conditional depending on the input from the user
            switch (choice)
            {
                //Calls certain functions depending on the input
                case "1":
                    AddBook();
                    //Recursive function - calling itself
                    ShowMenu();
                    break;
                case "2":
                    EditBook();
                    ShowMenu();
                    break;
                case "3":
                    SearchBook();
                    ShowMenu();
                    break;
                case "4":
                    ImportBooks();
                    ShowMenu();
                    break;
                //Breaks out of the switch statement
                default:
                    break;
            }
        }
        public void AddBook()
        {
            //Getting input from the user
            Console.WriteLine("Import book name:");
            string book = Console.ReadLine();

            Console.WriteLine("Input author name:");
            string author = Console.ReadLine();

            //Declares the main text file for the system
            string path = "BookRecord.txt";
            //Checking if the file doesn't exist
            if (!File.Exists(path))
            {
                //If it doesn't exist then create the file using the path variable and add the text inputted by the user
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(book + "," + author);
                }
            }
            else
            {
                //Else it would just add the text inputted by the user to the existing file
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(book + "," + author);
                }
            }
        }
        public void EditBook()
        {
            //Declares the main file
            string path = "BookRecord.txt";
            StreamReader sr = new StreamReader(path);

            //The i variable below will be used to output a list of all the books starting at the number 1
            int i = 1;
            //Below reads the lines of the main file
            string line = sr.ReadLine();
            //Will perform if the line isn't empty
            while (line != null)
            {
                //Splits the details of the book (line) using a delimiter and stores inside an array
                string[] bookDetails = line.Split(",");
                //Then it would output the details of the book
                Console.WriteLine(i + ": " + bookDetails[0] + " by " + bookDetails[1]);
                //For each book outputted, increment the i variable for the next book
                i++;
                line = sr.ReadLine();
            }
            sr.Close();

            //Infinite loop until correct input is entered
            while (true)
            {
                //Getting input from the user
                Console.WriteLine("Which book would you like to edit?");
                string bookToEdit = Console.ReadLine();

                //Validation to make sure that the correct value is entered (data type and within index)
                try
                {
                    //Converts the input into an integer and takes 1 away to for the index
                    int bookToEditInt = Int32.Parse(bookToEdit) - 1;
                    //Reads all the lines of the file and put in a array to be used
                    string[] fileLineArray = File.ReadAllLines(path);

                    //Splits the details of the book (line) using a delimiter and stores inside an array
                    string[] book = fileLineArray[bookToEditInt].Split(",");
                    
                    //Getting input from the user, stored within an array
                    Console.WriteLine("Input new book name:");
                    book[0] = Console.ReadLine();

                    Console.WriteLine("Input new author name:");
                    book[1] = Console.ReadLine();

                    //Changes the book that the user wanted to change to their input
                    fileLineArray[bookToEditInt] = book[0] + "," + book[1];
                    //Overwrites the file with the changes made
                    File.WriteAllLines(path, fileLineArray);

                    //Breaks out of the infinite loop if it has been successful
                    break;
                }
                //Catches an error if the user incorrectly entered input
                catch (Exception e)
                {
                    //Outputs an error message to the user
                    Console.WriteLine(e.Message + " Please try again.");
                }
            }
        }
        public void SearchBook()
        {
            //Getting input from the user
            Console.WriteLine("Input book to find:");
            string book = Console.ReadLine();

            //Declares the main file
            string path = "BookRecord.txt";
            StreamReader sr = new StreamReader(path);

            //Below reads the lines of the main file
            string line = sr.ReadLine();

            //Will perform if the line isn't empty
            while (line != null)
            {
                //Splits the details of the book (line) using a delimiter and stores inside an array
                string[] bookDetails = line.Split(",");
                //Checks if the input matches the book name of each line
                if (bookDetails[0].ToLower() == book.ToLower())
                {
                    //Outputs a message saying that they found the book within the system
                    Console.WriteLine(book + " found");
                }
                //If they don't match then this happens
                else
                {
                    //Outputs a message saying that they found the book within the system
                    Console.WriteLine(book + " NOT found");
                }
                line = sr.ReadLine();
            }
            sr.Close();
        }
        public void ImportBooks()
        {
            //Declares the main file
            string path = "BookRecord.txt";
            //Gets all the text files which is stored in an array
            string[] fileEntries = Directory.GetFiles("./", "*.txt");

            //Checks if the array has the main text file in it
            bool mainFile = fileEntries.Contains("./" + path);

            //If it does have the main text file then
            if (mainFile)
            {
                //It would just create a new array without the main text file
                fileEntries = fileEntries.Except(new string[]{"./" + path}).ToArray();
            }

            //Outputs to the user the records/files in the folder
            Console.WriteLine("Records in default folder:");
            //Loops through all of the text files
            for (int i = 0; i < fileEntries.Length; i++)
            {
                //Then prints all of the text file
                Console.WriteLine(i + 1 + ". " + fileEntries[i][2..]);
            }

            //Infinite loop until the correct values are entered
            while (true)
            {
                //Outputs to tell the user to choose a file
                Console.WriteLine("Choose a file to import books to main file:");

                //Validation to make sure that the correct value is entered (data type and within index)
                try
                {
                    //Converts the input to an integer and taking away 1 for the index
                    int fileIndex = Int32.Parse(Console.ReadLine()) - 1;

                    //Finds the file that the user has inputted
                    string file = fileEntries[fileIndex];

                    StreamReader sr = new StreamReader(file);
                    //Reads all of the lines of the file
                    string line = sr.ReadLine();

                    //If the file isn't empty then do whatever is within this while loop
                    while (line != null)
                    {
                        //Splits the book details of each line through a delimiter
                        string[] bookDetails = line.Split(",");
                        //Outputs all of the content of the file
                        Console.WriteLine(bookDetails[0] + " by " + bookDetails[1]);

                        line = sr.ReadLine();
                    }
                    sr.Close();

                    //Reads all of the lines of the file
                    string[] fileLineArray = File.ReadAllLines(file);

                    //Checks if the main file doesn't exist
                    if (!File.Exists(path))
                    {
                        //If the main file doesn't exist then it'll create a new one with the name
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            //It'll tell the user that the main file didn't exust
                            Console.WriteLine(path + " doesn't exist!");
                            //Then it'll loop through the contents of the inputted file
                            foreach (string fileLine in fileLineArray)
                            {
                                //Then write it to the main file
                                sw.WriteLine(fileLine);
                            }
                        }
                    }
                    //Or if it does exist then
                    else
                    {
                        //It'll just add the content to the file
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            //Loops through the content of the inputted file
                            foreach (string fileLine in fileLineArray)
                            {
                                //Then writes the content to the main file
                                sw.WriteLine(fileLine);
                            }
                        }
                    }

                    //Breaks out of the loop if everything was successful
                    break;
                }
                //Catches an error if the user incorrectly entered input
                catch (Exception e)
                {
                    //Outputs an error message to the user
                    Console.WriteLine(e.Message + " Please try again.");
                }
            }
        }
    }
}
