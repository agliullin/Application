using System;
using System.Collections.Generic;
using System.IO;

namespace neural.app
{
    // Класс, который хранит в себе нейроны.
    public class NeuralNetwork
    {
        public const int ArrayWidth = 20; // количество по горизонтали
        public const int ArrayHeight = 20; // количество по вертикали
        private const string Memory = @"memory.txt"; // Путь файла хранения сети.
        private List<Neuron> Neurons = null; // Типизированный список нейронов.

        // Конструктор.
        public NeuralNetwork()
        {
            Neurons = CreationNetwork();
        }

        // Метод, который открывает файл хранения сети и данные из этого файла преобразовывает в типизированный список нейронов
        private static List<Neuron> CreationNetwork()
        {
            if (!File.Exists(Memory)) // существует ли файл "Memory"
                return new List<Neuron>();
            string[] lines = File.ReadAllLines(Memory);
            if (lines.Length == 0)
                return new List<Neuron>();
            List<Neuron> res = new List<Neuron>();
            foreach (string line in lines) res.Add(CreationNeuron(line));
            return res;
        }

        // Метод, который преобразует полученную строку из памяти в класс нейрона.
        private static Neuron CreationNeuron(string line)
        {
            Neuron neuron = new Neuron();
            string []input = line.Split('#'); // Получаем строковый массив данных о нейроне, разделив полученную строку по знаку '#'.
            neuron.Name = input[0]; // Нейрону присвоим имя.
            neuron.NumberOfTrainings = Convert.ToInt32(input[1]); // Нейрону присвоим количество обучений.
            string[] weight = input[2].Split(';'); // Получаем строковый массив весов нейрона, разделив по знаку ';'.
            int arrSize = (int)Math.Sqrt(weight.Length); // Получаем размерность массива, т.к. массив квадратный используем Math.Sqrt().
            neuron.Weight = new double[arrSize, arrSize]; // Создаем массив весов для данного нейрона с определенной размерностью.
            int index = 0; // Индекс для того, чтобы пройти по всем полученным данным из памяти.
            for (int i = 0; i < arrSize; i++)
                for (int j = 0; j < arrSize; j++)
                {
                    neuron.Weight[i, j] = Double.Parse(weight[index]); // Преобразует строчное значение массива в double
                    index++;
                }
            return neuron;
        }

        // Метод, который распознаёт полученный образ.
        // Функция сравнивает входной массив с каждым нейроном из сети и возвращает имя нейрона наиболее похожего на него.
        public string Recognition(int[,] arr)
        {
            string Result = null;
            double Value = 0;
            foreach (var Neuron in Neurons) // Проходимся по всем нейронам.
            {
                double Deviation = Neuron.GetResult(arr);
                if (Deviation > Value)
                {
                    Value = Deviation;
                    Result = Neuron.GetName();
                }
            }
            return Result;
        }
        

        // Метод, который возвращает массив имен всех имеющихся образов из памяти
        public string[] GetNamesOfNeurons()
        {
            var res = new List<string>();
            for (int i = 0; i < Neurons.Count; i++)
                res.Add(Neurons[i].GetName());
            res.Sort(); // Сортируем полученные имена
            return res.ToArray();
        }

        // Метод, который заносит новый образ в память.
        public void SetTraining(string ReceivedName, int[,] ReceivedArray)
        {
            Neuron Neuron = Neurons.Find(v => v.Name.Equals(ReceivedName)); // В типизированном списке ищем данный нейрон по полученному имени.
                                                                            // v => v.Name.Equals(ReceivedName) - предикат.
            if (Neuron == null) // Проверка наличия нейрона с таким именем. Если не существует, создадим новыи и добавим в память.
            {                   
                Neuron = new Neuron();
                Neuron.Clear(ReceivedName, ArrayWidth, ArrayHeight);
                Neurons.Add(Neuron);
            }
            Neuron.Training(ReceivedArray); // Обучим нейрон новому образу.
        }
        // Метод, который сохраняет данные в память.
        public void Save()
        {
            string text = "";
            foreach (var Neuron in Neurons) {
                text += Neuron.Name + "#";
                text += Neuron.NumberOfTrainings.ToString() + "#";
                for (int i = 0; i < ArrayHeight; i++)
                {
                    for (int j = 0; j < ArrayWidth; j++)
                    {
                        text += Neuron.Weight[i, j].ToString() + ";";
                    }
                }
                text += "\n";
            }
            System.IO.File.WriteAllText(Memory, text);
        }
    }
}
