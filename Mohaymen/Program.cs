using Mohaymen.DataAccess;
using Mohaymen.DTOs;
using Mohaymen.Enums;
using Mohaymen.Exceptions;
using Mohaymen.repositories;
using Mohaymen.Services;

AppDbContext context = new AppDbContext();
UserRepository userRepository = new UserRepository(context);
Service service = new Service(userRepository);

while (true)
{
    Console.WriteLine("please enter command");

    string command = Console.ReadLine();
    string[] instructionParts = command.Split(' ');
    try
    {
        Enum instruction = (Enum)Enum.Parse(typeof(InstructionEnum), instructionParts[0]);
        switch (instruction)
        {
            case InstructionEnum.Register:

                AuthenticationDto registerCommandDto = ParseAuthCommand(instructionParts);
                if (registerCommandDto.Username == null || registerCommandDto.Password == null)
                {
                    Console.WriteLine("invalid command");
                }
                else
                {
                    service.Register(registerCommandDto.Username, registerCommandDto.Password);
                    Console.WriteLine("register is done");
                }

                break;
            case InstructionEnum.Login:
                registerCommandDto = ParseAuthCommand(instructionParts);
                if (registerCommandDto.Username == null || registerCommandDto.Password == null)
                {
                    Console.WriteLine("invalid command");
                }
                else
                {
                    service.Login(registerCommandDto.Username, registerCommandDto.Password);
                    Console.WriteLine("Login is done");
                }

                break;
        }
    }
    catch (ArgumentException)
    {
        Console.WriteLine("The input is invalid.  ");
    }
    catch (UserAlreadyExistException e)
    {
        Console.WriteLine(e.Message);
    }
    catch (UserNotFoundException e)
    {
        Console.WriteLine(e.Message);
    }

}
AuthenticationDto ParseAuthCommand(string[] instructionParts) 
{
    string username = null;
    string password = null;
    for (int i = 0; i < instructionParts.Length; i++)
    {
        if (instructionParts[i] == "--username")
        {

            if (i + 1 < instructionParts.Length && !instructionParts[i + 1].StartsWith("--"))
            {
                username = instructionParts[i + 1];
            }
        }
        else if (instructionParts[i] == "--password")
        {

            if (i + 1 < instructionParts.Length && !instructionParts[i + 1].StartsWith("--"))
            {
                password = instructionParts[i + 1];
            }
        }
    }
    AuthenticationDto registerCommandDto = new AuthenticationDto 
    {
      Username = username,
      Password = password
    };
    return registerCommandDto;
}
