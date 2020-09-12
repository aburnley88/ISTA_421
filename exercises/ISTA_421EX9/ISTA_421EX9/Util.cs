using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ISTA_421EX9
{
    class Util : IPrintUI
    {
        Dictionary<string, string> MyDictionary = new Dictionary<string, string>();
        void IPrintUI.IPrintUI()
        {
            bool temp = true;
            do
            {
                Console.WriteLine("---------------------------------------------------------------------------------------------------------\n");
                Console.WriteLine("********************PASSSWORD AUTHENTICATION SYSTEM*******************************************************\n\n");
                Console.WriteLine("                        Please Select One Option:\n\n\n");
                Console.WriteLine("                        1.Establish Account\n");
                Console.WriteLine("                        2.Authenticate Users\n");
                Console.WriteLine("                        3.System Exit\n\n\n");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------\n");

                string UserResponse = Console.ReadLine();
                if (String.IsNullOrEmpty(UserResponse) || UserResponse != "1" && UserResponse != "2" && UserResponse != "3")
                {
                    Console.WriteLine("Please enter a valid selection.");
                }
                else
                   SwitchMethod(UserResponse, true);

            } while (temp);          
        }
      
        private string Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            System.Security.Cryptography.MD5CryptoServiceProvider md5provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
       
        private void Exit()
        {
            Console.Clear();
            Console.WriteLine("Exiting system and erasing data...");
            Thread.Sleep(500);

            Print(MyDictionary);

            Console.WriteLine("\nSYSTEM SHUTDOWN");
            System.Environment.Exit(1);
        }

        private void GetUser()
        {
            bool temp = true; 
            Console.Clear();
            Console.WriteLine("Please enter a user to authenticate.");
            string UserResponse = Console.ReadLine();

            do
            {
                if (MyDictionary.ContainsKey(UserResponse))
                {
                    string UserName = UserResponse;
                    Console.WriteLine($"{UserName} exists in system. Please enter a password to authenticate account.");
                    UserResponse = Console.ReadLine();
                    UserResponse = Hash(UserResponse);
                    if (MyDictionary.ContainsValue(UserResponse))
                    {
                        string Password;
                        string value;
                        bool PasswordExist = MyDictionary.TryGetValue(UserName, out value);

                        if (UserResponse == value)
                        {
                            Password = value;
                            Console.WriteLine($"{UserName} authenticated with {Password}");
                            Thread.Sleep(1500);

                            Transition();
                            UserResponse = Console.ReadLine();
                            SwitchMethod(UserResponse, true);
                        }
                       
                    }
                }
                else
                {
                    Console.WriteLine($"{UserResponse} is invalid.");
                    Thread.Sleep(500);
                    Transition();
                    UserResponse = Console.ReadLine();
                    SwitchMethod(UserResponse, true);

                }

            } while (temp);

        }

        private void PostNewUser()
        {
            bool temp = true; 
            string UserName = "";
            string Password = "";
            do
            {
                Console.WriteLine("Please enter a username to add to the system.");
                string UserResponse = Console.ReadLine();
                if (string.IsNullOrEmpty(UserResponse))
                {
                    Console.WriteLine("Username cannot be empty");
                    PostNewUser();
                }
                if (MyDictionary.ContainsKey(UserResponse))
                {
                    Console.WriteLine($"{UserResponse} already exists in system.");
                    PostNewUser();
                }
                else
                {
                    UserName = UserResponse;
                    temp = false;
                }                 

            } while (temp);
            bool stop = true;

            do
            {

                Console.WriteLine($"Please enter a password to attach to {UserName}.");
                string UserResponse = Console.ReadLine();
                if (string.IsNullOrEmpty(UserResponse))
                {
                    Console.WriteLine("Paswword can't be empty.");
                }
                else
                {
                    Password = Hash(UserResponse);
                    stop = false;
                }
                   

            } while (stop);

            MyDictionary.Add(UserName, Password);
            Console.WriteLine($"\nAccount created {UserName} {Password}\n");
            Thread.Sleep(1500);
            
            bool temp2 = true;

            do
            {
                Transition();
                string UserResponse = Console.ReadLine();
                if (UserResponse != "1" && UserResponse != "2" && UserResponse != "3")
                {
                    Console.WriteLine("Please enter a valid selection.");
                }
                else
                    SwitchMethod(UserResponse, true);

            } while (temp2);
        }

        private void Print(Dictionary<string, string> MyDictionary)
        {
            foreach (var account in MyDictionary)
            {
                Console.WriteLine($"{account.Key}\t{account.Value}");
            }
        }
        private void Transition()
        {
            Console.Clear();
            Console.WriteLine("Please enter 1 to add an account.");
            Console.WriteLine("Please enter 2 to authenticate a  user.");
            Console.WriteLine("Please enter 3 to exit system.");
           
        }

        private void SwitchMethod(string UserResponse, bool temp)
        {
            switch (UserResponse)
            {
                case "1":
                    temp = false;
                    PostNewUser();
                    break;

                case "2":
                    temp = false;
                    GetUser();
                    break;

                case "3":
                    temp = false;
                    Exit();
                    break;
            }
        }
    }
}
