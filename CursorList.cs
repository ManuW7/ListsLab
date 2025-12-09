namespace CursorListNamespace
{
    public struct PersonData
    {
        private char[] Name;
        private char[] Address;

        public PersonData(char[] name, char[] address)
        {
            Name = new char[20];
            Address = new char[50];

            Array.Copy(name, Name, Math.Min(20, name.Length));
            Array.Copy(address, Address, Math.Min(50, address.Length));
        }

        public override bool Equals(object obj)
        {
            if (obj is PersonData other)
            {
                return new string(this.Name) == new string(other.Name) &&
                       new string(this.Address) == new string(other.Address);
            }
            return false;
        }

        public override string ToString()
        {
            string nameStr = new string(Name).TrimEnd('\0');
            string addressStr = new string(Address).TrimEnd('\0');
            return $"Name: {nameStr}, Address: {addressStr}";
        }
    }

    public class Node<T>
    {
        private T data;
        private int next;

        public Node()
        {
            this.next = -1;
        }

        public Node(T data)
        {
            this.data = data;
            this.next = -1;
        }

        public T getData()
        {
            return this.data;
        }

        public int getNext()
        {
            return this.next;
        }

        public void setNext(int next)
        {
            this.next = next;
        }

        public void setData(T data)
        {
            this.data = data;
        }


    }

    public class Position<T>
    {
        public int position;
        public Position(int index)
        {
            this.position = index;
        }
    }

    public class List<T>
    {
        private int start;
        private const int SIZE = 50;

        private static int space;
        private static Node<T>[] list;

        static List()
        {
            list = new Node<T>[SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                list[i] = new Node<T>();
                list[i].setNext(i + 1);
            }
            list[SIZE - 1].setNext(-1);

            space = 0;
        }

        public List()
        {
            this.start = -1;
        }
        
        // Вспомогательные методы

        // Метод для проверки наличия позиции в данном списке
        private bool PositionIsValid(Position<T> p)
        {
            int currentNode = this.start;

            while (currentNode != -1) {
                if (currentNode == p.position)
                {
                    return true;
                }
                currentNode = list[currentNode].getNext();
            }

            return false;
        }


        // Метод для возврата позиции последнего элемента в списке
        private Position<T> Last()
        {
            int currentNode = this.start;

            while (currentNode != -1)
            {
                if (list[currentNode].getNext() == -1)
                {
                    return new Position<T>(currentNode);
                }
                currentNode = list[currentNode].getNext();
            }

            // Если список пустой
            return null;
        }

        // Метод для поиска предыдущего узла для узла по данной позиции
        private Position<T> GetPreviousPosition(Position<T> p)
        {
            int currentNode = this.start;
            while (currentNode != -1)
            {
                if (list[currentNode].getNext() == p.position)
                {
                    return new Position<T>(currentNode);
                }
                currentNode = list[currentNode].getNext();
            }

            return null;
        }



        public Position<T> End()
        {
            return new Position<T>(-1);
        }

        public void Insert(Position<T> p, T x)  
        {
            // Если вставляемая позиция - это позиция после последнего
            if (p.position == -1)
            {
                // Сохраняем номер ячейки, которая сейчас первая свободная
                int nodeToInsert = space;
                // В space сохраняем номер следующей свободной ячейки
                space = list[space].getNext();
                // Сохраняем в ячейку передаваемое значение
                list[nodeToInsert].setData(x);
                list[nodeToInsert].setNext(-1);
                // Если при этом список был пустой, сохраняем в start ячейку
                if (this.start == -1)
                {
                    this.start = nodeToInsert;
                }
                // Если нет, то нужно найти последний, записать туда номер текущей ячейки
                else
                {
                    int last = this.Last().position;
                    list[last].setNext(nodeToInsert);
                }


            }
            // Если вставляем не в позицию после последнего
            else
            {
                int nodeToInsert = p.position;

                // Если такая позиция есть в списке
                if (this.PositionIsValid(p))
                {
                    // В пустую ячейку вставляем то, что раньше было во вставляемой
                    list[space].setData(list[nodeToInsert].getData());
                    // Сохраним то, куда сейчас ведет space 
                    int nextSpace = list[space].getNext();
                    // Теперь ячейка, которая была пустой, ведет туда, куда вела вставляемая ячейка
                    list[space].setNext(list[nodeToInsert].getNext());
                    // Следующая для вставляемой ячейки будет space
                    list[nodeToInsert].setNext(space);
                    // Обновляем space
                    space = nextSpace;
                    // Записываем собственно данные в ячейку p
                    list[nodeToInsert].setData(x);

                }

            }
        }

        public Position<T> Locate(T x)
        {
            // Проходим от начала по элементам, если нашли нужный, выводим позицию
            int currentNode = start;

            while (currentNode != -1)
            {
                if (list[currentNode].getData().Equals(x))
                {
                    return new Position<T>(currentNode);
                }

                currentNode = list[currentNode].getNext();
            }

            return End();
        }


        public T Retrieve(Position<T> p)
        {
            // Если в данном списке есть node с таким индексом, возвращаем содержимое node
            if (this.PositionIsValid(p)){
                return list[p.position].getData();
            }

            // Если нет, то 
            return default(T);

        }

        public void Delete(Position<T> p)
        {
            // Если мы удаляем start
            if (this.start == p.position)
            {
                int prevStart = this.start;

                // Записываем в start следующий для start элемент
                this.start = list[this.start].getNext();

                // там где раньше был start теперь пустая ячейка, которая ведет в предыдущий Space
                list[prevStart].setNext(space);
                // Теперь space - это то, что было стартом
                space = prevStart;
            }
            // Если же мы удаляем не старт
            else
            {
                // Если такая позиция в списке есть
                if (this.PositionIsValid(p)){
                    // Найдем позицию узла, предыдущего для узла по данной позиции
                    int prevNodePosition = this.GetPreviousPosition(p).position;
                    // В ячейку по найденной позиции запишем ссылку на следующий элемент для p
                    list[prevNodePosition].setNext(list[p.position].getNext());
                    // Удаляемая ячейка теперь свободна, можно добавить ее в качестве space
                    list[p.position].setNext(space);
                    space = p.position;
                }
            }

        }

        public Position<T> Next(Position<T> p)
        {
            // Если такая позиция в списке есть
            if (this.PositionIsValid(p))
            {
                // Если она - последняя
                if (this.Last().position == p.position)
                {
                    // Возвращаем позицию после последнего
                    return End();
                }
                // Если же нет
                return new Position<T>(list[p.position].getNext());
                
            }

            // Если такой позиции в списке нет - результат неопределен
            return null;

        }

        public Position<T> Previous(Position<T> p)
        {
            return this.GetPreviousPosition(p);

        }

        public Position<T> makeNull()
        {
            // Если список не пустой
            if (this.start != -1)
            {
                // Находим последний элемент списка
                int lastElement = this.Last().position;
                // Он теперь ведет в Space
                list[lastElement].setNext(space);
                // Space теперь - начало этого списка
                space = this.start;
            }
            return End();
        }

        public Position<T> First()
        {
            return (this.start == -1 ? End() : new Position<T>(this.start));
        }

        public void PrintList()
        {
            int currentNode = this.start;

            while (currentNode != -1)
            {
                Console.WriteLine(list[currentNode].getData());
                currentNode = list[currentNode].getNext();
            }
        }
    }

}