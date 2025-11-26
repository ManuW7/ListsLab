using System;

// Подключаем нужное пространство имен и даем псевдоним
//using DList = DoubleLinksListNamespace.List<CursorListNamespace.PersonData>;

using List = DoubleLinksListNamespace.List<DoubleLinksListNamespace.PersonData>;

namespace Lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем список выбранного типа
            List list = new List();

            // Пример данных
            var p1 = new DoubleLinksListNamespace.PersonData("Alice".ToCharArray(), "alice@example.com".ToCharArray());
            var p2 = new DoubleLinksListNamespace.PersonData("Bob".ToCharArray(), "bob@example.com".ToCharArray());
            var p3 = new DoubleLinksListNamespace.PersonData("Alice".ToCharArray(), "alice@example.com".ToCharArray());

            // Вставка элементов в список
            list.Insert(list.End(), p1);
            list.Insert(list.End(), p2);
            list.Insert(list.End(), p3);

            Console.WriteLine("Список до удаления дубликатов:");
            list.PrintList();

            // Удаление дубликатов
            RemoveDuplicates(list);

            Console.WriteLine("Список после удаления дубликатов:");
            list.PrintList();
        }

        // Метод удаления дубликатов
        static void RemoveDuplicates(List list)
        {
            // Начинаем с первой позиции
            var pos1 = list.First();

            while (pos1 != null && pos1.getPosition() != list.End().getPosition())
            {
                var current = list.Retrieve(pos1);
                var pos2 = list.Next(pos1);

                while (pos2 != null && pos2.getPosition() != list.End().getPosition())
                {
                    if (list.Retrieve(pos2).Equals(current))
                    {
                        var toDelete = pos2;
                        pos2 = list.Next(pos2);
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
