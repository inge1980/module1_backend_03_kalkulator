namespace module1_backend_03_kalkulator
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Velkommen til kalkulatorprogrammet!");

            while (true)
            {
                initMenu();
                HandleMenu();

                if (!activeMenu) {
                    break;
                }
            }
        }
        static bool activeMenu = true;
        private static void initMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Velg operasjon:");
            Console.WriteLine("1) Addisjon");
            Console.WriteLine("2) Subtraksjon");
            Console.WriteLine("3) Multiplikasjon");
            Console.WriteLine("4) Divisjon");
            Console.WriteLine("5) Avslutt");
        }
        private static void HandleMenu()
        {
            Console.Write("Ditt valg: ");
            var userChoice = Console.ReadLine()?.Trim();
            char operation;

            switch (userChoice)
            {
                case "1": operation = '+'; break;
                case "2": operation = '-'; break;
                case "3": operation = '*'; break;
                case "4": operation = '/'; break;
                case "5": Exit(); return;
                default:
                    Console.WriteLine("Ugyldig valg");
                    return;
            }

            AskForNumbersAndCompute(operation);
        }

        private static void AskForNumbersAndCompute(char operation)
        {
            Console.WriteLine($"Du valgte operasjon '{operation}'. Skriv inn tall separert med mellomrom:");
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ingen tall ble oppgitt.");
                return;
            }

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var values = new double[parts.Length];
            for (var i = 0; i < parts.Length; i++)
            {
                var token = parts[i].Trim().Replace(',', '.');
                if (!double.TryParse(token, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out values[i]))
                {
                    Console.WriteLine($"Kunne ikke tolke '{parts[i]}' som et tall.");
                    return;
                }
            }

            if (values.Length < 2)
            {
                Console.WriteLine("Du må skrive inn minst to tall.");
                return;
            }
            // Sjekk om alle tallene er int
            var allInts = true;
            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] % 1 != 0)
                {
                    allInts = false;
                    break;
                }
            }

            if (allInts)
            {
                var intValues = new int[values.Length];
                for (var i = 0; i < values.Length; i++)
                {
                    intValues[i] = (int)values[i];
                }

                var intResult = Calculate(operation, intValues);
                Console.WriteLine($"Resultat: {intResult}");
                return;
            }

            var result = Calculate(operation, values);
            Console.WriteLine($"Resultat: {result}");
        }
        // Overloads for double
        private static double Add(double[] values)
        {
            var sum = 0.0;
            foreach (var value in values)
            {
                sum += value;
            }
            return sum;
        }
        private static double Subtract(double[] values)
        {
            var result = values[0];
            for (var i = 1; i < values.Length; i++) result -= values[i];
            return result;
        }
        private static double Multiply(double[] values)
        {
            var result = 1.0;
            foreach (var value in values) result *= value;
            return result;
        }
        private static double Divide(double[] values)
        {
            var result = values[0];
            for (var i = 1; i < values.Length; i++)
            {
                if (values[i] == 0) throw new DivideByZeroException();
                result /= values[i];
            }
            return result;
        }
        // Overloads for int, with overflow checking
        private static int Add(int[] values)
        {
            var sum = 0;
            foreach (var value in values)
            {
                sum = checked(sum + value);
            }
            return sum;
        }

        private static int Subtract(int[] values)
        {
            var result = values[0];
            for (var i = 1; i < values.Length; i++) result = checked(result - values[i]);
            return result;
        }

        private static int Multiply(int[] values)
        {
            var result = 1;
            foreach (var value in values) result = checked(result * value);
            return result;
        }

        private static int Divide(int[] values)
        {
            var result = values[0];
            for (var i = 1; i < values.Length; i++)
            {
                if (values[i] == 0) throw new DivideByZeroException();
                result /= values[i];
            }
            return result;
        }
        // Overloads for double
        private static double Calculate(char operation, double[] values)
        {
            return operation switch
            {
                '+' => Add(values),
                '-' => Subtract(values),
                '*' => Multiply(values),
                '/' => Divide(values),
                _ => throw new ArgumentException($"Ukjent operasjon: {operation}", nameof(operation)),
            };
        }
        // Overload for int
        private static int Calculate(char operation, int[] values)
        {
            return operation switch
            {
                '+' => Add(values),
                '-' => Subtract(values),
                '*' => Multiply(values),
                '/' => Divide(values),
                _ => throw new ArgumentException($"Ukjent operasjon: {operation}", nameof(operation)),
            };
        }

        private static void Exit()
        {
            Console.WriteLine("Avslutter...");
            activeMenu = false;
        }
    }

}
