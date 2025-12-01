namespace BookStoreApp.Controllers.Helpers
{
    public static class InputHelper
    {
        public static int? SelectFromList<T>(List<T> items, Func<T, string> display, Func<T, int> getId)
        {
            for (int i = 0; i < items.Count; i++)
                Console.WriteLine($"{i + 1}. {display(items[i])}");

            Console.WriteLine("\n0. Back");

            int choice;

            while (true)
            {
                Console.Write("\nChoose an option: ");
                var input = Console.ReadLine();

                if (input == "0")
                {
                    return null;
                }
                if (int.TryParse(input, out choice) &&
                    choice >= 1 && choice <= items.Count)
                {
                    return getId(items[choice - 1]);
                }

                Console.Write("Invalid choice. Try again: ");
            }
        }

        public static bool Confirm(string message = "Are you sure? (y/n): ")
        {
            Console.Write(message);
            return Console.ReadLine()!.Trim().ToLower() == "y";
        }

    }
}
