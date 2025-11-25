using System;

// Подключаем нужное пространство имен и даем псевдоним
//using DList = DoubleLinksListNamespace.List<CursorListNamespace.PersonData>;

using CList = CursorListNamespace.List<CursorListNamespace.PersonData>;

namespace Lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем список выбранного типа
            CList list = new CList();

            // Пример данных
            var p1 = new CursorListNamespace.PersonData("Alice".ToCharArray(), "alice@example.com".ToCharArray());
            var p2 = new CursorListNamespace.PersonData("Bob".ToCharArray(), "bob@example.com".ToCharArray());
            var p3 = new CursorListNamespace.PersonData("Alice".ToCharArray(), "alice@example.com".ToCharArray());

            // Вставка элементов в список
            list.Insert(list.End(), p1);
            list.Insert(list.End(), p2);
            list.Insert(list.End(), p3);

            Console.WriteLine("Список до удаления дубликатов:");
            list.PrintList();

            // Удаление дубликатов
            RemoveDuplicates(list);

            Console.WriteLine("\nСписок после удаления дубликатов:");
            list.PrintList();
        }

        // Метод удаления дубликатов
        static void RemoveDuplicates(CList list)
        {
            // Начинаем с первой позиции
            var pos1 = list.First();

            while (pos1 != null && pos1.getPosition() != null)
            {
                var current = list.Retrieve(pos1);
                var pos2 = list.Next(pos1);

                while (pos2 != null && pos2.getPosition() != null)
                {
                    if (list.Retrieve(pos2).Equals(current))
                    {
                        var toDelete = pos2;
                        pos2 = list.Next(pos2); // сначала сохраняем следующую позицию
                        list.Delete(toDelete);
                    }
                    else
                    {
                        pos2 = list.Next(pos2);
                    }
                }

                pos1 = list.Next(pos1);
            }
        }
    }
}
