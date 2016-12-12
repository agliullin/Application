using System;

namespace neural.app
{
    // Класс нейрона.
    // Данный класс хранит в себе имя, вес и количество обучений нейрона образу.
    class Neuron
    {
        public string Name; // Имя образа, который хранит нейрон.
        public double[,] Weight; // Массив весов, который хранит в себе память нейрона.
        public int NumberOfTrainings; // Количество обучений определенному образу, т.е. количество вариантов образа в памяти.

        // Конструктор.
        public Neuron() { }

        // Метод, который возвращает имя нейрона.
        public string GetName() {
            return Name;
        }

        // Метод, который очищает память нейрона и присваивает ему новое имя.
        public void Clear(string name, int x, int y)
        {
            this.Name = name;
            Weight = new double[x, y];
            for (int i = 0; i < Weight.GetLength(0); i++)
                for (int j = 0; j < Weight.GetLength(1); j++)
                    Weight[i, j] = 0; // Обнуляем каждый элемент массива.
            NumberOfTrainings = 0; // Обнуляем количество обучений.
        }

        // Метод, который возвращает сумму величин отклонения входного массива от эталонного.
        // Чем ближе результат к единице, тем больше вероятность сходства полученного образа и эталонного. 
        public double GetResult(int[,] ReceivedArray) // ReceivedArray - полученный массив.
        {
            // Проверям размерности массивов. Если размер полученного массива отличается от массива весов, то выходим.
            if (Weight.GetLength(0) != ReceivedArray.GetLength(0) || Weight.GetLength(1) != ReceivedArray.GetLength(1))
                return -1;
            double Deviation = 0;
            for (int i = 0; i < Weight.GetLength(0); i++)
                for (int j = 0; j < Weight.GetLength(1); j++)
                    Deviation += 1 - Math.Abs(Weight[i, j] - ReceivedArray[i, j]); // Считаем отклонения каждого элемента входного массива от усреднённого значения из памяти.
            double AverageDeviation = Deviation / (Weight.GetLength(0) * Weight.GetLength(1));// Считаем среднее арифметическое отклонение по массиву
            return AverageDeviation; 
        }

        // Метод, который добавляет входной образ в память.
        public void Training(int[,] ReceivedArray) // ReceivedArray - полученный массив.
        {
            // Проверка существования полученного массива и совпадения размерности с массивом из памяти.
            if (ReceivedArray != null || Weight.GetLength(0) == ReceivedArray.GetLength(0) || Weight.GetLength(1) == ReceivedArray.GetLength(1))
            {
                NumberOfTrainings++;
                for (int i = 0; i < Weight.GetLength(0); i++)
                {
                    for (int j = 0; j < Weight.GetLength(1); j++)
                    {
                        // Полученный массив должен состоять только из 0 и 1.
                        double temp = ReceivedArray[i, j] == 0 ? 0 : 1;
                        // Каждый элемент в памяти пересчитывается с учетом значения из полученного массива - ReceivedArray
                        Weight[i, j] += 2 * (temp - 0.5f) / NumberOfTrainings; // ****************************
                        if (Weight[i, j] > 1) Weight[i, j] = 1; // Если значение памяти больше 1, то присваиваем 1.
                        if (Weight[i, j] < 0) Weight[i, j] = 0; // Если значение памяти меньше 0, то присваиваем 0.
                    }
                }
            }
        }
    }
}
