using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Papper
{
    public class Game
    {
        private readonly List<string> _availableMoves;
        private readonly byte[] _randomBytes = new byte[32];
        private readonly int _average;

        private int _computerInput;
        private int _userInput;

        public Game(List<string> availableMoves)
        {
            _availableMoves = availableMoves;
            _average = availableMoves.Count / 2;
        }

        public void Run()
        {
            GenerateComputerMove();
            PrintMenu();
            GetUserInput();

            if (_userInput < 0)
            {
                Console.WriteLine("Goodbye!");
                return;
            }

            ShowResult(GetResult());
        }

        private void GenerateComputerMove()
        {
            _computerInput = RandomNumberGenerator.GetInt32(_availableMoves.Count);
            RandomNumberGenerator.Fill(_randomBytes);
        }

        private static string GetHex(byte[] x)
        {
            return BitConverter.ToString(x).Replace("-", "");
        }

        private string GetHmac()
        {
            using var hmacHasher = new HMACSHA256(_randomBytes);
            return GetHex(hmacHasher.ComputeHash(Encoding.UTF8.GetBytes(_computerInput.ToString())));
        }

        private void PrintMenu()
        {
            Console.WriteLine($"HMAC: {GetHmac()}");
            Console.WriteLine("Available moves:");

            for (var i = 0; i < _availableMoves.Count; i++)
                Console.WriteLine($"{i + 1} - {_availableMoves[i]}");

            Console.WriteLine("0 - Exit");
            Console.Write("Enter your move: ");
        }

        private void GetUserInput()
        {
            while (true)
            {
                try
                {
                    var input = int.Parse(Console.ReadLine() ?? "");
                    if (input > _availableMoves.Count)
                        throw new Exception();
                    _userInput = input - 1;
                    return;
                }
                catch
                {
                    Console.WriteLine("Bad input. Try again...");
                    PrintMenu();
                }
            }
        }

        private GameResult GetResult()
        {
            var r = _userInput - _average;
            var computerNormalize = (_computerInput - r) % _availableMoves.Count;
            if (_average > computerNormalize)
                return GameResult.Win;
            if (_average < computerNormalize)
                return GameResult.Lose;
            return GameResult.Draw;
        }

        private void ShowResult(GameResult result)
        {
            Console.WriteLine($"Your move: {_availableMoves[_userInput]}");
            Console.WriteLine($"Computer move: {_availableMoves[_computerInput]}");
            Console.WriteLine(result switch
            {
                GameResult.Win => "You win!",
                GameResult.Lose => "You lose!",
                GameResult.Draw => "It's a draw!",
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
            });
            Console.WriteLine($"Key: {GetHex(_randomBytes)}");
        }

        private enum GameResult
        {
            Win,
            Lose,
            Draw,
        }
    }
}

