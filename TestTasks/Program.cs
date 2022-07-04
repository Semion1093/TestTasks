using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestTasks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                var text = File.ReadAllText("C:\\Users\\Семён\\source\\repos\\TestTasks\\Война и мир.txt")
                               .Split(new char[] { ' ', '.', ',', ';', ':', '?', '!', '(', ')' });

                var words1 = GetParallelThreaded(text);
                WriteText(words1, "C:\\Users\\Семён\\Desktop\\Результат параллельно.txt");

                var words2 = GetSingleThreaded(text);
                WriteText(words2, "C:\\Users\\Семён\\Desktop\\Результат однопоточно.txt");

                Console.WriteLine("==========================================================");
            }

            CreateInstanceOfAbstractClass(); 
            Console.WriteLine("==========================================================");

            // 3) Запросы к БД.
            // см. проект Queries

            FindTwoWeightsInHeap();

            Console.ReadKey();
        }

        /// <summary>
        /// 1) Поиск количества уникальных слов тексте однопоточно
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string GetSingleThreaded(string[] text)
        {
            var timer = Stopwatch.StartNew();
            Dictionary<string, int> dictionary = new Dictionary<string, int>(text.Length);

            foreach (var word in text)
            {
                if (word.Length <= 2)
                {
                    continue;
                }

                if (!dictionary.ContainsKey(word))
                {
                    dictionary.Add(word, 1);
                    continue;
                }

                dictionary[word] += 1;
            };

            timer.Stop();
            Console.WriteLine($"Время выполнения однопоточно: {timer.Elapsed.TotalMilliseconds} миллисекунд");

            return GetStringFromWordsDictionary(dictionary);
        }

        /// <summary>
        /// 1) Поиск количества уникальных слов тексте параллельно
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string GetParallelThreaded(string[] text)
        {
            var parallelTimer = Stopwatch.StartNew();
            ConcurrentDictionary<string, int> dictionary = new ConcurrentDictionary<string, int>(Environment.ProcessorCount, text.Length);

            var words = Parallel.ForEach(text, x =>
            {
                if (x.Length <= 2)
                {
                    return;
                }

                if (!dictionary.TryAdd(x, 1))
                {
                    dictionary[x] += 1;
                }
            });

            parallelTimer.Stop();
            Console.WriteLine($"Время выполнения параллельно: {parallelTimer.Elapsed.TotalMilliseconds} миллисекунд");

            return GetStringFromWordsDictionary(dictionary);
        }

        /// <summary>
        /// Конвертация словаря в строку
        /// </summary>
        /// <param name="wordsCounts"></param>
        /// <returns></returns>
        private static string GetStringFromWordsDictionary(IReadOnlyDictionary<string, int> wordsCounts)
        {
            var whiteSpace = " ";

            return wordsCounts.OrderByDescending(x => x.Value)
                    .Aggregate(new StringBuilder(wordsCounts.Count), (x, y) =>
                    {
                        x.Append(y.Key);
                        x.Append(whiteSpace);
                        x.Append(y.Value);
                        x.AppendLine();
                        return x;
                    })
                    .ToString();
        }

        /// <summary>
        /// Write text in file
        /// </summary>
        /// <param name="words"></param>
        /// <param name="path"></param>
        private static void WriteText(string words, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.Write(words);
            }
        }

        /// <summary>
        /// 2) Создайте экземпляр абстрактного класса с помощью рефлексии.
        /// </summary>
        private static void CreateInstanceOfAbstractClass()
        {
            var cat = (Cat)typeof(RuntimeTypeHandle).GetMethod("Allocate", BindingFlags.Static | BindingFlags.NonPublic)
                .Invoke(null, new object[] { typeof(Cat) });

            typeof(Cat).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null,
                new[] { typeof(int), typeof(string), typeof(int) }, null).Invoke(cat, new object[] { 4, "Барсик", 1 });

            Console.WriteLine($"Имя: {cat.Name}, Возраст: {cat.Age}, Количество хвостов: {cat.TailCount}");
        }

        /// <summary>
        /// 4) Поиск двух блинов, дающих в сумме 16 кг.
        /// </summary>
        private static void FindTwoWeightsInHeap()
        {
            Random random = new Random();
            var sum = 16;

            var weights = new int[20].Select(x => random.Next(1, 15)).ToList();
            var number = weights.FirstOrDefault(x => weights.Contains(sum - x));

            var result = number + (sum - number);

            if (result == 16)
            {
                Console.WriteLine($"Пара блинов найдена");
            }
            else
            {
                Console.WriteLine($"Пара блинов не найдена");
            }
        }
    }
}

