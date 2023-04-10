using System.Windows.Input;
using Banks.Console.Commands.Add;
using Banks.Console.Commands.Cancel;
using Banks.Console.Commands.Register;
using Banks.Console.Commands.RewindTime;
using Banks.Console.Commands.Show;
using Banks.Console.Commands.Transaction;
using Banks.Console.Commands.Update;
using Banks.Console.Interfaces;
using Banks.Exceptions;

namespace Banks.Console.CLI;

public class CLI
{
    private const string _commands = "  -register bank \n" +
                                     "  -register client \n" +
                                     "  -add bank account \n" +
                                     "  -show bank accounts \n" +
                                     "  -show account balance \n" +
                                     "  -make transaction \n" +
                                     "  -cancel transaction \n" +
                                     "  -update bank configuration \n" +
                                     "  -rewind time \n" +
                                     "\n" +
                                     "  enter -break to stop\n";

    private const string _options = "   -h, --help " +
                                    "   Prints help information\n";

    private readonly bool _flag = true;

    public CLI()
    {
        ShowCommandsAndOptions();
        while (_flag)
        {
            IBankCommand bankCommand;
            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
            switch (System.Console.ReadLine())
            {
                case "-register bank":
                    try
                    {
                        System.Console.ResetColor();
                        bankCommand = new RegisterBankBankCommand();
                        bankCommand.Execute();
                    }
                    catch (BanksException e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }

                    break;
                case "-register client":
                    try
                    {
                        System.Console.ResetColor();
                        bankCommand = new RegisterClientBankCommand();
                        bankCommand.Execute();
                    }
                    catch (BanksException e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }

                    break;
                case "-add bank account":
                    try
                    {
                        System.Console.ResetColor();
                        bankCommand = new AddBankAccountBankCommand();
                        bankCommand.Execute();
                    }
                    catch (BanksException e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }

                    break;
                case "-show bank accounts":
                    try
                    {
                        System.Console.ResetColor();
                        bankCommand = new ShowAccountsBankCommand();
                        bankCommand.Execute();
                    }
                    catch (BanksException e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }

                    break;
                case "-show account balance":
                    try
                    {
                        System.Console.ResetColor();
                        bankCommand = new ShowAccountBalanceBankCommand();
                        bankCommand.Execute();
                    }
                    catch (BanksException e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }

                    break;
                case "-make transaction":
                    try
                    {
                        System.Console.ResetColor();
                        bankCommand = new TransactionBankCommand();
                        bankCommand.Execute();
                    }
                    catch (BanksException e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }

                    break;
                case "-cancel transaction":
                    try
                    {
                        System.Console.ResetColor();
                        bankCommand = new CancelTransactionBankCommand();
                        bankCommand.Execute();
                    }
                    catch (BanksException e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }

                    break;
                case "-update bank configuration":
                    try
                    {
                        System.Console.ResetColor();
                        bankCommand = new UpdateBankConfigurationBankCommand();
                        bankCommand.Execute();
                    }
                    catch (BanksException e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }

                    break;
                case "-rewind time":
                    try
                    {
                        System.Console.ResetColor();
                        bankCommand = new RewindTimeBankCommand();
                        bankCommand.Execute();
                    }
                    catch (BanksException e)
                    {
                        System.Console.WriteLine(e);
                        throw;
                    }

                    break;
                case "-h":
                    ShowCommandsAndOptions();
                    break;
                case "--help":
                    ShowCommandsAndOptions();
                    break;
                case "-break":
                    _flag = false;
                    break;
            }
        }
    }

    private static void ShowCommandsAndOptions()
    {
        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
        System.Console.WriteLine("COMMANDS: ");
        System.Console.ResetColor();
        System.Console.WriteLine(_commands);

        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
        System.Console.WriteLine("OPTIONS: ");
        System.Console.ResetColor();
        System.Console.WriteLine(_options);
    }
}