using System.Drawing;

namespace neural.app
{
    // Класс, в котором хранятся все методы для преобразования изображения.
    class ImageTools
    {
        // Метод, который преобразует массив в изображение.
        // Если элемент массива равен 0, то он пикселю в данном изображении присваивает белый цвет, иначе черный.
        public static Bitmap GetBitmapFromArr(int[,] array)
        {
            Bitmap image = new Bitmap(array.GetLength(0), array.GetLength(1)); // создание точечного рисунка с размерностью двумерного массива
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    if (array[i, j] == 0)
                        image.SetPixel(i, j, Color.White); // задаем пикселю белый цвет
                    else
                        image.SetPixel(i, j, Color.Black);
            return image; 
        }
        // Метод, который преобразует изображение в массив.
        // Белый цвет заносится как 0, остальные как 1.
        public static int[,] GetArrayFromBitmap(Bitmap image)
        {
            int[,] array = new int[image.Width, image.Height]; // создание массива с размерностью полученного изображения
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    // color получает среднее значение среди полученных из трех компонент RGB.
                    int color = (image.GetPixel(i, j).R + image.GetPixel(i, j).G + image.GetPixel(i, j).B) / 3;
                    array[i, j] = color > 0 ? 1 : 0; // если это значение больше 0, то значению array[n,m] присваивается 1, иначе 0.
                }
            return array;
        }
        // Метод, который обрезает изображение и преобразует его в массив.
        public static int[,] CutBitmapAndGetArray(Bitmap image, Point point)
        {
            int x1 = 0, y1 = 0, x2 = point.X, y2 = point.Y; // x1 и y1 - координаты левого верхнего угла.
                                                            // x2 и y2 - координаты правого нижнего угла.
            for (int x = 0; x < image.Width && x1 == 0; x++) // x1 == 0 - условие необходимое для того, чтобы найти только первую точку.
                for (int y = 0; y < image.Height && x1 == 0; y++) // x1 == 0 - условие необходимое для того, чтобы найти только первую точку.
                    if (image.GetPixel(x, y).R < 50) x1 = x; // Движемся из левого верхнего угла пока не найдем первую не белую точку.
                                                                    // Полученная точка x1;
            for (int y = 0; y < image.Height && y1 == 0; y++) // y1 == 0 - условие необходимое для того, чтобы найти только первую точку.
                for (int x = 0; x < image.Width && y1 == 0; x++) // y1 == 0 - условие необходимое для того, чтобы найти только первую точку.
                    if (image.GetPixel(x, y).R < 50) y1 = y; // Движемся из левого верхнего угла пока не найдем первую не белую точку.
                                                                    // Полученная точка y1;
            for (int x = image.Width - 1; x >= 0 && x2 == point.X; x--) // x2 == point.X - условие необходимое для того, чтобы найти только первую точку.
                for (int y = 0; y < image.Height && x2 == point.X; y++) // x2 == point.X - условие необходимое для того, чтобы найти только первую точку.
                    if (image.GetPixel(x, y).R < 50) x2 = x; // Движемся из правого верхнего угла пока не найдем первую не белую точку.
                                                                    // Полученная точка x2;
            for (int y = image.Height - 1; y >= 0 && y2 == point.Y; y--) // y2 == point.Y - условие необходимое для того, чтобы найти только первую точку.
                for (int x = 0; x < image.Width && y2 == point.Y; x++) // y2 == point.Y - условие необходимое для того, чтобы найти только первую точку.
                    if (image.GetPixel(x, y).R < 50) y2 = y; // Движемся из левого нижнего угла пока не найдем первую не белую точку.
                                                                    // Полученная точка y2;

            if (x1 == 0 && y1 == 0 && x2 == point.X && y2 == point.Y) // Если мы в итоге не изменили размер ни в одной точке, то оставляем все без изменений
                return null;

            int size = x2 - x1 > y2 - y1 ? x2 - x1 + 1 : y2 - y1 + 1; // Определяем в какой оси наш рисунок больше.
            int dX = y2 - y1 > x2 - x1 ? ((y2 - y1) - (x2 - x1)) / 2 : 0; // Если дельта Y больше чем дельта X, то делим дельта X пополам, 
                                                                          // так как придется прибавить это значение с обоих сторон.
            int dY = y2 - y1 < x2 - x1 ? ((x2 - x1) - (y2 - y1)) / 2 : 0; // Если дельта X больше, чем дельта Y, то делим дельта Y пополам, 
                                                                          // так как придется прибавить это значение с обоих сторон.
            int[,] res = new int[size, size]; // Создаем новый массив с новой размерностью.
            // В дальнейшем проверяем пиксели в полученном изображении.
            // Если белый цвет, то в массиве данному элементу присвиваем 1.
            // Если черный цвет, то в массиве данному элементу присваиваем 0.
            for (int x = 0; x < res.GetLength(0); x++)
                for (int y = 0; y < res.GetLength(1); y++)
                {
                    int currentX = x + x1 - dX;
                    int currentY = y + y1 - dY;
                    if (currentX < 0 || currentX >= point.X || currentY < 0 || currentY >= point.Y)
                        res[x, y] = 0;
                    else
                        res[x, y] = image.GetPixel(x + x1 - dX, y + y1 - dY).R > 250 ? 0 : 1;
                }
            return res; // Выводим новый массив, с усеченного изображения.
        }
        // Метод, который произвольный массив приводит к массиву стандартных размеров.
        public static int[,] Standardizing(int[,] ReceivedArray, int[,] ResultingArray)
        {
            for (int i = 0; i < ResultingArray.GetLength(0); i++)
                for (int j = 0; j < ResultingArray.GetLength(1); j++) ResultingArray[i, j] = 0;

            // Коэффициент X, по которому результирующий массив отличается от полученного.
            double СoefficientX = (double)ResultingArray.GetLength(0) / (double)ReceivedArray.GetLength(0);

            // Коэффициент Y, по которому результирующий массив отличается от полученного.
            double СoefficientY = (double)ResultingArray.GetLength(1) / (double)ReceivedArray.GetLength(1); 

            for (int i = 0; i < ReceivedArray.GetLength(0); i++)
                for (int j = 0; j < ReceivedArray.GetLength(1); j++)
                {
                    int currentX = (int)(i * СoefficientX); // Текущий X с учетом коэффициента.
                    int currentY = (int)(j * СoefficientY); // Текущий Y с учетом коэффициента.
                    if (ResultingArray[currentX, currentY] == 0)
                        ResultingArray[currentX, currentY] = ReceivedArray[i, j];
                }
            return ResultingArray;
        }



        public static int[,] CutBitmapAndGetArrayPaint(Bitmap image, Point point)
        {
            int x1 = 0, y1 = 0, x2 = point.X, y2 = point.Y; // x1 и y1 - координаты левого верхнего угла.
                                                            // x2 и y2 - координаты правого нижнего угла.
            for (int x = 0; x < image.Width && x1 == 0; x++) // x1 == 0 - условие необходимое для того, чтобы найти только первую точку.
                for (int y = 0; y < image.Height && x1 == 0; y++) // x1 == 0 - условие необходимое для того, чтобы найти только первую точку.
                    if (image.GetPixel(x, y).ToArgb() != 0) x1 = x; // Движемся из левого верхнего угла пока не найдем первую не белую точку.
                                                             // Полученная точка x1;
            for (int y = 0; y < image.Height && y1 == 0; y++) // y1 == 0 - условие необходимое для того, чтобы найти только первую точку.
                for (int x = 0; x < image.Width && y1 == 0; x++) // y1 == 0 - условие необходимое для того, чтобы найти только первую точку.
                    if (image.GetPixel(x, y).ToArgb() != 0) y1 = y; // Движемся из левого верхнего угла пока не найдем первую не белую точку.
                                                             // Полученная точка y1;
            for (int x = image.Width - 1; x >= 0 && x2 == point.X; x--) // x2 == point.X - условие необходимое для того, чтобы найти только первую точку.
                for (int y = 0; y < image.Height && x2 == point.X; y++) // x2 == point.X - условие необходимое для того, чтобы найти только первую точку.
                    if (image.GetPixel(x, y).ToArgb() != 0) x2 = x; // Движемся из правого верхнего угла пока не найдем первую не белую точку.
                                                             // Полученная точка x2;
            for (int y = image.Height - 1; y >= 0 && y2 == point.Y; y--) // y2 == point.Y - условие необходимое для того, чтобы найти только первую точку.
                for (int x = 0; x < image.Width && y2 == point.Y; x++) // y2 == point.Y - условие необходимое для того, чтобы найти только первую точку.
                    if (image.GetPixel(x, y).ToArgb() != 0) y2 = y; // Движемся из левого нижнего угла пока не найдем первую не белую точку.
                                                             // Полученная точка y2;

            if (x1 == 0 && y1 == 0 && x2 == point.X && y2 == point.Y) // Если мы в итоге не изменили размер ни в одной точке, то оставляем все без изменений
                return null;

            int size = x2 - x1 > y2 - y1 ? x2 - x1 + 1 : y2 - y1 + 1; // Определяем в какой оси наш рисунок больше.
            int dX = y2 - y1 > x2 - x1 ? ((y2 - y1) - (x2 - x1)) / 2 : 0; // Если дельта Y больше чем дельта X, то делим дельта X пополам, 
                                                                          // так как придется прибавить это значение с обоих сторон.
            int dY = y2 - y1 < x2 - x1 ? ((x2 - x1) - (y2 - y1)) / 2 : 0; // Если дельта X больше, чем дельта Y, то делим дельта Y пополам, 
                                                                          // так как придется прибавить это значение с обоих сторон.
            int[,] res = new int[size, size]; // Создаем новый массив с новой размерностью.
            // В дальнейшем проверяем пиксели в полученном изображении.
            // Если белый цвет, то в массиве данному элементу присвиваем 1.
            // Если черный цвет, то в массиве данному элементу присваиваем 0.
            for (int x = 0; x < res.GetLength(0); x++)
                for (int y = 0; y < res.GetLength(1); y++)
                {
                    int currentX = x + x1 - dX;
                    int currentY = y + y1 - dY;
                    if (currentX < 0 || currentX >= point.X || currentY < 0 || currentY >= point.Y)
                        res[x, y] = 0;
                    else
                        res[x, y] = image.GetPixel(x + x1 - dX, y + y1 - dY).ToArgb() == 0 ? 0 : 1;
                }
            return res; // Выводим новый массив, с усеченного изображения.
        }
    }
}
