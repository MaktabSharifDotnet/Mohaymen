using Mohaymen.DataAccess;
using Mohaymen.DTOs;
using Mohaymen.Enums;
using Mohaymen.Exceptions;
using Mohaymen.repositories;
using Mohaymen.Services;
using System.IO;

AppDbContext appDbContext = new AppDbContext();
UserRepository userRepository = new UserRepository(appDbContext);
Service service = new Service(userRepository);

while (true)
{
    Console.WriteLine("please enter command:");
    string command = Console.ReadLine();
    string[] commandParts = command.Split(' ');
    try
    {
        InstructionEnum instructionEnum = (InstructionEnum)Enum.Parse(typeof(InstructionEnum), commandParts[0]);
        switch (instructionEnum)
        {
            case InstructionEnum.Register:

                AuthenticationDto authentication = ParseAuthCommand(commandParts);
                if (authentication.Username == null || authentication.Password == null)
                {
                    Console.WriteLine("The username or password is invalid or empty.");
                }
                else
                {
                    try 
                    {
                        service.Register(authentication.Username, authentication.Password);
                        Console.WriteLine("register is done");
                    }
                    catch (UserAlreadyExistException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                break;
            case InstructionEnum.Login:

                 authentication = ParseAuthCommand(commandParts);
                if (authentication.Username == null || authentication.Password == null)
                {
                    Console.WriteLine("The username or password is invalid or empty.");
                }
                else
                {
                    try
                    {
                        service.Login(authentication.Username, authentication.Password);
                        Console.WriteLine("Login is done");
                    }
                    catch (UserNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

                break;
        }
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid command");
    }
   

    AuthenticationDto ParseAuthCommand(string[] instructionParts) 
    {
        string username = null;
        string password = null;
        for (int i = 0; i < commandParts.Length; i++)
        {
            if (commandParts[i] == "--username" && i + 1 < commandParts.Length && !commandParts[i + 1].StartsWith("--"))
            {
                username = commandParts[i + 1];
            }
            if (commandParts[i] == "--password" && i + 1 < commandParts.Length && !commandParts[i + 1].StartsWith("--"))
            {
                password = commandParts[i + 1];
            }
        }
        AuthenticationDto authenticationDto = new AuthenticationDto 
        {
            Username = username,
            Password= password
        };
        return authenticationDto;
    }

}