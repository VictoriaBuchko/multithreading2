using System.Text;

namespace multithreading2
{
    internal class Program
    {
        class Horse
        {
            public int Number { get; set; }
            public int Time { get; set; }
            public int Track { get; set; }
        }

        static async Task Main(string[] args)
        {
            int numberOfHorses = 5;
            int finishLine = 50;
            Random random = new Random();

            Horse[] horses = new Horse[numberOfHorses];
            Task[] tasks = new Task[numberOfHorses];

            for (int i = 0; i < numberOfHorses; i++)
            {
                int horseNumber = i + 1;
                horses[i] = new Horse { Number = horseNumber, Track = i + 1 };
            }

            Console.WriteLine("Натисніть Enter для старту гонки...");
            Console.ReadLine();

            //запуск асинхронних задач для кожного коня
            for (int i = 0; i < numberOfHorses; i++)
            {
                int horseIndex = i;
                tasks[i] = RunRaceAsync(horses[horseIndex], finishLine, random);
            }

            //ожидаем завершения всех задач
            await Task.WhenAll(tasks);

            Console.WriteLine("\n\nРезультати перегонів:");
            foreach (var horse in horses.OrderBy(h => h.Time))
            {
                Console.WriteLine($"Конь {horse.Number} завершив за {horse.Time} секунд");
            }
        }

        static async Task RunRaceAsync(Horse horse, int finishLine, Random random)
        {
            int distanceCovered = 0;
            int timeTaken = 0;
            StringBuilder str = new StringBuilder();

            while (distanceCovered < finishLine)
            {
                int speed = random.Next(1, 10);
                distanceCovered += speed;

                if (distanceCovered > finishLine)
                {
                    distanceCovered = finishLine;
                }

                await Task.Delay(100); 
                timeTaken++;

                str.Clear();
                str.Append($"Доріжка {horse.Track}: Конь {horse.Number}: [");
                str.Append('#', distanceCovered);
                str.Append(']');
                str.Append($" {distanceCovered}/{finishLine}");

                lock (Console.Out) //используеться для коректного выводв в консоль
                {
                    Console.SetCursorPosition(0, horse.Track);
                    Console.Write(str.ToString());
                }
            }

            horse.Time = timeTaken;
        }
    }
}
