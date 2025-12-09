using System;

// Подключаем нужное пространство имен и даем псевдонимы

using Person = DoubleLinksListNamespace.PersonData;
using Position = DoubleLinksListNamespace.Position<DoubleLinksListNamespace.PersonData>;
using List = DoubleLinksListNamespace.List<DoubleLinksListNamespace.PersonData>;

//using Person = CursorListNamespace.PersonData;
//using Position = CursorListNamespace.Position<CursorListNamespace.PersonData>;
//using List = CursorListNamespace.List<CursorListNamespace.PersonData>;

namespace Lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем список
            List list = new List();

            // Пример данных
            Person p1 = new Person("Alice".ToCharArray(), "alice@example.com".ToCharArray());
            Person p2 = new Person("Bob".ToCharArray(), "bob@example.com".ToCharArray());
            Person p3 = new Person("Alice".ToCharArray(), "alice@example.com".ToCharArray());

            // Вставка элементов
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
            Position pos1 = list.First();

            while (pos1 != null && pos1.position != list.End().position)
            {
                Person current = list.Retrieve(pos1);
                Position pos2 = list.Next(pos1);

                while (pos2 != null && pos2.position != list.End().position)
                {
                    if (list.Retrieve(pos2).Equals(current))
                    {
                        Position toDelete = pos2;
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
